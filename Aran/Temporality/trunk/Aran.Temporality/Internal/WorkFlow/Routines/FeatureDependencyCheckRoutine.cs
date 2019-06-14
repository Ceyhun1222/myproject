using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Aim.Utilities;
using Aran.Geometries;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.Implementation.Storage;

namespace Aran.Temporality.Internal.WorkFlow.Routines
{
    [Flags]
    internal enum ValidationResult
    {
        Ok = 0,
        Warning = 1,
        Error = 2
    }

    internal class FeatureDependencyCheckRoutine : AbstractCheckRoutine
    {
        #region Overrides of AbstractCheckRoutine

        public override int GetReportType()
        {
            return (int) ReportType.FeatureDependencyReport;
        }

        public override bool CheckPrivateSlot()
        {
            InitPrivateSlot();

            var deps = StorageService.GetFeatureDependencies();
            CurrentOperationStatus.NextTask(deps.Count);

            var result = true;

            if (DependencyIdsToProcess != null)
            {
                deps = deps.Where(t => DependencyIdsToProcess.Contains(t.Id)).ToList();
            }


            foreach (var dependency in deps)
            {
                result = result & CheckConfiguration(dependency);
                CurrentOperationStatus.NextOperation();
            }


            ReleasePrivateSlot();
            return result;
        }

        public static CompressedFeatureDependencyConfiguration Deserialize(FeatureDependencyConfiguration configurationEntity)
        {
            var m = MemoryUtil.GetMemoryStream(configurationEntity.Data);
            var zip = new DeflateStream(m, CompressionMode.Decompress);
            var unzipped = MemoryUtil.GetMemoryStream();
            while (true)
            {
                var i = zip.ReadByte();
                if (i == -1) break;
                unzipped.WriteByte((byte)i);
            }
            return FormatterUtil.ObjectFromBytes<CompressedFeatureDependencyConfiguration>(unzipped.ToArray());
        }

        public bool CheckConfiguration(FeatureDependencyConfiguration configurationEntity)
        {
            var configuration = Deserialize(configurationEntity);

            var lightList = new ConcurrentBag<LightFeature>();

            var items = Context.LoadStates(configuration.FeatureType);
            var errors = 0;
            foreach (var lightFeature in items//.AsParallel()
                .Select(feature => EnlightFeature2(feature.Feature, configuration)))
            {
                lightFeature.CalculateHasProblems();
                lightList.Add(lightFeature);
                if (lightFeature.Flag != LightData.Ok)
                {
                    Interlocked.Increment(ref errors);
                }
            }

            var result = errors==0;

            var report = new ProblemReport
            {
                PublicSlotId = Context.PrivateSlot.PublicSlot.Id,
                PrivateSlotId = Context.PrivateSlot.Id,
                ReportType = GetReportType(),
                ConfigId = configurationEntity.Id,
                FeaturesProcessed = items.Count(),
                ErrorsFound = errors,
                ReportData = FormatterUtil.ObjectToBytes(lightList.ToList())
            };

            StorageService.UpdateProblemReport(report);
            return result;
        }

        private static void AddField(LightFeature lightFeature, LightField field)
        {
            if (field.Value != null && !field.Value.GetType().FullName.StartsWith("System") && !(field.Value is ValClassBase) && !(field.Value is Geometry))
            {
                return;
            }

            if (lightFeature.Fields == null)
            {
                lightFeature.Fields = new[] { field };
            }
            else
            {
                var list = lightFeature.Fields.ToList();
                list.Add(field);
                lightFeature.Fields = list.ToArray();
            }
        }

        private static LightFeature AddComplexField(LightFeature lightFeature, LightComplexField field)
        {
            if (lightFeature.ComplexFields == null)
            {
                field.Value = new LightFeature();
                lightFeature.ComplexFields = new[] { field };
                return field.Value;
            }

            var list = lightFeature.ComplexFields.ToList();

            var correspondingField = list.FirstOrDefault(t => t.Name == field.Name);
            if (correspondingField == null)
            {
                field.Value = new LightFeature();
                list.Add(field);
                lightFeature.ComplexFields = list.ToArray();
                return field.Value;
            }

            var result=correspondingField.Value;
            if (result==null)
            {
                result=new LightFeature();
                correspondingField.Value = result;
            }
            return result;
        }

        private static object GetValue(object feature, string propertyPath)
        {
             var type = feature.GetType();
             var property = type.GetProperty(propertyPath);
            if (property == null) return null;

            if (feature is ChoiceClass)
            {
                var choice = feature as IEditChoiceClass;

                var classInfo = AimMetadata.GetClassInfoByIndex(feature as IAimObject);
                var propInfo = classInfo.Properties[propertyPath];

                if (propInfo.IsFeatureReference)
                {
                    if (propInfo.PropType.SubClassType == AimSubClassType.AbstractFeatureRef)
                    {
                        if (AimMetadataUtility.GetAbstractChilds(propInfo.PropType)
                                .Any(ci => ci.Index == choice.RefType))
                            return choice.RefValue;
                    }
                    else
                    {
                        if ((int)propInfo.ReferenceFeature == choice.RefType)
                        {
                            return choice.RefValue;
                        }
                    }
                }
                else
                {
                    if (AimMetadata.GetAimTypeIndex(choice.RefValue as IAimObject) == propInfo.TypeIndex)
                    {
                        return choice.RefValue;
                    }
                }

                //var choiceValue=choice.RefValue;
                //if (choiceValue == null) return null;
                //if (property.PropertyType.IsInstanceOfType(choiceValue))
                //{
                //    if (choiceValue is FeatureRef)
                //    {
                //        var linked = property.PropertyType.GetCustomAttributes(typeof (LinkedFeatureAttribute), true);
                //        if (linked.Length > 0)
                //        {
                //            var attribute = (LinkedFeatureAttribute)linked[0];
                //            return attribute.LinkedFeature == choice.RefType ? choiceValue : null;
                //        }
                //        throw new Exception("Abnormal choice definition in object" +feature.GetType()+
                //                            " for property " + propertyPath);
                //    }
                //    return choiceValue;
                //}
                return null;
            }
           
            return property.GetValue(feature, null);
        }

        private void EnlightFeatureR(object feature, LightFeature lightFeature, Dictionary<string, int> config, Dictionary<String, CompressedFeatureDependencyConfiguration> linkFeatureTypes, String parentPath)
        {
            foreach (var pair in config)
            {
                var propertyPath = pair.Key;
                if (propertyPath==null) continue;
                
                var mask = pair.Value;
                CompressedFeatureDependencyConfiguration featureType = null;
                if (linkFeatureTypes.ContainsKey(propertyPath))
                {
                    featureType = linkFeatureTypes[propertyPath];
                }

                if (!string.IsNullOrEmpty(parentPath) && !propertyPath.StartsWith(parentPath)) continue;

                if (!string.IsNullOrEmpty(parentPath))
                {
                    if (propertyPath.Length <= parentPath.Length + 1) continue;
                    propertyPath = propertyPath.Substring(parentPath.Length+1);//+1 for \\
                }

                var i = propertyPath.IndexOf('\\');
                if (i == -1)
                {
                    //TODO: field and links
                    var value = GetValue(feature, propertyPath);
                    if (value == null)
                    {
                        AddNull(lightFeature, propertyPath, featureType, mask);
                        continue;
                    }

                    if (featureType !=null)
                    {
                        //it is link


                        if (value is FeatureRef)
                        {
                            var guid = (value as FeatureRef).Identifier;
                            var linkedFeature = Context.LoadFeature(featureType.FeatureType, guid);
                            var linkedLightFeature=EnlightFeature2(linkedFeature.Feature, featureType);
                   
                            //var subFeature2 = AddComplexField(lightFeature, new LightComplexField
                            //{
                            //    Name = propertyPath,
                            //    Flag = mask,
                            //    Value = linkedLightFeature
                            //});


                            AddLink(lightFeature, new LightLink { FeatureType = (int)featureType.FeatureType ,Flag = mask,Name = propertyPath,Value = linkedLightFeature});
                        }
                        
                        continue;
                    }

                    if (value is IList)
                    {
                        var list = value as IList;
                        if (list.Count == 0)
                        {
                            AddNull(lightFeature, propertyPath, featureType, mask);
                            continue;
                        }

                        foreach (var item in list)
                        {
                            AddField(lightFeature, new LightField
                            {
                                Name = propertyPath,
                                Flag = mask,
                                Value = item //may be to string?
                            });
                        }
                        continue;
                    }
                    AddField(lightFeature, new LightField
                    {
                        Name = propertyPath,
                        Flag = mask,
                        Value = value //may be to string?
                    });
                }
                else
                {
                    var currentProperty = propertyPath.Substring(0, i);
                    var nextPropertyPath = propertyPath.Substring(i + 1);

                    if (string.IsNullOrEmpty(parentPath))
                        parentPath = currentProperty;
                    else
                    {
                        parentPath += "\\" + currentProperty;
                    }

                    var value = GetValue(feature, currentProperty);
                    if (value == null)
                    {
                        AddNull(lightFeature, propertyPath, featureType, mask);
                        continue;
                    }

                    if (value is IList)
                    {
                        var list = value as IList;
                        if (list.Count == 0)
                        {
                            AddNull(lightFeature, propertyPath, featureType, mask);
                            continue;
                        }

                        foreach (var item in list)
                        {
                            var subFeature = AddComplexField(lightFeature, new LightComplexField
                            {
                                Name = currentProperty,
                                Flag = mask
                            });
                            EnlightFeatureR(item, subFeature, config, linkFeatureTypes, parentPath);
                        }
                        continue;
                    }
                    
                    var subFeature2 = AddComplexField(lightFeature, new LightComplexField
                    {
                        Name = currentProperty,
                        Flag = mask
                    });
                    EnlightFeatureR(value, subFeature2, config,linkFeatureTypes, parentPath);
                }
            }
          
        }


        private static void AddNull(LightFeature lightFeature, string name, CompressedFeatureDependencyConfiguration featureType, int mask)
        {
            if ((mask & LightData.IsReverseLink) != 0 || (mask & LightData.IsDirectLink) != 0)
            {
                AddLink(lightFeature, new LightLink
                {
                    Name = name,
                    Flag = LightData.Missing | mask,
                    
                });
                return;
            }

            AddField(lightFeature, new LightField
            {
                Name = name,
                Flag = LightData.Missing | mask
            });

        }

    

        private static bool ConvertProperty(LightFeature lightFeature, object feature, string propertyPath, int modifier)
        {
            var i = propertyPath.IndexOf('\\');
            if (i == -1)
            {
                var value = GetValue(feature, propertyPath);
                if (value == null)
                {
                    AddField(lightFeature, new LightField
                    {
                        Name = propertyPath,
                        Flag = LightData.Missing | modifier
                    });
                    return false;
                }
                if (value is IList)
                {
                    var list = value as IList;
                    if (list.Count == 0)
                    {
                        AddField(lightFeature, new LightField
                        {
                            Name = propertyPath,
                            Flag = LightData.Missing | modifier
                        });
                        return false;
                    }
                    foreach (var item in list)
                    {
                        AddField(lightFeature, new LightField
                        {
                            Name = propertyPath,
                            Flag = modifier,
                            Value = item //may be to string?
                        });
                    }
                    return true;
                }
                AddField(lightFeature, new LightField
                {
                    Name = propertyPath,
                    Flag = modifier,
                    Value = value //may be to string?
                });
                return true;
            }
            else
            {
                var currentProperty = propertyPath.Substring(0, i);
                var nextPropertyPath = propertyPath.Substring(i + 1);

                var value = GetValue(feature, currentProperty);
                if (value == null)
                {
                    AddField(lightFeature, new LightField
                    {
                        Name = propertyPath,
                        Flag = LightData.Missing | modifier
                    });
                    return false;
                }
                if (value is IList)
                {
                    var list = value as IList;
                    if (list.Count == 0)
                    {
                        AddField(lightFeature, new LightField
                        {
                            Name = propertyPath,
                            Flag = LightData.Missing | modifier
                        });
                        return false;
                    }
                    var result = true;
                    foreach (var item in list)
                    {
                        var subFeature = AddComplexField(lightFeature, new LightComplexField
                        {
                            Name = currentProperty,
                            Flag = modifier
                        });
                        result &= ConvertProperty(subFeature, item, nextPropertyPath, modifier);
                    }
                    return result;
                }

                var subFeature2 = AddComplexField(lightFeature, new LightComplexField
                {
                    Name = currentProperty,
                    Flag = modifier
                });
                return ConvertProperty(subFeature2, value, nextPropertyPath, modifier);

            }
        }

        private static void AddLink(LightFeature lightFeature, LightLink link)
        {
            if (lightFeature.Links == null)
            {
                lightFeature.Links = new[] { link };
            }
            else
            {
                var list = lightFeature.Links.ToList();
                list.Add(link);
                lightFeature.Links = list.ToArray();
            }
        }

        //public class Config
        //{
        //    public int Mask;
        //    public FeatureType FeatureType;
        //}

        public LightFeature EnlightFeature2(Feature feature, CompressedFeatureDependencyConfiguration configuration)
        {
            var result = new LightFeature
            {
                FeatureType = (int) feature.FeatureType,
                Guid = feature.Identifier
            };

            Dictionary<String, int> config=new Dictionary<string, int>();
            Dictionary<String, CompressedFeatureDependencyConfiguration> linkFeatureTypes = new Dictionary<string, CompressedFeatureDependencyConfiguration>();
            
            foreach (var prop in configuration.MandatoryProperties ?? new string[0])
            {
                config.Add(prop, LightData.IsMandatory);
            }
            foreach (var prop in (configuration.OptionalProperties ?? new string[0]).
                Except(configuration.MandatoryProperties ?? new string[0]))
            {
                config.Add(prop, LightData.IsOptional);
            }
            foreach (var child in configuration.Children ?? new CompressedFeatureDependencyConfiguration[0])
            {
                foreach (var mandatoryLinkPath in child.MandatoryLinks ?? new string[0])
                {
                    if (child.IsDirect)
                    {
                        config.Add(mandatoryLinkPath.Replace('/', '\\'), LightData.IsMandatory | LightData.IsDirectLink);
                        linkFeatureTypes.Add(mandatoryLinkPath.Replace('/', '\\'), child);
                    }
                }

                foreach (var optionalLinkPath in (child.OptionalLinks ?? new string[0]).
                    Except(child.MandatoryLinks ?? new string[0]))
                {
                    if (child.IsDirect)
                    {
                        config.Add(optionalLinkPath.Replace('/', '\\'), LightData.IsOptional | LightData.IsDirectLink);
                        linkFeatureTypes.Add(optionalLinkPath.Replace('/', '\\'), child);
                    }
                }
            }

            EnlightFeatureR(feature, result, config, linkFeatureTypes, null);


            foreach (var child in configuration.Children ?? new CompressedFeatureDependencyConfiguration[0])
            {
                foreach (var mandatoryLinkPath in child.MandatoryLinks ?? new string[0])
                {
                    var linkedFeatures = Context.GetLinks(child.IsDirect, child.FeatureType, feature, mandatoryLinkPath);
                    if (linkedFeatures.Count == 0)
                    {
                        AddLink(result, new LightLink
                        {
                            Name = mandatoryLinkPath,
                            Flag = LightData.Missing | LightData.IsMandatory | (child.IsDirect ? LightData.IsDirectLink : 0),
                            FeatureType = (int)child.FeatureType
                        });
                    }
                    else
                    {
                        foreach (var linkedFeature in linkedFeatures)
                        {
                            AddLink(result, new LightLink
                            {
                                Name = mandatoryLinkPath,
                                Value = EnlightFeature(linkedFeature, child),
                                Flag = LightData.IsMandatory | (child.IsDirect ? LightData.IsDirectLink : 0),
                                FeatureType = (int)child.FeatureType
                            });
                        }
                    }
                }

                foreach (var optionalLinkPath in (child.OptionalLinks ?? new string[0]).
                    Except(child.MandatoryLinks ?? new string[0]))
                {
                    var linkedFeatures = Context.GetLinks(child.IsDirect, child.FeatureType, feature, optionalLinkPath);
                    if (linkedFeatures.Count == 0)
                    {
                        AddLink(result, new LightLink
                        {
                            Name = optionalLinkPath,
                            Flag = LightData.Missing | LightData.IsOptional | (child.IsDirect ? LightData.IsDirectLink : 0),
                            FeatureType = (int)child.FeatureType
                        });
                    }
                    else
                    {
                        foreach (var linkedFeature in linkedFeatures)
                        {
                            AddLink(result, new LightLink
                            {
                                Name = optionalLinkPath,
                                Value = EnlightFeature(linkedFeature, child),
                                Flag = LightData.IsOptional | (child.IsDirect ? LightData.IsDirectLink : 0),
                                FeatureType = (int)child.FeatureType
                            });
                        }
                    }
                }

            }



            return result;
        }

     

        public LightFeature EnlightFeature(Feature feature, CompressedFeatureDependencyConfiguration configuration)
        {
            var result = new LightFeature
            {
                FeatureType = (int)feature.FeatureType,
                Guid = feature.Identifier
            };

            foreach (var prop in configuration.MandatoryProperties ?? new string[0])
            {
                if (ConvertProperty(result, feature, prop, LightData.IsMandatory)) continue;
                result.Flag |= LightData.Missing;
                result.Flag |= LightData.IsMandatory;
            }

            foreach (var prop in (configuration.OptionalProperties ?? new string[0]).
                Except(configuration.MandatoryProperties ?? new string[0]))
            {
                if (ConvertProperty(result, feature, prop, LightData.IsOptional)) continue;
                result.Flag |= LightData.Missing;
                result.Flag |= LightData.IsOptional;
            }

            foreach (var child in configuration.Children ?? new CompressedFeatureDependencyConfiguration[0])
            {
                foreach (var mandatoryLinkPath in child.MandatoryLinks ?? new string[0])
                {
                    var linkedFeatures = Context.GetLinks(child.IsDirect, child.FeatureType, feature, mandatoryLinkPath);
                    if (linkedFeatures.Count == 0)
                    {
                        AddLink(result, new LightLink
                                            {
                                                Name = mandatoryLinkPath,
                                                Flag = LightData.Missing | LightData.IsMandatory | (child.IsDirect?LightData.IsDirectLink:0),
                                                FeatureType =(int) child.FeatureType
                                            });
                    }
                    else
                    {
                        foreach (var linkedFeature in linkedFeatures)
                        {
                            AddLink(result, new LightLink
                            {
                                Name = mandatoryLinkPath,
                                Value = EnlightFeature(linkedFeature, child),
                                Flag = LightData.IsMandatory | (child.IsDirect ? LightData.IsDirectLink : 0),
                                FeatureType = (int)child.FeatureType
                            });
                        }
                    }
                }

                foreach (var optionalLinkPath in (child.OptionalLinks ?? new string[0]).
                    Except(child.MandatoryLinks ?? new string[0]))
                {
                    var linkedFeatures = Context.GetLinks(child.IsDirect, child.FeatureType, feature, optionalLinkPath);
                    if (linkedFeatures.Count == 0)
                    {
                        AddLink(result, new LightLink
                                            {
                                                Name = optionalLinkPath,
                                                Flag = LightData.Missing | LightData.IsOptional | (child.IsDirect ? LightData.IsDirectLink : 0),
                                                FeatureType = (int)child.FeatureType
                                            });
                    }
                    else
                    {
                        foreach (var linkedFeature in linkedFeatures)
                        {
                            AddLink(result, new LightLink
                            {
                                Name = optionalLinkPath,
                                Value = EnlightFeature(linkedFeature, child),
                                Flag = LightData.IsOptional | (child.IsDirect ? LightData.IsDirectLink : 0),
                                FeatureType = (int)child.FeatureType
                            });
                        }
                    }
                }

            }


            return result;
        }

        #endregion

        public int[] DependencyIdsToProcess { get; set; }
    }
}

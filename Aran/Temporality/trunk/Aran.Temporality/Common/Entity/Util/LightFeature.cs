using System;
using System.Runtime.Serialization;
using Aran.Package;
using Aran.Temporality.Common.Util;

namespace Aran.Temporality.Common.Entity.Util
{
    public class LightData
    {
        public static int Ok;
        public static int Missing = 1;
        public static int IsMandatory = 2;
        public static int IsOptional=4;
        public static int IsDirectLink = 8;
        public static int IsReverseLink = 16;

    }

    [Serializable]
    public class LightField : ISerializable
    {
        public string Name;
        public object Value;
        public int Flag;

        public override string ToString()
        {
            return Name+":"+Value;
        }

		 #region Implementation of ISerializable

		public LightField ( )
		{

		}

		public LightField ( SerializationInfo info, StreamingContext context )
		{

			Name = ( string ) info.GetValue ( "Name", typeof ( string ) );

			var bytes = info.GetValue ( "Value", typeof ( byte[] ) );

		    try
		    {
                if (bytes != null)
                {
                    var isPackable = (bool)info.GetValue("IsPackable", typeof(bool));

                    Value = isPackable ? FormatterUtil.ObjectFromBytes<IPackable>((byte[])bytes) :
                        FormatterUtil.ObjectFromBytes<object>((byte[])bytes);
                }
		    }
		    catch (Exception)
		    {
		        
		    }
			

			Flag = ( int ) info.GetValue ( "Flag", typeof ( int ) );
		}

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
			info.AddValue ( "Name", Name, typeof ( string ) );

			if ( Value != null )
			{
				info.AddValue ( "Value", FormatterUtil.ObjectToBytes ( Value ), typeof ( byte[] ) );
			}
			else
			{
			    info.AddValue ( "Value", null );
			}

            info.AddValue("IsPackable", Value is IPackable);
			info.AddValue ( "Flag", Flag, typeof ( int ) );
		}

        #endregion
    }

    [Serializable]
    public class LightComplexField
    {
        public string Name;
        public LightFeature Value;
        public int Flag;

        public override string ToString()
        {
            return Name + ":" + Value;
        }
    }


    [Serializable]
    public class LightLink
    {
        public string Name;
        public LightFeature Value;
        public int Flag;
        public int FeatureType;

        public override string ToString()
        {
            return Name + ":" + Value;
        }
    }

    [Serializable]
    public class LightFeature 
    {
        public int Flag;
        public int FeatureType;
        public Guid Guid;

        [NonSerialized]
        public long RowId;

        public bool IsDirect;//to parent

        public LightField[] Fields;
        public LightComplexField[] ComplexFields;
        public LightLink[] Links;


        #region FieldProblems
        private bool? _hasFieldProblems;
        public bool HasFieldProblems
        {
            get
            {
                if (_hasFieldProblems == null)
                {
                    _hasFieldProblems = CalculateHasFieldProblems();
                }
                return (bool)_hasFieldProblems;
            }
        }

        public bool CalculateHasFieldProblems()
        {
            foreach (var field in Fields??new LightField[0])
            {
                if (field==null) continue;
                if ((field.Flag & LightData.Missing) != 0 && (field.Flag & LightData.IsMandatory)!=0)
                {
                    return true;
                }
            }
            foreach (var complexField in ComplexFields ?? new LightComplexField[0])
            {
                if (complexField==null) continue; 
                if ((complexField.Flag & LightData.Missing) != 0 && (complexField.Flag & LightData.IsMandatory) != 0)
                {
                    return true;
                }
                if (complexField.Value != null && (complexField.Flag & LightData.IsMandatory) != 0)
                {
                    if (complexField.Value.HasFieldProblems)
                    {
                        return true;
                    }
                }
                else if (complexField.Value != null && (complexField.Flag & LightData.IsOptional) != 0)
                {
                    if (complexField.Value.HasFieldProblems)
                    {
                        //do nothing
                    }
                }
            }
            return false;
        }
        #endregion

        #region LinksProblems
        private bool? _hasLinksProblems;
        public bool HasLinksProblems
        {
            get
            {
                if (_hasLinksProblems == null)
                {
                    _hasLinksProblems = CalculateHasLinksProblems();
                }
                return (bool)_hasLinksProblems;
            }
        }
        public bool CalculateHasLinksProblems()
        {
            foreach (var link in Links ?? new LightLink[0])
            {
                if (link == null) continue;

                if ((link.Flag & LightData.Missing) != 0 && (link.Flag & LightData.IsMandatory) != 0)
                {
                    return true;
                }
                if (link.Value != null && (link.Flag & LightData.IsMandatory) != 0)
                {
                    if (link.Value.HasProblems)
                    {
                        return true;
                    }
                }
                else if (link.Value != null && (link.Flag & LightData.IsOptional) != 0)
                {
                    if (link.Value.HasProblems)
                    {
                        //do nothing
                    }
                }
            }
            return false;
        }
        #endregion

        #region Problems
        private bool? _hasProblems;
        public bool HasProblems
        {
            get
            {
                if (_hasProblems == null)
                {
                    _hasProblems = CalculateHasProblems();
                }
                return (bool)_hasProblems;
            }
        }
        public bool CalculateHasProblems()
        {
            if (HasFieldProblems)
            {
                return true;
            }
            if (HasLinksProblems)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}

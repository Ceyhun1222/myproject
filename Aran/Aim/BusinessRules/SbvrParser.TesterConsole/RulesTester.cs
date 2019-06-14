using Aran.Aim.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SbvrParser.TesterConsole
{
    public static class RulesTester
    {
        private static Dictionary<string, BRule> _rules;

        public static Dictionary<string, BRule> GetRules()
        {
            if (_rules == null)
            {
                _rules = new Dictionary<string, BRule>();

                var miArr = typeof(RulesTester).GetMethods(
                    BindingFlags.Static | BindingFlags.Public);

                foreach(var mi in miArr)
                {
                    if (mi.Name.StartsWith("Get_") && 
                        mi.ReturnType.Name == "BRule" &&
                        mi.Invoke(null, null) is BRule rule)
                    {
                        var id = mi.Name.Substring(4);
                        _rules.Add(id, rule);
                    }
                }
            }
            return _rules;
        }

        public static BRule Get_1028BA()
        {
            return new BRule
            {
                Type = RuleType.Obligatory,
                Noun = new Noun
                {
                    Name = "Service",
                    Childs =
                    {
                        new Noun
                        {
                            Name = "TrafficSeparationService",
                            Childs =
                            {
                                new Noun { Name = "AirTrafficControlService" }
                            }
                        }
                    }
                },
                Operation = new OperationGroup
                {
                    Type = LogicType.And,
                    Items =
                    {
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.Assigned,
                            Noun = "serviceProvider"
                        },
                        new Operation
                        {
                            Type = OperationType.ResolvedInto,
                            Noun = "serviceProvider",
                            Value = "exactly one",
                            Association = new OperAssociation
                            {
                                Name= "isProvidedBy",
                                Noun = new Noun { Name = "Unit" }
                            }
                        }
                    }
                }
            };
        }

        public static BRule Get_1028DB()
        {
            return new BRule
            {
                Type = RuleType.Obligatory,
                Noun = new Noun
                {
                    Name = "SegmentLeg",
                    Childs =
                    {
                        new Noun
                        {
                            Name = "ApproachLeg",
                            Childs =
                            {
                                new Noun { Name = "MissedApproachLeg" }
                            }
                        }
                    }
                },
                Operation = new OperationGroup
                {
                    Type = LogicType.And,
                    Items =
                    {
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.Assigned,
                            Noun = "holding.HoldingUse.theHoldingPattern"
                        },
                        new Operation
                        {
                            Type = OperationType.ResolvedInto,
                            Noun = "holding.HoldingUse.theHoldingPattern",
                            Value = "exactly one",
                            Association = new OperAssociation
                            {
                                Name= "establishes",
                                Noun = new Noun{ Name = "HoldingPattern" }
                            }
                        }
                    }
                }
            };
        }

        public static BRule Get_1A529E()
        {
            return new BRule
            {
                Type = RuleType.Obligatory,
                Noun = new Noun
                {
                    Name = "PropertiesWithSchedule",
                    Childs =
                    {
                        new Noun { Name = "VerticalStructurePart" }
                    }
                },
                Operation = new OperationGroup
                {
                    Type = LogicType.And,
                    Items =
                    {
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.Assigned,
                            Noun = "specialDateAuthority"
                        },
                        new Operation
                        {
                            Type = OperationType.ResolvedInto,
                            Noun = "specialDateAuthority",
                            Value = "exactly one",
                            Association = new OperAssociation
                            {
                                Name = "appliesSpecialDatesOf",
                                Noun = new Noun { Name = "OrganisationAuthority" }
                            }
                        }
                    }
                }
            };
        }

        public static BRule Get_1A8548()
        {
            return new BRule
            {
                Type = RuleType.Obligatory,
                Noun = new Noun { Name = "ObstacleArea" },
                Operation = new OperationGroup
                {
                    Type = LogicType.And,
                    Items =
                    {
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.Assigned,
                            Noun = "obstacle"
                        },
                        new Operation
                        {
                            Type = OperationType.ResolvedInto,
                            Noun = "obstacle",
                            Value = "exactly one",
                            Association = new OperAssociation
                            {
                                Name = "hasObstacle",
                                Noun = new Noun { Name = "VerticalStructure" }
                            }
                        }
                    }
                }
            };
        }

        public static BRule Get_34FA9()
        {
            return new BRule
            {
                Type = RuleType.Obligatory,
                Noun = new Noun { Name = "VerticalStructure" },
                Operation = new OperationGroup
                {
                    Type = LogicType.And,
                    Items =
                    {
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.Assigned,
                            Noun = "supportedGroundLight"
                        },
                        new Operation
                        {
                            Type = OperationType.ResolvedInto,
                            Noun = "supportedGroundLight",
                            Value = "exactly one",
                            Association = new OperAssociation
                            {
                                Name = "supports",
                                Noun = new Noun
                                {
                                    Name = "GroundLightSystem",
                                    Childs =
                                    {
                                        new Noun { Name = "ApronLightSystem" },
                                        new Noun { Name = "TaxiwayLightSystem" },
                                        new Noun { Name = "TouchDownLiftOffLightSystem" },
                                        new Noun { Name = "RunwayDirectionLightSystem" },
                                        new Noun { Name = "VisualGlideSlopeIndicator" },
                                        new Noun { Name = "TaxiHoldingPositionLightSystem" },
                                        new Noun { Name = "RunwayProtectAreaLightSystem" },
                                        new Noun { Name = "GuidanceLineLightSystem" }
                                    }
                                }
                            }
                        }
                    }
                }
                
            };
        }

        public static BRule Get_F07AF()
        {
            return new BRule
            {
                Type = RuleType.Prohibited,
                Noun = new Noun { Name = "AirportHeliport" },
                Operation = new Operation
                {
                    Type = OperationType.Equal,
                    Noun = "fieldElevation",
                    Value = "('FLOOR', 'CEILING', 'GND', 'UNL')"
                }
            };
        }

        public static BRule Get_F07A9()
        {
            return new BRule
            {
                Type = RuleType.Prohibited,
                Noun = new Noun { Name = "RouteSegment" },
                Operation = new Operation
                {
                    Type = OperationType.Equal,
                    Noun = "minimumObstacleClearanceAltitude",
                    Value = "('FLOOR', 'CEILING')"
                }
            };
        }

        public static BRule Get_135151()
        {
            return new BRule
            {
                Type = RuleType.Prohibited,
                Noun = new Noun { Name = "OrganisationAuthority" },
                Operation = new Operation
                {
                    Type = OperationType.Equal,
                    Noun = "type",
                    Value = "('STATE', 'STATE_GROUP', 'INTL_ORG')"
                }
            };
        }

        public static BRule Get_135152()
        {
            return new BRule
            {
                Type = RuleType.Prohibited,
                Noun = new Noun { Name = "GeoBorder" },
                Operation = new Operation
                {
                    Type = OperationType.Equal,
                    Noun = "type",
                    Value = "'STATE'"
                }
            };
        }

        public static BRule Get_135153()
        {
            return new BRule
            {
                Type = RuleType.Prohibited,
                Noun = new Noun { Name = "Airspace" },
                Operation = new Operation
                {
                    Type = OperationType.Equal,
                    Noun = "type",
                    Value = "('NAS', 'FIR', 'UIR')"
                }
            };
        }

        public static BRule __Get_33070()
        {
            return new BRule
            {
                Type = RuleType.Prohibited,
                Noun = new Noun { Name = "Navaid" },
                Operation = new OperationGroup
                {
                    Type = LogicType.And,
                    Items =
                    {
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.Equal,
                            Noun = "type",
                            Value = "'SDF'"
                        },
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.Assigned,
                            Noun = "servedAirport"
                        },
                        new Operation
                        {
                            Type = OperationType.Undefined,
                            Noun = "location.ElevatedPoint",
                            Value = "coordinates expressed with less than 5 decimals"
                        }
                    }
                }
            };
        }

        public static BRule Get_12B510()
        {
            return new BRule
            {
                Type = RuleType.Prohibited,
                Noun = new Noun { Name = "AirspaceVolume" },
                Operation = new OperationGroup
                {
                    Type = LogicType.And,
                    Items =
                    {
                        new Operation
                        {
                            IsNot = true,
                            IsWith = true,
                            Type = OperationType.Assigned,
                            Noun = "contributorAirspace"
                        },
                        new Operation
                        {
                            IsNot = true,
                            Type = OperationType.Assigned,
                            Noun = "horizontalProjection"
                        },
                        new Operation
                        {
                            IsNot = true,
                            Type = OperationType.Assigned,
                            Noun = "centreline"
                        }
                    }
                }
            };
        }

        public static BRule Custom_12B510()
        {
            return new BRule
            {
                Type = RuleType.Prohibited,
                Noun = new Noun { Name = "Airspace" },
                Operation = new OperationGroup
                {
                    Type = LogicType.And,
                    Items =
                    {
                        new Operation
                        {
                            IsNot = true,
                            IsWith = true,
                            Type = OperationType.Assigned,
                            Noun = "geometryComponent.AirspaceGeometryComponent.theAirspaceVolume.AirspaceVolume.contributorAirspace"
                        },
                        new Operation
                        {
                            IsNot = true,
                            Type = OperationType.Assigned,
                            Noun = "geometryComponent.AirspaceGeometryComponent.theAirspaceVolume.AirspaceVolume.horizontalProjection"
                        },
                        new Operation
                        {
                            IsNot = true,
                            Type = OperationType.Assigned,
                            Noun = "geometryComponent.AirspaceGeometryComponent.theAirspaceVolume.AirspaceVolume.centreline"
                        }
                    }
                }
            };
        }

        public static BRule Get_1AB7DA()
        {
            return new BRule
            {
                Type = RuleType.Obligatory,
                Noun = new Noun { Name = "PilotControlledLighting" },
                Operation = new OperationGroup
                {
                    Type = LogicType.And,
                    Items =
                    {
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.Assigned,
                            Noun = "activatedGroundLighting"
                        },
                        new Operation
                        {
                            Type = OperationType.ResolvedInto,
                            Noun = "activatedGroundLighting",
                            Value = "exactly one",
                            Association = new OperAssociation
                            {
                                Name= "activates",
                                Noun = new Noun
                                {
                                    Name = "GroundLightSystem",
                                    Childs =
                                    {
                                        new Noun { Name = "ApronLightSystem" },
                                        new Noun { Name = "TaxiwayLightSystem" },
                                        new Noun { Name = "TouchDownLiftOffLightSystem" },
                                        new Noun { Name = "RunwayDirectionLightSystem" },
                                        new Noun { Name = "VisualGlideSlopeIndicator" },
                                        new Noun { Name = "TaxiHoldingPositionLightSystem" },
                                        new Noun { Name = "RunwayProtectAreaLightSystem" },
                                        new Noun { Name = "GuidanceLineLightSystem" },

                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        public static BRule Get_D4A5E()
        {
            return new BRule
            {
                Type = RuleType.Each,
                Noun = new Noun { Name = "AirportHeliport" },
                Operation = new OperationGroup
                {
                    Type = LogicType.And,
                    Items =
                    {
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.Assigned,
                            Noun = "ARP.ElevatedPoint.verticalAccuracy"
                        },
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.Assigned,
                            Noun = "ARP.ElevatedPoint.geoidUndulation"
                        },
                        new Operation
                        {
                            Type = OperationType.Equal,
                            Noun = "ARP.ElevatedPoint.verticalAccuracy.uom",
                            Value = "$prop$ARP.ElevatedPoint.geoidUndulation.uom"
                        }
                    }
                }
            };
        }

        public static BRule Get_26D18()
        {
            return new BRule
            {
                Type = RuleType.Obligatory,
                Noun = new Noun { Name = "Airspace" },
                Operation = new OperationGroup
                {
                    Type = LogicType.And,
                    Items =
                    {
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.MoreThanOne,
                            Noun = "geometryComponent"
                        },
                        new Operation
                        {
                            Type = OperationType.ExactlyOne,
                            Noun = "geometryComponent.AirspaceGeometryComponent.operation",
                            InnerOper = new InnerOperation
                            {
                                Type = InnerOperationType.Equal,
                                Value = "'BASE'"
                            }
                        }
                    }
                }
            };
        }

        public static BRule Get_19F870()
        {
            return new BRule
            {
                Type = RuleType.Prohibited,
                Noun = new Noun { Name = "Airspace" },
                Operation = new OperationGroup
                {
                    Type = LogicType.And,
                    Items =
                    {
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.MoreThanOne,
                            Noun = "geometryComponent"
                        },
                        new Operation
                        {
                            IsNot = true,
                            Type = OperationType.Assigned,
                            Noun = "geometryComponent.AirspaceGeometryComponent.operation"
                        }
                    }
                }
            };
        }

        public static BRule Get_1A8530()
        {
            return new BRule
            {
                Type = RuleType.Obligatory,
                Noun = new Noun { Name = "DistanceIndication" },
                Operation = new OperationGroup
                {
                    Type = LogicType.And,
                    Items =
                    {
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.Assigned,
                            Noun = "pointChoice.SignificantPoint.navaidSystem"
                        },
                        new Operation
                        {
                            Type = OperationType.ResolvedInto,
                            Noun = "pointChoice.SignificantPoint.navaidSystem",
                            Value = "exactly one",
                            Association = new OperAssociation
                            {
                                Name = "uses",
                                Noun = new Noun { Name = "Navaid" }
                            }
                        }
                    }
                }
            };
        }

        public static BRule Get_D4A72()
        {
            return new BRule
            {
                Type = RuleType.Each,
                Noun = new Noun { Name = "RunwayDirection" },
                Operation = new OperationGroup
                {
                    Type = LogicType.And,
                    Items =
                    {
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.Assigned,
                            Noun = "elevationTDZ"
                        },
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.Assigned,
                            Noun = "elevationTDZAccuracy"
                        },
                        new Operation
                        {
                            Type = OperationType.Equal,
                            Noun = "elevationTDZ.uom",
                            Value = "$prop$elevationTDZAccuracy.uom"
                        }
                    }
                }
            };
        }

        public static BRule Get_1A5285()
        {
            return new BRule
            {
                Type = RuleType.Obligatory,
                Noun = new Noun
                {
                    Name = "PropertiesWithSchedule",
                    Childs =
                    {
                        new Noun { Name = "AirportHeliportAvailability" }
                    }
                },
                Operation = new OperationGroup
                {
                    Type = LogicType.And,
                    Items =
                    {
                        new Operation
                        {
                            IsWith = true,
                            Type = OperationType.Assigned,
                            Noun = "specialDateAuthority"
                        },
                        new Operation
                        {
                            Type = OperationType.ResolvedInto,
                            Noun = "specialDateAuthority",
                            Value = "exactly one",
                            Association = new OperAssociation
                            {
                                Noun = new Noun { Name = "OrganisationAuthority" },
                                Name = "appliesSpecialDatesOf"
                            }
                        }
                    }
                }
            };
        }

    }
}

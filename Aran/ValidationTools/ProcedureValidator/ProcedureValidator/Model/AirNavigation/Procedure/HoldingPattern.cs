using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace PVT.Model
{
    public class HoldingPattern : Feature
    {
        private readonly Aran.Aim.Features.HoldingPattern _pattern;
        private Aran.Aim.Features.HoldingAssessment _assessment;
        public Unique<ValDistanceVertical> UpperLimit { get; }
        public string UpperLimitString => UpperLimit != null ? $"{UpperLimit.Value}" : null;
        public String UpperLimitUom => UpperLimit?.Value.Uom.ToString() ?? "";
        public Unique<CodeVerticalReference?> UpperLimitReference { get; }
        public Unique<ValDistanceVertical> LowerLimit { get; }
        public string LowerLimitString => LowerLimit != null ? $"{LowerLimit.Value}" : null;
        public Unique<CodeVerticalReference?> LowerLimitReference { get; }
        public Unique<ValSpeed> SpeedLimit { get; }
        public string SpeedLimitString => SpeedLimit != null ? $"{SpeedLimit.Value}" : null;
        public ValDistance LegLengthToward { get; }
        public ValDistance LegLengthAway { get; }
        public Unique<SegmentPoint> HoldingPoint { get; }
        public String HoldingPointName => HoldingPoint?.Value?.Name;
        public List<ObstacleAssessmentArea> AssessmentAreas { get; }
        public string Type { get; }
        public double? InboundCourse { get; }
        public double? OutboundCourse { get; }
        public CodeCourse? OutboundCourseType { get; }
        public CodeDirectionTurn? TurnDirection { get; }
        public string TextInstructioType { get; }
        public Aran.Geometries.MultiLineString Extent => _pattern.Extent?.Geo;

        public HoldingPattern(Aran.Aim.Features.HoldingPattern pattern, HoldingAssessment assessment) : this(pattern)
        {
            _pattern = pattern;
            _assessment = assessment;
            Type = pattern.Type?.ToString();
            InboundCourse = pattern.InboundCourse;
            OutboundCourse = pattern.OutboundCourse;
            OutboundCourseType = pattern.OutboundCourseType;
            TurnDirection = pattern.TurnDirection;
            TextInstructioType = pattern.Instruction;

            UpperLimit = new Unique<ValDistanceVertical>() { FirstValue = assessment.UpperLimit, SecondValue = pattern.UpperLimit };
            UpperLimitReference = new Unique<CodeVerticalReference?>() { FirstValue = assessment.UpperLimitReference, SecondValue = pattern.UpperLimitReference };
            LowerLimit = new Unique<ValDistanceVertical>() { FirstValue = assessment.LowerLimit, SecondValue = pattern.LowerLimit };
            LowerLimitReference = new Unique<CodeVerticalReference?>() { FirstValue = assessment.LowerLimitReference, SecondValue = pattern.LowerLimitReference };
            SpeedLimit = new Unique<ValSpeed>() { FirstValue = assessment.SpeedLimit, SecondValue = pattern.SpeedLimit };
            LegLengthToward = assessment.LegLengthToward;
            LegLengthAway = assessment.LegLengthAway;

            HoldingPoint = new Unique<SegmentPoint>();
            if (assessment.HoldingPoint != null)
                HoldingPoint.FirstValue = new SegmentPoint(assessment.HoldingPoint);

            if (pattern.HoldingPoint != null)
                HoldingPoint.SecondValue = new SegmentPoint(pattern.HoldingPoint);

            var x = HoldingPoint.IsUnique;
            AssessmentAreas = new List<ObstacleAssessmentArea>();
            foreach (var t in assessment.ObstacleAssessment.Where(t => t.Surface?.Geo != null))
            {
                AssessmentAreas.Add(new ObstacleAssessmentArea(t));
            }
        }

        public HoldingPattern(Aran.Aim.Features.Feature feature) : base(feature)
        {
            Reports = Engine.Environment.Current.DbProvider.GetFeatureReport(feature.Identifier).Select(x => new FeatureReport(x)).ToList<FeatureReport>();
        }



        private static readonly List<HoldingPattern> HoldingPatterns = new List<HoldingPattern>();
        private static List<HoldingAssessment> _holdingAssessments;
        private static List<Aran.Aim.Features.HoldingPattern> _holdingPatterns;

        public static List<HoldingPattern> Load()
        {
            if (_holdingAssessments == null)
                return HoldingPatterns;

            bool error = false;
            foreach (var assessment in _holdingAssessments)
            {
                if (assessment.AssessedHoldingPattern?.Identifier != null)
                {
                    var selectedpatterns = _holdingPatterns.Where(p => p.Identifier == assessment.AssessedHoldingPattern?.Identifier).ToList();
                    foreach (var pattern in selectedpatterns)
                    {
                        
                        try
                        {
                            HoldingPatterns.Add(new HoldingPattern(pattern, assessment));
                        }
                        catch (Exception ex)
                        {
                            error = true;
                            Engine.Environment.Current.Logger.Error(ex, $"Error on loading {nameof(HoldingPattern)}. pattern: {pattern.Identifier}, assessment: {assessment.Identifier}");
                        }
                    }

                }

                   
            }

            if (error)
                MessageBox.Show("There are some error on loading holding patterns. Please, see logs.");
            return HoldingPatterns;
        }

        public static void Fetch()
        {
            _holdingAssessments = Engine.Environment.Current.DbProvider.GetHoldingAssessments();
            _holdingPatterns = Engine.Environment.Current.DbProvider.GetHoldingPatterns();
        }
    }

    public class Unique<T>
    {

        public T FirstValue { get; set; }
        public T SecondValue { get; set; }

        public T Value => IsNull(FirstValue) ? SecondValue : FirstValue;

        public string Error => !IsUnique ? $"First value {FirstValue} & second values {SecondValue} are not equal." : null;

        public bool IsUnique
        {
            get
            {
                if (IsNull(FirstValue))
                    return IsNull(SecondValue);
                return FirstValue.Equals(SecondValue);
            }
        }

        public bool Different => !IsUnique;
        private bool IsNull(T obj) => EqualityComparer<T>.Default.Equals(obj, default(T));
    }

}

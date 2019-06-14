
using GalaSoft.MvvmLight;
using PVT.Model.Drawing;
using PVT.Utils;

namespace PVT.Settings
{
    public class Options
    {
        public Options()
        {
            CommonViewColorOptions = new StyleOption();

            CommonViewColorOptions.PointStyle = new Styles(PointStyle.Circle);
            CommonViewColorOptions.PointStyle.PointStyle.Color = new Color(System.Drawing.Color.Black.GetColor());

            CommonViewColorOptions.NominalTrackStyle = new Styles(LineStyle.Solid);
            CommonViewColorOptions.NominalTrackStyle.LineStyle.Color = new Color(System.Drawing.Color.Green.GetColor());

            CommonViewColorOptions.FixToleranceAreaStyle = new Styles(LineStyle.Solid);
            CommonViewColorOptions.FixToleranceAreaStyle.LineStyle.Color = new Color(System.Drawing.Color.Red.GetColor());

            CommonViewColorOptions.PrimaryProtectedAreaStyle = new AreaStyles(LineStyle.Solid);
            CommonViewColorOptions.PrimaryProtectedAreaStyle.ObstacleStyle = new Styles(PointStyle.Circle, LineStyle.Solid);
            CommonViewColorOptions.PrimaryProtectedAreaStyle.ObstacleStyle.PointStyle.Color = new Color(System.Drawing.Color.Black.GetColor());
            CommonViewColorOptions.PrimaryProtectedAreaStyle.ObstacleStyle.LineStyle.Color = new Color(System.Drawing.Color.Black.GetColor());
            CommonViewColorOptions.PrimaryProtectedAreaStyle.ObstacleStyle.Enabled = false;
            CommonViewColorOptions.PrimaryProtectedAreaStyle.LineStyle.Color = new Color(System.Drawing.Color.Blue.GetColor());

            CommonViewColorOptions.SecondaryProtectedAreaStyle = new AreaStyles(LineStyle.Solid);
            CommonViewColorOptions.SecondaryProtectedAreaStyle.ObstacleStyle = new Styles(PointStyle.Circle, LineStyle.Solid);
            CommonViewColorOptions.SecondaryProtectedAreaStyle.ObstacleStyle.PointStyle.Color = new Color(System.Drawing.Color.Black.GetColor());
            CommonViewColorOptions.SecondaryProtectedAreaStyle.ObstacleStyle.LineStyle.Color = new Color(System.Drawing.Color.Black.GetColor());
            CommonViewColorOptions.SecondaryProtectedAreaStyle.ObstacleStyle.Enabled = false;
            CommonViewColorOptions.SecondaryProtectedAreaStyle.LineStyle.Color = new Color(System.Drawing.Color.BlueViolet.GetColor());


            DetailedViewColorOptions = new StyleOption();

            DetailedViewColorOptions.PointStyle = new Styles(PointStyle.Circle);
            DetailedViewColorOptions.PointStyle.PointStyle.Color = new Color(System.Drawing.Color.Yellow.GetColor());

            DetailedViewColorOptions.NominalTrackStyle = new Styles(LineStyle.Solid);
            DetailedViewColorOptions.NominalTrackStyle.LineStyle.Color = new Color(System.Drawing.Color.OrangeRed.GetColor());

            DetailedViewColorOptions.FixToleranceAreaStyle = new Styles(LineStyle.Solid);
            DetailedViewColorOptions.FixToleranceAreaStyle.LineStyle.Color = new Color(System.Drawing.Color.YellowGreen.GetColor());

            DetailedViewColorOptions.PrimaryProtectedAreaStyle = new AreaStyles(LineStyle.Solid);
            DetailedViewColorOptions.PrimaryProtectedAreaStyle.ObstacleStyle = new Styles(PointStyle.Circle, LineStyle.Solid);
            DetailedViewColorOptions.PrimaryProtectedAreaStyle.ObstacleStyle.PointStyle.Color = new Color(System.Drawing.Color.Black.GetColor());
            DetailedViewColorOptions.PrimaryProtectedAreaStyle.ObstacleStyle.LineStyle.Color = new Color(System.Drawing.Color.Black.GetColor());
            DetailedViewColorOptions.PrimaryProtectedAreaStyle.ObstacleStyle.Enabled = false;
            DetailedViewColorOptions.PrimaryProtectedAreaStyle.LineStyle.Color = new Color(System.Drawing.Color.Violet.GetColor());

            DetailedViewColorOptions.SecondaryProtectedAreaStyle = new AreaStyles(LineStyle.Solid);
            DetailedViewColorOptions.SecondaryProtectedAreaStyle.ObstacleStyle = new Styles(PointStyle.Circle, LineStyle.Solid);
            DetailedViewColorOptions.SecondaryProtectedAreaStyle.ObstacleStyle.PointStyle.Color = new Color(System.Drawing.Color.Black.GetColor());
            DetailedViewColorOptions.SecondaryProtectedAreaStyle.ObstacleStyle.LineStyle.Color = new Color(System.Drawing.Color.Black.GetColor());
            DetailedViewColorOptions.SecondaryProtectedAreaStyle.ObstacleStyle.Enabled = false;
            DetailedViewColorOptions.SecondaryProtectedAreaStyle.LineStyle.Color = new Color(System.Drawing.Color.DarkViolet.GetColor());
        }

        private static Options _current;
        public static Options Current
        {
            get {
                if(_current == null)
                {
                    _current = new Options();
                }
                return _current;
            }
        }

        public StyleOption CommonViewColorOptions { get; }
        public StyleOption DetailedViewColorOptions { get; }

    }

    public class StyleOption : ObservableObject
    {

        private Styles _pointStyle;
        public Styles PointStyle
        {
            get
            {
                return _pointStyle;
            }
            set
            {
                Set(() => PointStyle, ref _pointStyle, value);
            }
        }



        private Styles _nominalTrackStyle;
        public Styles NominalTrackStyle
        {
            get
            {
                return _nominalTrackStyle;
            }
            set
            {
                Set(() => NominalTrackStyle, ref _nominalTrackStyle, value);
            }
        }

        private Styles _fixToleranceAreaStyle;
        public Styles FixToleranceAreaStyle
        {
            get
            {
                return _fixToleranceAreaStyle;
            }
            set
            {
                Set(() => FixToleranceAreaStyle, ref _fixToleranceAreaStyle, value);
            }
        }


        private AreaStyles _primaryProtectedAreaStyle;
        public AreaStyles PrimaryProtectedAreaStyle
        {
            get
            {
                return _primaryProtectedAreaStyle;
            }
            set
            {
                Set(() => PrimaryProtectedAreaStyle, ref _primaryProtectedAreaStyle, value);
            }
        }


        private AreaStyles _secondaryProtectedAreaStyle;
        public AreaStyles SecondaryProtectedAreaStyle
        {
            get
            {
                return _secondaryProtectedAreaStyle;
            }
            set
            {
                Set(() => SecondaryProtectedAreaStyle, ref _secondaryProtectedAreaStyle, value);
            }
        }

    }


}

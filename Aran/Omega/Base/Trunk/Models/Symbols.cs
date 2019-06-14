using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment.Symbols;
using Aran.Panda.Common;

namespace Omega.Models
{
    public class Symbols
    {
        static Symbols()
        {
            //Innerhorizontal surface symbols
            int innerHorColor = Aran.Panda.Common.ARANFunctions.RGB(3, 93, 138);
            InnerHorizontalDefaultSymbol = new FillSymbol();
            InnerHorizontalDefaultSymbol.Color = innerHorColor ;
            InnerHorizontalDefaultSymbol.Outline = new LineSymbol(eLineStyle.slsDash,innerHorColor, 2);
            InnerHorizontalDefaultSymbol.Size = 2;
            InnerHorizontalDefaultSymbol.Style = eFillStyle.sfsNull;

            InnerHorizontalSelectedSymbol = new FillSymbol();
            InnerHorizontalSelectedSymbol.Color = innerHorColor;
            InnerHorizontalSelectedSymbol.Outline = new LineSymbol(eLineStyle.slsNull, innerHorColor, 3);
            InnerHorizontalSelectedSymbol.Size = 3;
            InnerHorizontalSelectedSymbol.Style = eFillStyle.sfsNull;

            //end

            //Conical surface symbols
            int conicalColor = ARANFunctions.RGB(85, 91, 94);
            ConicalDefaultSymbol = new FillSymbol();
            ConicalDefaultSymbol.Color = conicalColor;
            ConicalDefaultSymbol.Outline = new LineSymbol(eLineStyle.slsDashDot, conicalColor, 1);
            ConicalDefaultSymbol.Size = 1;
            ConicalDefaultSymbol.Style = eFillStyle.sfsNull;

            ConicalSelectedSymbol = new FillSymbol();
            ConicalSelectedSymbol.Color = conicalColor;
            ConicalSelectedSymbol.Outline = new LineSymbol(eLineStyle.slsDashDot, conicalColor, 2);
            ConicalSelectedSymbol.Size = 2;
            ConicalSelectedSymbol.Style = eFillStyle.sfsCross;

            //End

            //Outer horizontal symbols
            int outerHorizontalColor = ARANFunctions.RGB(217, 212, 217);
            OuterHorizontalDefaultSymbol = new FillSymbol();
            OuterHorizontalDefaultSymbol.Color = outerHorizontalColor;
            OuterHorizontalDefaultSymbol.Outline = new LineSymbol(eLineStyle.slsNull, outerHorizontalColor, 1);
            OuterHorizontalDefaultSymbol.Size = 1;
            OuterHorizontalDefaultSymbol.Style = eFillStyle.sfsNull;

            OuterHorizontalSelectedSymbol = new FillSymbol();
            OuterHorizontalSelectedSymbol.Color = outerHorizontalColor;
            OuterHorizontalSelectedSymbol.Outline = new LineSymbol(eLineStyle.slsNull, outerHorizontalColor, 3);
            OuterHorizontalSelectedSymbol.Size = 3;
            OuterHorizontalSelectedSymbol.Style = eFillStyle.sfsBackwardDiagonal;
            
            //End

            //Strip symbols
            int stripColor = ARANFunctions.RGB(90, 212, 30);
            StripDefaultSymbol = new FillSymbol();
            StripDefaultSymbol.Color = stripColor;
            StripDefaultSymbol.Outline = new LineSymbol(eLineStyle.slsNull, stripColor, 1);
            StripDefaultSymbol.Size = 1;
            StripDefaultSymbol.Style = eFillStyle.sfsNull;

            StripSelectedSymbol = new FillSymbol();
            StripSelectedSymbol.Color = stripColor;
            StripSelectedSymbol.Outline = new LineSymbol(eLineStyle.slsDot, stripColor, 2);
            StripSelectedSymbol.Size = 2;
            StripSelectedSymbol.Style = eFillStyle.sfsHorizontal;
            
            //end


            //InnerApproach symbols
            int innerApproachColor = ARANFunctions.RGB(184, 165, 168);
            InnerApproachDefaultSymbol = new FillSymbol();
            InnerApproachDefaultSymbol.Color = innerApproachColor;
            InnerApproachDefaultSymbol.Outline = new LineSymbol(eLineStyle.slsInsideFrame, innerApproachColor, 1);
            InnerApproachDefaultSymbol.Size = 1;
            InnerApproachDefaultSymbol.Style = eFillStyle.sfsNull;

            InnerApproachSelectedSymbol = new FillSymbol();
            InnerApproachSelectedSymbol.Color = innerApproachColor;
            InnerApproachSelectedSymbol.Outline = new LineSymbol(eLineStyle.slsNull, innerApproachColor, 2);
            InnerApproachSelectedSymbol.Size = 2;
            InnerApproachSelectedSymbol.Style = eFillStyle.sfsVertical;

            //end
            
            //Transitional symbols
            int transitionalColor = ARANFunctions.RGB(110, 97, 13);
            TransitionalDefaultSymbol = new FillSymbol();
            TransitionalDefaultSymbol.Color = transitionalColor;
            TransitionalDefaultSymbol.Outline = new LineSymbol(eLineStyle.slsInsideFrame, transitionalColor, 1);
            TransitionalDefaultSymbol.Size = 1;
            TransitionalDefaultSymbol.Style = eFillStyle.sfsNull;

            TransitionalSelectedSymbol = new FillSymbol();
            TransitionalSelectedSymbol.Color = transitionalColor;
            TransitionalSelectedSymbol.Outline = new LineSymbol(eLineStyle.slsNull, transitionalColor, 2);
            TransitionalSelectedSymbol.Size = 2;
            TransitionalSelectedSymbol.Style = eFillStyle.sfsVertical;    
            
            //end


            //Approach symbols
            int approachColor = ARANFunctions.RGB(91, 67, 186);
            ApproachDefaultSymbol = new FillSymbol();
            ApproachDefaultSymbol.Color = approachColor;
            ApproachDefaultSymbol.Outline = new LineSymbol(eLineStyle.slsInsideFrame, approachColor, 1);
            ApproachDefaultSymbol.Size = 1;
            ApproachDefaultSymbol.Style = eFillStyle.sfsNull;

            ApproachSelectedSymbol = new FillSymbol();
            ApproachSelectedSymbol.Color = approachColor;
            ApproachSelectedSymbol.Outline = new LineSymbol(eLineStyle.slsNull, approachColor, 2);
            ApproachSelectedSymbol.Size = 2;
            ApproachSelectedSymbol.Style = eFillStyle.sfsHorizontal;

            //end


            //BalkedLanding symbols
            int balkedColor = ARANFunctions.RGB(109, 122, 110);
            BalkedLandingDefaultSymbol = new FillSymbol();
            BalkedLandingDefaultSymbol.Color = balkedColor;
            BalkedLandingDefaultSymbol.Outline = new LineSymbol(eLineStyle.slsInsideFrame, balkedColor, 2);
            BalkedLandingDefaultSymbol.Size = 2;
            BalkedLandingDefaultSymbol.Style = eFillStyle.sfsNull;

            BalkedLandingSelectedSymbol = new FillSymbol();
            BalkedLandingSelectedSymbol.Color = balkedColor;
            BalkedLandingSelectedSymbol.Outline = new LineSymbol(eLineStyle.slsNull, balkedColor, 3);
            BalkedLandingSelectedSymbol.Size = 3;
            BalkedLandingSelectedSymbol.Style = eFillStyle.sfsHorizontal;

            //end
        }

        public static FillSymbol InnerHorizontalDefaultSymbol { get; set; }
        public  static FillSymbol InnerHorizontalSelectedSymbol { get; set; }
        
        public static FillSymbol OuterHorizontalDefaultSymbol  { get; set; }
        public static FillSymbol OuterHorizontalSelectedSymbol { get; set; }

        public static FillSymbol ConicalDefaultSymbol { get; set; }
        public static FillSymbol ConicalSelectedSymbol { get; set; }

        public static FillSymbol StripDefaultSymbol { get; set; }
        public static FillSymbol StripSelectedSymbol { get; set; }

        public static FillSymbol InnerApproachDefaultSymbol { get; set; }
        public static FillSymbol InnerApproachSelectedSymbol { get; set; }

        public static FillSymbol TransitionalDefaultSymbol { get; set; }
        public static FillSymbol TransitionalSelectedSymbol { get; set; }

        public static FillSymbol ApproachDefaultSymbol { get; set; }
        public static FillSymbol ApproachSelectedSymbol { get; set; }

        public static FillSymbol BalkedLandingDefaultSymbol { get; set; }
        public static FillSymbol BalkedLandingSelectedSymbol { get; set; }



    }
}

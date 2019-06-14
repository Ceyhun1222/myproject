using Aerodrome.Enums;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    public class SurfaceCompositionConverter : IConverter<CodeSurfaceCompositionType, SurfaceComposition>
    {
        public SurfaceComposition Convert(CodeSurfaceCompositionType source)
        {
            switch (source)
            {
                case CodeSurfaceCompositionType.CONC:                   
                        return SurfaceComposition.Concrete_Grooved;
                case CodeSurfaceCompositionType.ASPH:
                    return SurfaceComposition.Asphalt_Grooved;
                case CodeSurfaceCompositionType.SAND:
                    return SurfaceComposition.Desert_or_Sand_or_Dirt;
                case CodeSurfaceCompositionType.EARTH:
                    return SurfaceComposition.Bare_Earth;
                case CodeSurfaceCompositionType.SNOW:
                    return SurfaceComposition.Snow_or_Ice;
                case CodeSurfaceCompositionType.ICE:
                    return SurfaceComposition.Snow_or_Ice;
                case CodeSurfaceCompositionType.WATER:
                    return SurfaceComposition.Water;
                case CodeSurfaceCompositionType.GRASS:
                    return SurfaceComposition.Grass_or_Turf;
                case CodeSurfaceCompositionType.GRAVEL:
                    return SurfaceComposition.Gravel_or_Cinders;
                case CodeSurfaceCompositionType.PIERCED_STEEL:
                    return SurfaceComposition.Pierced_Steel_Planks;
                case CodeSurfaceCompositionType.BITUM:
                    return SurfaceComposition.Bitumen;
                case CodeSurfaceCompositionType.BRICK:
                    return SurfaceComposition.Brick;
                case CodeSurfaceCompositionType.MACADAM:
                    return SurfaceComposition.Macadam;
                case CodeSurfaceCompositionType.STONE:
                    return SurfaceComposition.Stone;
                case CodeSurfaceCompositionType.CORAL:
                    return SurfaceComposition.Coral;
                case CodeSurfaceCompositionType.CLAY:
                    return SurfaceComposition.Clay;
                case CodeSurfaceCompositionType.LATERITE:
                    return SurfaceComposition.Laterite;
                case CodeSurfaceCompositionType.MATS:
                    return SurfaceComposition.Landing_Mats;
                case CodeSurfaceCompositionType.MEMBRANE:
                    return SurfaceComposition.Membrane;
                case CodeSurfaceCompositionType.WOOD:
                    return SurfaceComposition.Wood;

                default:
                    return SurfaceComposition.Concrete_Grooved;
            }
        }
    }
}
   
 
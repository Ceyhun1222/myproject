﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Enums;

namespace Aran.Omega
{
    public class RwyCenterlineClass
    {
        public RwyCenterlineClass()
        {

        }

        public Aran.Geometries.Point PtGeo { get; set; }
        public Aran.Geometries.Point  PtPrj { get; set; }
        public double Elevation { get; set; }
        public CodeRunwayPointRole? Role { get; set; }
        public string Id { get; set; }
        public double DistFromThreshold { get; set; }
    }
}
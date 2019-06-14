using System;
using System.Collections.Generic;
using System.Windows.Media;
using Aran.Temporality.CommonUtil.Extender;

namespace TOSSM.ViewModel.Control.DataTocView
{
    public class DataLayer : DataLayerViewModel
    {
        private Color _fillColor = Colors.White.GetRandomShade();
        public Color FillColor
        {
            get
            {
                return _fillColor;
            }
            set { _fillColor = value; }
        }

        private Color _outLineColor = Colors.White.GetRandomShade();
        public Color OutLineColor
        {
            get { return _outLineColor; }
            set { _outLineColor = value; }
        }

       
        private List<ShapeInfo> _points;
        public List<ShapeInfo> Points
        {
            get { return _points??(_points=new List<ShapeInfo>()); }
        }

        private List<ShapeInfo> _lines;
        public List<ShapeInfo> Lines
        {
            get { return _lines??(_lines=new List<ShapeInfo>()); }
        }

        private List<ShapeInfo> _polygons;
        public List<ShapeInfo> Polygons
        {
            get { return _polygons??(_polygons=new List<ShapeInfo>()); }
        }

        private List<ShapeCluster> _clusters;
        public List<ShapeCluster> Clusters
        {
            get { return _clusters ?? (_clusters=new List<ShapeCluster>()); }
            set { _clusters = value; }
        }

        public DateTime ActualDate { get; set; }

        public override void OnClusteringChangedAction()
        {
            if (IsClustering) return;
            Clusters.Clear();
            foreach(var shape in Points)
            {
                shape.IsConsumed = false;
            }
            foreach (var shape in Lines)
            {
                shape.IsConsumed = false;
            }
            foreach (var shape in Polygons)
            {
                shape.IsConsumed = false;
            }

            base.OnClusteringChangedAction();
        }
    }
}
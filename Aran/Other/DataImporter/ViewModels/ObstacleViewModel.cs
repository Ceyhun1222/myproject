using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.AranEnvironment;
using Aran.AranEnvironment.Symbols;
using Aran.PANDA.Common;
using DataImporter.Enums;
using DataImporter.Factories;
using DataImporter.Factories.ObstacleLoader;
using DataImporter.Models;
using DataImporter.Repository;
using MahApps.Metro.Controls.Dialogs;

namespace DataImporter.ViewModels
{
    public class ObstacleViewModel:ViewModel
    {
        private readonly IObstacleDbSaver _obstacleDbSaver;

        private Obstacle _obstacle;
        private int _obsHandle;
        private readonly IAranGraphics _graphics;
        private readonly IObstacleLoaderFactory _obstacleLoaderFactory;
        private IDialogCoordinator _dialogCoordinator;
        private ILogger _logger;

        public ObstacleViewModel(IObstacleDbSaver obstacleDbSaver,
            IAranGraphics graphics,
            IObstacleLoaderFactory obstacleLoaderFactory,
            ICommonObject commonObject)
        {
            _obstacleDbSaver = obstacleDbSaver;
            _graphics = graphics;
            _obstacleLoaderFactory = obstacleLoaderFactory;
            _logger = commonObject.Logger;
            _dialogCoordinator = commonObject.DialogCoordinator;

            Obstacles = new ObservableCollection<Obstacle>();
        }

        public ObservableCollection<Obstacle> Obstacles { get; set; }

        public Obstacle SelectedObstacle
        {
            get => _obstacle;
            set
            {
                _obstacle = value;
                Draw();
                NotifyPropertyChanged(nameof(SelectedObstacle));
            }
        }

        private void Draw()
        {
            _graphics.SafeDeleteGraphic(_obsHandle);
            if (_obstacle?.Geo != null)
            {
                var obstacleGeo = _obstacle.GeoPrj;
                switch (_obstacle.GeoType)
                {
                    case ObstacleGeoType.Point:
                        _obsHandle =_graphics.DrawPointWithText(obstacleGeo as Aran.Geometries.Point, _obstacle.Name, 2324);
                        break;
                    case ObstacleGeoType.Polygon:
                        _obsHandle =_graphics.DrawMultiPolygon(obstacleGeo as Aran.Geometries.MultiPolygon, eFillStyle.sfsCross);
                        break;
                    case ObstacleGeoType.PolyLine:
                        _obsHandle = _graphics.DrawMultiLineString(obstacleGeo as Aran.Geometries.MultiLineString,2,1323);
                        break;
                    case ObstacleGeoType.Circle:
                        _obsHandle = _graphics.DrawMultiPolygon(obstacleGeo as Aran.Geometries.MultiPolygon);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override void Save()
        {
            _obstacleDbSaver?.Save(Obstacles.ToList());
        }

        public override void Load(string fileName)
        {
            //Obstacles.Clear();
            var obstacleLoader = _obstacleLoaderFactory.CreateFileLoader(ObstacleFileFormatType.ExcelStandard, fileName);
            obstacleLoader?.Obstacles.ForEach(obs=>Obstacles.Add(obs));
        }

        public override IFeatType FeatType => new FeatTypeClass { FeatType = FeatureType.Obstacle, Header = "Vertical Structure" };
    }
}

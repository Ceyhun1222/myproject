using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aran.PANDA.Common;
using Aran.Panda.RadarMA.Models;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.ViewModels
{
    public class MergeSectorViewModel:NotifyableBase
    {
        private Sector _selectedSector;
        private Sector _mergeSector;
        private double _sectorElevation;
        private double _selectedMocValue;
        private double _selectedBufferValue;
        private UnitConverter _unitConverter;
        private int _selectedSectorHandle;
        private int _selectedMereableSectorHandle;
        private ESRI.ArcGIS.Display.ISymbol _mergeSectorSymbol;
        private List<State> _stateList;
        private ObservableCollection<Sector> _mainSectorList;
        private ISymbol _sectorSymbol; 

        public MergeSectorViewModel(ObservableCollection<Sector> sectorList,List<State> stateList,IEnumerable<double> mocList,IEnumerable<double> bufferValueList )
        {

            _mergeSectorSymbol = CreateMergeSectorSymbol();

            _unitConverter = GlobalParams.UnitConverter;

            MOCList = new List<double>(mocList);
            SelectedMOCValue = MOCList[0];

            BufferValueList =new List<double>(bufferValueList);
            SelectedBufferValue = BufferValueList[0];

            MergeableSectorList = new ObservableCollection<Sector>();
            SectorList =new ObservableCollection<Sector>(sectorList);
            SelectedSector = SectorList[0];

            _mainSectorList = sectorList;

            _stateList = stateList;

            _sectorSymbol = CreateSectorSymbol();

            ApplyCommand = new RelayCommand(Apply_OnClick);
        }

        public ObservableCollection<Sector> SectorList { get; set; }
        public ObservableCollection<Sector> MergeableSectorList { get; set; }

        public List<double> BufferValueList { get; set; }
        public List<double> MOCList { get; set; }

        public RelayCommand ApplyCommand { get; set; }

        public Sector SelectedSector
        {
            get { return _selectedSector; }
            set
            {
                _selectedSector?.Clear();

                _selectedSector = value;

                MergeableSectorList.Clear();
                if (_selectedSector != null)
                {
                    foreach (var sector in SectorList)
                    {
                        if (sector.Number == _selectedSector.Number)
                            continue;

                        if (!GeomOperators.Disjoint(_selectedSector.Geo, sector.Geo))
                            MergeableSectorList.Add(sector);
                    }

                    _selectedSector.DrawWithOtherSymbol(_mergeSectorSymbol);
                }
                if (MergeableSectorList.Count > 0)
                    SelectedMergeableSector = MergeableSectorList[0];

                NotifyPropertyChanged("");
            }
        }

        public Sector SelectedMergeableSector
        {
            get { return _mergeSector; }
            set
            {
                _mergeSector?.Clear();

                _mergeSector = value;
                if (_mergeSector != null)
                {
                    _mergeSector.DrawWithOtherSymbol(_mergeSectorSymbol);

                    SectorElevation = SelectedSectorElevation > MergeSectorElevation
                        ? SelectedSectorElevation
                        : MergeSectorElevation;

                    if (Math.Abs(SelectedSectorBufferValue - MergeSectorBufferValue) > 1)
                    {
                        // SectorElevation = CalculateElevation();
                    }

                    int index = 0;

                    var tmpBufferValue = SelectedSectorBufferValue > MergeSectorBufferValue
                        ? SelectedSectorBufferValue
                        : MergeSectorBufferValue;

                    foreach (var bufferValue in BufferValueList)
                    {
                        if (Math.Abs(bufferValue - tmpBufferValue) < 0.1)
                            SelectedBufferValue = BufferValueList[index];
                    }


                    var tmpMocValue = SelectedSectorMOC > MergeSectorMOC ? SelectedSectorMOC : MergeSectorMOC;


                    foreach (var moc in MOCList)
                    {
                        if (Math.Abs(tmpMocValue - moc) < 0.1)
                            SelectedMOCValue = MOCList[index];
                    }
                }
                NotifyPropertyChanged("");
            }
        }

        public double SelectedSectorElevation => _selectedSector?.Height ?? 0;

        public double SelectedSectorMOC => _selectedSector?.MOC ?? 0;

        public double SelectedSectorBufferValue => _selectedSector?.BufferValue ?? 0;

        public double MergeSectorElevation => _mergeSector?.Height ?? 0;

        public double MergeSectorMOC => _mergeSector?.MOC ?? 0;

        public double MergeSectorBufferValue => _mergeSector?.BufferValue ?? 0;

        public double SectorElevation
        {
            get => _sectorElevation;
            set
            {
                _sectorElevation = value;
                NotifyPropertyChanged(nameof(SectorElevation));
            }
        }

        public double SelectedMOCValue
        {
            get => _selectedMocValue;
            set
            {
                _selectedMocValue = value;
                NotifyPropertyChanged(nameof(SelectedMOCValue));
            }
        }

        public double SelectedBufferValue
        {
            get => _selectedBufferValue;
            set
            {
                _selectedBufferValue = value;
                NotifyPropertyChanged(nameof(SelectedBufferValue));
            }
        }

        public string DistanceUnit => _unitConverter.DistanceUnit;

        public string HeightUnit => _unitConverter.HeightUnit;

        public void Clear()
        {
            _selectedSector?.Clear();
            _mergeSector?.Clear();
        }

        private void Apply_OnClick(object obj)
        {
            if (_selectedSector != null && _mergeSector != null)
            {
                var mergeSectorGeo = GeomOperators.UnionPolygon(_selectedSector.Geo, _mergeSector.Geo);

                foreach (var tmpState in _stateList)
                {
                    if (tmpState.SectorNumber == _selectedSector.Number || tmpState.SectorNumber == _mergeSector.Number)
                        tmpState.Clear();
                }

                var obstacleReports = new List<ObstacleReport>(_selectedSector.Reports);
                obstacleReports.AddRange(_mergeSector.Reports);

                var state = CreateState(GlobalParams.RadarSymbol.SectorSymbol, mergeSectorGeo, OperationType.JoinSectors,obstacleReports);
                

                state.JoinSectorList.Add(_selectedSector);
                state.JoinSectorList.Add(_mergeSector);

               RemoveSectorAfterMerge();

                state.SectorNumber = _mainSectorList.Count;
                _stateList.Add(state);

                var unionGeomety = mergeSectorGeo;
                if (SectorList.Count>1)
                    unionGeomety = SectorList[SectorList.Count - 1].UnionGeometry;
                var newSector = new Sector(mergeSectorGeo,GlobalParams.RadarSymbol.CircleSymbol ,obstacleReports,_unitConverter)
                {
                    Height = state.Altitude,
                    MOC = state.Moc,
                    BufferValue = state.BufferValue,
                    Buffer = state.BufferGeo,
                    UnionGeometry = unionGeomety
                };

                SectorList.Add(newSector);
                _mainSectorList.Add(newSector);


                int secNumber = 1;
                foreach (var sector in _mainSectorList)
                {
                    sector.Number = secNumber++;
                    sector.Update();
                }

                ClosEventHandler?.Invoke(this,new EventArgs());
            }
        }

        private void RemoveSectorAfterMerge()
        {
            _mainSectorList.Remove(_selectedSector);
            _mainSectorList.Remove(_mergeSector);

            SectorList.Remove(_selectedSector);
            SectorList.Remove(_mergeSector);

            MergeableSectorList.Remove(_mergeSector);
        }

        private State CreateState(ISymbol symbol, IPolygon stateGeo, OperationType sOperationType,List<ObstacleReport> obstacleReports)
        {
            try
            {
                if (stateGeo == null || stateGeo.IsEmpty) return null;

                var state = new State(symbol)
                {
                    Altitude = _sectorElevation,
                    BufferValue = _selectedBufferValue,
                    Moc = _selectedMocValue,
                    OperType = sOperationType,
                    StateGeo = stateGeo,
                    ObstacleReports = obstacleReports
                };

                var bv = GlobalParams.UnitConverter.HeightToInternalUnits(SelectedBufferValue);
                var bufferGeo = GeomOperators.Buffer(stateGeo, bv);
                if (bufferGeo != null)
                    state.BufferGeo = bufferGeo;

                SectorElevation = _unitConverter.HeightToDisplayUnits(CalculateElevation());

                state.Draw(false);

                return state;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error extracting Elevation from Raster!" + e.Message);
                return null;
            }
        }

        private double CalculateElevation()
        {
            return 0;
        }

        private ISymbol CreateMergeSectorSymbol()
        {
            IRgbColor pRGB = null;

            pRGB = new RgbColor();
            //pRGB.RGB = Aran.Panda.Common.ARANFunctions.RGB(128, 128, 128);
            ISimpleFillSymbol pFillSym = new SimpleFillSymbol();

            pFillSym.Color = pRGB;
            pFillSym.Style = esriSimpleFillStyle.esriSFSNull;
            //(pFillSym as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen; // esriSFSNull 'esriSFSDiagonalCross

            ILineSymbol pLineSimbol = new SimpleLineSymbol();

            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = ARANFunctions.RGB(0,0,255);

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = 3;
            pFillSym.Outline = pLineSimbol;

            return pFillSym as ISymbol;
        }

        private ISymbol CreateSectorSymbol()
        {
            IRgbColor pRGB = null;

            pRGB = new RgbColor();
            //pRGB.RGB = Aran.Panda.Common.ARANFunctions.RGB(128, 128, 128);
            ISimpleFillSymbol pFillSym = new SimpleFillSymbol();

            pFillSym.Color = pRGB;
            pFillSym.Style = esriSimpleFillStyle.esriSFSCross;
            //(pFillSym as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen; // esriSFSNull 'esriSFSDiagonalCross

            ILineSymbol pLineSimbol = new SimpleLineSymbol();

            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = 128;

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = 2;
            pFillSym.Outline = pLineSimbol;

            return pFillSym as ISymbol;
        }

        public EventHandler ClosEventHandler;

    }
}

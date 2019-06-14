using Aran.Omega.Models;
using Aran.PANDA.Constants;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aran.Omega.View
{

    public enum SaveType
    {
        AllSurfaces,
        RunwaySurfaces,
        RunwayDirectionSurfaces,
        Custom
    }
    /// <summary>
    /// Interaction logic for OmegaSaveView.xaml
    /// </summary>
    public partial class OmegaSaveView : MetroWindow, INotifyPropertyChanged
    {
        private readonly SaveDbWrapper _saveDbWrapper;
        private readonly List<DrawingSurface> _allSufaces;
        private bool _runwaySurfacesIsChecked;
        private bool _allSurfacesIsChecked;
        private bool _runwayDirectionSurfacesIsChecked;
        private bool _customSurfacesIsChecked;
        private SaveType _saveType;

        public OmegaSaveView(List<DrawingSurface> surfaces,SaveDbWrapper saveDbWrapper)
        {
            InitializeComponent();
            _saveDbWrapper = saveDbWrapper;
            _allSufaces =new List<DrawingSurface>(surfaces);
            Surfaces = new List<SurfaceViewModel>();
            surfaces.ForEach(x => Surfaces.Add(new SurfaceViewModel { Name = x.ViewCaption, IsChecked = true,Surface=x }));
            AllSurfacesIsChecked = true;
            DataContext = this;
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    List<DrawingSurface> surfacesToSave = null;
                    switch (_saveType)
                    {
                        case SaveType.AllSurfaces:
                            surfacesToSave = _allSufaces;
                            break;
                        case SaveType.RunwaySurfaces:
                            surfacesToSave = _allSufaces.Where(x => IsRunwaySurfaces(x)).ToList();
                            break;
                        case SaveType.RunwayDirectionSurfaces:
                            surfacesToSave = _allSufaces.Where(x => !IsRunwaySurfaces(x)).ToList();
                            break;
                        case SaveType.Custom:
                            surfacesToSave = new List<DrawingSurface>();
                            Surfaces.Where(x => x.IsChecked).ToList().ForEach(x => surfacesToSave.Add(x.Surface));
                            break;
                        default:
                            throw new ArgumentException("Save type is not supported");
                    }

                    if (_saveDbWrapper.Save(surfacesToSave))
                    {
                        MessageBox.Show("Successfully saved to Database!");
                        Close();
                    }
                });
            }
        }

        public List<SurfaceViewModel> Surfaces { get; set; }

        public bool AllSurfacesIsChecked
        {
            get { return _allSurfacesIsChecked; }
            set
            {
                _allSurfacesIsChecked = value;
                if (value)
                    _saveType = SaveType.AllSurfaces;
            }
        }

        public bool RunwaySurfacesIsChecked
        {
            get { return _runwaySurfacesIsChecked; }
            set
            {
                _runwaySurfacesIsChecked = value;
                if (value)
                    _saveType = SaveType.RunwaySurfaces;
            }
        }
        
        public bool RunwayDirectionSurfacesIsChecked
        {
            get { return _runwayDirectionSurfacesIsChecked; }
            set
            {
                _runwayDirectionSurfacesIsChecked = value;
                if (value)
                    _saveType = SaveType.RunwayDirectionSurfaces;
            }
        }

        public bool CustomSurfacesIsChecked
        {
            get { return _customSurfacesIsChecked; }
            set
            {
                _customSurfacesIsChecked = value;
                if (value)
                {
                    _saveType = SaveType.Custom;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomSurfacesIsChecked)));
            }
        }

        private bool IsRunwaySurfaces(DrawingSurface surface)
        {
            return surface.SurfaceType == SurfaceType.InnerHorizontal
                || surface.SurfaceType == SurfaceType.CONICAL
                || surface.SurfaceType == SurfaceType.OuterHorizontal;
        }


        public event PropertyChangedEventHandler PropertyChanged;

    }

    public class SurfaceViewModel
    {
        public bool IsChecked { get; set; }

        public string Name { get; set; }

        public DrawingSurface Surface { get; set; }
    }
}

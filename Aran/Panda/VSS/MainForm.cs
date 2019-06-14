using Aran.AranEnvironment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Panda.Vss;

namespace Aran.PANDA.Vss
{
    public partial class MainForm : Form
    {
        private List<PageControl> _pageControlList;
        private PageControl _currentPageControl;
        private ReportForm _reportForm;
        private FirstPageControl _firstPageControl;


        public MainForm()
        {
            InitializeComponent();

            Globals.MainForm = this;

            _pageControlList = new List<PageControl>();
            _pageControlList.Add(new FirstPageControl());
            _pageControlList.Add(new SecondPageControl());
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (var pageControl in _pageControlList) {
                pageControl.IsFirstChanged += PageControl_IsFirstChanged;
                pageControl.IsLastChanged += PageControl_IsLastChanged;
                pageControl.PageChanged += PageControl_PageChanged;

                pageControl.Visible = false;
                pageControl.Parent = ui_pageContainerPanel;

                pageControl.SetAllPageControls(_pageControlList);
            }

            _currentPageControl = _pageControlList[0];
            _currentPageControl.Visible = true;
            _currentPageControl.LoadPage();

            _firstPageControl = _currentPageControl as FirstPageControl;
            _firstPageControl.AreaChanged += FirstPage_AreaChanged;
        }

        private void FirstPage_AreaChanged(object sender, EventArgs e)
        {
            if (_reportForm != null)
                ui_reportChB.Checked = false;
        }

        private void PageControl_PageChanged(object sender, PageChangedEventArgs e)
        {
            var prevPageControl = sender as PageControl;
            prevPageControl.Visible = false;

            _currentPageControl = e.Page;
            _currentPageControl.Visible = true;

            PageControl_IsLastChanged(_currentPageControl, null);
            PageControl_IsFirstChanged(_currentPageControl, null);
        }

        private void PageControl_IsLastChanged(object sender, EventArgs e)
        {
            var pageControl = sender as PageControl;
            ui_nextButton.Enabled = !pageControl.IsLast;
        }

        private void PageControl_IsFirstChanged(object sender, EventArgs e)
        {
            var pageControl = sender as PageControl;
            ui_backButton.Enabled = !pageControl.IsFirst;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            _currentPageControl.NextClicked();
        }
        
        private void Back_Click(object sender, EventArgs e)
        {
            _currentPageControl.BackClicked();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Report_CheckedChanged(object sender, EventArgs e)
        {
            var isChecked = (sender as CheckBox).Checked;

            if (!isChecked) {
                if (_reportForm != null) {
                    _reportForm.Close();
                    _reportForm = null;
                }
            }
            else {
                if (_reportForm == null) {
                    _reportForm = new ReportForm();
                    _reportForm.FormClosed += ReportForm_FormClosed;
                    var obsList = _firstPageControl.CalculateObstacles();
                    _reportForm.Obstacles.AddRange(obsList);
                    _reportForm.Show(this);
                    _reportForm.RefreshGrid();
                }
            }
        }

        private void ReportForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _reportForm = null;
            ui_reportChB.Checked = false;
        }

        
    }
}

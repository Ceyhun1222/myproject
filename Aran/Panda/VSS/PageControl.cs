using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.PANDA.Vss
{
    public partial class PageControl : UserControl
    {
        private bool _isFirst;
        private bool _isLast;


        public PageControl()
        {
            InitializeComponent();
        }


        public event EventHandler IsFirstChanged;
        public event EventHandler IsLastChanged;
        public event PageChangedEventHandler PageChanged;

        
        public virtual void SetAllPageControls(IEnumerable<PageControl> allPageControls)
        {

        }

        public virtual void LoadPage()
        {
        }

        public virtual void NextClicked()
        {

        }

        public virtual void BackClicked()
        {

        }

        public virtual bool IsFirst
        {
            get { return _isFirst; }
            set
            {
                if (_isFirst == value)
                    return;

                _isFirst = value;
                if (IsFirstChanged != null)
                    IsFirstChanged(this, null);
            }
        }

        public virtual bool IsLast
        {
            get { return _isLast; }
            set
            {
                if (_isLast == value)
                    return;

                _isLast = value;
                if (IsLastChanged != null)
                    IsLastChanged(this, null);
            }
        }

        protected void DoPageChanged(PageControl pageControl, bool isNext)
        {
            if (PageChanged == null)
                return;

            PageChanged(this, new PageChangedEventArgs(pageControl, isNext));
        }
    }

    
}

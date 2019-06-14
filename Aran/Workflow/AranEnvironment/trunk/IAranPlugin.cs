using Aran.Aim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.AranEnvironment
{
    public abstract class AranPlugin
    {
        private ToolStrip _toolbar;

        public AranPlugin()
        {
            IsSystemPlugin = false;
        }


        public event EventHandler ToolbarChanged;


        public abstract Guid Id { get; }

        public abstract void Startup(IAranEnvironment aranEnv);

        public abstract string Name { get; }

        public virtual bool IsSystemPlugin { get; protected set; }

        public virtual void AddChildSubMenu(List<string> hierarcy)
        {
        }

        public virtual List<FeatureType> GetLayerFeatureTypes()
        {
            return new List<FeatureType>();
        }

        public ToolStrip Toolbar
        {
            get { return _toolbar; }
            set
            {
                if (_toolbar == value)
                    return;

                _toolbar = value;
                if (ToolbarChanged != null)
                    ToolbarChanged(this, null);
            }
        }
    }
}

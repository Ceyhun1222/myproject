using Aran.Temporality.CommonUtil.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenaToolBox
{
    public partial class FormTOSS_Slots : Form
    {

        public IList<Aran.Temporality.Common.Entity.PublicSlot> PublicSlotsList { get; set; }

        public IList<Aran.Temporality.Common.Entity.PrivateSlot> PrivateSlotsList { get; set; }
        public Aran.Temporality.Common.Entity.PrivateSlot SelectedPrivateSlot { get; set; }
        public Aran.Temporality.Common.Entity.PublicSlot SelectedPublicSlot { get; set; }

        public FormTOSS_Slots()
        {
            InitializeComponent();
        }
        public FormTOSS_Slots(IList<Aran.Temporality.Common.Entity.PublicSlot> _publicSlotsList)
        {
            InitializeComponent();
            if(comboBox1.Items !=null) comboBox1.Items.Clear();
            foreach (var pubSlot in _publicSlotsList)
            {
                string fr = pubSlot.Frozen? " frozen " : "";
                string ed = pubSlot.Editable? "" : " not edit.";
                comboBox1.Items.Add(pubSlot.Name + fr + ed);
            }
            PublicSlotsList = _publicSlotsList;


            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Items!=null)  comboBox2.Items.Clear();

            var privateSlotList = CurrentDataContext.CurrentNoAixmDataService.GetPrivateSlots(CurrentDataContext.CurrentUser.Id, PublicSlotsList[comboBox1.SelectedIndex].Id);
            if (privateSlotList !=null)
            {
                foreach (var _Slot in privateSlotList)
                {
                    comboBox2.Items.Add(_Slot.Name);
                }

                PrivateSlotsList = privateSlotList;

                if (comboBox2.Items.Count > 0) comboBox2.SelectedIndex = 0;

            }

            SelectedPublicSlot = PublicSlotsList[comboBox1.SelectedIndex];
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedPrivateSlot = PrivateSlotsList[comboBox2.SelectedIndex];
        }
    }
}

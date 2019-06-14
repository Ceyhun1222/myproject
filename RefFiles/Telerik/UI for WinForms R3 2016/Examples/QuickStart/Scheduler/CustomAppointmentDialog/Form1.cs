using Telerik.WinControls.UI;
using Telerik.QuickStart.WinControls;

namespace Telerik.Examples.WinControls.Scheduler.CustomAppointmentDialog
{
    public partial class Form1 : ExamplesForm
    {
        CustomEditAppointmentDialog appointmentDialog = null;

        public Form1()
        {
            InitializeComponent();

            this.radSchedulerDemo.AppointmentFactory = new CustomAppointmentFactory();
        }

        private void radSchedulerDemo_AppointmentEditDialogShowing(object sender, AppointmentEditDialogShowingEventArgs e)
        {
            this.appointmentDialog = new CustomEditAppointmentDialog();
            this.appointmentDialog.ThemeName = this.radSchedulerDemo.ThemeName;
            e.AppointmentEditDialog = this.appointmentDialog;
        }

        protected override void WireEvents()
        {
            this.radSchedulerDemo.AppointmentEditDialogShowing += new System.EventHandler<Telerik.WinControls.UI.AppointmentEditDialogShowingEventArgs>(this.radSchedulerDemo_AppointmentEditDialogShowing);
        }
    }
}

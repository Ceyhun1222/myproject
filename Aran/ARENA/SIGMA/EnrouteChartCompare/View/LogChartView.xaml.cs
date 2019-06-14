using System.Windows;

namespace EnrouteChartCompare.View
{
	/// <summary>
	/// Interaction logic for MainUpdateChartView.xaml
	/// </summary>
	public partial class LogChartView : Window
	{
		//private EventHandler _closedHandler;

	    public LogChartView()
		{
			InitializeComponent();
		}

		//public void SetData(List<PDM.PDMObject> newPdmList, List<PDM.PDMObject> oldPdmList, string fileName, IHookHelper mHookHelper)
		//{
			//GlobalParams.HookHelper = mHookHelper;
			//var vModel = new LogChartViewModel(oldPdmList, newPdmList,fileName);
			//this.DataContext = vModel;
			//vModel.Close += delegate ( )
			//{
			//	_closedHandler ( ( ( LogChartViewModel ) this.DataContext ).ResultList, null );
			//};
		//}

		//public void AddDestroyEventHandler ( EventHandler closedEnrouteUpdateWindow )
		//{
		//	_closedHandler = closedEnrouteUpdateWindow;
		//}
	}
}

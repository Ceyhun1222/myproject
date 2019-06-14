using System.Linq;
using TOSSM.ViewModel.Tool;
using Xceed.Wpf.AvalonDock.Layout;

namespace TOSSM.ViewModel.Pane
{
    public class LayoutInitializer : ILayoutUpdateStrategy
    {

        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {

            //AD wants to add the anchorable into destinationContainer
            //just for test provide a new anchorablepane 
            //if the pane is floating let the manager go ahead
            //var destPane = destinationContainer as LayoutAnchorablePane;

            //if (destinationContainer != null &&
            //    destinationContainer.FindParent<LayoutFloatingWindow>() != null)
            //    return false;

           
            if (anchorableToShow.Content is FeatureSelectorToolViewModel)
            {
                var selectorPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(d => d.Name == "SelectorPane");
                if (selectorPane != null)
                {
                    //clear Children
                    selectorPane.Children.Clear();
                    selectorPane.Children.Add(anchorableToShow);
                    return true;
                }
            }


            if (anchorableToShow.Content is FeaturePresenterToolViewModel)
            {
                var presenterPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(d => d.Name == "PresenterPane");
                if (presenterPane != null)
                {
                    presenterPane.Children.Clear();
                    presenterPane.Children.Add(anchorableToShow);
                    return true;
                }
            }




            return false;
        }


        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
        }


        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
        {
            return false;
        }

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
        {

        }
    }
}

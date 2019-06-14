using System.Windows;
using System.Windows.Controls;
using TOSSM.ViewModel.Document.Slot.Problems.ProblemList;

namespace TOSSM.ViewModel.Document.Slot.Problems
{
    public class ProblemViewModelTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
            if (item == null) return null;
// ReSharper restore HeuristicUnreachableCode
// ReSharper restore ConditionIsAlwaysTrueOrFalse

            var element = container as ContentPresenter;
            if (element != null)
            {
                //process readonly
                if (item is BusinessRulesCategoryViewModel)
                {
                    return element.FindResource("BusinessRulesProblemTemplate") as DataTemplate;
                }


                if (item is MissingLinksProblemListViewModel)
                {
                    return element.FindResource("MissingLinksProblemTemplate") as DataTemplate;
                }

                if (item is SyntaxProblemListViewModel)
                {
                    return element.FindResource("SyntaxProblemTemplate") as DataTemplate;
                }

                if (item is DataSetCategoryViewModel)
                {
                    return element.FindResource("DatasetProblemTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}

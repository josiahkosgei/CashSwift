using CashSwiftDeposit.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace CashSwiftDeposit.UserControls
{
    public partial class SummaryScreen : UserControl, IComponentConnector
    {
        public SummaryScreen(List<SummaryListItem> boundList)
        {
            InitializeComponent();
            SummaryListBox.ItemsSource = boundList;
        }

        private void OnManipulationBoundaryFeedback(
          object sender,
          ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

    }
}

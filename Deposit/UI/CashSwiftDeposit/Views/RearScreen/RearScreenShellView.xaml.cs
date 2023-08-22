using System.Windows;
using System.Windows.Markup;

namespace CashSwiftDeposit.Views.RearScreen
{
    public partial class RearScreenShellView : Window, IComponentConnector
    {
        public RearScreenShellView() => InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e) => (sender as Window).WindowState = WindowState.Maximized;

    }
}

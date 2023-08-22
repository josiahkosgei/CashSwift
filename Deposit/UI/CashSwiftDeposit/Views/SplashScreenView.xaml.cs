using CashSwiftDeposit.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace CashSwiftDeposit.Views
{
    public partial class SplashScreenView : UserControl, IComponentConnector
    {
        private int videoLoopCount = 0;

        public SplashScreenView() => InitializeComponent();

        public SplashScreenViewModel viewModel => DataContext as SplashScreenViewModel;

        public void SplashScreenTouched(object sender, RoutedEventArgs e) => viewModel.ApplicationViewModel.CloseDialog(true);

        private void myMediaElement_Loaded(object sender, RoutedEventArgs e) => myMediaElement.Play();

        private void myMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            ApplicationViewModel.Log.TraceFormat(GetType().Name, "MediaEnded", "SplashScreen", "Loop {0} complete", videoLoopCount + 1);
            myMediaElement.Stop();
            myMediaElement.Position = TimeSpan.FromSeconds(0.0);
            myMediaElement.Play();
            ++videoLoopCount;
        }

    }
}

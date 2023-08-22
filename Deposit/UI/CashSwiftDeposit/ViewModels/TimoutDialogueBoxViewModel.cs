// ViewModels.TimoutDialogueBoxViewModel


using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace CashSwiftDeposit.ViewModels
{
    [Guid("257981B3-4CB4-4863-940D-EA742ADF16B3")]
    public sealed class TimoutDialogueBoxViewModel : DialogueScreenBase
    {
        private TimoutDialogueBoxViewModel(ApplicationViewModel applicationViewModel, int timerDuration = 30)
          : base(applicationViewModel, timerDuration, 2)
        {
            if (!(Application.Current.FindResource("Dialog_ScreenIdleTimeout_TitleText") is string str1))
                str1 = "Hello";
            ScreenTitle = str1;
            DialogImage = "Resources/Icons/Main/clock.png";
            MessageBoxButton = MessageBoxButton.YesNo;
            if (!(Application.Current.FindResource("Dialog_ScreenIdleTimeout_DescriptionText") is string str2))
                str2 = "Would you like more time?";
            DialogBoxMessage = str2;
        }

        public static MessageBoxResult ShowDialog(
          ApplicationViewModel applicationViewModel,
          int timerDuration = 30)
        {
            using (TimoutDialogueBoxViewModel screen = new TimoutDialogueBoxViewModel(applicationViewModel, timerDuration = 30))
            {
                applicationViewModel.ShowDialogBox(screen);
                while (!screen.HasReturned)
                {
                    Thread.CurrentThread.Join(10);
                    Thread.Sleep(100);
                }
                return screen.MessageBoxResult;
            }
        }
    }
}

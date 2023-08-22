namespace CashSwiftDeposit.ViewModels.RearScreen
{
    public interface ICashSwiftWindowConductor
    {
        void CloseDialog(bool generateScreen = true);

        void ShowDialog(object screen);

        void ShowDialogBox(object screen);
    }
}

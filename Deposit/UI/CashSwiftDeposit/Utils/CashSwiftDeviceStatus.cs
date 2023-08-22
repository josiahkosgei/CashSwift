using Caliburn.Micro;
using CashSwift.Library.Standard.Statuses;

namespace CashSwiftDeposit.Utils
{
    public class CashSwiftDeviceStatus : PropertyChangedBase
    {
        private CashSwiftDeviceState _cashSwiftDeviceState;
        private ControllerStatus _controllerStatus;
        private CoreBankingStatus _coreBankingStatus;

        public CashSwiftDeviceState CashSwiftDeviceState
        {
            get => _cashSwiftDeviceState & ~CashSwiftDeviceState.COUNTING_DEVICE & ~CashSwiftDeviceState.DATABASE & ~CashSwiftDeviceState.PRINTER;
            set
            {
                if (_cashSwiftDeviceState == value)
                    return;
                _cashSwiftDeviceState = value;
                NotifyOfPropertyChange(() => CashSwiftDeviceState);
            }
        }

        public ControllerStatus ControllerStatus
        {
            get => _controllerStatus;
            set
            {
                _controllerStatus = value;
                NotifyOfPropertyChange(() => ControllerStatus);
            }
        }

        public CoreBankingStatus CoreBankingStatus
        {
            get => _coreBankingStatus;
            set
            {
                _coreBankingStatus = value;
                NotifyOfPropertyChange(() => CoreBankingStatus);
            }
        }
    }
}

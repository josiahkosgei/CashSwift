using CashSwift.Finacle.Integration.CQRS.DTOs.FundsTransfer;
using MediatR;

namespace CashSwift.Finacle.Integration.CQRS.Events
{
    public class FundsTransfered : INotification
    {
        public FundsTransferResponseDto _fundsTransferResponseDto { get; }

        public FundsTransfered(FundsTransferResponseDto fundsTransferResponseDto)
        {
            _fundsTransferResponseDto = fundsTransferResponseDto;
        }
    }
}

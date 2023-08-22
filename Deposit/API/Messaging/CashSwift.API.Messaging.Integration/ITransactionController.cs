using CashSwift.API.Messaging.Integration.Transactions;
using System.Threading.Tasks;

namespace CashSwift.API.Messaging.Integration
{
    public interface ITransactionController
    {
        Task<PostTransactionResponse> PostTransactionAsync(
          PostTransactionRequest request);

        Task<PostCITTransactionResponse> PostCITTransactionAsync(
          PostCITTransactionRequest request);
    }
}

using Core.Entities;

namespace Core.Interfaces;

public interface IPaymantsService
{
    Task<Object> TxnSetup(TxnInternalRequestDto requestData);
    Task<string> InquireTransaction(InquireTransactionsRequestDto requestData);

}

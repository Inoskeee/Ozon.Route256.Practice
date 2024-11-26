using Dapper;
using Ozon.Route256.Practice.ClientBalance.Dal.Contracts;
using Ozon.Route256.Practice.ClientBalance.Dal.Contracts.Repositories;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities;

namespace Ozon.Route256.Practice.ClientBalance.Dal.Repositories;

public sealed class ClientBalanceRepository : BaseRepository, IClientBalanceRepository
{
    public async Task<ClientBalanceEntity[]> Get(long clientId, CancellationToken token)
    {
        const string sqlQuery = @"
                select *
                from client_balances
                where client_id = @ClientId";

        var param = new DynamicParameters();
        param.Add("ClientId", clientId);
        
        return await ExecuteQueryAsync<ClientBalanceEntity>(sqlQuery, param, token);        
    }

    public async Task<ResultEntity<long?>> UpdateOrInsert(ClientBalanceEntity balance, bool isTopUp, CancellationToken token)
    {
        var currentBalance = await GetSpecificBalance(balance.ClientId, balance.CurrencyCode, token);
        
        if (!isTopUp)
        {
            if (currentBalance is null)
            {
                return new ResultEntity<long?>(
                    IsSuccess: false,
                    Result: null,
                    Message: "Невозможно снять средства. Баланс пользователя не существует.");
            }
            
            var totalCurrentBalance = currentBalance.Units * 1000000000 + currentBalance.Nanos;
            var totalWithdrawAmount = balance.Units * 1000000000 + balance.Nanos;

            if (totalWithdrawAmount > totalCurrentBalance)
            {
                return new ResultEntity<long?>(
                    IsSuccess: false,
                    Result: null,
                    Message: "Недостаточно средств для снятия.");
            }
        }
        const string sqlQuery = @"
                insert into client_balances(client_id, currency_code, units, nanos)
                values (@ClientId, @CurrencyCode, @Units, @Nanos)
                on conflict (client_id, currency_code)
                do update
                    set units = client_balances.units + @Units * @Factor + 
                                (client_balances.nanos + @Nanos * @Factor) / 1000000000,
                    nanos = (client_balances.nanos + @Nanos * @Factor) % 1000000000
                returning client_id";

        var param = new DynamicParameters();
        param.Add("ClientId", balance.ClientId);
        param.Add("CurrencyCode", balance.CurrencyCode);
        param.Add("Units", balance.Units);
        param.Add("Nanos", balance.Nanos);
        param.Add("Factor", isTopUp ? 1 : -1);
        
        var queryResult = await ExecuteNonQueryAsync<long?>(sqlQuery, param, token);

        return queryResult is not null
            ? new ResultEntity<long?>(
                IsSuccess: true,
                Result: queryResult,
                Message: "Ok")
            : new ResultEntity<long?>(
                IsSuccess: false,
                Result: queryResult,
                Message: "Не удалось выполнить запрос");
    }
    
    private async Task<ClientBalanceEntity?> GetSpecificBalance(long clientId, string currencyCode, CancellationToken token)
    {
        const string sqlQuery = @"
                select *
                from client_balances
                where client_id = @ClientId and currency_code = @CurrencyCode";

        var param = new DynamicParameters();
        param.Add("ClientId", clientId);
        param.Add("CurrencyCode", currencyCode);
        
        return (await ExecuteQueryAsync<ClientBalanceEntity>(sqlQuery, param, token)).FirstOrDefault();        
    }

}
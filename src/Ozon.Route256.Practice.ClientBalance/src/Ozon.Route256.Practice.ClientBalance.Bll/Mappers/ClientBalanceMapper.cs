using Ozon.Route256.Practice.ClientBalance.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.ClientBalance.Bll.Models;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities;

namespace Ozon.Route256.Practice.ClientBalance.Bll.Mappers;

public class ClientBalanceMapper : IClientBalanceMapper
{
    public MoneyModel[] MapEntityToModel(ClientBalanceEntity[] clientBalance)
    {
        return clientBalance
            .Select(balance => new MoneyModel(
                CurrencyCode: balance.CurrencyCode, 
                Units: balance.Units, 
                Nanos: balance.Nanos))
            .ToArray();
    }
}
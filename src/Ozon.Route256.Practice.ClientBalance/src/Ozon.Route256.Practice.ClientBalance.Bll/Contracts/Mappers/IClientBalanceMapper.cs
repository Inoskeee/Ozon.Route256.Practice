﻿using Ozon.Route256.Practice.ClientBalance.Bll.Models;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities;

namespace Ozon.Route256.Practice.ClientBalance.Bll.Contracts.Mappers;

public interface IClientBalanceMapper
{
    MoneyModel[] MapEntityToModel(ClientBalanceEntity[] clientBalance);
}
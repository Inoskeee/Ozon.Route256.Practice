﻿namespace Ozon.Route256.Practice.ClientOrders.Bll.Models.ResultModels;

public sealed record ActionSuccessModel<T>(
    T SuccessContent);
using Ozon.Route256.DataGenerator.Bll.Creators;
using Ozon.Route256.DataGenerator.Bll.Models;
using Ozon.Route256.DataGenerator.Bll.Services.Contracts;

namespace Ozon.Route256.DataGenerator.Bll.Services;

public class CustomerService : ICustomerService
{
    public IReadOnlyList<Customer> Create(int count) => CustomerCreator.Create(count);
}

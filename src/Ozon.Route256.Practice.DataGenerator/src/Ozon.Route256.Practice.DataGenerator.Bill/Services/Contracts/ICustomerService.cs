using Ozon.Route256.DataGenerator.Bll.Models;

namespace Ozon.Route256.DataGenerator.Bll.Services.Contracts;

public interface ICustomerService
{
    IReadOnlyList<Customer> Create(int count);
}

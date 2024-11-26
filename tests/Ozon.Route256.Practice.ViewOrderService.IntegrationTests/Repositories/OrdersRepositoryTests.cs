using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Repositories;
using Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Fixtures;

namespace Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Repositories;

[Collection(nameof(PostgreSqlFixture))]
public partial class OrdersRepositoryTests
{
    private readonly IOrdersRepository _repository;
    
    public OrdersRepositoryTests(
        PostgreSqlFixture fixture)
    {
        _repository = fixture.OrdersRepository;
    }
}
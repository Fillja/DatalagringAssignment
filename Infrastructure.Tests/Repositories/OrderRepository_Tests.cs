using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Respositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class OrderRepository_Tests
{
    private readonly SecondDataContext _context =
    new(new DbContextOptionsBuilder<SecondDataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public void GetOneShould_FindOneOrderEntity_AndReturnTheEntity()
    {
        //Arrange
        var _orderRepository = new OrderRepository(_context);
        var orderEntity = new Order { Id = 1, UserId = 1 };
        _orderRepository.Create(orderEntity);

        //Act
        var result = _orderRepository.GetOne(x => x.Id == orderEntity.Id);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(1, result.UserId);
    }

    [Fact]
    public void GetOneShould_NotFindOneOrderEntity_AndReturnNull()
    {
        //Arrange
        var _orderRepository = new OrderRepository(_context);
        var orderEntity = new Order { Id = 1, UserId = 1 };

        //Act
        var result = _orderRepository.GetOne(x => x.Id == orderEntity.Id);

        //Assert
        Assert.Null(result);
    }
}

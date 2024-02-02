using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Respositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class OrderRowRepository_Tests
{
    private readonly SecondDataContext _context =
    new(new DbContextOptionsBuilder<SecondDataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public void ExistsShould_CheckIfOrderRowEntityExists_AndReturnTrue()
    {
        //Arrange
        var _orderRowRepository = new OrderRowRepository(_context);
        var orderRowEntity = new OrderRow { ProductId = 1, OrderId = 1, Amount = 1 };
        _orderRowRepository.Create(orderRowEntity);

        //Act
        var result = _orderRowRepository.Exists(x => x.ProductId == orderRowEntity.ProductId);

        //Assert
        Assert.True(result);
        Assert.Equal(1, orderRowEntity.ProductId);
    }

    [Fact]
    public void ExistsShould_CheckIfOrderRowEntityExists_AndReturnFalse()
    {
        //Arrange
        var _orderRowRepository = new OrderRowRepository(_context);
        var orderRowEntity = new OrderRow { ProductId = 1, OrderId = 1, Amount = 1 };

        //Act
        var result = _orderRowRepository.Exists(x => x.ProductId == orderRowEntity.ProductId);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void DeleteShould_CheckIfOrderRowEntityIsNull_ThenDeleteTheEntity_AndReturnTrue()
    {
        //Arrange
        var _orderRowRepository = new OrderRowRepository(_context);
        var orderRowEntity = new OrderRow { ProductId = 1, OrderId = 1, Amount = 1 };
        _orderRowRepository.Create(orderRowEntity);

        //Act
        var result = _orderRowRepository.Delete(x => x.ProductId == orderRowEntity.ProductId);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void DeleteShould_CheckIfOrderRowEntityIsNull_AndReturnFalse()
    {
        //Arrange
        var _orderRowRepository = new OrderRowRepository(_context);
        var orderRowEntity = new OrderRow();

        //Act
        var result = _orderRowRepository.Delete(x => x.ProductId == orderRowEntity.ProductId);

        //Assert
        Assert.False(result);
    }
}

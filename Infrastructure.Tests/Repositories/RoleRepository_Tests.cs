using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Respositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class RoleRepository_Tests
{
    private readonly DataContext _context =
    new(new DbContextOptionsBuilder<DataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public void UpdateShould_CheckIfRoleEntityExists_ThenUpdateTheEntity_AndReturnTheEntity()
    {
        //Arrange
        var _roleRepository = new RoleRepository(_context);
        var roleEntity = new RoleEntity { Id = 1, RoleName = "Admin" };
        _roleRepository.Create(roleEntity);

        //Act
        roleEntity.RoleName = "Guest";
        var result = _roleRepository.Update(roleEntity, x => x.Id == roleEntity.Id);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Guest", result.RoleName);
    }

    [Fact]
    public void UpdateShould_CheckIfRoleEntityExists_ThenNotFindEntity_AndReturnNull()
    {
        //Arrange
        var _roleRepository = new RoleRepository(_context);
        var roleEntity = new RoleEntity { Id = 1, RoleName = "Admin" };

        //Act
        var result = _roleRepository.Update(roleEntity, x => x.Id == roleEntity.Id);

        //Assert
        Assert.Null(result);
    }
}

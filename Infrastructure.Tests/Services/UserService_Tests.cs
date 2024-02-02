using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Respositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Services;

public class UserService_Tests
{
    private readonly DataContext _context =
    new(new DbContextOptionsBuilder<DataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public void CreateUserShould_TakeUserRegDtoAsParam_ThenCreateAndSaveAllEntities_AndReturnTrue()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);
        var _userService = new UserService(_profileRepository, _userFactories);

        var user = new UserRegistrationForm
        {
            FirstName = "Test",
            LastName = "Testsson",
            Email = "Test@domain.com",
            Password = "Password",
            Street = "TestStreet",
            City = "TestCity",
            PostalCode = "12345",
        };

        //Act
        var result = _userService.CreateUser(user);

        //Arrange
        Assert.True(result);
    }

    [Fact]
    public void CreateUserShould_TakeIncompleteUserRegDtoAsParam_ThenFailToCreateAllEntities_AndReturnFalse()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);
        var _userService = new UserService(_profileRepository, _userFactories);

        var user = new UserRegistrationForm();

        //Act
        var result = _userService.CreateUser(user);

        //Arrange
        Assert.False(result);
    }

    [Fact]
    public void GetAllUsersShould_GetAllProfileEntities_ThenCompileAllEntitiesIntoUserDto_ThenAddCompleteUserDtoToNewList_AndReturnListAsIEnumerableOfTypeUserDto()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);
        var _userService = new UserService(_profileRepository, _userFactories);

        var userEntity = new UserEntity { Id = 1, FirstName = "Test", LastName = "Testsson" };
        var addressEntity = new AddressEntity { Id = 1, City = "TestCity", Street = "TestStreet", PostalCode = "12345" };
        var verificationEntity = new VerificationEntity { UserId = 1, User = userEntity, Email = "Test@domain.com", Password = "Test123" };
        var roleEntity = new RoleEntity { Id = 1, RoleName = "TestRole" };
        var profileEntity = new ProfileEntity 
        { 
            UserId = 1, 
            AddressId = 1, 
            RoleId = 1, 
            User = userEntity, 
            Address = addressEntity, 
            Role = roleEntity 
        };

        //Act
        _userRepository.Create(userEntity);
        _addressRepository.Create(addressEntity);
        _verificationRepository.Create(verificationEntity);
        _roleRepository.Create(roleEntity);
        _profileRepository.Create(profileEntity);

        var result = _userService.GetAllUsers();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<UserDto>>(result);
        Assert.Single(result);
    }

    [Fact]
    public void GetAllUsersShould_GetNoProfileEntities_ThenCompileNothing_ThenAddEmptyUserDtoToNewList_AndReturnEmptyListAsIEnumerableOfTypeUserDto()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);
        var _userService = new UserService(_profileRepository, _userFactories);

        //Act
        var result = _userService.GetAllUsers();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<UserDto>>(result);
        Assert.Empty(result);
    }
}

using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Respositories;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Infrastructure.Tests.Factories;

public class UserFactories_Tests
{

    private readonly DataContext _context =
    new(new DbContextOptionsBuilder<DataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);


    [Fact]
    public void CompileFullUserShould_TakeProfileEntityAsParam_AndBuildCompleteUserDto_ThenReturnUserDto()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);

        var userEntity = new UserEntity { Id = 1, FirstName = "Test", LastName = "Testsson" };
        var addressEntity = new AddressEntity { Id = 1, City = "TestCity", Street = "TestStreet", PostalCode = "12345" };
        var verificationEntity = new VerificationEntity { UserId = 1, User = userEntity, Email = "Test@domain.com", Password = "Test123" };
        var roleEntity = new RoleEntity { Id = 1, RoleName = "TestRole" };
        var profileEntity = new ProfileEntity { UserId = 1, AddressId = 1, RoleId = 1, User = userEntity, Address = addressEntity, Role = roleEntity };

        //Act
        _userRepository.Create(userEntity);
        _addressRepository.Create(addressEntity);
        _verificationRepository.Create(verificationEntity);
        _roleRepository.Create(roleEntity);
        _profileRepository.Create(profileEntity);
        var result = _userFactories.CompileFullUser(profileEntity);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<UserDto>(result);
        Assert.Equal("Test", result.FirstName);
        Assert.Equal("TestStreet", result.Street);
        Assert.Equal("Test@domain.com", result.Email);
        Assert.Equal("TestRole", result.RoleName);
    }

    [Fact]
    public void CompileFullUserShould_TakeIncompleteProfileEntityAsParam_AndFailToBuildCompleteUserDto_ThenReturnNull()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);

        var profileEntity = new ProfileEntity { UserId = 1, AddressId = 1, RoleId = 1 };

        //Act
        _profileRepository.Create(profileEntity);
        var result = _userFactories.CompileFullUser(profileEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void CreateUserEntityShould_TakePropertiesAsParams_ThenCreateUserEntity_AndReturnTheEntity()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);

        var firstName = "Test";
        var lastName = "Testsson";

        //Act
        var result = _userFactories.CreateUserEntity(firstName, lastName);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<UserEntity>(result);
        Assert.Equal(firstName, result.FirstName);
    }

    [Fact]
    public void GetOrCreateAddressEntityShould_TakePropertiesAsParams_ThenCheckIfParamsExist_ThenCreateTheEntity_AndReturnTheEntity()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);

        var street = "TestStreet";
        var postalCode = "TestPostalCode";
        var city = "TestCity";

        //Act
        var result = _userFactories.GetOrCreateAddressEntity(street, postalCode, city);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<AddressEntity>(result);
        Assert.Equal(street, result.Street);
    }

    [Fact]
    public void GetOrCreateAddressEntityShould_TakePropertiesAsParams_ThenCheckIfParamsExist_ThenFindExistingEntity_AndReturnTheEntity()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);

        var street = "TestStreet";
        var postalCode = "TestPostalCode";
        var city = "TestCity";

        var addressEnity = new AddressEntity { Id = 1, Street = street, City = city, PostalCode = postalCode };

        //Act
        _addressRepository.Create(addressEnity);

        var result = _userFactories.GetOrCreateAddressEntity(street, postalCode, city);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<AddressEntity>(result);
        Assert.Equal(street, result.Street);
    }

    [Fact]
    public void GetOrCreateRoleShould_TakePropertyAsParam_ThenCheckIfParamExist_ThenCreateTheEntity_AndReturnTheEntity()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);

        var roleName = "Admin";

        //Act
        var result = _userFactories.GetOrCreateRole(roleName);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<RoleEntity>(result);
        Assert.Equal(roleName, result.RoleName);
    }

    [Fact]
    public void GetOrCreateRoleShould_TakePropertyAsParam_ThenCheckIfParamExist_ThenFindExistingEntity_AndReturnTheEntity()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);

        var roleName = "Admin";

        var roleEntity = new RoleEntity { Id = 1, RoleName = roleName };

        //Act
        _roleRepository.Create(roleEntity);
        var result = _userFactories.GetOrCreateRole(roleName);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<RoleEntity>(result);
        Assert.Equal(roleName, result.RoleName);

    }

    [Fact]
    public void GetOrCreateRoleEntityShould_TakePropertyAsParam_ThenCheckIfNameIsAdminPrivileged_AndReturnTheEntity()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);

        var firstName = "Hans";

        //Act
        var result = _userFactories.GetOrCreateRoleEntity(firstName);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<RoleEntity>(result);
        Assert.Equal("admin", result.RoleName);
    }

    [Fact]
    public void GetOrCreateRoleEntityShould_TakePropertiesAsParams_ThenCheckIfNameIsUserPrivileged_AndReturnTheEntity()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);

        var firstName = "Test";

        //Act
        var result = _userFactories.GetOrCreateRoleEntity(firstName);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<RoleEntity>(result);
        Assert.Equal("user", result.RoleName);
    }

    [Fact]
    public void CreateVerificationEntityShould_TakePropertiesAsParams_ThenCheckIfParamsExists_ThenCreateTheEntity_AndReturnTheEntity()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);

        var userId = 1;
        var email = "Test@domain.com";
        var password = "Test";

        //Act
        var result = _userFactories.CreateVerificationEntity(userId, email, password);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<VerificationEntity>(result);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public void CreateVerificationEntityShould_TakePropertiesAsParams_ThenCheckIfParamsExist_ThenSkipCreatingTheEntity_AndReturnNull()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);

        var userId = 1;
        var email = "Test@domain.com";
        var password = "Test";

        var userEntity = new UserEntity { Id = 1, FirstName = "Test", LastName = "Testsson" };
        var verificationEntity = new VerificationEntity { UserId = userId, Email = email, Password = password, User = userEntity };

        //Act
        _verificationRepository.Create(verificationEntity);

        var result = _userFactories.CreateVerificationEntity(userId, email, password);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void CreateProfileEntityShould_TakePropertiesAsParams_ThenCreateProfileEntity_AndReturnTheEntity()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var _roleRepository = new RoleRepository(_context);
        var _addressRepository = new AddressRepository(_context);
        var _verificationRepository = new VerificationRepository(_context);
        var _profileRepository = new ProfileRepository(_context);
        var _userFactories = new UserFactories(_userRepository, _roleRepository, _addressRepository, _verificationRepository, _profileRepository);

        var userId = 1;
        var addressId = 1;
        var roleId = 1;

        //Act
        var result = _userFactories.CreateProfileEntity(userId, addressId, roleId);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ProfileEntity>(result);
        Assert.Equal(1, result.UserId);
    }
}

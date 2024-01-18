using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Respositories;
using System.Diagnostics;

namespace Infrastructure.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly RoleRepository _roleRepository;
    private readonly AddressRepository _addressRepository;
    private readonly VerificationRepository _verificationRepository;
    private readonly ProfileRepository _profileRepository;
    private readonly UserFactories _userFactories;

    public UserService(UserRepository userRepository, RoleRepository roleRepository, AddressRepository addressRepository, VerificationRepository verificationRepository, ProfileRepository profileRepository, UserFactories userFactories)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _addressRepository = addressRepository;
        _verificationRepository = verificationRepository;
        _profileRepository = profileRepository;
        _userFactories = userFactories;
    }

    public bool CreateUser(UserRegistrationForm userReg)
    {
        try
        {
            var userEntity = new UserEntity
            {
                FirstName = userReg.FirstName,
                LastName = userReg.LastName
            };

            var userResult = _userRepository.Create(userEntity);

            var addressEntity = new AddressEntity
            {
                Street = userReg.Street,
                City = userReg.City,
                PostalCode = userReg.PostalCode,
            };

            var addressResult = _addressRepository.Create(addressEntity);

            var roleEntity = new RoleEntity
            {
                RoleName = "admin",
            };

            var roleResult = _roleRepository.Create(roleEntity);

            var verificationEntity = new VerificationEntity
            {
                UserId = userResult.Id,
                Email = userReg.Email,
                Password = userReg.Password
            };

            _verificationRepository.Create(verificationEntity);

            var profileEntity = new ProfileEntity
            {
                UserId = userResult.Id,
                AddressId = addressResult.Id,
                RoleId = roleResult.Id,
            };

            _profileRepository.Create(profileEntity);

            return true;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return false;
    }

    public IEnumerable<User> GetAllUsers()
    {
        var profileList = _profileRepository.GetAll();
        var userList = new List<User>();

        foreach (var item in profileList)
        {
            var user = _userFactories.CreateFullUser(item);
            userList.Add(user);
        }

        return userList;
    }
}

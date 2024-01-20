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
            var userEntity = _userFactories.CreateUserEntity(userReg.FirstName!, userReg.LastName!);

            var addressEntity = _userFactories.GetOrCreateAddressEntity(userReg.Street, userReg.PostalCode, userReg.City);

            var roleEntity = _userFactories.GetOrCreateRoleEntity(userReg.FirstName!);

            var verificationEntity = _userFactories.GetOrCreateVerificationEntity(userEntity.Id, userReg.Email, userReg.Password);

            var profileEntity = _userFactories.CreateProfileEntity(userEntity.Id, addressEntity.Id, roleEntity.Id);

            return true;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return false;
    }

    public IEnumerable<User> GetAllUsers()
    {
        try
        {
            var profileList = _profileRepository.GetAll();
            var userList = new List<User>();

            foreach (var entity in profileList)
            {
                var user = _userFactories.CompileFullUser(entity);
                userList.Add(user);
            }

            return userList;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return null!;
    }
}

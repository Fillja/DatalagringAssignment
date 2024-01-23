using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Respositories;
using System.Diagnostics;

namespace Infrastructure.Services;

public class UserService(UserRepository userRepository, RoleRepository roleRepository, AddressRepository addressRepository, VerificationRepository verificationRepository, ProfileRepository profileRepository, UserFactories userFactories)
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly VerificationRepository _verificationRepository = verificationRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;
    private readonly UserFactories _userFactories = userFactories;

    public bool CreateUser(UserRegistrationForm userReg)
    {
        try
        {
            var userEntity = _userFactories.CreateUserEntity(userReg.FirstName, userReg.LastName);

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

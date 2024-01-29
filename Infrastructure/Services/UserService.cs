using Infrastructure.Dtos;
using Infrastructure.Factories;
using Infrastructure.Respositories;
using System.Diagnostics;

namespace Infrastructure.Services;

public class UserService(ProfileRepository profileRepository, UserFactories userFactories)
{
    private readonly ProfileRepository _profileRepository = profileRepository;
    private readonly UserFactories _userFactories = userFactories;

    public bool CreateUser(UserRegistrationForm userReg)
    {
        try
        {
            var userEntity = _userFactories.CreateUserEntity(userReg.FirstName, userReg.LastName);

            var addressEntity = _userFactories.GetOrCreateAddressEntity(userReg.Street, userReg.PostalCode, userReg.City);

            var roleEntity = _userFactories.GetOrCreateRoleEntity(userReg.FirstName);

            var verificationEntity = _userFactories.GetOrCreateVerificationEntity(userEntity.Id, userReg.Email, userReg.Password);

            var profileEntity = _userFactories.CreateProfileEntity(userEntity.Id, addressEntity.Id, roleEntity.Id);

            if(profileEntity != null)
               return true;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return false;
    }


    // Passwordskyddande Get()-function som hämtar DTO istället för Entity.
    public IEnumerable<UserDto> GetAllUsers()
    {
        try
        {
            var profileList = _profileRepository.GetAll();
            var userList = new List<UserDto>();

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

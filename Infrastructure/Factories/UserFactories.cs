using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Respositories;
using System.Diagnostics;

namespace Infrastructure.Factories;

public class UserFactories(UserRepository userRepository, RoleRepository roleRepository, AddressRepository addressRepository, VerificationRepository verificationRepository, ProfileRepository profileRepository)
{

    private readonly UserRepository _userRepository = userRepository;
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly VerificationRepository _verificationRepository = verificationRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;

    public UserDto CompileFullUser(ProfileEntity entity)
    {
        try
        {
            var userEntity = _userRepository.GetOne(x => x.Id == entity.UserId);
            var addressEntity = _addressRepository.GetOne(x => x.Id == entity.AddressId);
            var verificationEntity = _verificationRepository.GetOne(x => x.UserId == entity.UserId);
            var roleEntity = _roleRepository.GetOne(x => x.Id == entity.RoleId);
            var user = new UserDto
            {
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Street = addressEntity.Street,
                City = addressEntity.City,
                PostalCode = addressEntity.PostalCode,
                Email = verificationEntity.Email,
                RoleName = roleEntity.RoleName
            };
            return user;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public UserEntity CreateUserEntity(string firstName, string lastName)
    {
        try
        {
            var userEntity = new UserEntity
            {
                FirstName = firstName,
                LastName = lastName
            };

            userEntity = _userRepository.Create(userEntity);
            return userEntity;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return null!;
    }

    public AddressEntity GetOrCreateAddressEntity(string street, string postalCode, string city)
    {
        try
        {
            if(_addressRepository.Exists(x => x.City == city && x.Street == street && x.PostalCode == postalCode))
            {
                var addressEntity = _addressRepository.GetOne(x => x.City == city && x.Street == street && x.PostalCode == postalCode);
                return addressEntity;
            }
            else
            {
                var addressEntity = new AddressEntity
                {
                    Street = street,
                    PostalCode = postalCode,
                    City = city,
                };

                addressEntity = _addressRepository.Create(addressEntity);
                return addressEntity;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return null!;
    }

    public RoleEntity GetOrCreateRoleEntity(string firstName)
    {
        try
        {
            if(firstName == "Hans" || firstName == "Joakim" || firstName == "Tommy" || firstName == "Robin")
            {
                var roleEntity = GetOrCreateRole("admin");
                return roleEntity;

            }
            else
            {
                var roleEntity = GetOrCreateRole("user");
                return roleEntity;

            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return null!;
    }

    public VerificationEntity GetOrCreateVerificationEntity(int userId, string email, string password)
    {
        try
        {
            if (!_verificationRepository.Exists(x => x.Email == email))
            {
                var verificationEntity = new VerificationEntity
                {
                    UserId = userId,
                    Email = email,
                    Password = password
                };

                verificationEntity = _verificationRepository.Create(verificationEntity);
                return verificationEntity;
            }
          }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return null!;
    }

    public ProfileEntity CreateProfileEntity(int userId, int addressId, int roleId)
    {
        try
        {
            var profileEntity = new ProfileEntity
            {
                UserId = userId,
                AddressId = addressId,
                RoleId = roleId,
            };

            profileEntity = _profileRepository.Create(profileEntity);
            return profileEntity;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return null!;
    }

    public RoleEntity GetOrCreateRole(string roleName)
    {
        try
        {
            if (_roleRepository.Exists(x => x.RoleName == roleName))
            {
                var roleEntity = _roleRepository.GetOne(x => x.RoleName == roleName);
                return roleEntity;
            }
            else
            {
                var roleEntity = new RoleEntity
                {
                    RoleName = roleName,
                };
                roleEntity = _roleRepository.Create(roleEntity);
                return roleEntity;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return null!;
    }
}

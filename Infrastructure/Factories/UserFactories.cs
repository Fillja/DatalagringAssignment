using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Respositories;
using System.Diagnostics;

namespace Infrastructure.Factories;

public class UserFactories
{

    private readonly UserRepository _userRepository;
    private readonly RoleRepository _roleRepository;
    private readonly AddressRepository _addressRepository;
    private readonly VerificationRepository _verificationRepository;
    private readonly ProfileRepository _profileRepository;

    public UserFactories(UserRepository userRepository, RoleRepository roleRepository, AddressRepository addressRepository, VerificationRepository verificationRepository, ProfileRepository profileRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _addressRepository = addressRepository;
        _verificationRepository = verificationRepository;
        _profileRepository = profileRepository;
    }

    public User CreateFullUser(ProfileEntity entity)
    {
        try
        {
            var userEntity = _userRepository.GetOne(x => x.Id == entity.UserId);
            var addressEntity = _addressRepository.GetOne(x => x.Id == entity.AddressId);
            var verificationEntity = _verificationRepository.GetOne(x => x.UserId == entity.UserId);
            var roleEntity = _roleRepository.GetOne(x => x.Id == entity.RoleId);
            var user = new User
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
}

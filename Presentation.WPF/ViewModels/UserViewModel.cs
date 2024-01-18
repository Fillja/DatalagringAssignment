using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Services;
using Presentation.WPF.Models;
using System.Collections.ObjectModel;

namespace Presentation.WPF.ViewModels;

public partial class UserViewModel(UserService userService) : ObservableObject
{

    private readonly UserService _userService = userService;

    [ObservableProperty]
    private ObservableCollection<UserModel> _users = new ObservableCollection<UserModel>();


    [RelayCommand]
    private void AddToList()
    {

    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Services;
using Presentation.WPF.Models;
using System.Collections.ObjectModel;

namespace Presentation.WPF.ViewModels;

public partial class MainViewModel(UserService userService) : ObservableObject
{
    private readonly UserService _userService = userService;

}

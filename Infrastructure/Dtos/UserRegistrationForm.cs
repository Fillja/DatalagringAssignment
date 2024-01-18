﻿namespace Infrastructure.Dtos;

public class UserRegistrationForm
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;

}
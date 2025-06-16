﻿namespace Marketplace.BBL.DTO.User;

public class UserDto
{
    public string Id { get; set; } 
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
}
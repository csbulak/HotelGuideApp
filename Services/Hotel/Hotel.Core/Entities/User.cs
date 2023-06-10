﻿namespace Hotel.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string PasswordHash { get; set; }
}
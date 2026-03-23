using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Services;


public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<List<simpleUserInfoDto>> GetUsersAsync()
    {
        var users = await _userManager.Users.Select(u => new simpleUserInfoDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
        }).ToListAsync();

        return users;
    }
}
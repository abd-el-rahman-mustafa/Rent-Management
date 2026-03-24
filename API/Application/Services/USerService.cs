using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using API.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Services;


public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private DataContext _context;

    public UserService(UserManager<AppUser> userManager, DataContext context)
    {
        _userManager = userManager;
        _context = context;

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

    public async Task<simpleUserInfoDto> GetUserByIdAsync(int id)
    {
        var user = await _userManager.Users.Where(u => u.Id == id).Select(u => new simpleUserInfoDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
        }).FirstOrDefaultAsync();

        return user;
    }

    // helper method to take the pagination parameters and return the paginated result

    private List<simpleUserInfoDto> Paginate(int pageNumber, int pageSize)
    {
        return _userManager.Users.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(u => new simpleUserInfoDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
        }).ToList();
    }

}
using API.Application.DTOs;

namespace API.Application.Interfaces;


public interface IUserService
{
    Task<List<simpleUserInfoDto>> GetUsersAsync();
}
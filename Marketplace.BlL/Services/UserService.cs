using AutoMapper;
using Marketplace.BLL.DTO.User;
using Marketplace.BLL.Exceptions;
using Marketplace.BLL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Marketplace.DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.BLL.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordHasher = new PasswordHasher<User>();
    }
    
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.UserRepository.FindAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> GetUserByIdAsync(int userId)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(userId);
        if (user == null) 
            throw new NotFoundException($"User with id {userId} not found");
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        var user = await _unitOfWork.UserRepository.FindUserByEmailAsync(email);
        if (user == null) 
            throw new NotFoundException($"User with email {email} not found");
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetUserByUsernameAsync(string username)
    {
        var user = await _unitOfWork.UserRepository.FindUserByUsernameAsync(username);
        if (user == null) 
            throw new NotFoundException($"User with username {username} not found");
        return _mapper.Map<UserDto>(user);
    }
    
    public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role)
    {
        var users = await _unitOfWork.UserRepository.FindUsersByRoleAsync(role);
        if (users == null) 
            throw new NotFoundException($"Users with role {role} not found");
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }
    
    public async Task UpdateUserAsync(UpdateUserDto dto)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(dto.UserId);
        if (user == null) 
            throw new NotFoundException($"User with id {dto.UserId} not found");
        
        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName = dto.LastName ?? user.LastName;
        user.MiddleName = dto.MiddleName ?? user.MiddleName;
        user.PhoneNumber = dto.Phone ?? user.PhoneNumber;

        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(userId);
        if (user == null) 
            throw new NotFoundException($"User with id {userId} not found");
        
        _unitOfWork.UserRepository.Delete(user);
        await _unitOfWork.SaveAsync();
    }
}
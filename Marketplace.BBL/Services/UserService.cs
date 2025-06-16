using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Marketplace.BBL.Services.Interfaces;
using Marketplace.DAL.Repositories.Interfaces;
using AutoMapper;
using Marketplace.BBL.DTO.User;
using Marketplace.BBL.Exceptions;
using Marketplace.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ValidationException = Marketplace.BBL.Exceptions.ValidationException;

namespace Marketplace.BBL.Services;

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
    
    public async Task ChangePasswordAsync(ChangePasswordDto dto)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(dto.UserId);
        if (user == null) 
            throw new NotFoundException($"User with id {dto.UserId} not found");

        var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.OldPassword);
        if (verify == PasswordVerificationResult.Failed)
            throw new ValidationException("Current password is incorrect");

        if (dto.NewPassword != dto.ConfirmPassword)
            throw new ValidationException("New password and confirmation do not match");
        
        user.PasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword);

        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task ChangeEmailAsync(ChangeEmailDto dto)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(dto.UserId);
        if (user == null) throw new Exception("User not found");

        var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (verify == PasswordVerificationResult.Failed)
            throw new ValidationException("Password is incorrect");
        
        if (await _unitOfWork.UserRepository.CheckUserExistsByEmailAsync(dto.NewEmail))
            throw new ValidationException("Email already in use");

        user.Email = dto.NewEmail;

        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task ChangeUsernameAsync(ChangeUsernameDto dto)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(dto.UserId);
        if (user == null) throw new Exception("User not found");

        var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (verify == PasswordVerificationResult.Failed)
            throw new ValidationException("Password is incorrect");
        
        if (await _unitOfWork.UserRepository.CheckUserExistsByUsernameAsync(dto.NewUsername))
            throw new ValidationException("Username already in use");

        user.UserName = dto.NewUsername;

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
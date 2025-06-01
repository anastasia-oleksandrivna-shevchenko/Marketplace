using Marketplace.BBL.Services.Interfaces;
using Marketplace.DAL.Repositories.Interfaces;
using AutoMapper;
using Marketplace.BBL.DTO.User;
using Marketplace.DAL.Entities;
using Microsoft.AspNetCore.Identity;

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

    public async Task<UserDto> RegisterUserAsync(CreateUserDto dto)
    {
        var existsEmail = await _unitOfWork.UserRepository.CheckUserExistsByEmailAsync(dto.Email);
        if (existsEmail)
            throw new Exception("Email already in use");
        
        var existsUsername = await _unitOfWork.UserRepository.CheckUserExistsByUsernameAsync(dto.Username);
        if (existsUsername)
            throw new Exception("Username already in use");
        
        var user = _mapper.Map<User>(dto);
        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
        
        await _unitOfWork.UserRepository.CreateAsync(user);
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<UserDto>(user);

    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.UserRepository.FindAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> GetUserByIdAsync(int userId)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(userId);
        if(user == null)
            throw new Exception("User not found");
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        var user = await _unitOfWork.UserRepository.FindUserByEmailAsync(email);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetUserByUsernameAsync(string username)
    {
        var user = await _unitOfWork.UserRepository.FindUserByUsernameAsync(username);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }
    
    public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role)
    {
        var users = await _unitOfWork.UserRepository.FindUsersByRoleAsync(role);
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }
    
    public async Task UpdateUserAsync(UpdateUserDto dto)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(dto.UserId);
        if (user == null) 
            throw new Exception("User not found");
        
        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName = dto.LastName ?? user.LastName;
        user.MiddleName = dto.MiddleName ?? user.MiddleName;
        user.Phone = dto.Phone ?? user.Phone;

        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task ChangePasswordAsync(ChangePasswordDto dto)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(dto.UserId);
        if (user == null) 
            throw new Exception("User not found");

        var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.OldPassword);
        if (verify == PasswordVerificationResult.Failed)
            throw new Exception("Current password is incorrect");

        if (dto.NewPassword != dto.ConfirmPassword)
            throw new Exception("New password and confirmation do not match");
        
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
            throw new Exception("Password is incorrect");
        
        if (await _unitOfWork.UserRepository.CheckUserExistsByEmailAsync(dto.NewEmail))
            throw new Exception("Email already in use");

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
            throw new Exception("Password is incorrect");
        
        if (await _unitOfWork.UserRepository.CheckUserExistsByUsernameAsync(dto.NewUsername))
            throw new Exception("Username already in use");

        user.Username = dto.NewUsername;

        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(userId);
        if(user == null)
            throw new Exception("User not found");
        
        _unitOfWork.UserRepository.Delete(user);
        await _unitOfWork.SaveAsync();
    }
    
}
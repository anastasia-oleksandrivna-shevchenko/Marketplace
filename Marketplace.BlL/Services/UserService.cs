using AutoMapper;
using Marketplace.BLL.DTO.User;
using Marketplace.BLL.Exceptions;
using Marketplace.BLL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Marketplace.DAL.UnitOfWork;
using System.Linq;

namespace Marketplace.BLL.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _unitOfWork.UserRepository.FindAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(userId, cancellationToken);
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
    
    public async Task UpdateUserAsync(UpdateUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(dto.UserId, cancellationToken);
        if (user == null) 
            throw new NotFoundException($"User with id {dto.UserId} not found");
        
        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName = dto.LastName ?? user.LastName;
        user.MiddleName = dto.MiddleName ?? user.MiddleName;
        user.PhoneNumber = dto.Phone ?? user.PhoneNumber;

        _unitOfWork.UserRepository.Update(user, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(userId, cancellationToken);
        if (user == null) 
            throw new NotFoundException($"User with id {userId} not found");
        
        _unitOfWork.UserRepository.Delete(user, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
   
}
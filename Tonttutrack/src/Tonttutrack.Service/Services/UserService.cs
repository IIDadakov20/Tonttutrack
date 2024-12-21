using Microsoft.AspNetCore.Identity;
using Tonttutrack.Service.Contracts;
using Tonttutrack.DataAccess.Data.Models;
using Tonttutrack.Service.DTO;
using AutoMapper;

namespace Tonttutrack.Service.Services;

internal class UserService : IUserService
{
	private readonly UserManager<User> _userManager;
	private readonly IMapper _mapper;

	public UserService(
		UserManager<User> userManager,
		IMapper mapper)
	{
		_userManager = userManager;
		_mapper = mapper;
	}

	public async Task<IdentityResult> CreateUserAsync(UserDTO userInput)
	{
		var user = _mapper.Map<User>(userInput);
		var identityResult = await _userManager.CreateAsync(user, userInput.Password);

		return identityResult;
	}

    public async Task<UserDTO?> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return null;

        return _mapper.Map<UserDTO>(user);
    }
}
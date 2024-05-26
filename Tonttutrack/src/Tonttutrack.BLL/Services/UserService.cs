using Microsoft.AspNetCore.Identity;
using Tonttutrack.BLL.Contracts;
using Tonttutrack.DAL.Data.Models;
using Tonttutrack.SharedModels.DTO;
using AutoMapper;

namespace Tonttutrack.BLL.Services;

internal class UserService : IUserService
{
	private readonly UserManager<User> _userManager;
	private readonly IMapper _mapper;

	public UserService(UserManager<User> userManager, IMapper mapper)
	{
		_userManager = userManager;
		_mapper = mapper;
	}

	public async Task<IdentityResult> CreateUserAsync(RegisterDTO userInput)
	{
		var user = _mapper.Map<User>(userInput);
		var identityResult = await _userManager.CreateAsync(user, userInput.Password);

		if (!identityResult.Succeeded)
		{
			return identityResult;
		}

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
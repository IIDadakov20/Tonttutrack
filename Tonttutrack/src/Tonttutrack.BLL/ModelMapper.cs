using AutoMapper;
using Tonttutrack.DAL.Data.Models;
using Tonttutrack.SharedModels.DTO;

namespace Tonttutrack.BLL;

public class ModelMapper : Profile
{
    public ModelMapper()
    {
        CreateMap<RegisterDTO, User>();
        CreateMap<User, UserDTO>();
    }
}
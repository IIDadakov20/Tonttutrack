using AutoMapper;
using Tonttutrack.DataAccess.Data.Models;
using Tonttutrack.Service.DTO;

namespace Tonttutrack.Service.Common;

public class ModelMapper : Profile
{
    public ModelMapper()
    {
        CreateMap<UserDTO, User>();
    }
}
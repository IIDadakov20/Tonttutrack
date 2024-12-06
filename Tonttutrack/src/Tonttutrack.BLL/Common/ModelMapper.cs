using AutoMapper;
using Tonttutrack.DataAccess.Data.Models;
using Tonttutrack.BLL.DTO;

namespace Tonttutrack.BLL.Common;

public class ModelMapper : Profile
{
    public ModelMapper()
    {
        CreateMap<UserDTO, User>();
    }
}
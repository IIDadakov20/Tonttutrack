using AutoMapper;
using Tonttutrack.DataAccess.Data.Models;
using Tonttutrack.Domain.DTOs.Authentication;

namespace Tonttutrack.Service.Common;

public class ModelMapper : Profile
{
    public ModelMapper()
    {
        CreateMap<RegisterDTO, User>();
    }
}
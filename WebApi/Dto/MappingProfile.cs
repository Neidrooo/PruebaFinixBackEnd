using AutoMapper;
using Core.Entities;

namespace WebApi.Dto
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Banks, BanksDto>();
        }
    }
}

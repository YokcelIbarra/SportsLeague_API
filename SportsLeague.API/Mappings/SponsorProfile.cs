using AutoMapper;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;

namespace SportsLeague.API.Mappings
{
    public class SponsorProfile : Profile
    {
        public SponsorProfile()
        {
            // Request → Entity
            CreateMap<SponsorRequestDTO, Sponsor>();

            // Entity → Response
            CreateMap<Sponsor, SponsorResponseDTO>();
        }
    }
}
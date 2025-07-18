using AuctionService.DTOs;
using AuctionService.Modules;
using AutoMapper;

namespace AuctionService.RequestHelpers;

public class MappingProfiles : Profile
{

    public MappingProfiles()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        CreateMap<Item, AuctionDto>();
        CreateMap<CreateAuctionDTO, Auction>()
            .ForMember(d => d.Item, o => o.MapFrom(s => s));

        CreateMap<CreateAuctionDTO, Item>();
        

    }
}

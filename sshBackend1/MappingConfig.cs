using sshBackend1.Models;
using sshBackend1.Models.DTOs;
using AutoMapper;


namespace sshBackend1
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Event, EventDTO>();
            CreateMap<EventDTO, Event>();

            CreateMap<Florist, FloristDTO>();
            CreateMap<FloristDTO, Florist>();

            CreateMap<VenueProvider, VenueProviderDTO>();
            CreateMap<VenueProviderDTO, VenueProvider>();

            CreateMap<OrderStatus, OrderStatusDTO>();
            CreateMap<OrderStatusDTO, OrderStatus>();

            CreateMap<VenueType, VenueTypeDTO>();
            CreateMap<VenueTypeDTO, VenueType>();

            CreateMap<Venue, VenueDTO>();
            CreateMap<VenueDTO, Venue>();

            CreateMap<VenueOrder, VenueOrderDTO>();
            CreateMap<VenueOrderDTO, VenueOrder>();

            CreateMap<FlowerArrangementType, FlowerArrangementTypeDTO>();
            CreateMap<FlowerArrangementTypeDTO, FlowerArrangementType>();

            CreateMap<FlowerArrangementOrder, FlowerArrangementOrderDTO>();
            CreateMap<FlowerArrangementOrderDTO, FlowerArrangementOrder>();

            CreateMap<FlowerArrangement, FlowerArrangementDTO>();
            CreateMap<FlowerArrangementDTO, FlowerArrangement>();

            CreateMap<RestaurantStatus, RestaurantStatusDTO>();
            CreateMap<RestaurantStatusDTO, RestaurantStatus>();

            CreateMap<Restaurant, RestaurantDTO>();
            CreateMap<RestaurantDTO, Restaurant>();

            CreateMap<Table, TableDTO>();
            CreateMap<TableDTO, Table>();

            CreateMap<PerformerType,PerformerTypeDTO>();
            CreateMap<PerformerTypeDTO, PerformerType>();

            CreateMap<MusicProvider, MusicProviderDTO>();
            CreateMap<MusicProviderDTO, MusicProvider>();

            CreateMap<MusicProviderOrder, MusicProviderOrderDTO>();
            CreateMap<MusicProviderOrderDTO, MusicProviderOrder>();

            CreateMap<MenuTypeDTO,MenuType>();
            CreateMap<MenuType, MenuTypeDTO>();

            CreateMap<MenuOrderDTO, MenuOrder>();
            CreateMap<MenuOrder, MenuOrderDTO>();

            CreateMap<Menu, MenuDTO>();
            CreateMap<MenuDTO, Menu>();

        }
    }
}

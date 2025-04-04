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
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C_
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Event, GetEventDto>();
            CreateMap<AddEventDto, Event>();
            CreateMap<UpdateEventDto, Event>();
        }
    }
}
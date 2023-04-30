using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C_.Services.EventService
{
    public class EventService : IEventService
    {
        private static List<Event> events = new List<Event>{
            new Event(){Name="Gangna", Id=1},
            new Event(){Name="Euroviziaaa", Id=2},
        }; 
        private readonly IMapper _mapper; 
        public EventService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> AddEvent(AddEventDto newEvent)
        {
            var currentEvent = _mapper.Map<Event>(newEvent);
            currentEvent.Id = events.Max(c => c.Id) + 1;

            events.Add(currentEvent);

            var serviceResponse = new ServiceResponse<string>();
            serviceResponse.Data = "Added";
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetEventDto>>> GetAllEvents()
        {
            var serviceResponse = new ServiceResponse<List<GetEventDto>>();
            serviceResponse.Data = events.Select(c => _mapper.Map<GetEventDto>(c)).ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetEventDto>> GetSingleEvent(int id)
        {
            var serviceResponse = new ServiceResponse<GetEventDto>();
            var singleEvent = events.FirstOrDefault(c=> c.Id == id);
            serviceResponse.Data = _mapper.Map<GetEventDto>(singleEvent);

            return serviceResponse;
        }
    }
}
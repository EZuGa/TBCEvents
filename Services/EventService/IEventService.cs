using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C_.Services.EventService
{
    public interface IEventService
    {
        Task<ServiceResponse<List<GetEventDto>>> GetAllEvents();
        Task<ServiceResponse<GetEventDto>> GetSingleEvent(int id);
        Task<ServiceResponse<string>> AddEvent(AddEventDto newEvent);
    }
}
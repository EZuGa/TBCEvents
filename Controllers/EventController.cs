using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace C_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController: ControllerBase
    {
        IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }


        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetEventDto>>>> Get(){
            return Ok(await _eventService.GetAllEvents());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetEventDto>>> GetSingle(int id){
            return Ok(await _eventService.GetSingleEvent(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<string>>> AddEvent(AddEventDto newEvent){
            return Ok(await _eventService.AddEvent(newEvent));
        }
        [HttpPut]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateEvent(UpdateEventDto updatedEvent){
            return Ok(await _eventService.UpdateEvent(updatedEvent));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteEvent(int id){
            return Ok(await _eventService.DeleteEvent(id));
        }
        
    }
}
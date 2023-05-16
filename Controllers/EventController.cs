using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace C_.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EventController: ControllerBase
    {
        IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }


        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetEventDto>>>> Get(){

        if (HttpContext.GetRequestedApiVersion().MajorVersion == 2)
        {
            return Ok("This is version 2.0");
        }
            return Ok(await _eventService.GetAllEvents());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetEventDto>>> GetSingle(int id){
            return Ok(await _eventService.GetSingleEvent(id));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<string>>> AddEvent(AddEventDto newEvent){
            Console.WriteLine(HttpContext.Request.Headers["Authorization"].ToString());
            return Ok(await _eventService.AddEvent(newEvent));
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateEvent(UpdateEventDto updatedEvent){
            return Ok(await _eventService.UpdateEvent(updatedEvent));
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteEvent(int id){
            return Ok(await _eventService.DeleteEvent(id));
        }
        [HttpPost("makeLive/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<string>>> MakeLive(int id){
            return Ok(await _eventService.MakeLive(id));
        }
        [HttpPost("setUpdateDeadline/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<string>>> setUpdateDeadline(int id, DateTime updateDeadline){
            return Ok(await _eventService.SetUpdateDeadline(id, updateDeadline));
        }
        
    }
}
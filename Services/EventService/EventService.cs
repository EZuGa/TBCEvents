using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace C_.Services.EventService
{
    public class EventService : IEventService
    {
        // private static List<Event> events = new List<Event>{
        //     new Event(){Name="Gangna", Id=1},
        //     new Event(){Name="Euroviziaaa", Id=2},
        // }; 
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public EventService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<string>> AddEvent(AddEventDto newEvent)
        {
            var authToken = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"].ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(authToken.Replace("bearer ", ""));
            var userName = token.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")!.Value;
            // Console.WriteLine(userName);
            var currentEvent = _mapper.Map<Event>(newEvent);
            currentEvent.CreatedBy = userName;
            // Console.WriteLine("ABC");
            // Console.WriteLine(userName);

            

            _context.Events.Add(currentEvent);
            await _context.SaveChangesAsync();

            var serviceResponse = new ServiceResponse<string>();
            serviceResponse.Data = "Added";
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetEventDto>>> GetAllEvents()
        {
            var serviceResponse = new ServiceResponse<List<GetEventDto>>();
            var dbEvents = await _context.Events.ToListAsync();

            serviceResponse.Data = dbEvents.Select(c => _mapper.Map<GetEventDto>(c)).ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetEventDto>> GetSingleEvent(int id)
        {
            var serviceResponse = new ServiceResponse<GetEventDto>();
            var dbEvent = await _context.Events.FirstOrDefaultAsync(c=> c.Id == id);
            serviceResponse.Data = _mapper.Map<GetEventDto>(dbEvent);

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetEventDto>> UpdateEvent(UpdateEventDto updateEvent)
        {
            var serviceResponse = new ServiceResponse<GetEventDto>();
            try{
            // var tbcEvent = events.FirstOrDefault(c => c.Id == updateEvent.Id);
            var tbcEvent = await _context.Events.FindAsync(updateEvent.Id);

            if(tbcEvent is null)
                throw new Exception("Event doesn't exist and could be updated");

            _mapper.Map(updateEvent, tbcEvent);

            serviceResponse.Data = _mapper.Map<GetEventDto>(tbcEvent);

            await _context.SaveChangesAsync();
            }catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }


            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> DeleteEvent(int id)
        {
            var serviceResponse = new ServiceResponse<string>();
            // var tbcEvent = events.First(c => c.Id == id);
            var tbcEvent = await _context.Events.FindAsync(id);


            _context.Events.Remove(tbcEvent);

            await _context.SaveChangesAsync();

            serviceResponse.Data = "REMOVED!";

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> MakeLive(int id)
        {
            var serviceResponse = new ServiceResponse<string>();

            var eventToUpdate = await _context.Events.FindAsync(id);

            eventToUpdate.IsActive = true;

            serviceResponse.Data = "Is live!";
            await _context.SaveChangesAsync();

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> SetUpdateDeadline(int id, DateTime updateDeadline)
        {
            var serviceResponse = new ServiceResponse<string>();

            var eventToUpdate = await _context.Events.FindAsync(id);

            eventToUpdate.ModificationDeadline = updateDeadline;
            await _context.SaveChangesAsync();

            serviceResponse.Data = "Updated";

            return serviceResponse;

        }
    }
}
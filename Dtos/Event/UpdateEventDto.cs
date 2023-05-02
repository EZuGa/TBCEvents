using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C_.Dtos.Event
{
    public class UpdateEventDto
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = "Unnamed";
        public string Details { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public DateTime Date { get; set; }
        public DateTime ModificationDeadline { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}
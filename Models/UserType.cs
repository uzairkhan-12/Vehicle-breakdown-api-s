using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleBreakdownAssistant.Models
{
    public class UserType
    {
        public int Id { get; set; }
        public string UserTypeName { get; set; }
        public Boolean IsActive { get; set; }
    }
}

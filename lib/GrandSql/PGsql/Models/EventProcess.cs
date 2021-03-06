using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrandSql.PGsql.Models
{
    public partial class EventProcess
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public string ArchitectureId { get; set; }
        public string UserId { get; set; }
        public string Attachdata { get; set; }
        public string CommandPlanId { get; set; }
        public DateTime? Createtime { get; set; }
    }
}

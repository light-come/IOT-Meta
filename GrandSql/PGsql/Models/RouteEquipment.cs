﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrandSql.PGsql.Models
{
    public partial class RouteEquipment
    {
        public string Id { get; set; }
        public string RouteId { get; set; }
        public string Equipmentcode { get; set; }
        public short? Important { get; set; }
    }
}

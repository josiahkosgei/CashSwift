﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashSwift.Finacle.Integration.DataAccess.Entities
{
    public partial class GetDeviceConfigByUserGroupResult
    {
        public Guid id { get; set; }
        public int group_id { get; set; }
        public string config_id { get; set; }
        public string config_value { get; set; }
    }
}

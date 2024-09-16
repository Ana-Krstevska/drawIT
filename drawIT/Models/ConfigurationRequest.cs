﻿using drawIT.Models.Enums;

namespace drawIT.Models
{
    public class ConfigurationRequest
    {
        public string? Id {  get; set; }
        public CloudProvider Cloud {  get; set; }
        public List<string>? CloudServices { get; set; }
        public string? UserDescription { get; set; }
    }
}
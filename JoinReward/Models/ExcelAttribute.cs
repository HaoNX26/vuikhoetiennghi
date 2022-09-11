using System;

namespace PAN.Warranty.Models
{
    public class ExportAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string DisplayFormat { get; set; }
    }

    public class ImportAttribute : Attribute
    {
        public string DisplayName { get; set; }        
    }
}
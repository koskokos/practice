using System;
using System.ComponentModel.DataAnnotations;

namespace epiPGSInter.Tmigma.Data
{
    public class JsonEvent
    {
        public string Event { get; set; }
        
        public DateTime? Time { get; set; }
        
        public string Contact { get; set; }
        
        public string Topic { get; set; }
        
        public string Comments { get; set; }
        
        public string SourceReliability { get; set; }

        public string User { get; set; }

        public int Id { get; set; }

    }
}
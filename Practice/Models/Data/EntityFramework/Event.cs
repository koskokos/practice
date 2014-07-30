using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace epiPGSInter.Tmigma.Data
{
    public class Event
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public string Contact { get; set; }
        public string Topic { get; set; }
        public string Comments { get; set; }
        public int User_Id { get; set; }
        public int SourceReliability_Id { get; set; }
        [ForeignKey("SourceReliability_Id")]
        public SourceReliability SourceReliability { get; set; }
        [ForeignKey("User_Id")]
        public User User { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Rallypoint.Models
{
    public class UpdateScoresViewModel : BaseEntity
    {
        [Required,Display(Name="Round One Score")]
        public int playeroneroundoneScore {get;set;}
        [Required,Display(Name="Round One Score")]
        public int playertworoundoneScore {get;set;}
        [Required,Display(Name="Round Two Score")]
        public int playeroneroundtwoScore {get;set;}
        [Required,Display(Name="Round Two Score")]
        public int playertworoundtwoScore {get;set;}
        [Display(Name="Round Three Score")]
        public int? playeroneroundthreeScore {get;set;}
        [Display(Name="Round Three Score")]
        public int? playertworoundthreeScore {get;set;}
    }
}

using System;
using System.ComponentModel.DataAnnotations;

public class Later : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var d = Convert.ToDateTime(value);
        return d >= DateTime.Now.AddDays(1);
    }
}

namespace Rallypoint.Models
{
    public class GameViewModel : BaseEntity
    {
        [Required,Display(Name="Player One")]
        public int playeroneId {get;set;}
        [Required,Display(Name="Player Two")]
        public int playertwoId {get;set;}
        [Required,Display(Name="Location")]
        public string address {get;set;}
        [Later(ErrorMessage="Must be later than today."), Required, Display(Name="Game Date")]
        public DateTime? date {get;set;}
    }
}

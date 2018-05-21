using System.ComponentModel.DataAnnotations;

namespace Rallypoint.Models{

	public class Rallypoint:BaseEntity{

		public int Id { get; set; }
	}

	public class User : BaseEntity
    {
        
        public string first_name { get; set; }

        public string last_name { get; set; }

        public string email { get; set; }

        public string password { get; set; }
    }
}

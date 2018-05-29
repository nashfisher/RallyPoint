using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rallypoint.Models{
	
	public class User: BaseEntity{
		public string first_name {get; set;}
		public string last_name {get; set;}
		public string username {get; set;}
		public string email {get; set;}
		public string password {get; set;}

		public string imagelink {get; set;}
		public int wins {get; set;}
		public int losses {get; set;}
		public bool admin {get; set;}
		public List<Post> posts {get; set;}
		public List<Game> gamescreated {get; set;}
		public List<Game> gamesjoined {get; set;}
		public List<Like> likedpost {get; set;}
		public User(){
			posts =  new List<Post>();
			gamescreated = new List<Game>();
			gamesjoined = new List<Game>();
			likedpost = new List<Like>();
			admin = false;
		}
	}

	public class Game: BaseEntity{

		public int? playertwoId {get; set;}
		public User playertwo {get; set;}

		public int? playeroneId {get; set;}
		public User playerone {get; set;}

		public DateTime date {get; set;}
		public string address {get; set;}

		public int? playeroneroundoneScore {get; set;}
		public int? playertworoundoneScore {get; set;}
		public int? playeroneroundtwoScore {get; set;}
		public int? playertworoundtwoScore {get; set;}
		public int? playeroneroundthreeScore {get; set;}
		public int? playertworoundthreeScore {get; set;}


	}

	public class Post : BaseEntity{

		public string title {get;set;}

		public string category {get;set;}
		public string post {get; set;}

		public User user {get; set;}

		public int? UserId {get; set;}

		public int? likes {get; set;}

		List<Like> likedpost {get; set;}

		public Post(){
			likedpost = new List<Like>();
		}

	}

	public class Like : BaseEntity{
		public User user {get; set;}

		public int? UserId {get; set;}

		public Post post {get; set;}

		public int? PostId {get; set;}
	}


	public class Comment : BaseEntity{

		public string comment {get; set;}

		public int? PostId {get; set;}

		public Post post {get; set;}

		public int? UserId {get; set;}

		public User user {get; set;}

		public List<Like> commentlikes {get; set;}


		public Comment(){
			commentlikes = new List<Like>();
		}
	}

}

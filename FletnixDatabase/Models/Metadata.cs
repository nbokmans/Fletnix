using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FletnixDatabase.Models
{
    public class CastMetadata { }

    public class CastMemberMetadata { }

    public class CountryMetadata { }

    public class GenreMetadata { }

    public class KeywordMetadata { }

    public class MovieMetadata
    {
        [DisplayName("Sequel of")]
        public Nullable<int> SequelOf_TitleID { get; set; }
    }

    public class MovieDiscountMetadata { }

    public class MovieReviewMetadata { }

    public class PurchaseHistoryMetadata { }

    public class ReviewRatingMetadata { }

    public class RoleMetadata { }

    public class UserMetadata
    {
        [DisplayName("First name")]
        public string Firstname { get; set; }
        [DisplayName("Last name")]
        public string Lastname { get; set; }
        [DisplayName("Gender")]
        public string Gender { get; set; }
        [DisplayName("Date of birth")]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName("Username")]
        public string Username { get; set; }
        [DisplayName("User type")]
        public string UserType { get; set; }
        [DisplayName("Email address")]
        public string EmailAddress { get; set; }
        [DisplayName("Password")]
        public string Password { get; set; }
        [DisplayName("Bank account number")]
        public string BankAccountNumber { get; set; }
    
    }
    public class UserDiscountMetadata { }

    public class UserTypeMetadata { }

    public class WatchHistoryMetadata { }

    public class SeasonMetadata { }

    public class SeriesMetadata
    {
        
        [DisplayName("Series name")]
        public string Name { get; set; }
    }

    public class TitleMetadata
    {
        [DisplayName("Title")]
        [Column("Title")]
        public string Title1 { get; set; }
        [DisplayName("Price")]
        public decimal Price { get; set; }
        [DisplayName("Is available")]
        public bool IsWatchable { get; set; }
        [DisplayName("Run time")]
        public Nullable<int> Duration { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Publication date")]
        public Nullable<System.DateTime> PublicationDate { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal DiscountPercentage { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<decimal> AverageRating { get; set; }
    }
}



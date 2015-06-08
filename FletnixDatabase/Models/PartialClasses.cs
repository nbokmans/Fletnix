using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FletnixDatabase.Models
{
    [MetadataType(typeof(CastMetadata))]
    public partial class Cast
    {
    }

    [MetadataType(typeof(CastMemberMetadata))]
    public partial class CastMember
    {
    }

    [MetadataType(typeof(CountryMetadata))]
    public partial class Country
    {
    }

    [MetadataType(typeof(GenreMetadata))]
    public partial class Genre
    {
    }

    [MetadataType(typeof(KeywordMetadata))]
    public partial class Keyword
    {
    }

    [MetadataType(typeof(MovieMetadata))]
    public partial class Movie
    {
    }

    [MetadataType(typeof(MovieDiscountMetadata))]
    public partial class MovieDiscount
    {
    }

    [MetadataType(typeof(MovieReviewMetadata))]
    public partial class MovieReview
    {
    }

    [MetadataType(typeof(PurchaseHistoryMetadata))]
    public partial class PurchaseHistory
    {
    }

    [MetadataType(typeof(ReviewRatingMetadata))]
    public partial class ReviewRating
    {
        public static List<SelectListItem> AllowedRatings
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "1", Value = "1"},
                    new SelectListItem { Text = "2", Value = "2"},
                    new SelectListItem { Text = "3", Value = "3"},
                    new SelectListItem { Text = "4", Value = "4"},
                    new SelectListItem { Text = "5", Value = "5"}
                };
            }
        }
    }

    [MetadataType(typeof(RoleMetadata))]
    public partial class Role
    {
    }

    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public const string DEFAULT_USER_TYPE = "Customer";
        public const bool DEFAULT_IS_VERIFIED = false;
    }

    [MetadataType(typeof(UserDiscountMetadata))]
    public partial class UserDiscount
    {
    }

    [MetadataType(typeof(UserTypeMetadata))]
    public partial class UserType
    {
    }

    [MetadataType(typeof(WatchHistoryMetadata))]
    public partial class WatchHistory
    {
    }

    [MetadataType(typeof(SeasonMetadata))]
    public partial class Season
    {
    }

    [MetadataType(typeof(SeriesMetadata))]
    public partial class Series
    {
    }

    [MetadataType(typeof(TitleMetadata))]
    public partial class Title
    {
        public String PublicationDateDisplayString
        {
            get
            {
                if (PublicationDate != null)
                    return PublicationDate.Value.ToString("d");
                return "";
            }
        }
    }
    public partial class TitleReview
    {

        public double AverageRating
        {
            get
            {
                if (this.ReviewRating != null && this.ReviewRating.Count > 0)
                {
                    var average = (int)(this.ReviewRating.Average(a => a.Rating) * 10);
                    return (double) average / 10;
                }
                return 0;
            }
            private set { }
        }

        public static List<SelectListItem> AllowedRatings
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "1", Value = "1"},
                    new SelectListItem { Text = "2", Value = "2"},
                    new SelectListItem { Text = "3", Value = "3"},
                    new SelectListItem { Text = "4", Value = "4"},
                    new SelectListItem { Text = "5", Value = "5"},
                    new SelectListItem { Text = "6", Value = "6"},
                    new SelectListItem { Text = "7", Value = "7"},
                    new SelectListItem { Text = "8", Value = "8"},
                    new SelectListItem { Text = "9", Value = "9"},
                    new SelectListItem { Text = "10", Value = "10"}
                };
            }
        }
      }
}



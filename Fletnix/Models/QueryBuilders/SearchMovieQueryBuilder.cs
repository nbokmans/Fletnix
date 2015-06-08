using System.Collections.Generic;
using Microsoft.Ajax.Utilities;

namespace Fletnix.Models.QueryBuilders
{
    public class SearchMovieQueryBuilder : AbstractQueryBuilder
    {
        public SearchMovieQueryBuilder()
        {
            Columns = new List<string>
            {
                "title.TitleID", 
                "title.title",
                "title.price",
                "title.duration",
                "title.PublicationDate",
                "title.AverageRating"
            };
            From = @"Title title";
        }
        public SearchMovieQueryBuilder(List<string> columns, string from)
        {
            Columns = columns;
            From = from;
        }

        public SearchMovieQueryBuilder Title(string title)
        {
            if (!title.IsNullOrWhiteSpace())
            {
                var titleWhere = new WherePart("Title", title, "LIKE", "VARCHAR");
                whereQueue.Enqueue(titleWhere);
            }
            return this;
        }

        public SearchMovieQueryBuilder Movie()
        {
            var movieJoin = new JoinPart("Title", "Movie", "TitleID");
            joinQueue.Enqueue(movieJoin);

            return this;
        }

        public SearchMovieQueryBuilder TvSeries()
        {
            var episodeJoin = new JoinPart("Title", "TvEpisode", "TitleID");
            joinQueue.Enqueue(episodeJoin);

            return this;
        }

        public SearchMovieQueryBuilder ToYear(string toYear)
        {
            if (!toYear.IsNullOrWhiteSpace())
            {
                var toYearWhere = new WherePart("PublicationDate", toYear, "<=", "DATE");
                whereQueue.Enqueue(toYearWhere);
            }
            return this;
        }

        public SearchMovieQueryBuilder FromYear(string fromYear)
        {
            if (!fromYear.IsNullOrWhiteSpace())
            {
                var fromYearWhere = new WherePart("PublicationDate", fromYear + "0101", ">=", "DATE");
                whereQueue.Enqueue(fromYearWhere);
            }
            return this;
        }
        public SearchMovieQueryBuilder Genre(string genre)
        {
            if (!genre.IsNullOrWhiteSpace())
            {
                var genreJoin = new JoinPart("Title", "Movie_Genre", "TitleID");
                joinQueue.Enqueue(genreJoin);
                var genreWhere = new WherePart("Genre", genre, "=", "VARCHAR");
                whereQueue.Enqueue(genreWhere);
            }
            return this;
        }

        public SearchMovieQueryBuilder Keyword(string keyword)
        {
            if (!keyword.IsNullOrWhiteSpace())
            {
                var keywordJoin = new JoinPart("Title", "Title_Keyword", "TitleID");
                joinQueue.Enqueue(keywordJoin);
                var keywordWhere = new WherePart("Keyword", keyword, "=", "VARCHAR");
                whereQueue.Enqueue(keywordWhere);
            }
            return this;
        }

        public SearchMovieQueryBuilder Firstname(string firstname)
        {
            if (!firstname.IsNullOrWhiteSpace())
            {
                var firstnameJoin = new JoinPart("Title", "Cast", "TitleID");
                joinQueue.Enqueue(firstnameJoin);
                var firstnameJoin2 = new JoinPart("Cast", "CastMember", "CastMemberID");
                joinQueue.Enqueue(firstnameJoin2);
                var firstnameWhere = new WherePart("Firstname", firstname, "LIKE", "VARCHAR");
                whereQueue.Enqueue(firstnameWhere);
            }
            return this;
        }


        public override AbstractQueryBuilder Ordering(string column, string ordering)
        {
            switch (column)
            {
                case "Rating":
                    order = ordering == "Descending"
                        ? " ORDER BY AverageRating DESC, Title ASC "
                        : " ORDER BY AverageRating ASC, Title ASC ";
                    break;
                case "Watched":
                    order = ordering == "Descending"
                        ? " ORDER BY Title DESC "
                        : " ORDER BY Title ASC ";
                    break;
                case "TitleID":
                    order = ordering == "Descending"
                        ? " ORDER BY TitleID DESC "
                        : " ORDER BY TitleID ASC ";
                    break;
                default:
                    order = ordering == "Descending"
                        ? " ORDER BY Title DESC "
                        : " ORDER BY Title ASC ";
                    break;
            }
            return this;
        }
    }
}
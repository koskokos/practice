using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using epiPGSInter.Tmigma.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Text;
using System.Data.SqlClient;

namespace epiPGSInter.Tmigma.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("tmigmaDB")
        { }

        public DbSet<Event> Events { get; set; }
        public DbSet<SourceReliability> SourceReliability { get; set; }
        public DbSet<User> User { get; set; }

        /// <summary>
        /// Constructs and executes query to get events from db, applying filtering by user name and by event name, reverse ordering by id - newer are first - and controlling offset and count of returned items. Each parameter is independent from others.
        /// </summary>
        /// <param name="user">User name to filter by</param>
        /// <param name="eventName">Event name to filter by</param>
        /// <param name="count">Count of elements to fetch</param>
        /// <param name="offset">Count of elements to skip</param>
        /// <returns></returns>
        public IEnumerable<JsonEvent> GetEvents(string user, string eventName, int? count, int? offset)
        {
            var userFull = User.FirstOrDefault(us => us.Name == user);

            const string query = // query template, common part without filtering and paging
@"SELECT         e.Id, e.Text as Event, e.Time, e.Contact, e.Topic, e.Comments, 
                         sr.Text AS SourceReliability, u.Name AS [User]
FROM            dbo.Events as e LEFT OUTER JOIN
                         dbo.Users as u ON e.User_Id = u.Id LEFT OUTER JOIN
                         dbo.SourceReliabilities as sr ON e.SourceReliability_Id = sr.Id {0}
order by e.Id desc{1}";
            const string userIdStr = "e.User_Id = {0}"; // template for filtering after UserId
            const string eventNameStr = "e.Text = @eventNameStr"; // parametrized template for filtering after eventName for protection against SQL Injection

            #region Constructing where clause

            var u = userFull != null;
            var e = eventName != null;
            
            string where = "";

            if (u & e)
                where = string.Format("WHERE {0} and {1}",
                    string.Format(userIdStr, userFull.Id),
                    eventNameStr);
            else if (u)
                where = string.Format("WHERE " + userIdStr, userFull.Id);
            else if (e)
                where = "WHERE " + eventNameStr;

            #endregion

            #region constructing offset-fetch

            string offsetFetch = string.Format(" OFFSET {0} ROWS", offset.HasValue ? offset.Value : 0);
            if (count.HasValue) offsetFetch += string.Format(" FETCH NEXT {0} ROWS ONLY", count.Value);
            
            #endregion

            var resQ = string.Format(query, where, offsetFetch);



            return e 
                ? this.Database.SqlQuery<JsonEvent>(resQ, new SqlParameter("@eventNameStr", eventName)) // if eventName is not null we include eventName to parametrized query
                : this.Database.SqlQuery<JsonEvent>(resQ);
        }

    }
}
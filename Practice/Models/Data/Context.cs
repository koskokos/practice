using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace epiPGSInter.Tmigma.Data
{
    public class Context : IDisposable
    {
        DataContext db = new DataContext();

        public void AddEvents(IEnumerable<JsonEvent> events)
        {
            var ret = db.Events.AddRange(events.Select(ToDbEvent));
            db.SaveChanges();
        }

        public int AddEvent(JsonEvent e)
        {
            var inserted = db.Events.Add(ToDbEvent(e));
            db.SaveChanges();
            e.Id = inserted.Id;
            e.Time = inserted.Time;
            return inserted.Id;
        }

        public IEnumerable<JsonEvent> GetEvents(string user, string eventName, int? count, int? offset)
        {
            return db.GetEvents(user, eventName, count, offset);
        }

        public JsonEvent GetEvent(int id)
        {
            var ev = db.Events
                .Include(e => e.SourceReliability)
                .Include(e=>e.User)
                .SingleOrDefault(e => e.Id == id);
            if (ev == null) return null;
            return ToJsonEvent(ev);
        }

        private JsonEvent ToJsonEvent(Event e)
        {
            return new JsonEvent
            {
                Id = e.Id,
                Event = e.Text,
                Comments = e.Comments,
                Contact = e.Contact,
                Time = e.Time,
                SourceReliability = e.SourceReliability != null ? e.SourceReliability.Text : null,
                Topic = e.Topic,
                User = e.User != null ? e.User.Name : null
            };
        }

        private Event ToDbEvent(JsonEvent e)
        {
            return new Event
            {
                Text = e.Event,
                Comments = e.Comments,
                Contact = e.Contact,
                Time = e.Time.HasValue ? e.Time.Value : DateTime.Now,
                SourceReliability = e.SourceReliability != null ? db
                    .SourceReliability
                    .SingleOrDefault(sr => sr.Text == e.SourceReliability)
                    ?? new SourceReliability { Text = e.SourceReliability } : null,
                Topic = e.Topic,
                User = e.User != null ? db
                    .User
                    .SingleOrDefault(u => u.Name == e.User)
                    ?? new User { Name = e.User } : null
            };
        }

        public void Dispose()
        {
            db.Dispose();
        }
        
        public void UpdateEvent(JsonEvent jevent)
        {
            var ev = db.Events.Find(jevent.Id);
            var nev = ToDbEvent(jevent);

            ev.Comments = nev.Comments;
            ev.Contact = nev.Contact;
            ev.SourceReliability = nev.SourceReliability;
            ev.Text = nev.Text;
            ev.Time = nev.Time;
            ev.Topic = nev.Topic;
            ev.User = nev.User;

            db.SaveChanges();
        }

        public JsonEvent DeleteEvent(int id)
        {
            var je = db.Events
                .Include(e=>e.SourceReliability)
                .Include(e=>e.User)
                .Single(e=>e.Id==id);
            var r = ToJsonEvent(je);
            db.Events.Remove(je);
            db.SaveChanges();
            return r;
        }
    }
}
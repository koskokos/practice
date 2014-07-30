using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using epiPGSInter.Tmigma.Data;

namespace Practice.Controllers
{
    /*
    To add a route for this controller, merge these statements into the Register method of the WebApiConfig class. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using epiPGSInter.Tmigma.Data;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Event>("Event");
    builder.EntitySet<SourceReliability>("SourceReliability"); 
    builder.EntitySet<User>("User"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class EventController : ODataController
    {
        private DataContext db = new DataContext();

        // GET odata/Event
        [Queryable]
        public IQueryable<Event> GetEvent()
        {
            return db.Events;
        }

        // GET odata/Event(5)
        [Queryable]
        public SingleResult<Event> GetEvent([FromODataUri] int key)
        {
            return SingleResult.Create(db.Events.Where(@event => @event.Id == key));
        }

        // PUT odata/Event(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != @event.Id)
            {
                return BadRequest();
            }

            db.Entry(@event).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(@event);
        }

        // POST odata/Event
        public async Task<IHttpActionResult> Post(Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Events.Add(@event);
            await db.SaveChangesAsync();

            return Created(@event);
        }

        // PATCH odata/Event(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Event> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Event @event = await db.Events.FindAsync(key);
            if (@event == null)
            {
                return NotFound();
            }

            patch.Patch(@event);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(@event);
        }

        // DELETE odata/Event(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Event @event = await db.Events.FindAsync(key);
            if (@event == null)
            {
                return NotFound();
            }

            db.Events.Remove(@event);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Event(5)/SourceReliability
        [Queryable]
        public SingleResult<SourceReliability> GetSourceReliability([FromODataUri] int key)
        {
            return SingleResult.Create(db.Events.Where(m => m.Id == key).Select(m => m.SourceReliability));
        }

        // GET odata/Event(5)/User
        [Queryable]
        public SingleResult<User> GetUser([FromODataUri] int key)
        {
            return SingleResult.Create(db.Events.Where(m => m.Id == key).Select(m => m.User));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(int key)
        {
            return db.Events.Count(e => e.Id == key) > 0;
        }
    }
}

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
    builder.EntitySet<SourceReliability>("SourceReliability");
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SourceReliabilityController : ODataController
    {
        private DataContext db = new DataContext();

        // GET odata/SourceReliability
        [Queryable]
        public IQueryable<SourceReliability> GetSourceReliability()
        {
            return db.SourceReliability;
        }

        // GET odata/SourceReliability(5)
        [Queryable]
        public SingleResult<SourceReliability> GetSourceReliability([FromODataUri] int key)
        {
            return SingleResult.Create(db.SourceReliability.Where(sourcereliability => sourcereliability.Id == key));
        }

        // PUT odata/SourceReliability(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, SourceReliability sourcereliability)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != sourcereliability.Id)
            {
                return BadRequest();
            }

            db.Entry(sourcereliability).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SourceReliabilityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(sourcereliability);
        }

        // POST odata/SourceReliability
        public async Task<IHttpActionResult> Post(SourceReliability sourcereliability)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SourceReliability.Add(sourcereliability);
            await db.SaveChangesAsync();

            return Created(sourcereliability);
        }

        // PATCH odata/SourceReliability(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<SourceReliability> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SourceReliability sourcereliability = await db.SourceReliability.FindAsync(key);
            if (sourcereliability == null)
            {
                return NotFound();
            }

            patch.Patch(sourcereliability);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SourceReliabilityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(sourcereliability);
        }

        // DELETE odata/SourceReliability(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            SourceReliability sourcereliability = await db.SourceReliability.FindAsync(key);
            if (sourcereliability == null)
            {
                return NotFound();
            }

            db.SourceReliability.Remove(sourcereliability);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SourceReliabilityExists(int key)
        {
            return db.SourceReliability.Count(e => e.Id == key) > 0;
        }
    }
}

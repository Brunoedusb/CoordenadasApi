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
using System.Web.Http.Cors;
using System.Web.Http.Description;
using API.Models;
using Microsoft.AspNet.Identity;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Authorize]
    public class AddressController : ApiController
    {
        private AddressContext db = new AddressContext();

        // GET: api/AddressModels
        public IQueryable<AddressModels> GetAddressModels()
        {
            String id = User.Identity.GetUserId();
            return db.AddressModels.Where(ad => ad.UserId == id);
        }

        // GET: api/AddressModels/5
        [ResponseType(typeof(AddressModels))]
        public async Task<IHttpActionResult> GetAddressModels(int id)
        {
            AddressModels addressModels = await db.AddressModels.FindAsync(id);
            if (addressModels == null)
            {
                return NotFound();
            }

            return Ok(addressModels);
        }

        // PUT: api/AddressModels/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAddressModels(int id, AddressModels addressModels)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != addressModels.AddressId)
            {
                return BadRequest();
            }

            db.Entry(addressModels).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressModelsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AddressModels
        [ResponseType(typeof(AddressModels))]
        public async Task<IHttpActionResult> PostAddressModels(AddressModels addressModels)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            String id = User.Identity.GetUserId();
            addressModels.UserId = id;

            db.AddressModels.Add(addressModels);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = addressModels.AddressId }, addressModels);
        }

        // DELETE: api/AddressModels/5
        [ResponseType(typeof(AddressModels))]
        public async Task<IHttpActionResult> DeleteAddressModels(int id)
        {
            AddressModels addressModels = await db.AddressModels.FindAsync(id);
            if (addressModels == null)
            {
                return NotFound();
            }

            db.AddressModels.Remove(addressModels);
            await db.SaveChangesAsync();

            return Ok(addressModels);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AddressModelsExists(int id)
        {
            return db.AddressModels.Count(e => e.AddressId == id) > 0;
        }
    }
}
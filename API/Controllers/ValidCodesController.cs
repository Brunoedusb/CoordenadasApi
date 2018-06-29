using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI.WebControls;
using API.Models;
using Microsoft.AspNet.Identity;

namespace API.Controllers
{
    [Authorize]
    public class ValidCodesController : ApiController
    {
        private ValidCodeContext db = new ValidCodeContext();

        // GET: api/ValidCodes
        public IQueryable<ValidCode> GetValidCodes()
        {
            return db.ValidCodes;
        }

        // GET: api/AddressModels/5
        [ResponseType(typeof(ValidCode))]
        public async Task<IHttpActionResult> GetAddressModels(int id)
        {
            ValidCode validCode = await db.ValidCodes.FindAsync(id);
            if (validCode == null)
            {
                return NotFound();
            }

            return Ok(validCode);
        }

        // POST: api/ValidCodes/5
        [HttpPost]
        [Route("api/CheckValidCode")]
        public async Task<IHttpActionResult> CheckValidCode(GetCode Code)
        {
            try
            {
                String id = User.Identity.GetUserId();
                ValidCode validCode = await db.ValidCodes
                                            .Where(x => x.Code == Code.Code && x.UserId == id)
                                            .FirstOrDefaultAsync();

                if (validCode == null)
                {
                    return NotFound();
                }

                return Ok(validCode);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/ValidCodes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutValidCode(int id, ValidCode validCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != validCode.Id)
            {
                return BadRequest();
            }

            db.Entry(validCode).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValidCodeExists(id))
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

        // POST: api/ValidCodes
        [ResponseType(typeof(ValidCode))]
        public async Task<IHttpActionResult> PostValidCode(SendCodeByEmail Email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidCode validCode = new ValidCode();

            try
            {

                validCode.Code = RandomString(6);
                validCode.UserId = User.Identity.GetUserId();

                db.ValidCodes.Add(validCode);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            try
            {
                MailMessage mail = new MailMessage("sdcoordenadas@gmail.com", Email.Email);
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                mail.Subject = "Recuperação de senha";
                mail.IsBodyHtml = true;
                mail.Body = "<div>" +
                                "<p>Olá</p>" +
                                "<p>O código para recuperar sua senha é:</p><br/>" +
                                "<h1>"+ validCode.Code + "</h1>" +
                            "</div>";
                client.Credentials = new System.Net.NetworkCredential("sdcoordenadas@gmail.com", "cledsoncoords");
                client.Send(mail);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }


            return CreatedAtRoute("DefaultApi", new { id = validCode.Id }, validCode);
        }

        // DELETE: api/ValidCodes/5
        [ResponseType(typeof(ValidCode))]
        public async Task<IHttpActionResult> DeleteValidCode(int id)
        {
            ValidCode validCode = await db.ValidCodes.FindAsync(id);
            if (validCode == null)
            {
                return NotFound();
            }

            db.ValidCodes.Remove(validCode);
            await db.SaveChangesAsync();

            return Ok(validCode);
        }

        public string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ValidCodeExists(int id)
        {
            return db.ValidCodes.Count(e => e.Id == id) > 0;
        }
    }
}
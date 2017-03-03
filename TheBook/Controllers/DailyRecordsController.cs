using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BibleApi.Models;

namespace BibleApi.Controllers
{
    [Route("DailyRecords")]
    [AllowAnonymous]
    public class DailyRecordsController : ApiController
    {
        private BibleApiContext db = new BibleApiContext();
        
        [HttpGet]
        public async Task<IHttpActionResult> Get(string user="")
        {
            DateTime dt = DateTime.Now.Date;
            DailyRecord dailyRecord = db.DailyRecords.FirstOrDefault(p => p.User == user && p.RecordDate == dt);
            if (dailyRecord == null)
            {
                return NotFound();
            }

            return Ok(dailyRecord);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(DailyRecord dailyRecord)
        {
            dailyRecord.SubmitTime = DateTimeOffset.Now;
            dailyRecord.RecordDate = dailyRecord.RecordDate.Date;
            dailyRecord.User = dailyRecord.User.Trim();
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.DailyRecords.AddOrUpdate(dailyRecord);
            await db.SaveChangesAsync();
            return Ok("insert or updated");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
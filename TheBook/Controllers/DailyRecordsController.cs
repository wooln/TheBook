using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using TheBook.Models;

namespace TheBook.Controllers
{
    [Route("DailyRecords")]
    [AllowAnonymous]
    public class DailyRecordsController : ApiController
    {
        private BibleApiContext db = new BibleApiContext();

        [HttpGet]
        public async Task<IHttpActionResult> Get(int weekOfficeSet)
        {
            Thread.Sleep(1000);

            DateTime currentData = DateTime.Now.Date.AddDays(7 * weekOfficeSet);
            
            DateTimeOffset startSunday = currentData.AddDays(0 - (double)currentData.DayOfWeek);
            DateTimeOffset nextSunday = startSunday.AddDays(7);

            var recrods = db.DailyRecords.Where(r => startSunday <= r.RecordDate && r.RecordDate < nextSunday).ToArray();

            WeekyReport report = this.GetReport(recrods, startSunday);

            return Ok(report);
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

        private WeekyReport GetReport(DailyRecord[] recrods, DateTimeOffset startSunday)
        {
            var groups = recrods.GroupBy(r => r.User).OrderBy(g=>g.Key).ToArray();
            PersonWeekyReport[] report = groups.Select(g =>
            {
                PersonWeekyReport personReport = new PersonWeekyReport()
                {
                    User = g.Key,
                    WeekRecord = new Dictionary<DayOfWeek, DailyRecord>()
                };

                foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
                {
                    var daily = g.SingleOrDefault(r => r.RecordDate == startSunday.AddDays((int)dayOfWeek));
                    personReport.WeekRecord[dayOfWeek] = daily;
                    if (daily != null)
                    {
                        personReport.Count += daily.Chapters;
                    }

                }
                return personReport;

            }).OrderByDescending(r=>r.Count).ToArray();

            return new WeekyReport()
            {
                DateSpan = startSunday.ToString("yyyy-MM-dd") + " ~ " + startSunday.AddDays(6).ToString("yyyy-MM-dd"),
                Items = report
            };
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
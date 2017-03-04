using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheBook.Models
{
    public class WeekyReport
    {
        public string DateSpan { get; set; }
        public PersonWeekyReport[] Items { get; set; }
    }

    public class PersonWeekyReport
    {
        public string User { get; set; }

        public int Count { get; set; }

        public Dictionary<DayOfWeek, DailyRecord> WeekRecord { get; set; }
    }
}
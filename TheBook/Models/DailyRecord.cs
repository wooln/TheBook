﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BibleApi.Models
{
    public class DailyRecord
    {
        [Key, Column("User", Order = 0)]
        public string User { get; set; }

        [Key, Column("RecordDate", Order = 1)]
        public DateTimeOffset RecordDate { get; set; }

        [Column("SubmitTime")]
        public DateTimeOffset SubmitTime { get; set; }

        [Column("Content")]
        public string Content { get; set; }
        
        [Column("Chapters")]
        public int Chapters { get; set; }
        
        [Column("Remark")]
        public string Remark { get; set; }

    }
}
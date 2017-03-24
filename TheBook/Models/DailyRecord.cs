using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBook.Models
{
    public class DailyRecord
    {
        [Key, Column("Group", Order = 0)]
        public string Group { get; set; }

        [Key, Column("User", Order = 1)]
        public string User { get; set; }

        [Key, Column("RecordDate", Order = 2)]
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
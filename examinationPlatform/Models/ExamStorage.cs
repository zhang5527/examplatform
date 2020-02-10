using System;
using System.Collections.Generic;

namespace examinationPlatform.Models
{
    public partial class ExamStorage
    {
        public ExamStorage()
        {
            ExamContent = new HashSet<ExamContent>();
            ExamRecord = new HashSet<ExamRecord>();
            UserHistory = new HashSet<UserHistory>();
        }

        public int Id { get; set; }
        public int? Publisher { get; set; }
        public string PublishTime { get; set; }
        public string Subject { get; set; }
        public string Grade { get; set; }
        public string Group { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }

        public string Code { get; set; }

        public string Time { get; set; }

        public virtual Users PublisherNavigation { get; set; }
        public virtual ICollection<ExamContent> ExamContent { get; set; }
        public virtual ICollection<ExamRecord> ExamRecord { get; set; }

        public virtual ICollection<UserHistory> UserHistory { get; set; }
    }
}

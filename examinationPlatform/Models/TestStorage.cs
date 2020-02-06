using System;
using System.Collections.Generic;

namespace examinationPlatform.Models
{
    public partial class TestStorage
    {
        public TestStorage()
        {
            Collection = new HashSet<Collection>();
            ExamContent = new HashSet<ExamContent>();
            UserHistory = new HashSet<UserHistory>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Grade { get; set; }
        public string Content { get; set; }

        public string difficulty { get; set; }
        public string Answer { get; set; }
        public int? Publisher { get; set; }
        public string PublishDate { get; set; }
        public string Explain { get; set; }

        public virtual Users PublisherNavigation { get; set; }
        public virtual ICollection<Collection> Collection { get; set; }
        public virtual ICollection<ExamContent> ExamContent { get; set; }
        public virtual ICollection<UserHistory> UserHistory { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace examinationPlatform.Models
{
    public partial class UserHistory
    {
        public int Id { get; set; }
        public int? UsersId { get; set; }
        public int? TestId { get; set; }
        public int? ExamId { get; set; }
        public int? State { get; set; }
        public string Answer { get; set; }
        public string UpdateDate { get; set; }

        public virtual ExamStorage Exam { get; set; }
        public virtual TestStorage Test { get; set; }
        public virtual Users Users { get; set; }
    }
}

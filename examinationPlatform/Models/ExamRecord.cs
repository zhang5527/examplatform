using System;
using System.Collections.Generic;

namespace examinationPlatform.Models
{
    public partial class ExamRecord
    {
        public int Id { get; set; }
        public int? UsersId { get; set; }
        public int? ExamId { get; set; }
        public string Score { get; set; }
        public int? WrongCount { get; set; }
        public string PublishDate { get; set; }

        public virtual ExamStorage Exam { get; set; }
        public virtual Users Users { get; set; }
    }
}

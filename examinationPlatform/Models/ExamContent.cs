using System;
using System.Collections.Generic;

namespace examinationPlatform.Models
{
    public partial class ExamContent
    {
        public int Id { get; set; }
        public int? TestId { get; set; }
        public int? ExamId { get; set; }

        public virtual ExamStorage Exam { get; set; }
        public virtual TestStorage Test { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;

namespace examinationPlatform.Models
{
    public partial class ExamStorage
    {
        public ExamStorage()
        {
            ExamContent = new HashSet<ExamContent>();
            ExamRecord = new HashSet<ExamRecord>();
        }

        public int Id { get; set; }
        public int? Publisher { get; set; }
        public string PublishTime { get; set; }
        public string Subject { get; set; }
        public string Grade { get; set; }
        public string Group { get; set; }

        public virtual Users PublisherNavigation { get; set; }
        public virtual ICollection<ExamContent> ExamContent { get; set; }
        public virtual ICollection<ExamRecord> ExamRecord { get; set; }
    }
}
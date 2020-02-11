using System;
using System.Collections.Generic;

namespace examinationPlatform.Models
{
    public partial class Information
    {
        public int Id { get; set; }
        public string Publisher { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string PublishDate { get; set; }

        public virtual Users PublisherNavigation { get; set; }
    }
}

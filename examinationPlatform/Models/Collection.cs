using System;
using System.Collections.Generic;

namespace examinationPlatform.Models
{
    public partial class Collection
    {
        public int Id { get; set; }
        public int? UsersId { get; set; }
        public int? TestId { get; set; }
        public string Remark { get; set; }

        public virtual TestStorage Test { get; set; }
        public virtual Users Users { get; set; }
    }
}

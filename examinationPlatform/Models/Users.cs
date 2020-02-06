using System;
using System.Collections.Generic;

namespace examinationPlatform.Models
{
    public partial class Users
    {
        public Users()
        {
            Collection = new HashSet<Collection>();
            ExamRecord = new HashSet<ExamRecord>();
            ExamStorage = new HashSet<ExamStorage>();
            Information = new HashSet<Information>();
            TestStorage = new HashSet<TestStorage>();
            UserHistory = new HashSet<UserHistory>();
        }

        public int Id { get; set; }
        public string UserAccount { get; set; }
        public string UserPassword { get; set; }
        public int? UserGroup { get; set; }
        public string LastLogin { get; set; }
        public string Nickname { get; set; }
        public string College { get; set; }
        public string ImgUrl { get; set; }
        public string class_ { get; set; }
        public string major { get; set; }
        public string name { get; set; }

        public virtual ICollection<Collection> Collection { get; set; }
        public virtual ICollection<ExamRecord> ExamRecord { get; set; }
        public virtual ICollection<ExamStorage> ExamStorage { get; set; }
        public virtual ICollection<Information> Information { get; set; }
        public virtual ICollection<TestStorage> TestStorage { get; set; }
        public virtual ICollection<UserHistory> UserHistory { get; set; }
    }
}

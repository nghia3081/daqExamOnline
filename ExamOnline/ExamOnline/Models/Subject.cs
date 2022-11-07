using System;
using System.Collections.Generic;

namespace ExamOnline.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Exams = new HashSet<Exam>();
            Students = new HashSet<Account>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? TeacherEmail { get; set; }

        public virtual Account? Teacher { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<Account> Students { get; set; }
    }
}

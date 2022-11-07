using System;
using System.Collections.Generic;

namespace ExamOnline.Models
{
    public partial class Account
    {
        public Account()
        {
            Scores = new HashSet<Score>();
            Subjects = new HashSet<Subject>();
            SubjectsOfStudent = new HashSet<Subject>();
        }

        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        public Guid? RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Subject> SubjectsOfStudent { get; set; }
    }
}

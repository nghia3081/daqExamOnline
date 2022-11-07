using System;
using System.Collections.Generic;

namespace ExamOnline.Models
{
    public partial class Exam
    {
        public Exam()
        {
            Questions = new HashSet<Question>();
            Scores = new HashSet<Score>();
            Subjects = new HashSet<Subject>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? Duration { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Score> Scores { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}

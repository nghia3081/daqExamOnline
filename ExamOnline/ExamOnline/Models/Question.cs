using System;
using System.Collections.Generic;

namespace ExamOnline.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
            AnswersNavigation = new HashSet<Answer>();
        }

        public Guid Id { get; set; }
        public string? Content { get; set; }
        public Guid? ExamId { get; set; }

        public virtual Exam? Exam { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ICollection<Answer> AnswersNavigation { get; set; }
    }
}

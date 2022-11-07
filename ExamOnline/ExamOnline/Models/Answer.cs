using System;
using System.Collections.Generic;

namespace ExamOnline.Models
{
    public partial class Answer
    {
        public Answer()
        {
            Questions = new HashSet<Question>();
        }

        public Guid Id { get; set; }
        public string? Content { get; set; }
        public Guid? QuestionId { get; set; }

        public virtual Question? Question { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}

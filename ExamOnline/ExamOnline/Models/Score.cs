using System;
using System.Collections.Generic;

namespace ExamOnline.Models
{
    public partial class Score
    {
        public string StudentEmail { get; set; } = null!;
        public Guid ExamId { get; set; }
        public int? CorrectTotal { get; set; }

        public virtual Exam Exam { get; set; } = null!;
        public virtual Account Student { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;

namespace ExamOnline.Models
{
    public partial class AccountDTO
    {
        public AccountDTO()
        {
        }

        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Guid? RoleId { get; set; }
    }
}

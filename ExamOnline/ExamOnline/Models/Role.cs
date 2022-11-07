using System;
using System.Collections.Generic;

namespace ExamOnline.Models
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}

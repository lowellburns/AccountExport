using System;
using System.Collections.Generic;

namespace AccountExportCore2.Models
{
    public partial class Patient
    {
        public Patient()
        {
            Account = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string SocialSecuirtyNumber { get; set; }

        public ICollection<Account> Account { get; set; }

        public string FirstInitial { get {
                return FirstName.Substring(0, 1);
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace AccountExportCore2.Models
{
    public partial class Client
    {
        public Client()
        {
            Account = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Account> Account { get; set; }
    }
}

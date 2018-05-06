using System;
using System.Collections.Generic;

namespace AccountExportCore2.Models
{
    public partial class Insurance
    {
        public Insurance()
        {
            AccountInsurance = new HashSet<AccountInsurance>();
        }

        public int Id { get; set; }
        public string PlanName { get; set; }
        public string Policy { get; set; }
        public string GroupNumber { get; set; }

        public ICollection<AccountInsurance> AccountInsurance { get; set; }
    }
}

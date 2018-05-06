using System;
using System.Collections.Generic;

namespace AccountExportCore2.Models
{
    public partial class AccountInsurance
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int InsuranceId { get; set; }

        public Account Account { get; set; }
        public Insurance Insurance { get; set; }
    }
}

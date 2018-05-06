using System;
using System.Collections.Generic;

namespace AccountExportCore2.Models
{
    public partial class Facility
    {
        public Facility()
        {
            Account = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string FacilityName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public ICollection<Account> Account { get; set; }
    }
}

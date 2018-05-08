using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountExportCore.Models.DTO
{
    public class QueryableTable
    {
        public string TableName { get; set; }
        public bool IncludeInQuery { get; set; }

        public List<QueryableColumn> Columns { get; set; }

    }
}

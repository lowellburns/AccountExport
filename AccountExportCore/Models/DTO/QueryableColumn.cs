using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountExportCore.Models.DTO
{
    public class QueryableColumn
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool IncludeInQuery { get; set; }
        public bool FilterColumn { get; set; }
        public string FilterType { get; set; }
        public string FilterValue { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountExportCore.Models.DTO
{
    public class ExportDefinition
    {

        public string FileNameFormat { get; set; }
        public int  ClientId { get; set; }
        public string ExportFormat { get; set; }
        public List<QueryableTable> queryableTables { get; set; }
        public bool RepeatByFacility { get; set; }


    }
}

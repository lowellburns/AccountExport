using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountExportCore.Models.DTO
{
    public class ExportResults
    {
        public string Filename { get; set; }
        public string FileContents { get; set; }
    }
}

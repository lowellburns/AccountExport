using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountExportCore.Models.DTO;
using AccountExportCore2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountExportCore.Controllers
{
    [Produces("application/json")]
    [Route("api/QueryableTables")]
    public class QueryableTablesController : Controller
    {
        private readonly AccountExportContext _context;
        private readonly List<string> _queryableTables = new List<string>() {"account","facility","insurance","patient" };

        public QueryableTablesController(AccountExportContext context)
        {
            _context = context;
        }


        // GET: api/QueryableTables
        [HttpGet]
        public IEnumerable<QueryableTable> GetQueryableTables()
        {

            List<QueryableTable> returnObject = new List<QueryableTable>();

            foreach (var table in _context.Model.GetEntityTypes())
            {
                var currentTableName = table.Name.ToLower().Split('.').Last();
                if (_queryableTables.Any(q => currentTableName == q ))
                {
                    //we have a table not in our ignore list. 
                    var newTable = new QueryableTable();
                    newTable.TableName = currentTableName;
                    newTable.Columns = new List<QueryableColumn>();

                    foreach (var column in table.GetProperties())
                    {
                        var newColumn = new QueryableColumn()
                        {
                            ColumnName = column.Name,
                            DataType = column.PropertyInfo.PropertyType.FullName
                        };
                        newTable.Columns.Add(newColumn);
                    }

                    returnObject.Add(newTable);

                }

            }

            return returnObject;
        }

        

    }
}

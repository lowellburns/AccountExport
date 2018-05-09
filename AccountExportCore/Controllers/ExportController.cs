using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AccountExportCore.Models.DTO;
using AccountExportCore2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace AccountExportCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Export")]
    public class ExportController : Controller
    {

        private readonly AccountExportContext _context;

        public ExportController(AccountExportContext context)
        {
            _context = context;
        }

        //// GET: api/Export
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Export/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        
        // POST: api/Export
        [HttpPost]
        public List<ExportResults> DoExport([FromBody]ExportDefinition exportDefinition)
        {
            //possible tables "account","facility","insurance","patient"

            var returnList = new List<ExportResults>();

            var returnData = _context.Account.Where(a => a.ClientId == exportDefinition.ClientId);

            var AccountFilter = exportDefinition.queryableTables.Where(t => t.TableName.ToLower() == "account" && t.IncludeInQuery).FirstOrDefault();
            var FacilityFilter = exportDefinition.queryableTables.Where(t => t.TableName.ToLower() == "facility" && t.IncludeInQuery).FirstOrDefault();
            var PatientFilter = exportDefinition.queryableTables.Where(t => t.TableName.ToLower() == "patient" && t.IncludeInQuery).FirstOrDefault();
            var InsuranceFilter = exportDefinition.queryableTables.Where(t => t.TableName.ToLower() == "insurance" && t.IncludeInQuery).FirstOrDefault();

            if (FacilityFilter != null)
            {
                //we are supposed to include the facility in the export.
                returnData = returnData.Include(a => a.Facility);
            }

            if (PatientFilter != null)
            {
                //we are supposed to include the patient in the export.
                returnData = returnData.Include(a => a.Patient);
            }

            if (InsuranceFilter != null)
            {
                //we are supposed to include the insurance in the export.
                returnData = returnData.Include(a => a.AccountInsurance).ThenInclude(a => a.Insurance);
            }

            if (AccountFilter != null)
            {
                var accountExportTable = exportDefinition.queryableTables.Where(t => t.TableName.ToLower() == "account").FirstOrDefault();
                //select only the columns we need. 
                foreach (var item in accountExportTable.Columns)
                {
                    if (item.FilterColumn)
                    {
                       
                        if(item.DataType.ToLower().Contains("int"))
                        {
                            //filter on the specified column with the chosen filterValue
                            //compare to returns -1 for less than, 0 for equals, and 1 for greater than. 
                            returnData = returnData.Where(a => int.Parse(a.GetType().GetProperty(item.ColumnName).GetValue(a).ToString()).CompareTo(int.Parse(item.FilterValue)) == int.Parse(item.FilterType));
                        }
                        else if (item.DataType.ToLower().Contains("decimal"))
                        {

                            //filter on the specified column with the chosen filterValue
                            //compare to returns -1 for less than, 0 for equals, and 1 for greater than. 
                            returnData = returnData.Where(a => decimal.Parse(a.GetType().GetProperty(item.ColumnName).GetValue(a).ToString()).CompareTo(decimal.Parse(item.FilterValue)) == int.Parse(item.FilterType));
                        }
                        else if (item.DataType.ToLower().Contains("date"))
                        {

                            //filter on the specified column with the chosen filterValue
                            //compare to returns -1 for less than, 0 for equals, and 1 for greater than. 
                            returnData = returnData.Where(a => DateTime.Parse(a.GetType().GetProperty(item.ColumnName).GetValue(a).ToString()).CompareTo(DateTime.Parse(item.FilterValue)) == int.Parse(item.FilterType));
                        }
                        else
                        {
                            //string
                            //filter on the specified column with the chosen filterValue
                            //compare to returns -1 for less than, 0 for equals, and 1 for greater than. 
                            returnData = returnData.Where(a => a.GetType().GetProperty(item.ColumnName).GetValue(a).ToString().CompareTo(item.FilterValue) == int.Parse(item.FilterType));
                        }
                    }

                   

                }

            }

            var fileName = exportDefinition.FileNameFormat;

            fileName = fileName.Replace("[yyyy]", DateTime.Now.Year.ToString().PadLeft(4, '0'));
            fileName = fileName.Replace("[mm]", DateTime.Now.Month.ToString().PadLeft(2, '0'));
            fileName = fileName.Replace("[dd]", DateTime.Now.Day.ToString().PadLeft(2, '0'));
            


            List<Object> selectedObjects = new List<object>();
            if (returnData != null)
            {
                returnData = returnData.Distinct();
                if (exportDefinition.RepeatByFacility)
                {
                    var FacilityIds = returnData.Select(a => a.FacilityId).ToList().Distinct();

                    foreach (var faclityId in FacilityIds)
                    {
                        var faclityData = returnData.Where(d => d.FacilityId == faclityId);
                        var newFileName = fileName;
                        var CurrentFaclityName = faclityData.FirstOrDefault().Facility.FacilityName;
                        selectedObjects = getSelectedObjectList(faclityData, AccountFilter, FacilityFilter, PatientFilter, InsuranceFilter);

                        newFileName = newFileName.Replace("[FacilityName]", CurrentFaclityName);

                        var exportMemoryStream = getExportFileContents(selectedObjects, exportDefinition.ExportFormat);


                        //var response = File(exportMemoryStream, "application/octet-stream"); // FileStreamResult

                        returnList.Add(new ExportResults()
                        {
                            FileContents = new StreamReader(exportMemoryStream).ReadToEnd(),
                            Filename = newFileName
                        }
                        );

                    }

                }
                else
                {

                    selectedObjects = getSelectedObjectList(returnData, AccountFilter, FacilityFilter, PatientFilter, InsuranceFilter);

                    var exportMemoryStream = getExportFileContents(selectedObjects, exportDefinition.ExportFormat);


                    //var response = File(exportMemoryStream, "application/octet-stream"); // FileStreamResult

                    returnList.Add(new ExportResults()
                    {
                        FileContents = new StreamReader(exportMemoryStream).ReadToEnd(),
                        Filename = fileName
                    }
                    );
                }
            }
            return returnList;

        }

        private List<Object> getSelectedObjectList(IQueryable<Account> returnData, QueryableTable AccountFilter, QueryableTable FacilityFilter, QueryableTable PatientFilter, QueryableTable InsuranceFilter)
        {
            List<Object> returnDataList = new List<object>();


            foreach (var item in returnData.Distinct())
            {
                var newExportRow = new List<KeyValuePair<string, object>>();
                if (AccountFilter != null)
                    foreach (var column in AccountFilter.Columns.Where(c => c.IncludeInQuery))
                    {
                        //add a single column value to our export row. 
                        var exportValue = item.GetType().GetProperty(column.ColumnName).GetValue(item);
                        newExportRow.Add(new KeyValuePair<string, object>(column.ColumnName, exportValue));
                    }

                if (FacilityFilter != null)
                {
                    foreach (var column in FacilityFilter.Columns.Where(c => c.IncludeInQuery))
                    {
                        if (item.Facility != null)
                        {
                            //add a single column value to our export row. 
                            var exportValue = item.Facility.GetType().GetProperty(column.ColumnName).GetValue(item.Facility);
                            newExportRow.Add(new KeyValuePair<string, object>(column.ColumnName, exportValue));
                        }
                        else
                        {
                            //we don't have a facility for this account (bad data) but lets add a placeholder value anyways. otherwise our return dataset will be missing columns. 
                            newExportRow.Add(new KeyValuePair<string, object>(column.ColumnName, ""));
                        }
                    }
                }

                if (PatientFilter != null)
                {
                    foreach (var column in PatientFilter.Columns.Where(c => c.IncludeInQuery))
                    {
                        if (item.Patient != null)
                        {
                            //add a single column value to our export row. 
                            var exportValue = item.Patient.GetType().GetProperty(column.ColumnName).GetValue(item.Patient);
                            newExportRow.Add(new KeyValuePair<string, object>(column.ColumnName, exportValue));
                        }
                        else
                        {
                            //we don't have a Patient for this account (bad data) but lets add a placeholder value anyways. otherwise our return dataset will be missing columns. 
                            newExportRow.Add(new KeyValuePair<string, object>(column.ColumnName, ""));
                        }
                    }
                }

                if (InsuranceFilter != null && InsuranceFilter.Columns.Where(c => c.IncludeInQuery).Count() > 0)
                {

                    var InsuranceRows = new List<Object>();

                    foreach (var InsuranceItem in item.AccountInsurance)
                    {

                        var tempRow = newExportRow;
                        foreach (var column in InsuranceFilter.Columns.Where(c => c.IncludeInQuery))
                        {
                            //add a single column value to our export row. 
                            var exportValue = InsuranceItem.Insurance.GetType().GetProperty(column.ColumnName).GetValue(InsuranceItem.Insurance);
                            tempRow.Add(new KeyValuePair<string, object>(column.ColumnName, exportValue));

                        }

                        InsuranceRows.Add(tempRow);

                        newExportRow = tempRow;


                    }
                    returnDataList.AddRange(InsuranceRows);
                }
                else
                {
                    returnDataList.Add(newExportRow);
                }


            }

            return returnDataList;
        }

        private MemoryStream getExportFileContents(List<Object> returnObjectList, string exportFormat)
        {
            MemoryStream returnStream = new MemoryStream();
            TextWriter tw = new StreamWriter(returnStream);


            //assume we have a custom csv or pipe separated format.
            foreach (var row in returnObjectList)
            {
                string newLine = exportFormat;
                List<KeyValuePair<string, object>> rowObject = (List<KeyValuePair<string, object>>) row;
                foreach (var field in rowObject)
                {
                    newLine = newLine.Replace(string.Format("[{0}]", field.Key), (field.Value ?? "").ToString());
                }
                tw.WriteLine(newLine);
            }

            tw.Flush();
            returnStream.Position = 0;

            return returnStream;

        }
        
        //// PUT: api/Export/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}
        
        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

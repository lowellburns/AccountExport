import { Todo } from './todo';
import { Client } from './client';
import { UserState } from './UserState';
import { DataService } from './DataService';
import { inject } from 'aurelia-framework';
import { debug } from 'util';
import { ExportDefinition } from './exportDefinition';
import { QueryableTable } from './queryableTable';
import { QueryableColumn } from './queryableColumn';
import { ExportResults } from 'ExportResults';

@inject(UserState)
export class App {
    heading = "Account Export";
    clients: Client[] = new Array<Client>();
    selectedClientId;
  config;
  exportLink;
  currentExportDef;


    get selectedClient(): Client {
        return this.userState._currentClient
    }

    constructor(private userState: UserState) {


      var receiveClients = (newClients: Client[]): void => {

        this.clients.push(new Client("-1", "Select a Client"));
        //this.clients.push(new Client("1", "General Hospital"));
        //this.clients.push(new Client("2", "Veteran Hospital"));
        
        //this.clients = newClients;
        
        for (let newClient of newClients) {
             this.clients.push(new Client(newClient.id.toString(),newClient.name.toString()));
        }


      }


      let dataService = new DataService();
      dataService.getClients(receiveClients);

      


    }

  generateExport() {

    let dataService = new DataService();

      var receiptExport = (exportDataList: ExportResults[]): void => {

        for (let exportData of exportDataList) {


          let binaryData = [];
          binaryData.push(exportData.fileContents);

          let newURL = URL.createObjectURL(new Blob(binaryData, { type: "application/text" }));

          const a: HTMLAnchorElement = document.createElement('a') as HTMLAnchorElement;

          a.href = newURL;
          a.download = exportData.filename;
          document.body.appendChild(a);
          this.exportLink = a;
          a.click();

          document.body.removeChild(a);
          URL.revokeObjectURL(newURL);

        }


      }

      


    dataService.postExportDefinition(this.currentExportDef, receiptExport);

    }
    


    DropdownChanged(selectedClientId) {
        
        this.selectedClientId = selectedClientId;
      this.userState._currentClient = this.clients.filter(c => c.Id == this.selectedClientId)[0];
      this.SetExportDefinitionForClient(selectedClientId);
    }

  SetExportDefinitionForClient(selectedClientId) {
    //this module mocks up the export definition for this client. In the real world the client would be able to change the export definition in the UI and possibly save 
    //export definitions to their account. 

    if (selectedClientId == 1) {
      //general hospital
      let exportTables = new Array<QueryableTable>();
      let exportAccountColumns: QueryableColumn[] = new Array<QueryableColumn>();
      exportAccountColumns.push(new QueryableColumn("ClientId", "int", true, false, "", ""));
      exportAccountColumns.push(new QueryableColumn("AccountNumber", "string", true, false, "", ""));
      exportAccountColumns.push(new QueryableColumn("Balance", "decimal", true, false, "", ""));
      exportAccountColumns.push(new QueryableColumn("FacilityId", "int", true, false, "", ""));
      exportAccountColumns.push(new QueryableColumn("AdmitDate", "datetime", true, false, "", ""));
      exportAccountColumns.push(new QueryableColumn("DischargeDate", "datetime", true, false, "", ""));
      exportAccountColumns.push(new QueryableColumn("PatientId", "int", true, false, "", ""));


      let exportAccountTable: QueryableTable = new QueryableTable("Account", "true", exportAccountColumns);
      exportTables.push(exportAccountTable);


      let exportPatientColumns: QueryableColumn[] = new Array<QueryableColumn>();
      exportPatientColumns.push(new QueryableColumn("FirstName", "string", true, false, "", ""));
      exportPatientColumns.push(new QueryableColumn("LastName", "string", true, false, "", ""));
      exportPatientColumns.push(new QueryableColumn("FirstInitial", "string", true, false, "", ""));


      let exportPatientTable: QueryableTable = new QueryableTable("Patient", "true", exportPatientColumns);
      exportTables.push(exportPatientTable);

      let exportDef: ExportDefinition = new ExportDefinition("[yyyy][mm][dd]-gh.data.dat", selectedClientId, "[ClientId]|[AccountNumber]|[Balance]|[FacilityId]|[AdmitDate]|[DischargeDate]|[PatientId]|[LastName],[FirstInitial]", false,exportTables);

      this.currentExportDef = exportDef;

    }
    else if (selectedClientId == 2) {
      // veteran hospital




    }


   
  }

    
}

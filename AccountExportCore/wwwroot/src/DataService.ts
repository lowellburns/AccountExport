//<reference path="./aurelia-http-client.d.ts"/>
//import HttpClient = require("aurelia-http-client");
import { HttpClient } from 'aurelia-http-client';
import { Client } from './client';
import { ExportDefinition } from './exportDefinition';
import { debug } from 'util';
import { ExportResults } from 'ExportResults';
//import * as HttpClient from '../scripts/aurelia-http-client.js';

export class DataService {
    _dataService;

    getDataService() {
        return this._dataService;
    }

  getData(path, callback: (n: string) => any) {

        let client = new HttpClient();
        client.get(path)
          .then(response => {

            console.log(response);
            if (response.isSuccess) {  
              callback(response.response);
            }

            
            });
       
  }

  postData(path,data, callback: (n: string) => any) {

    let client = new HttpClient();
    client.post(path, data)
      .then(response => {

        console.log(response);
        if (response.isSuccess) {
          
          callback(response.response);
        }


      });

  }

  getClients(callbackParent: (n: Client[]) => any) {

    var receiveClientsString = (newClients: string): void => {

      var returnObject = this.toInstance(new Array<Client>(), newClients);

      callbackParent(returnObject);

    }

    this.getData("api/Clients", receiveClientsString);

  }

  postExportDefinition(exportDef : ExportDefinition, callbackParent: (n: ExportResults[]) => any) {

    
    var receivePostResponse = (postResponse: string): void => {

      
      var returnObject = this.toInstance(new Array<ExportResults>(), postResponse);
      debugger
      callbackParent(returnObject);

    }

    this.postData("api/Export", exportDef, receivePostResponse);
  }


  //copied from https://stackoverflow.com/questions/29758765/json-to-typescript-class-instance?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
  toInstance<T>(obj: T, json: string): T { 
    var jsonObj = JSON.parse(json);

    if (typeof obj["fromJSON"] === "function") {
      obj["fromJSON"](jsonObj);
    }
    else {
      for (var propName in jsonObj) {
        obj[propName] = jsonObj[propName]
      }
    }

    return obj;
  }


}

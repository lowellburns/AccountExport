import { Todo } from './todo';
import { Client } from './client';
import { UserState } from './UserState';
import { DataService } from './DataService';
import { inject } from 'aurelia-framework';
import { debug } from 'util';

@inject(UserState)
export class App {
    heading = "Account Export";
    clients: Client[] = new Array<Client>();
    selectedClientId;
    config;


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
      var clientsdb = dataService.getClients(receiveClients);
     }
    


    DropdownChanged(selectedClientId) {
        
        this.selectedClientId = selectedClientId;
        this.userState._currentClient = this.clients.filter(c => c.Id == this.selectedClientId)[0];
    }

    
}

"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
//<reference path="./aurelia-http-client.d.ts"/>
//import HttpClient = require("aurelia-http-client");
var aurelia_http_client_1 = require("aurelia-http-client");
//import * as HttpClient from '../scripts/aurelia-http-client.js';
var DataService = /** @class */ (function () {
    function DataService() {
    }
    DataService.prototype.getDataService = function () {
        return this._dataService;
    };
    DataService.prototype.getData = function (path) {
        var client = new aurelia_http_client_1.HttpClient();
        client.get(path)
            .then(function (data) {
            console.log(data);
            return data;
        });
    };
    DataService.prototype.getClients = function () {
        ;
        var clients = this.getData("'api/Clients'");
        return clients;
    };
    return DataService;
}());
exports.DataService = DataService;
//# sourceMappingURL=DataService.js.map
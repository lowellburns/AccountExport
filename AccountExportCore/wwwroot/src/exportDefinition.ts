import { QueryableTable } from "./queryableTable";

export class ExportDefinition {

  constructor(public FileNameFormat: string,
    public ClientId: string,
    public ExportFormat: string,
    public RepeatByFacility: boolean,
    public queryableTables: Array<QueryableTable>) { }
}




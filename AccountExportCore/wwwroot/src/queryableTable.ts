import { QueryableColumn } from "./queryableColumn";

export class QueryableTable {

  constructor(public TableName: string,
    public IncludeInQuery: string,
    public Columns: Array<QueryableColumn>) { }
}




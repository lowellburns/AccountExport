export class QueryableColumn {

  constructor(public ColumnName: string,
    public DataType: string,
    public IncludeInQuery: boolean    ,
    public FilterColumn: boolean,
    public FilterType: string,
    public FilterValue: string
  ) { }
}




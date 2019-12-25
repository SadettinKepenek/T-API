class AddColumnDto {
    TableName = null;
    ColumnName = null;
    DataType = null;
    DataLength = null;
    NotNull = null;
    AutoInc = null;
    Unique = null;
    PrimaryKey = null;
    DefaultValue = null;
    HasLength = null;
    DatabaseId = null;
    Provider='';
    constructor(databaseId,provider) {
        this.DatabaseId = databaseId;
        this.Provider = provider;
    }
}
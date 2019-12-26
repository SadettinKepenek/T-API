class AddColumnDto {
    TableName = '';
    ColumnName = '';
    DataType = '';
    DataLength = 0;
    NotNull = false;
    AutoInc = false;
    Unique = false;
    PrimaryKey = false;
    DefaultValue = null;
    HasLength = false;
    DatabaseId = 0;
    Provider='';
    constructor(databaseId,provider) {
        this.DatabaseId = databaseId;
        this.Provider = provider;
    }
}

class AddForeignKey {
    SourceTable = '';
    SourceColumn = '';
    TargetTable = '';
    TargetColumn = '';
    Provider = '';
    DatabaseId = 0;
    ForeignKeyName='';
    constructor(databaseId, provider) {
        this.DatabaseId = databaseId;
        this.Provider = provider;
    }
}
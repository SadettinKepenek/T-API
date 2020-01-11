class AddKeyDto {
    TableName = '';
    KeyName = '';
    KeyColumn = '';
    IsPrimary = false;
    DatabaseId = 0;
    Provider = '';
    constructor(databaseId, provider) {
        this.DatabaseId = databaseId;
        this.Provider = provider;
    }
}
class DeleteKeyDto {
    KeyName = '';
    TableName = '';
    Provider = '';
    DatabaseId = 0;
    constructor(KeyName, TableName, Provider, DatabaseId) {
        this.Provider = Provider;
        this.DatabaseId = DatabaseId;
        this.KeyName = KeyName;
        this.TableName = TableName;
    }
}
class UpdateKeyDto {
    KeyName = '';
    DatabaseId = 0;
    KeyColumn = '';
    IsPrimary = false;
    TableName = '';
    OldKey = null;
    constructor(databaseId, provider) {
        this.DatabaseId = databaseId;
        this.Provider = provider;
    }
}

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

class UpdateColumnDto {
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
    Provider = '';
    OldColumn=null;
    constructor(databaseId, provider) {
        this.DatabaseId = databaseId;
        this.Provider = provider;
    }
}

class DeleteTableDto {
    TableName = '';
    DatabaseName = '';
    DatabaseId = '';
    constructor(databaseId, databaseName,tableName) {
        this.DatabaseId = databaseId;
        this.DatabaseName = databaseName;
        this.TableName = tableName;
    }
};

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

class UpdateForeignKey {
    SourceTable = '';
    SourceColumn = '';
    TargetTable = '';
    TargetColumn = '';
    Provider = '';
    DatabaseId = 0;
    ForeignKeyName = '';
    OldForeignKey=null;
    constructor(databaseId, provider) {
        this.DatabaseId = databaseId;
        this.Provider = provider;
    }
}
baseUrl = "http://t-api-bash.herokuapp.com";

var init = function init(databaseId, databaseName, provider) {
    var table = $('#addTableColumns').DataTable({
        destroy: true,
        columns: [
            { title: "Column Name" },
            { title: "Data Type" },
            { title: "Data Length" },
            { title: "Default Value" },
            { title: "Is Unique?" },
            { title: "Is Primary?" },
            { title: "Is Not Null?" },
            { title: "Is Auto Inc?" },
            { title: "Has Length?" },
            { title: '<button id="btn_AddNewColumn" type="button" class="btn btn-primary btn-sm">Add</button>' },
        ],
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        columnDefs: [{
            orderable: false,
            targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9],
            "className": "text-center"
        }],
        "createdRow": function (row, data, index) {
        },

    });
    window.columnCount = 0;
    window.provider = provider;
    window.databaseName = databaseName;
    window.databaseId = databaseId;
    getDataTypes(provider);


    $('#btn_AddNewColumn').on('click',
        function () {
            table.row.add([
                getColumnNameString(), getDataTypeString(), getDataLengthString(), getDefaultValueString(), getIsUniqueString(), getIsPrimaryString(), getNotNullString(), getAutoIncString(), getHasLengthString(), getIndexString()
            ]).draw(false);
            window.columnCount++;
        });

    $('#addTableColumns tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

};

var getColumnNameString = function getColumnNameString() {
    var columnNameStr = '<input type="text" class="form-control" name="Columns[' + window.columnCount + '].ColumnName" required="required"/>';
    return columnNameStr;
};

var getIndexString = function getIndexString() {
    var indexStr = '<button type="button" onclick="removeColumn()" class="btn btn-danger btn-sm">Delete</button>';
    return indexStr;
};
var getDataTypeString = function getDataTypeString() {
    var dataTypeStr = '<select onchange="dataTypeChanged(this);" class="form-control" size="1" id="row-' + window.columnCount + '-dataType" name="Columns[' + window.columnCount + '].DataType" required="required">';

    $('#' + 'row-' + window.columnCount + '-dataType').find('option').not(':first').remove();
    dataTypeStr += '<option  disabled="disabled">Data Type</option>';

    window.dataTypes.forEach(function (d) {
        dataTypeStr += '<option value=' + d + '>' + d + '</option>';
    });
    dataTypeStr += '</select>';
    return dataTypeStr;
};


var getDataLengthString = function getDataLengthString() {
    var dataLengthString = '<input type="number" class="form-control" id="DataLength_Column_' + window.columnCount + '" name="Columns[' + window.columnCount + '].DataLength"/>';
    return dataLengthString;
};

var getDefaultValueString = function getDefaultValueString() {
    var defaultValueStr = '<input type="text" class="form-control"  name="Columns[' + window.columnCount + '].DefaultValue"/>';
    return defaultValueStr;
};

var getHasLengthString = function getHasLengthString() {
    var hasLengthString = '<input type="checkbox" id="HasLength_' + window.columnCount + '"  name="Columns[' + window.columnCount + '].HasLength" value="true" />';
    return hasLengthString;
};

var getIsUniqueString = function getIsUniqueString() {
    var isUniqueString = '<input type="checkbox"  name="Columns[' + window.columnCount + '].Unique" value="true"/>';
    return isUniqueString;
};
var getIsPrimaryString = function getIsPrimaryString() {
    var isPrimaryStr = '<input type="checkbox"  name="Columns[' + window.columnCount + '].PrimaryKey" value="true"/>';
    return isPrimaryStr;
};
var getNotNullString = function getIsPrimaryString() {
    var notNullStr = '<input type="checkbox"  name="Columns[' + window.columnCount + '].NotNull" value="true"/>';
    return notNullStr;
};
var getAutoIncString = function getAutoIncString() {
    var autoIncStr = '<input type="checkbox"  name="Columns[' + window.columnCount + '].AutoInc" value="true" />';
    return autoIncStr;
};
var dataTypeChanged = function dataTypeChanged(selectInput) {


    var rowId = selectInput.id.replace("row-", "");
    rowId = rowId.replace("-dataType", "");
    var selectedType = selectInput.value;
    console.log(rowId);
    var hasLengthId = 'HasLength_' + rowId;
    var dataLengthId = 'DataLength_Column_' + rowId + '';

    if (selectedType === 'char' ||
        selectedType === 'varchar' ||
        selectedType.search('text') >= 0) {

        console.log($('#' + hasLengthId));
        $('#' + hasLengthId).prop("checked", true);
        $('#' + hasLengthId).val(true);
        $('#' + dataLengthId).prop("required", true);
        $('#' + dataLengthId).prop("disabled", false);

    } else {
        $('#' + hasLengthId).prop("checked", false);
        $('#' + hasLengthId).val(false);
        $('#' + dataLengthId).prop("required", false);
        $('#' + dataLengthId).prop("disabled", true);

    }
};
var getDataTypes = function getDataTypes(provider, selectName) {
    $.ajax({
        url: baseUrl+'/Database/GetDataTypes?provider=' + provider,
        type: 'GET',
        success: function (data, textStatus, xhr) {
            window.dataTypes = data;
        },
        complete: function (xhr, textStatus) {
        }
    }).done(function (result) {
        // do something
    }).fail(function (jqXHR, textStatus, error) {
        $('#errorModalTitle').text('Hata!');
        $('#errorModalBodyText').text('Veri Tipleri yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..!');
        $('#errorModal').modal('show');
        $('#errorModal').on('hidden.bs.modal', function (e) {
            window.location.replace(baseUrl +"/Database/");
        });
    });

};

var removeColumn = function removeColumn() {
    console.log($('#addTableColumns', 'tbody tr'));
    $('#addTableColumns').DataTable().row('.selected').remove().draw(false);

    //window.columnCount--;

}
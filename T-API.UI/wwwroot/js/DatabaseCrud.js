window.tableCount = 0;
var getDataTypes = function getDataTypes() {

};

var addTableContainer = function addTable(object) {

    var table = JSON.parse(object);
    var tabTables = $('#v-pills-tab-tables');
    var tableContent = $('#v-pills-tabContent-tables');


    table.forEach(function (table) {
        var tabTablesString = '<a class="nav-link ';
        if (window.tableCount === 0) {
            tabTablesString += ' active"';
        } else {
            tabTablesString += '"';
        }
        tabTablesString += ' id="v-pills-table_' + window.tableCount + '-tab" ' +
            'data-toggle="pill"' +
            'href="#v-pills-' + window.tableCount + '"' +
            'role="tab" aria-controls="v-pills-home" aria-selected="true">' + table.tableName + '</a>';

        var tableContentString = '<div class="tab-pane fade ';
        if (window.tableCount === 0) {
            tableContentString += ' show active"';
        } else {
            tableContentString += '"';
        }


        tableContentString += 'id="v-pills-' + window.tableCount + '" role="tabpanel" ' +
            'aria-labelledby="v-pills-home-tab">';
        tableContentString += '<div class="container">';

        tableContentString += '<div class="row">';
        tableContentString += '<div class="col-md-2">';
        tableContentString += 'Column Name';
        tableContentString += '</div>';
        tableContentString += '<div class="col-md-2">';
        tableContentString += 'Data Type';
        tableContentString += '</div>';
        tableContentString += '<div class="col-md-2">';
        tableContentString += 'Default Value';
        tableContentString += '</div>';
        tableContentString += '<div class="col-md-2">';
        tableContentString += 'Is Primary';
        tableContentString += '</div>';
        tableContentString += '<div class="col-md-2">';
        tableContentString += 'Is Auto Inc';
        tableContentString += '</div>';
        tableContentString += '<div class="col-md-2">';
        tableContentString += '<button type="button"' +
            ' class="btn btn-info btn-sm"' +
            ' style="margin-bottom: 5px"' +
            ' data-toggle="modal" ' +
            'data-target="#addColumnModal">';
        tableContentString += 'Add Column';
        tableContentString += '</button>';
        tableContentString += '</div>';
        tableContentString += '<hr/>';
        tableContentString += '</div>';
        tabTables.append(tabTablesString);

        table.columns.forEach(function (column) {
            tableContentString += '<hr/>';
            tableContentString += '<div class="row">';
            tableContentString += '<div class="col-md-2">';
            tableContentString += column.columnName;
            tableContentString += '</div>';
            tableContentString += '<div class="col-md-2">';
            tableContentString += column.dataType;
            tableContentString += '</div>';
            tableContentString += '<div class="col-md-2">';
            tableContentString += column.defaultValue;
            console.log(column.defaultValue);
            tableContentString += '</div>';
            tableContentString += '<div class="col-md-2">';
            tableContentString += column.primaryKey;
            tableContentString += '</div>';
            tableContentString += '<div class="col-md-2">';
            tableContentString += column.autoInc;
            tableContentString += '</div>';
            tableContentString += '</div>';

        });
        tableContentString += '<hr/>';
        tableContentString += '</div>';
        tableContent.append(tableContentString);

        window.tableCount++;
    });




};


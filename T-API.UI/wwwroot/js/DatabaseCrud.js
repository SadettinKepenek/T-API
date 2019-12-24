window.tableCount = 0;
var getDataTypes = function getDataTypes() {

};

var addTableContainer = function addTable() {
    var tabTables = $('#v-pills-tab-tables');
    var tabTablesString = '<a class="nav-link ';
    if (window.tableCount === 0) {
        tabTablesString += ' active"';
    } else {
        tabTablesString += '"';
    }
    tabTablesString += ' id="v-pills-table_' + window.tableCount + '-tab" ' +
        'data-toggle="pill"' +
        'href="#v-pills-' + window.tableCount + '"' +
        'role="tab" aria-controls="v-pills-home" aria-selected="true">Test</a>';

    var tableContent = $('#v-pills-tabContent-tables');

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

    tableContentString += '</div>';
    tableContentString += '</div>';

    tabTables.append(tabTablesString);
    tableContent.append(tableContentString);

    window.tableCount++;

};


window.tableCount = 0;

var init = function init(databaseId, dbProvider) {
    window.databaseId = databaseId;
    window.dbProvider = dbProvider;
    $("body").on('DOMSubtreeModified',
        "#v-pills-tab-tables",
        function () {
            window.tableCount = $('#v-pills-tab-tables').children().length;
        });

    getDatabase(window.databaseId);
    $('#addColumnModal').on('shown.bs.modal',
        function (e) {
            $('#addColumnForm').trigger('reset');
            getDataTypes(window.dbProvider);

            window.addColumnDto = new AddColumnDto(parseInt(window.databaseId), window.dbProvider);
            var tableName = $(e.relatedTarget).data('id');
            window.addColumnDto.TableName = tableName;
            $('#tableName').val(tableName);
            $('#providerInfo').val(window.dbProvider);
        });
    prepareInputChangeEvents();
};

var parseTables = function parseTables(table) {

    var tabTables = $('#v-pills-tab-tables');
    var tableContent = $('#v-pills-tabContent-tables');


    table.forEach(function (table) {

        // Add Tab Link

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
        

        // Add Table Content Div

        var tableContentString = '<div class="tab-pane fade ';
        if (window.tableCount === 0) {
            tableContentString += ' show active"';
        } else {
            tableContentString += '"';
        }


        tableContentString += 'id="v-pills-' + window.tableCount + '" role="tabpanel" ' +
            'aria-labelledby="v-pills-home-tab">';
        tableContentString += '<div class="container">';


        // Add Column Section
        tableContentString += '<div class="row">';
        tableContentString += '<h5>Columns</h5>';
        tableContentString += '<button type="button"' +
            ' class="btn btn-info btn-sm"' +
            ' style="margin-bottom: 5px;margin-left:10px;"' +
            ' data-toggle="modal" ' +
            ' data-id=' + table.tableName +
            ' data-target="#addColumnModal">';
        tableContentString += 'Add';
        tableContentString += '</button>';
        tableContentString += '</div>';


        tableContentString += '<div class="row">';
        tableContentString += '<div class="col-md-4">';
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
        tableContentString += '<hr/>';
        tableContentString += '</div>';
        tabTables.append(tabTablesString);

        table.columns.forEach(function (column) {
            tableContentString += '<hr/>';
            tableContentString += '<div class="row">';
            tableContentString += '<div class="col-md-4">';
            tableContentString += column.columnName;
            tableContentString += '</div>';
            tableContentString += '<div class="col-md-2">';
            tableContentString += column.dataType;
            tableContentString += '</div>';
            tableContentString += '<div class="col-md-2">';
            tableContentString += column.defaultValue;
            tableContentString += '</div>';
            tableContentString += '<div class="col-md-2">';
            tableContentString += column.primaryKey;
            tableContentString += '</div>';
            tableContentString += '<div class="col-md-2">';
            tableContentString += column.autoInc;
            tableContentString += '</div>';
            tableContentString += '</div>';

        });

        // Add Foreign Key Section


        tableContentString += '<hr/>';
        tableContentString += '<div class="row">';
        tableContentString += '<h5>Foreign Keys</h5>';
        tableContentString += '<button type="button"' +
            ' class="btn btn-info btn-sm"' +
            ' style="margin-bottom: 5px;margin-left:10px;"' +
            ' data-toggle="modal" ' +
            ' data-id=' + table.tableName +
            ' data-target="#addForeignKeyModal">';
        tableContentString += 'Add';
        tableContentString += '</button>';
        tableContentString += '</div>';

        tableContentString += '<br/>';
        tableContentString += '<div class="row">';
        tableContentString += '<div class="col-md-4">';
        tableContentString += 'Key Name';
        tableContentString += '</div>';
        tableContentString += '<div class="col-md-2">';
        tableContentString += 'Source';
        tableContentString += '</div>';
        tableContentString += '<div class="col-md-2">';
        tableContentString += 'Target';
        tableContentString += '</div>';
        tableContentString += '<div class="col-md-2">';
        tableContentString += 'Source';
        tableContentString += '</div>';
        tableContentString += '<div class="col-md-2">';
        tableContentString += 'Target';
        tableContentString += '</div>';

        tableContentString += '<hr/>';
        tableContentString += '</div>';


        table.foreignKeys.forEach(function (foreignKey) {
            tableContentString += '<hr/>';
            tableContentString += '<div class="row">';
            tableContentString += '<div class="col-md-4">';
            tableContentString += foreignKey.foreignKeyName;
            tableContentString += '</div>';
            tableContentString += '<div class="col-md-2">';
            tableContentString += foreignKey.sourceTable;
            tableContentString += '</div>';
            tableContentString += '<div class="col-md-2">';
            tableContentString += foreignKey.targetTable;
            tableContentString += '</div>';
            tableContentString += '<div class="col-md-2">';
            tableContentString += foreignKey.sourceColumn;
            tableContentString += '</div>';
            tableContentString += '<div class="col-md-2">';
            tableContentString += foreignKey.targetColumn;
            tableContentString += '</div>';
            tableContentString += '</div>';
        });

        tableContentString += '</div>';


        tableContent.append(tableContentString);
        window.tableCount++;
    });



};

var getDatabase = function getDatabase(databaseId) {
    var tabTables = $('#v-pills-tab-tables');
    var tableContent = $('#v-pills-tabContent-tables');
    tabTables.empty();
    tableContent.empty();

    $.ajax({
        url: 'https://localhost:44383/Database/GetDatabase?databaseId=' + databaseId,
        type: 'GET',
        success: function (data, textStatus, xhr) {
            parseTables(data.tables);
        },
        complete: function (xhr, textStatus) {

        }
    }).done(function (result) {

    }).fail(function (jqXHR, textStatus, error) {
        $('#errorModalTitle').text('Hata!');
        $('#errorModalBodyText').text('Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..!');
        $('#errorModal').modal('show');
        $('#errorModal').on('hidden.bs.modal', function (e) {
            window.location.replace("https://localhost:44383/Database/");
        });
    });
};


var getTable = function getTable( tableName, provider) {
    $.ajax({
        url: 'https://localhost:44383/Database/GetTable?databaseId=' + window.databaseId
            + '&tableName='+tableName+'&provider='+provider,
        type: 'GET',
        success: function (data, textStatus, xhr) {
            console.log(data);
        },
        complete: function (xhr, textStatus) {

        }
    }).done(function (result) {

    }).fail(function (jqXHR, textStatus, error) {
        $('#errorModalTitle').text('Hata!');
        $('#errorModalBodyText').text('Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..!');
        $('#errorModal').modal('show');
        $('#errorModal').on('hidden.bs.modal', function (e) {
            window.location.replace("https://localhost:44383/Database/");
        });
    });
};




var getDataTypes = function getDataTypes(provider) {
    $.ajax({
        url: 'https://localhost:44383/Database/GetDataTypes?provider=' + provider,
        type: 'GET',
        success: function (data, textStatus, xhr) {
            data.forEach(function (d) {
                $('#columnTypesSelect').append($('<option>',
                    {
                        value: d,
                        text: d
                    }));
            });
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
            window.location.replace("https://localhost:44383/Database/");
        });
    });

};

var prepareInputChangeEvents = function prepareInputChangeEvents() {
    $('#dataLength').fadeOut();
    $('#addColumnModalSubmit').click(function (e) {
        e.preventDefault();
        if (window.addColumnDto === null) {
            $('#errorModalTitle').text('Hata!');
            $('#errorModalBodyText').text('Herhangi bir veri gönderilmedi..!');
            $('#errorModal').modal('show');
        }
        addColumn(window.addColumnDto);
    });
    $('#columnName').on('input', function () {
        if (this.value.search(' ') >= 0) {
            alert('Lütfen Sütun isimlerinde boşluk bırakmayınız');

            $('#columnName').val(this.value.replace(/\s/g, ''));
            window.addColumnDto.ColumnName = $('#columnName').val();
        } else {
            window.addColumnDto.ColumnName = $('#columnName').val();
        }
    });
    $('#columnTypesSelect').on('change', function (e) {
        window.addColumnDto.DataType = this.value;

        if (this.value === 'char' ||
            this.value === 'varchar' ||
            this.value.search('text') >= 0) {
            $('#dataLength').fadeIn();
            window.addColumnDto.HasLength = true;
            //
        } else {
            $('#dataLength').fadeOut();
            window.addColumnDto.HasLength = false;
        }

    });
    $('#isPrimary').on('change', function () {
        window.addColumnDto.PrimaryKey = this.checked;
    });
    $('#isAutoInc').on('change', function () {
        window.addColumnDto.AutoInc = this.checked;
    });
    $('#isNotNull').on('change', function () {
        window.addColumnDto.NotNull = this.checked;
    });
    $('#isUnique').on('change', function () {
        window.addColumnDto.Unique = this.checked;
    });
    $('#defaultValue').on('change', function () {
        window.addColumnDto.DefaultValue = this.value;
    });
    $('#dataLength').on('change', function () {
        if (window.addColumnDto.HasLength === false) {
            alert('Bu tip için length girilemez');
        } else {
            window.addColumnDto.DataLength = parseInt(this.value);
        }
    });



};

var addColumn = function addColumn(columnObj) {
    $.ajax({
        type: 'POST',
        beforeSend: function (request) {
            request.setRequestHeader("Content-Type", "application/json");
        },
        url: 'https://localhost:44383/Database/AddColumn',
        data: JSON.stringify(columnObj),
        contentType: "application/json",
        success: function (data) {
            getDatabase(columnObj.DatabaseId);
            $('#addColumnModal').modal('toggle');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(textStatus);
            console.log(errorThrown);
            $('#errorModalTitle').text('Hata!');
            $('#errorModalBodyText').text(XMLHttpRequest.responseText);
            $('#errorModal').modal('show');
        },
        done: function (data) {
            console.log(data.statusCode);
        }
    });
};
﻿window.tableCount = 0;


// JS Dosyasını Initialize eder
var init = function init(databaseId, dbProvider) {
    window.databaseId = databaseId;
    window.dbProvider = dbProvider;
    $("body").on('DOMSubtreeModified',
        "#v-pills-tab-tables",
        function () {
            window.tableCount = $('#v-pills-tab-tables').children().length;
        });

    getDatabase(window.databaseId);

    // addColumnModal Shown Event

    $('#addColumnModal').on('shown.bs.modal',
        function (e) {
            $('#addColumnForm').trigger('reset');
            $('#dataLength').fadeOut();
            getDataTypes(window.dbProvider);

            window.addColumnDto = new AddColumnDto(parseInt(window.databaseId), window.dbProvider);
            var tableName = $(e.relatedTarget).data('id');
            window.addColumnDto.TableName = tableName;
            $('#tableName').val(tableName);
            $('#providerInfo').val(window.dbProvider);
        });

    // addForeignKeyModal Shown Event

    $('#addForeignKeyModal').on('shown.bs.modal',
        function (e) {
            if (window.databaseTables === null || window.foreignKeys === null) {
                showCriticalError('Hata', 'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..', "https://localhost:44383/Database/")
                return;
            }


            $('#addForeignKeyForm').trigger('reset');
            $('#foreignKeySourceColumn').find('option').not(':first').remove();
            $('#foreignKeyTargetTable').find('option').not(':first').remove();
            $('#foreignKeyTargetColumn').find('option').not(':first').remove();




            window.addForeignKeyDto = new AddForeignKey(parseInt(window.databaseId), window.dbProvider);
            var tableName = $(e.relatedTarget).data('id');
            window.addForeignKeyDto.TableName = tableName;
            window.addForeignKeyDto.SourceTable = tableName;
            window.addForeignKeyDto.Provider = window.dbProvider;

            $('#foreignKeySourceTable').val(tableName);
            var found = getColumnsByTableName(tableName);
            var columnCount = 0;
            found.columns.forEach(function (d) {

                if (checkRelationColumnAvailability(d.columnName, found.tableName)) {
                    $('#foreignKeySourceColumn').append($('<option>',
                        {
                            value: d.columnName,
                            text: d.columnName
                        }));
                    columnCount++;
                }

            });
            if (columnCount === 0) {
                //
                $('#errorModalTitle').text('Hata!');
                $('#errorModalBodyText').text('Hiçbir sütun ilişkilendirme için kullanılabilir değil..!');
                $('#errorModal').modal('show');
                $('#errorModal').on('hidden.bs.modal', function (e) {
                    $('#addForeignKeyModal').modal('toggle');

                });
            }

            var otherTables = window.databaseTables.filter(x => x.tableName !== tableName);
            otherTables.forEach(function (table) {
                $('#foreignKeyTargetTable').append($('<option>',
                    {
                        value: table.tableName,
                        text: table.tableName
                    }));
            });

        });
    prepareInputChangeEvents();

    //Loading barı kapatır.
    $('#loadingSpinner').fadeOut();

    // Sistemi belirtilen sürede bir yeniler. ex.10dk
    setInterval(systemCheck, 600000);
};


var systemCheck = function systemCheck() {
    $('#loadingSpinner').fadeIn(500);
    getDatabase(window.databaseId);
    $('#loadingSpinner').fadeOut(500);
};






var parseDatabase = function parseDatabase(table) {
    var tabTables = $('#v-pills-tab-tables');
    var tableContent = $('#v-pills-tabContent-tables');
    tabTables.empty();
    tableContent.empty();

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
        //Container başlangıç
        tableContentString += '<div class="container">';
        // Add Column Section
        tableContentString += '<h5 class="text-center">Columns</h5>';
        tableContentString += '<hr/>';
        tableContentString += '<div class="row">';
        tableContentString += '<table class="table table-striped table-bordered" id="databaseTable_Columns_' + table.tableName + '"></table>';
        tableContentString += '</div>';
        // Add Foreign Key Section
        tableContentString += '<h5 class="text-center">Foreign Keys</h5>';
        tableContentString += '<hr/>';
        tableContentString += '<div class="row">';
        tableContentString += '<table class="table table-striped table-bordered" id="databaseTable_Foreigns_' + table.tableName + '"></table>';
        tableContentString += '</div>';
        // Container bitiş
        tableContentString += '</div>';
        tabTables.append(tabTablesString);
        tableContent.append(tableContentString);
        // Initialize Datatable for Columns
        initDataTableForColumns(table.tableName);
        initDataTableForForeigns(table.tableName);
        window.tableCount++;
    });
};

// Table için foreignkeylerin datatableini ayarlar.

var initDataTableForForeigns = function initDataTableForForeigns(tableName) {

    if (window.databaseTables === null || window.databaseTables === undefined) {
        showCriticalError('Hata', 'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..', "https://localhost:44383/Database/")
    }

    var data = window.databaseTables.find(x => x.tableName === tableName);
    if (data) {
        console.log(data.columns);
        var buttonStr = '';
        buttonStr += '<button type="button"' +
            ' class="btn btn-info btn-sm"' +
            ' style="margin-bottom: 5px;margin-left:10px;"' +
            ' data-toggle="modal" ' +
            ' data-id=' + tableName +
            ' data-target="#addForeignKeyModal">';
        buttonStr += 'Add';
        buttonStr += '</button>';

        $('#databaseTable_Foreigns_' + tableName).DataTable({
            processing: true,
            data: data.foreignKeys,
            destroy: true,
            columns: [
                { data: "foreignKeyName", title: "Key Name" },
                { data: "targetTable", title: "Target Table" },
                { data: "targetColumn", title: "Target Column" },
                { data: "sourceTable", title: "Source Table" },
                { data: "sourceColumn", title: "Source Column" },
                { data: "onUpdateAction", title: "OnUpdate Action" },
                { data: "onDeleteAction", title: "OnDelete Action" },
                {
                    data: null,
                    title: buttonStr,
                    className: "center",
                    defaultContent: '<a href="" class="editor_edit">Edit</a> / <a href="" class="editor_remove">Delete</a>'
                }
            ],
            "paging": false,
            "ordering": false,
            "info": false,
            "searching": false

        });
    } else {
        showCriticalError('Hata', 'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..', "https://localhost:44383/Database/")
    }


};

// Table için sütunların datatableını ayarlar.

var initDataTableForColumns = function initDataTableForColumns(tableName) {

    if (window.databaseTables === null || window.databaseTables === undefined) {
        showCriticalError('Hata', 'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..', "https://localhost:44383/Database/")
    }

    var data = window.databaseTables.find(x => x.tableName === tableName);
    if (data) {
        console.log(data.columns);
        var buttonStr = '';
        buttonStr += '<button type="button"' +
            ' class="btn btn-info btn-sm"' +
            ' style="margin-bottom: 5px;margin-left:10px;"' +
            ' data-toggle="modal" ' +
            ' data-id=' + data.tableName +
            ' data-target="#addColumnModal">';
        buttonStr += 'Add';
        buttonStr += '</button>';

        $('#databaseTable_Columns_' + tableName).DataTable({
            processing: true,
            data: data.columns,
            destroy: true,
            columns: [
                { data: "columnName", title: "Column Name" },
                { data: "dataType", title: "Data Type" },
                { data: "dataLength", title: "Data Length" },
                { data: "primaryKey", title: "Primary Key?" },
                { data: "unique", title: "Unique?" },
                { data: "autoInc", title: "Auto Increment?" },
                { data: "notNull", title: "Not Null?" },
                {
                    data: null,
                    title: buttonStr,
                    className: "center",
                    defaultContent: '<a href="" class="editor_edit">Edit</a> / <a href="" class="editor_remove">Delete</a>'
                }
            ],
            "oLanguage": {
                "sEmptyTable": "Your custom message for empty table"
            },
            "paging": false,
            "ordering": false,
            "info": false,
            "searching": false

        });
    } else {
        showCriticalError('Hata', 'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..', "https://localhost:44383/Database/")
    }


};

// Tablo ismine göre tableın sütunlarını getirir.
var getColumnsByTableName = function getColumns(tableName) {
    if (window.databaseTables) {
        let found = window.databaseTables.find(x => x.tableName === tableName);
        if (found) {
            return found;
        } else {
            showCriticalError('Hata', 'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..', "https://localhost:44383/Database/")
            return null;
        }

    } else {
        showCriticalError('Hata', 'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..', "https://localhost:44383/Database/")
        return null;
    }
};

// Sitenin kapatılacağı hatayı dinamik şekilde gösterir
var showCriticalError = function showCriticialError(title, body, href) {
    $('#errorModalTitle').text(title);
    $('#errorModalBodyText').text(body);
    $('#errorModal').modal('show');
    $('#errorModal').on('hidden.bs.modal', function (e) {
        window.location.replace(href);
    });
}

// Source Column için kullanılacak sütunun uygunluğuna bakılır.
var checkRelationColumnAvailability = function checkRelationColumnAvailability(columnName, tableName) {
    var targetCheck = true;

    window.databaseTables.forEach(function (table) {
        var result = table.foreignKeys.find(x =>
            x.targetColumn === columnName && x.targetTable === tableName);
        if (result !== null && result !== undefined) {
            targetCheck = false;
            return;
        }
    });
    var checkPoint2 = window.databaseTables.find(x => x.tableName === tableName);
    if (checkPoint2 === null || checkPoint2 === undefined) {
        return false;
    }
    var sourceCheck = checkPoint2.foreignKeys.find(x =>
        x.sourceColumn === columnName && x.sourceTable === tableName);



    return targetCheck && (sourceCheck === null || sourceCheck === undefined);


};

// Veritabanı Idsine göre server-side işlem yapılır.
var getDatabase = function getDatabase(databaseId) {
    var tabTables = $('#v-pills-tab-tables');
    var tableContent = $('#v-pills-tabContent-tables');
    tabTables.empty();
    tableContent.empty();

    $.ajax({
        url: 'https://localhost:44383/Database/GetDatabase?databaseId=' + databaseId,
        type: 'GET',
        success: function (data, textStatus, xhr) {
            window.databaseTables = data.tables;
            parseDatabase(data.tables);
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

// Tablo ismine ve providere göre tablo getirilir.
var getTable = function getTable(tableName, provider) {
    $.ajax({
        url: 'https://localhost:44383/Database/GetTable?databaseId=' + window.databaseId
            + '&tableName=' + tableName + '&provider=' + provider,
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

// Kullanılabilir veri tipi providere bağlı olarak getirilir.
var getDataTypes = function getDataTypes(provider) {
    $.ajax({
        url: 'https://localhost:44383/Database/GetDataTypes?provider=' + provider,
        type: 'GET',
        success: function (data, textStatus, xhr) {
            $('#columnTypesSelect').find('option').not(':first').remove();
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

// Sistem üzerinde property change eventleri initialize edilir.
var prepareInputChangeEvents = function prepareInputChangeEvents() {

    // Add Column Modal
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


    // Add Foreign Key Modal


    $('#foreignKeyTargetTable').on('change', function (e) {
        window.addForeignKeyDto.TargetTable = this.value;
        var found = getColumnsByTableName(this.value);
        found.columns.forEach(function (d) {
            if (checkRelationColumnAvailability(d.columnName, found.tableName)) {
                $('#foreignKeyTargetColumn').append($('<option>',
                    {
                        value: d.columnName,
                        text: d.columnName
                    }));
            }

        });


    });
    $('#foreignKeyTargetColumn').on('change', function (e) {
        window.addForeignKeyDto.TargetColumn = this.value;
    });
    $('#foreignKeySourceColumn').on('change', function (e) {
        window.addForeignKeyDto.SourceColumn = this.value;
    });
    $('#foreignKeyName').on('change', function (e) {
        window.addForeignKeyDto.ForeignKeyName = this.value;
    });


    $('#addForeignKeySubmit').click(function (e) {
        e.preventDefault();
        if (window.addForeignKeyDto === null) {
            $('#errorModalTitle').text('Hata!');
            $('#errorModalBodyText').text('Herhangi bir veri gönderilmedi..!');
            $('#errorModal').modal('show');
        }
        addForeignKey(window.addForeignKeyDto);
    });

};

// Sütun ekleme (Server-Side) 
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
            $('#errorModalTitle').text('Hata!');
            $('#errorModalBodyText').text(XMLHttpRequest.responseText);
            $('#errorModal').modal('show');
        },
        done: function (data) {
        }
    });
};

// Relationship ekleme (Server-Side)
var addForeignKey = function addForeignKey(foreignKey) {
    $.ajax({
        type: 'POST',
        beforeSend: function (request) {
            request.setRequestHeader("Content-Type", "application/json");
        },
        url: 'https://localhost:44383/Database/AddForeignKey',
        data: JSON.stringify(foreignKey),
        contentType: "application/json",
        success: function (data) {

            $('#addForeignKeyModal').modal('toggle');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            $('#errorModalTitle').text('Hata!');
            $('#errorModalBodyText').text(XMLHttpRequest.responseText);
            $('#errorModal').modal('show');
        },
        done: function (data) {
        }
    });
}
﻿window.tableCount = 0;

baseUrl = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '');


// JS Dosyasını Initialize eder
var init = function init(databaseId, dbProvider) {
    window.databaseId = databaseId;
    window.dbProvider = dbProvider;
    $("body").on('DOMSubtreeModified',
        "#v-pills-tab-tables",
        function () {
            window.tableCount = $('#v-pills-tab-tables').children().length;
        });

    getDatabase(window.databaseId, 'columnTypesSelect');


    setTimeout(function () {
        $('#addColumnModal').on('shown.bs.modal',
            function (e) {
                $('#addColumnForm').trigger('reset');
                $('#dataLength').fadeOut();
                getDataTypes(window.dbProvider, 'columnTypesSelect');

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
                    showCriticalError('Hata',
                        'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..',
                        baseUrl + "/Database/");
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

        // updateColumnModal Shown Event
        $('#updateColumnModal').on('shown.bs.modal',
            function (e) {

                if (window.databaseTables === null || window.databaseTables === undefined)
                    showCriticalError('Hata',
                        'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..',
                        baseUrl + "/Database/");
                $('#updateColumnForm').trigger('reset');
                $('#updateDataLength').fadeOut();
                getDataTypes(window.dbProvider, 'updateDataType');
                window.updateColumnDto = null;
                window.updateColumnDto = new UpdateColumnDto(parseInt(window.databaseId), window.dbProvider);
                var tableName = $(e.relatedTarget).data('id');
                var columnName = $(e.relatedTarget).data('columnname');
                var column = window.databaseTables.find(x => x.tableName === tableName).columns.find(y => y.columnName === columnName);

                window.updateColumnDto.TableName = tableName;
                window.updateColumnDto.ColumnName = column.columnName;
                var dataType = column.dataType.replace(/[()]/g, '');
                dataType = dataType.replace(/[0-9]/g, '');;
                window.updateColumnDto.DataType = dataType;


                window.updateColumnDto.DataLength = column.dataLength;
                window.updateColumnDto.NotNull = column.notNull;
                window.updateColumnDto.AutoInc = column.autoInc;
                window.updateColumnDto.Unique = column.unique;
                window.updateColumnDto.PrimaryKey = column.primaryKey;
                window.updateColumnDto.DefaultValue = column.defaultValue;
                window.updateColumnDto.HasLength = column.hasLength;
                window.updateColumnDto.databaseId = databaseId;
                window.updateColumnDto.Provider = dbProvider;
                window.updateColumnDto.OldColumn = column;

                setTimeout(function () {
                    $('#updateTableName').val(tableName);
                    $('#updateProviderInfo').val(window.dbProvider);
                    $("#updateDataType").val(dataType).change();
                    $('#updateColumnName').prop('readonly', true);
                    $('#updateColumnName').val(columnName);
                    $('#updateDataLength').val(column.dataLength);

                    $('#updateIsAutoInc').prop('checked', column.autoInc);
                    $('#updateIsAutoInc').val(column.autoInc);

                    $('#updateIsNotNull').prop('checked', column.notNull);
                    $('#updateIsNotNull').val(column.notNull);

                    $('#updateIsUnique').prop('checked', column.unique);
                    $('#updateIsUnique').val(column.unique);

                    $('#updateIsPrimary').prop('checked', column.primaryKey);
                    $('#updateIsPrimary').val(column.primaryKey);

                }, 300);



            });
        $('#updateForeignKeyModal').on('shown.bs.modal', function (e) {
            if (window.databaseTables === null || window.databaseTables === undefined)
                showCriticalError('Hata',
                    'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..',
                    baseUrl + "/Database/");
            $('#updateForeignKeyForm').trigger('reset');

            $('#updateForeignKeySourceColumn').find('option').not(':first').remove();
            $('#updateForeignKeyTargetTable').find('option').not(':first').remove();
            $('#updateForeignKeyTargetColumn').find('option').not(':first').remove();


            window.updateForeignKeyDto = new UpdateForeignKey(parseInt(window.databaseId), window.dbProvider);
            var tableName = $(e.relatedTarget).data('id');
            var foreignKeyName = $(e.relatedTarget).data('foreignKeyName');

            var existingKey = window.databaseTables.find(x => x.tableName === tableName).foreignKeys
                .find(x => x.ForeignKeyName === foreignKeyName);




            window.updateForeignKeyDto.TableName = tableName;
            window.updateForeignKeyDto.SourceTable = tableName;
            window.updateForeignKeyDto.SourceColumn = existingKey.sourceColumn;
            window.updateForeignKeyDto.TargetColumn = existingKey.targetColumn;
            window.updateForeignKeyDto.TargetTable = existingKey.targetTable;
            window.updateForeignKeyDto.ForeignKeyName = existingKey.foreignKeyName;
            window.updateForeignKeyDto.Provider = window.dbProvider;
            window.updateForeignKeyDto.OldForeignKey = existingKey;

            $('#updateForeignKeySourceTable').val(tableName);
            var found = getColumnsByTableName(tableName);
            var columnCount = 0;


            $('#updateForeignKeySourceColumn').append($('<option>',
                {
                    value: existingKey.sourceColumn,
                    text: existingKey.sourceColumn
                }));

            $('#updateForeignKeyTargetColumn').append($('<option>',
                {
                    value: existingKey.targetColumn,
                    text: existingKey.targetColumn
                }));
            columnCount++;


            found.columns.forEach(function (d) {

                if (checkRelationColumnAvailability(d.columnName, found.tableName)) {
                    $('#updateForeignKeySourceColumn').append($('<option>',
                        {
                            value: d.columnName,
                            text: d.columnName
                        }));
                    columnCount++;
                }

            });


            var otherTables = window.databaseTables.filter(x => x.tableName !== tableName);
            otherTables.forEach(function (table) {
                $('#updateForeignKeyTargetTable').append($('<option>',
                    {
                        value: table.tableName,
                        text: table.tableName
                    }));
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

            $("#updateForeignKeySourceColumn").val(existingKey.sourceColumn).change();
            $("#updateForeignKeyTargetColumn").val(existingKey.targetColumn).change();
            $("#updateForeignKeyTargetTable").val(existingKey.targetTable).change();
            $('#updateForeignKeyName').val(existingKey.foreignKeyName);


        });


        // add Key Shown Event

        $('#addKeyModal').on('shown.bs.modal',
            function (e) {
                if (window.databaseTables === null || window.keys === null) {
                    showCriticalError('Hata',
                        'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..',
                        baseUrl + "/Database/");
                    return;
                }


                $('#addKeyForm').trigger('reset');
                $('#addKeyColumn').find('option').not(':first').remove();





                window.addKeyDto = new AddKeyDto(parseInt(window.databaseId), window.dbProvider);
                var tableName = $(e.relatedTarget).data('id');
                window.addKeyDto.TableName = tableName;
                window.addKeyDto.Provider = window.dbProvider;

                $('#addKeyTableName').val(tableName);
                $('#addKeyProviderInfo').val(addKeyDto.Provider);
                var table = window.databaseTables.find(x => x.tableName === tableName);
                if (table === null || table === undefined)
                    showCriticalError('Hata',
                        'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..',
                        baseUrl + "/Database/");
                else {

                    table.columns.forEach(function (column) {
                        var key = table.keys.find(x => x.keyColumn === column.columnName);
                        if (key === null || key === undefined) {
                            $('#addKeyColumn').append($('<option>',
                                {
                                    value: column.columnName,
                                    text: column.columnName
                                }));
                        }


                    });
                }

            });

        //update key show event
        $('#updateKeyModal').on('shown.bs.modal',
            function (e) {


                setTimeout(function () {
                    if (window.databaseTables === null || window.keys === null) {
                        showCriticalError('Hata',
                            'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..',
                            baseUrl + "/Database/");
                        return;
                    }


                    $('#updateKeyForm').trigger('reset');
                    $('#updateKeyColumn').find('option').not(':first').remove();

                    var tableName = $(e.relatedTarget).data('id');
                    var keyName = $(e.relatedTarget).data('keyname');
                    var table = window.databaseTables.filter(x => x.tableName === tableName)[0];
                    var oldKey = table.keys.find(x => x.keyName === keyName);



                    window.UpdateKeyDto = new UpdateKeyDto(parseInt(window.databaseId), window.dbProvider);
                    window.UpdateKeyDto.TableName = tableName;
                    window.UpdateKeyDto.Provider = window.dbProvider;
                    window.UpdateKeyDto.OldKey = oldKey;
                    window.UpdateKeyDto.KeyName = keyName;



                    //window.UpdateKeyDto.IsPrimary = oldKey.isPrimary;
                    $('#updateKeyTableName').val(tableName);
                    $('#updateKeyProviderInfo').val(window.UpdateKeyDto.Provider);
                    $('#updateKeyName').val(keyName);


                    $('#updateKeyIsPrimary').prop('checked', oldKey.isPrimary);
                    $('#updateKeyIsPrimary').val(oldKey.isPrimary).change();

                    if (table === null || table === undefined) {
                        showCriticalError('Hata',
                            'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..',
                            baseUrl + "/Database/");
                        return;
                    }

                    table.columns.forEach(function (column) {

                        $('#updateKeyColumn').append($('<option>',
                            {
                                value: column.columnName,
                                text: column.columnName
                            }));

                    });
                    $("#updateKeyColumn").val(oldKey.keyColumn).change();
                }, 100);






            });
        //Loading barı kapatır.
        $('#loadingSpinner').fadeOut();

        // Sistemi belirtilen sürede bir yeniler. ex.10dk
        setInterval(systemCheck, 600000);
    }, 2500);
    // addColumnModal Shown Event


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
            'data-id=' + table.tableName +
            ' href="#v-pills-' + window.tableCount + '"' +
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
        tableContentString += '<table class="table table-striped  table-bordered text-center " id="databaseTable_Columns_' + table.tableName + '"></table>';
        tableContentString += '</div>';
        // Add Foreign Key Section
        tableContentString += '<h5 class="text-center">Foreign Keys</h5>';
        tableContentString += '<hr/>';
        tableContentString += '<div class="row">';
        tableContentString += '<table class="table table-striped  table-bordered" id="databaseTable_Foreigns_' + table.tableName + '"></table>';
        tableContentString += '</div>';
        // Add Keys Section
        tableContentString += '<h5 class="text-center">Keys</h5>';
        tableContentString += '<hr/>';
        tableContentString += '<div class="row">';
        tableContentString += '<table class="table table-striped  table-bordered" id="databaseTable_Keys_' + table.tableName + '"></table>';
        tableContentString += '</div>';
        // Container bitiş
        tableContentString += '</div>';
        tabTables.append(tabTablesString);
        tableContent.append(tableContentString);
        // Initialize Datatable for Columns
        initDataTableForColumns(table.tableName);
        initDataTableForForeigns(table.tableName);
        initDataTableForKeys(table.tableName);
        window.tableCount++;
    });
};

// Table için foreignkeylerin datatableini ayarlar.

var initDataTableForForeigns = function initDataTableForForeigns(tableName) {

    if (window.databaseTables === null || window.databaseTables === undefined) {
        showCriticalError('Hata',
            'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..',
            baseUrl + "/Database/");
    }

    var data = window.databaseTables.find(x => x.tableName === tableName);
    if (data) {
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
                    render: function (data, type, row, meta) {
                        return type === 'display' ? getForeignKeyButtonString(data) : data;
                    },
                    title: buttonStr,
                    className: "center"
                }
            ],
            "paging": false,
            "ordering": false,
            "info": false,
            "searching": false,

        });
    } else {
        showCriticalError('Hata',
            'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..',
            baseUrl + "/Database/");
    }


};

// Table için Indexlerin datatableini ayarlar.

var initDataTableForKeys = function initDataTableForKeys(tableName) {

    if (window.databaseTables === null || window.databaseTables === undefined) {
        showCriticalError('Hata',
            'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..',
            baseUrl + "/Database/");
    }

    var data = window.databaseTables.find(x => x.tableName === tableName);
    if (data) {
        var buttonStr = '';
        buttonStr += '<button type="button"' +
            ' class="btn btn-info btn-sm"' +
            ' style="margin-bottom: 5px;margin-left:10px;"' +
            ' data-toggle="modal" ' +
            ' data-id=' + tableName +
            ' data-target="#addKeyModal">';
        buttonStr += 'Add';
        buttonStr += '</button>';

        $('#databaseTable_Keys_' + tableName).DataTable({
            processing: true,
            data: data.keys,
            destroy: true,
            columns: [
                { data: "keyName", title: "Key Name" },
                { data: "keyColumn", title: "Key Column" },
                { data: "isPrimary", title: "Is Primary" },
                { data: "tableName", title: "Table Name" },
                {
                    data: null,
                    render: function (data, type, row, meta) {
                        return type === 'display' ? getKeyEditButtonString(data) : data;
                    },
                    title: buttonStr,
                    className: "center"
                }
            ],
            "paging": false,
            "ordering": false,
            "info": false,
            "searching": false,

        });
    } else {
        showCriticalError('Hata',
            'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..',
            baseUrl + "/Database/");
    }


};


// Table için sütunların datatableını ayarlar.
var getKeyEditButtonString = function getKeyEditButtonString(data) {
    var buttonStr = '';
    buttonStr += '<button type="button"' +
        ' class="btn btn-info btn-sm"' +
        ' style="margin-bottom: 5px;margin-left:10px;"' +
        ' data-toggle="modal" ' +
        ' data-keyName=' + data.keyName +
        ' data-id=' +
        data.tableName +
        ' data-target="#updateKeyModal">';
    buttonStr += 'Edit';
    buttonStr += '</button>';
    buttonStr += '<button type="button"' +
        ' class="btn btn-danger btn-sm"' +
        ' style="margin-bottom: 5px;margin-left:10px;"' +
        ' onclick="deleteKey(\'' + data.keyName + '\',\'' + data.tableName + '\')">';
    buttonStr += 'Delete';
    buttonStr += '</button>';
    return buttonStr;
};
var getColumnEditButtonString = function getColumnEditButtonString(data) {
    var buttonStr = '';
    buttonStr += '<button type="button"' +
        ' class="btn btn-info btn-sm"' +
        ' style="margin-bottom: 5px;margin-left:10px;"' +
        ' data-toggle="modal" ' +
        ' data-columnName=' + data.columnName +
        ' data-id=' +
        data.tableName +
        ' data-target="#updateColumnModal">';
    buttonStr += 'Edit';
    buttonStr += '</button>';
    buttonStr += '<button type="button"' +
        ' class="btn btn-danger btn-sm"' +
        ' style="margin-bottom: 5px;margin-left:10px;"' +
        ' onclick="deleteColumn(\'' + data.columnName + '\',\'' + data.tableName + '\')">';
    buttonStr += 'Delete';
    buttonStr += '</button>';
    return buttonStr;
};
var getForeignKeyButtonString = function getForeignKeyButtonString(data) {
    var buttonStr = '';
    buttonStr += '<button type="button"' +
        ' class="btn btn-info btn-sm"' +
        ' style="margin-bottom: 5px;margin-left:10px;"' +
        ' data-toggle="modal" ' +
        ' data-foreignKeyName=' + data.foreignKeyName +
        ' data-id=' +
        data.sourceTable +
        ' data-target="#updateForeignKeyModal">';
    buttonStr += 'Edit';
    buttonStr += '</button>';
    buttonStr += '<button type="button"' +
        ' class="btn btn-danger btn-sm"' +
        ' onclick="deleteForeignKey(\'' + data.foreignKeyName + '\',\'' + data.sourceTable + '\')"' +
        ' style="margin-bottom: 5px;margin-left:10px;">';
    buttonStr += 'Delete';
    buttonStr += '</button>';
    return buttonStr;
};
var initDataTableForColumns = function initDataTableForColumns(tableName) {

    if (window.databaseTables === null || window.databaseTables === undefined) {
        showCriticalError('Hata',
            'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..',
            baseUrl + "/Database/");
    }

    var data = window.databaseTables.find(x => x.tableName === tableName);
    if (data) {
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
                {
                    data: "primaryKey", title: "Primary Key?", render: function (data, type, row, meta) {
                        return iconGeneratorForDataTables(type, data);

                    }
                },
                {
                    data: "unique", title: "Unique?", render: function (data, type, row, meta) {
                        return iconGeneratorForDataTables(type, data);

                    }
                },
                {
                    data: "autoInc", title: "Auto Increment?", render: function (data, type, row, meta) {
                        return iconGeneratorForDataTables(type, data);

                    }
                },
                {
                    data: "notNull", title: "Not Null?", render: function (data, type, row, meta) {
                        return iconGeneratorForDataTables(type, data);
                    }
                },
                {
                    data: null,
                    render: function (data, type, row, meta) {
                        return type === 'display' ? getColumnEditButtonString(data) : data;
                    },
                    title: buttonStr,
                    className: "center"
                }
            ],
            "oLanguage": {
                "sEmptyTable": "Your custom message for empty table"
            },
            "paging": false,
            "ordering": false,
            "info": false,
            "searching": false,


        });
    } else {
        showCriticalError('Hata',
            'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..',
            baseUrl + "/Database/");
    }


};

var iconGeneratorForDataTables = function iconGeneratorForDataTables(type, data) {
    if (type === 'display') {
        //
        if (data) {
            return '<i class="fas fa-check-circle"></i>';
        } else {
            return '<i class="far fa-times-circle"></i>';
        }
    } else {
        return data;
    }
};

// Tablo ismine göre tableın sütunlarını getirir.
var getColumnsByTableName = function getColumns(tableName) {
    if (window.databaseTables) {
        let found = window.databaseTables.find(x => x.tableName === tableName);
        if (found) {
            return found;
        } else {
            showCriticalError('Hata',
                'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..',
                baseUrl + "/Database/");
            return null;
        }

    } else {
        showCriticalError('Hata',
            'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..',
            baseUrl + "/Database/");
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
};

var showWarningError = function showWarningError(title, body) {
    $('#errorModalTitle').text(title);
    $('#errorModalBodyText').text(body);
    $('#errorModal').modal('show');

};
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
        url: baseUrl + '/Database/GetDatabase?databaseId=' + databaseId,
        type: 'GET',
        success: function (data, textStatus, xhr) {
            window.databaseTables = data.tables;
            window.databaseName = data.databaseName;
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
            window.location.replace(baseUrl + "/Database/");
        });
    });
};

// Tablo ismine ve providere göre tablo getirilir.
var getTable = function getTable(tableName, provider) {
    $.ajax({
        url: baseUrl + '/Database/GetTable?databaseId=' + window.databaseId
            + '&tableName=' + tableName + '&provider=' + provider,
        type: 'GET',
        success: function (data, textStatus, xhr) {
        },
        complete: function (xhr, textStatus) {

        }
    }).done(function (result) {

    }).fail(function (jqXHR, textStatus, error) {
        $('#errorModalTitle').text('Hata!');
        $('#errorModalBodyText').text('Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..!');
        $('#errorModal').modal('show');
        $('#errorModal').on('hidden.bs.modal', function (e) {
            window.location.replace(baseUrl + "/Database/");
        });
    });
};

// Kullanılabilir veri tipi providere bağlı olarak getirilir.
var getDataTypes = function getDataTypes(provider, selectId) {
    $.ajax({
        url: baseUrl + '/Database/GetDataTypes?provider=' + provider,
        type: 'GET',
        success: function (data, textStatus, xhr) {
            $('#' + selectId).find('option').not(':first').remove();
            data.forEach(function (d) {
                $('#' + selectId).append($('<option>',
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
            window.location.replace(baseUrl + "/Database/");
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

    // Update Column Modal

    $('#updateDataLength').fadeOut();

    $('#updateColumnModalSubmit').click(function (e) {
        e.preventDefault();
        if (window.updateColumnDto === null) {
            $('#errorModalTitle').text('Hata!');
            $('#errorModalBodyText').text('Herhangi bir veri gönderilmedi..!');
            $('#errorModal').modal('show');
        }
        updateColumn(window.updateColumnDto);
    });
    $('#updateColumnName').on('input', function () {
        if (this.value.search(' ') >= 0) {
            alert('Lütfen Sütun isimlerinde boşluk bırakmayınız');

            $('#columnName').val(this.value.replace(/\s/g, ''));
            window.updateColumnDto.ColumnName = $('#updateColumnName').val();
        } else {
            window.updateColumnDto.ColumnName = $('#updateColumnName').val();
        }
    });
    $('#updateDataType').on('change', function (e) {
        window.updateColumnDto.DataType = this.value;
        if (this.value === 'char' ||
            this.value === 'varchar' ||
            this.value.search('text') >= 0) {
            $('#updateDataLength').fadeIn();
            window.updateColumnDto.HasLength = true;
            //
        } else {
            $('#updateDataLength').fadeOut();
            window.updateColumnDto.HasLength = false;
        }

    });
    $('#updateIsPrimary').on('change', function () {
        window.updateColumnDto.PrimaryKey = this.checked;
    });
    $('#updateIsAutoInc').on('change', function () {
        window.updateColumnDto.AutoInc = this.checked;
    });
    $('#updateIsNotNull').on('change', function () {
        window.updateColumnDto.NotNull = this.checked;
    });
    $('#updateIsUnique').on('change', function () {
        window.updateColumnDto.Unique = this.checked;
    });
    $('#updateDefaultValue').on('change', function () {
        window.updateColumnDto.DefaultValue = this.value;
    });
    $('#updateDataLength').on('change', function () {
        if (window.updateColumnDto.HasLength === false) {
            alert('Bu tip için length girilemez');
        } else {
            window.updateColumnDto.DataLength = parseInt(this.value);
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

    // Update Foreign Key Modal


    $('#updateForeignKeyTargetTable').on('change', function (e) {
        window.updateForeignKeyDto.TargetTable = this.value;
        $('#updateForeignKeyTargetColumn').find('option').not(':first').remove();
        var found = getColumnsByTableName(this.value);
        found.columns.forEach(function (d) {
            if (checkRelationColumnAvailability(d.columnName, found.tableName)) {
                $('#updateForeignKeyTargetColumn').append($('<option>',
                    {
                        value: d.columnName,
                        text: d.columnName
                    }));
            }

        });
        if (this.value === window.updateForeignKeyDto.OldForeignKey.targetTable) {
            $('#updateForeignKeyTargetColumn').append($('<option>',
                {
                    value: updateForeignKeyDto.OldForeignKey.targetColumn,
                    text: updateForeignKeyDto.OldForeignKey.targetColumn
                }));

            $("#updateForeignKeyTargetColumn").val(updateForeignKeyDto.OldForeignKey.targetColumn).change();

        }


    });
    $('#updateForeignKeyTargetColumn').on('change', function (e) {
        window.updateForeignKeyDto.TargetColumn = this.value;
    });
    $('#updateForeignKeySourceColumn').on('change', function (e) {
        window.updateForeignKeyDto.SourceColumn = this.value;
    });
    $('#updateForeignKeyName').on('change', function (e) {
        window.updateForeignKeyDto.ForeignKeyName = this.value;
    });
    $('#updateForeignKeySubmit').click(function (e) {
        e.preventDefault();
        if (window.updateForeignKeyDto === null) {
            $('#errorModalTitle').text('Hata!');
            $('#errorModalBodyText').text('Herhangi bir veri gönderilmedi..!');
            $('#errorModal').modal('show');
        }
        updateForeignKey(window.updateForeignKeyDto);
    });

    // Add Key Modal

    $('#addKeyModalSubmit').click(function (e) {
        e.preventDefault();
        if (window.addColumnDto === null) {
            $('#errorModalTitle').text('Hata!');
            $('#errorModalBodyText').text('Herhangi bir veri gönderilmedi..!');
            $('#errorModal').modal('show');
        }
        addKey(window.addKeyDto);
    });
    $('#addKeyName').on('input', function (e) {
        if (this.value.search(' ') >= 0) {
            alert('Lütfen Key isimlerinde boşluk bırakmayınız');

            $('#addKeyName').val(this.value.replace(/\s/g, ''));
            window.addKeyDto.KeyName = $('#addKeyName').val();
        } else {
            window.addKeyDto.KeyName = $('#addKeyName').val();
        }
    });

    $('#addKeyColumn').on('change', function (e) {
        window.addKeyDto.KeyColumn = this.value;
    });
    $('#addKeyIsPrimary').on('change', function () {
        window.addKeyDto.IsPrimary = this.checked;
    });

    //Update Key Modal

    $('#updateKeyModalSubmit').click(function (e) {
        e.preventDefault();
        if (window.addColumnDto === null) {
            $('#errorModalTitle').text('Hata!');
            $('#errorModalBodyText').text('Herhangi bir veri gönderilmedi..!');
            $('#errorModal').modal('show');
        }
        updateKey(window.UpdateKeyDto);
    });
    $('#updateKeyName').on('input', function (e) {
        if (this.value.search(' ') >= 0) {
            alert('Lütfen Key isimlerinde boşluk bırakmayınız');

            $('#updateKeyName').val(this.value.replace(/\s/g, ''));
            window.UpdateKeyDto.KeyName = $('#updateKeyName').val();
        } else {
            window.UpdateKeyDto.KeyName = $('#updateKeyName').val();
        }
    });

    $('#updateKeyColumn').on('change', function (e) {
        window.UpdateKeyDto.KeyColumn = this.value;
    });
    $('#updateKeyIsPrimary').on('change', function () {
        window.UpdateKeyDto.IsPrimary = this.checked;
    });

};

// Sütun ekleme (Server-Side) 
var addColumn = function addColumn(columnObj) {
    $.ajax({
        type: 'POST',
        beforeSend: function (request) {
            request.setRequestHeader("Content-Type", "application/json");
        },
        url: baseUrl + '/Database/AddColumn',
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


// Key Ekleme (Server-side)
var addKey = function addKey(keyObj) {
    $.ajax({
        type: 'POST',
        beforeSend: function (request) {
            request.setRequestHeader("Content-Type", "application/json");
        },
        url: baseUrl + '/Database/AddKey',
        data: JSON.stringify(keyObj),
        contentType: "application/json",
        success: function (data) {
            getDatabase(keyObj.DatabaseId);
            $('#addKeyModal').modal('toggle');
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

//Update Key
var updateKey = function updateKey(keyObj) {
    if (window.UpdateKeyDto.IsPrimary === true) {
        showWarningError('Hata', 'Primary Key Değiştirilemez. ..');
        return;
    }
    $.ajax({
        type: 'POST',
        beforeSend: function (request) {



            request.setRequestHeader("Content-Type", "application/json");
        },
        url: baseUrl + '/Database/UpdateKey',
        data: JSON.stringify(keyObj),
        contentType: "application/json",
        success: function (data) {
            getDatabase(keyObj.DatabaseId);
            $('#updateKeyModal').modal('toggle');
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
        url: baseUrl + '/Database/AddForeignKey',
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
};

var updateColumn = function updateColumn(columnObj) {
    $.ajax({
        type: 'POST',
        beforeSend: function (request) {
            request.setRequestHeader("Content-Type", "application/json");
        },
        url: baseUrl + '/Database/UpdateColumn',
        data: JSON.stringify(columnObj),
        contentType: "application/json",
        success: function (data) {
            getDatabase(columnObj.DatabaseId);
            $('#updateColumnModal').modal('toggle');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            showCriticalError('Hata', 'Veritabanı yüklenirken hata oluştu lütfen daha sonra tekrar deneyiniz..', baseUrl + "/Database/")
        },
        done: function (data) {

        }
    });


};

var deleteColumn = function deleteColumn(columnName, tableName) {

    var key = window.databaseTables.find(x => x.tableName === tableName).columns
        .find(y => y.columnName === columnName);

    var data = {
        "ColumnName": key.columnName,
        "TableName": tableName,
        "Provider": window.dbProvider,
        "DatabaseId": window.databaseId

    };


    $.ajax({
        type: 'DELETE',
        beforeSend: function (request) {
            request.setRequestHeader("Content-Type", "application/json");
        },
        url: baseUrl + '/Database/DeleteColumn',
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function (data) {
            getDatabase(window.databaseId);
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

//delete key
var deleteKey = function deleteKey(keyName, tableName) {

    var key = window.databaseTables.find(x => x.tableName === tableName).keys
        .find(y => y.keyName === keyName);
    console.log(key);
    var dto = new DeleteKeyDto(keyName, tableName, window.dbProvider, window.databaseId);
    if (key.isPrimary)
        showWarningError("Hata!", "Primary Key Drop Edilemez.");
    else
        $.ajax({
            type: 'DELETE',
            beforeSend: function (request) {
                request.setRequestHeader("Content-Type", "application/json");
            },
            url: baseUrl + '/Database/DeleteKey',
            data: JSON.stringify(dto),
            contentType: "application/json",
            success: function (data) {
                getDatabase(window.databaseId);
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


// Update Relation
var updateForeignKey = function updateForeignKey(foreignKey) {
    $.ajax({
        type: 'POST',
        beforeSend: function (request) {
            request.setRequestHeader("Content-Type", "application/json");
        },
        url: baseUrl + '/Database/UpdateForeignKey',
        data: JSON.stringify(foreignKey),
        contentType: "application/json",
        success: function (data) {
            getDatabase(foreignKey.DatabaseId);
            $('#updateForeignKeyModal').modal('toggle');
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

// Delete Relation
var deleteForeignKey = function deleteForeignKey(foreignKey, tableName) {

    var key = window.databaseTables.find(x => x.tableName === tableName).foreignKeys
        .find(y => y.foreignKeyName === foreignKey);

    var data = {
        "ForeignKeyName": key.foreignKeyName,
        "SourceTable": key.sourceTable,
        "DatabaseId": window.databaseId
    };


    $.ajax({
        type: 'DELETE',
        beforeSend: function (request) {
            request.setRequestHeader("Content-Type", "application/json");
        },
        url: baseUrl + '/Database/DeleteForeignKey',
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function (data) {
            getDatabase(window.databaseId);
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

var dropTable = function dropTable() {
    var activeTable = $('#v-pills-tab-tables').children('.nav-link.active');
    var tableName = activeTable.data('id');
    $('#confirmationModal').fadeIn('toggle');
    $('#confirmationModalTitle').html(tableName.toUpperCase() + ' Adlı Tabloyu Silmek istediğinizden Emin Misiniz?');
    $('#confirmationModalBody').html('Bilginize Silinen verilerin geri dönüştürülme işlemi mümkün değildir.');
    $('#confirmationModalSaveButton').html("Tabloyu Sil");
    $('#confirmationModalSaveButton').on('click', function (e) {
        $('#confirmationModal').fadeOut('toggle');
        dropTableRequest(tableName);
    });


};
var dropTableRequest = function dropTableRequest(tableName) {

    var dto = new DeleteTableDto(window.databaseId, window.databaseName, tableName);


    $.ajax({
        type: 'DELETE',
        beforeSend: function (request) {
            request.setRequestHeader("Content-Type", "application/json");
        },
        url: baseUrl + '/Database/DeleteTable',
        data: JSON.stringify(dto),
        contentType: "application/json",
        success: function (data) {
            getDatabase(window.databaseId);
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

function refreshProductTable() {
    $("#productTable").DataTable().ajax.reload(null, false);
}

function OpenModal(curl) {
    if (curl != null && curl.length > 0) {
        $.ajax({
            url: curl,
            method: "GET",
            success: function (data) {
                $('#modal-container').html(data);
                $('#modal-container').modal({
                    show: true,
                    keyboard: false,
                    backdrop: 'static'
                });
                manageModalEscapeKey();
            }
        });
    }
}

function manageModalEscapeKey() {
    $(document).off('keyup');
    $(document).keyup(function () {
        if (event.which == 27) {
            e.preventDefault();
            e.stoppropagation();
            return false;
        }
    });
};

var productDashboardDefaultSettings = {
    stateSave: true,
    stateSaveCallback: function (settings, data) {
        data.search = undefined;
    },
    retrieve: true,
    select: { style: 'single' },
    pagingType: "full_numbers",
    lengthMenu: [5, 10, 25, 50, 75, 100],
    pageLength: 5,
    language: {
        select: {
            rows: {
                1: " - 1 row selected",
                0: ""
            }
        },
        lengthMenu: "_MENU_",
        search: "",
        searchPlaceholder: "Search"
    },
    order: [[0, "desc"]],
    processing: true,
    serverSide: true,
    searchDelay: 500,
    columnDefs: [
        {
            targets: [1, 2, 3, 4, 5], orderable: false, searchable: false
        }
    ]
}

var ProductTableInit = {
    Product: function () {
        var table = $('#productTable').DataTable($.extend({}, productDashboardDefaultSettings, {
            ajax: function (data, callback, settings) {
                $.ajax({
                    url: "/product/getProducts",
                    type: 'GET',
                    data: data,
                    success: function (data) {
                        if (data.recordsTotal === -1)
                            data.recordsTotal = table.page.info().Total;

                        if (data.recordsFiltered === -1)
                            data.recordsFiltered = table.page.info().recordsDisplay;

                        callback(data);
                    }
                });
            },
            columnDefs: [{ "targets": [1, 2, 3, 4, 5], orderable: false }],
            order: [[1, "desc"]],
            columns: [
                { "data": "name" },
                { "data": "description" },
                { "data": "additionalNote" },
                { "data": "category" },
                { "data": "status" },
                {
                    "data": "Actions", render: function (data, type, full) {
                        var returnValue = "";

                        returnValue += '<div class="btn-group">';
                        returnValue += '<button type="button" class="btn btn-default btn-open-product-form">Edit</button>';
                        returnValue += '<button type="button" class="btn btn-default btn-remove-product">Remove</button>';
                        returnValue += '<input id="hidProductID" name="hidProductID" type="hidden" value="' + full.id + '">';
                        returnValue += '</div>';

                        return returnValue;
                    }
                }
            ], drawCallback: function () {
                $(".btn-open-product-form").off('click');
                $(".btn-open-product-form").on('click', function () {
                    var productId = $(this).closest('.btn-group').find("input[name=hidProductID]").val();

                    OpenModal("/product/edit?productId=" + productId);
                });

                $(".btn-remove-product").off('click');
                $(".btn-remove-product").on('click', function () {
                    var productId = $(this).closest('.btn-group').find("input[name=hidProductID]").val();

                    $.ajax({
                        url: "/product/delete?productId=" + productId,
                        method: "POST",
                        success: function (response) {
                            refreshProductTable();
                        },
                        error: function () {
                            alert('An unexpected error occurred');
                        }
                    });
                });

            }
        }));

        return table;
    }
}

$('#modal-container').on('shown.bs.modal', function () {
    $('.modal-dialog').trigger('focus')
})

$(document).ready(function () {
    ProductTableInit.Product();

    $(".btn-add-product-form").off('click');
    $(".btn-add-product-form").on('click', function () {
        OpenModal("/product/edit?productId=" + 0);
    });
});
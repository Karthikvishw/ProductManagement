
function refreshAllProducts() {
    if (typeof refreshProductTable == "function") {
        refreshProductTable();
    }
}

$(document).ready(function () {

    $("#saveProduct").off('click');
    $("#saveProduct").on('click', function (e) {

        e.preventDefault();
        $.ajax({
            url: "/product/addorUpdate",
            method: "POST",
            data: $("#editFormproduct").serialize(),
            success: function (response) {
                $('#modal-container').modal().hide();
                $('#modal-container').removeData();
                $('.modal-backdrop').remove();

                //refreshAllProducts();
            },
            error: function () {
                alert('An unexpected error occurred');
            }
        });
    });
});
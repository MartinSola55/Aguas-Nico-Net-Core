
$(document).ready(function () {
    const dayOfWeek = ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"];
    $("#selectProduct").on('change', function () {
        $("#clientsTable tbody").empty();
        let loadingRow = `<tr>
            <td>
                <div class="spinner-border text-primary" role="status">
                    <span class="sr-only">Cargando...</span>
                </div>
            </td>
            <td></td>
        </tr>`;
        $('#clientsTable tbody').append($(loadingRow));
        let form = $('#form-searchClients');
        $("#form-searchClients input[name='productID']").val($(this).val());
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                $('#clientsTable tbody').empty();
                response.data.forEach(client => {
                    let row = `<tr>
                        <td>${client.name}</td>
                        <td>${client.address}</td>
                        <td>${client.dealer.userName} - ${dayOfWeek[client.deliveryDay]}</td>
                    </tr>`;
                    $('#clientsTable tbody').append($(row));
                });
            },
            error: function (error) {
                $('#clientsTable tbody').empty();
                let row = `<tr>
                    <td>
                        <h6 class="text-danger">No se pudo cargar la información</h6>
                    </td>
                    <td></td>
                </tr>`;
                $('#clientsTable tbody').append($(row));
            }
        });
    });

    // Crear, editar y eliminar productos
    $("#btnDeleteProduct").click(function () {
        Swal.fire({
            title: '¿Está seguro que desea eliminar el producto?',
            text: "Esta acción no se puede revertir",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#1e88e5',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                let form = $('#form-deleteProduct');
                $("#form-deleteProduct input[name='id']").val($("#Product_ID").val());
                $.ajax({
                    url: $(form).attr('action'),
                    method: $(form).attr('method'),
                    data: $(form).serialize(),
                    success: function (response) {
                        $("#btnCloseModal").click();
                        Swal.fire({
                            icon: 'success',
                            title: response.message,
                            confirmButtonColor: '#1e88e5',
                            allowOutsideClick: false,
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.reload();
                            }
                        });
                    },
                    error: function (errorThrown) {
                        Swal.fire({
                            icon: 'error',
                            title: errorThrown.responseJSON.title,
                            text: errorThrown.responseJSON.message,
                            confirmButtonColor: '#1e88e5',
                        });
                    }
                });
            }
        });
    });

    $("#btnSendModal").click(function () {
        let form = $('#form-product');
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                $("#btnCloseModal").click();
                Swal.fire({
                    icon: 'success',
                    title: response.message,
                    confirmButtonColor: '#1e88e5',
                    allowOutsideClick: false,
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.reload();
                    }
                });
            },
            error: function (errorThrown) {
                Swal.fire({
                    icon: 'error',
                    title: errorThrown.responseJSON.title,
                    text: errorThrown.responseJSON.message,
                    confirmButtonColor: '#1e88e5',
                });
            }
        });
    });
});

function editProduct(product) {
    $("#Product_ID").val(product.id);
    $("#Product_Name").val(product.name);
    $("#Product_Price").val(product.price);
    $("#Product_Type").val(product.type);
    $(".modal-title").text("Editar producto");
    $("#btnDeleteProduct").show();
    $("#form-product").attr("action", "/Products/Edit");
}

function createProduct() {
    $("#Product_ID").val("");
    $("#Product_Name").val("");
    $("#Product_Price").val("");
    $("#Product_Type").val("");
    $(".modal-title").text("Crear producto");
    $("#btnDeleteProduct").hide();
    $("#form-product").attr("action", "/Products/Create");
}
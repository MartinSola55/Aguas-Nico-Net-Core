function openModal(clientID, clientName) {
    $("#form-searchClientProducts input[name='id']").val(clientID);
    $("#form-confirmCart input[name='Cart.ClientID']").val(clientID);
    $("#form-confirmCart input:not([type='hidden']").val("");
    $("#cartPaymentMethod").val("");
    $("#cartPaymentAmountContainer").hide();
    $("#divClientAbonos").hide();

    $(".modal-title").text(`Confirmar bajada para ${clientName}`);

    let form = $("#form-searchClientProducts");
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            $("#clientProductsTable tbody").empty();
            $("#clientAbonoProductsTable tbody").empty();

            response.products.forEach(product => {
                $("#clientProductsTable tbody").append(`
                    <tr data-type="${product.type}">
                        <td>${product.name}</td>
                        <td>$${product.price}</td>
                        <td><input type="number" class="form-control" name="quantity" value="" min="1"></td>
                    </tr>
                `);
            });
            if (response.abonoProducts.length > 0) {
                $("#divClientAbonos").show();
            }
            response.abonoProducts.forEach(product => {
                $("#clientAbonoProductsTable tbody").append(`
                    <tr data-type="${product.type}">
                        <td>${product.name}</td>
                        <td>${product.available}</td>
                        <td><input type="number" class="form-control" name="quantity" value="" min="1" max="${product.available}"></td>
                    </tr>
                `);
            });

            $("#modal").modal('show');
            $("input[name='quantity']").on("input", function () {
                let total = 0;
                let rows = $('#clientProductsTable tbody tr');
                for (let i = 0; i < rows.length; i++) {
                    let row = rows[i];
                    if (row.cells[2].children[0].value == "") {
                        continue;
                    }
                    let quantity = parseInt(row.cells[2].children[0].value);
                    let price = parseInt(row.cells[1].innerText.replace("$", ""));
                    total += quantity * price;
                }
                $("#totalCart").text(`Total: $${total}`);
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

function confirmCart() {
    let abonoProducts = [];
    let rows = $('#clientAbonoProductsTable tbody tr');
    for (let i = 0; i < rows.length; i++) {
        let row = rows[i];
        let quantity = parseInt(row.cells[2].children[0].value);
        if (quantity <= 0) {
            Swal.fire({
                icon: 'warning',
                title: "Error",
                text: "La cantidad debe ser mayor a cero",
                confirmButtonColor: '#1e88e5',
            });
            return false;
        }
        if (quantity > parseInt(parseInt(row.cells[1].textContent.trim()))) {
            Swal.fire({
                icon: 'warning',
                title: "Error",
                text: "No se puede bajar más productos del abono de los que dispone",
                confirmButtonColor: '#1e88e5',
            });
            return false;
        }
        if (quantity > 0) {
            abonoProducts.push({
                Quantity: quantity,
                Type: parseInt(row.dataset.type)
            });

        }
    }

    let products = [];
    rows = $('#clientProductsTable tbody tr');
    for (let i = 0; i < rows.length; i++) {
        let row = rows[i];
        let quantity = parseInt(row.cells[2].children[0].value);
        if (quantity <= 0) {
            Swal.fire({
                icon: 'warning',
                title: "Error",
                text: "La cantidad debe ser mayor a cero",
                confirmButtonColor: '#1e88e5',
            });
            return false;
        }
        if (quantity > 0) {
            products.push({
                Quantity: quantity,
                Type: parseInt(row.dataset.type)
            });

        }
    }

    let methods = [];
    methods.push({
        PaymentMethodID: $("#cartPaymentMethod").val(),
        Amount: $("#cartPaymentAmount").val()
    });

    let productsData = {
        Cart: {
            Products: products,
            PaymentMethods: methods,
            AbonoProducts: abonoProducts
        }
    };
    if (products.length <= 0 & abonoProducts.length <= 0 && methods[0].Amount <= 0) {
        Swal.fire({
            icon: 'warning',
            title: "Error",
            text: "No se puede confirmar una bajada sin productos y dinero.",
            confirmButtonColor: '#1e88e5',
        });
        return false;
    }

    let form = $("#form-confirmCart");
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize() + "&" + $.param(productsData),
        success: function (response) {
            Swal.fire({
                title: response.message,
                icon: 'success',
                showCancelButton: false,
                confirmButtonColor: '#1e88e5',
                confirmButtonText: 'OK',
                allowOutsideClick: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = window.location.origin + "/Routes/Details/" + response.data;
                }
            });
        },
        error: function (errorThrown) {
            Swal.fire({
                icon: 'error',
                title: errorThrown.responseJSON.title,
                html: errorThrown.responseJSON.message + "<br/>" + errorThrown.responseJSON.error,
                confirmButtonColor: '#1e88e5',
            });
        }
    });
}

$(document).ready(function () {
    $("#cartPaymentMethod").on("change", function () {
        $("#cartPaymentAmountContainer").show();
    });

    $('#DataTable').DataTable({
        "order": false,
        "language": {
            "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ clientes",
            "sInfoEmpty": "Mostrando 0 a 0 de 0 clientes",
            "sInfoFiltered": "(filtrado de _MAX_ clientes en total)",
            "emptyTable": 'No hay clientes que coincidan con la búsqueda',
            "sLengthMenu": "Mostrar _MENU_ clientes",
            "sSearch": "Buscar:",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior",
            },
        },
    });
});
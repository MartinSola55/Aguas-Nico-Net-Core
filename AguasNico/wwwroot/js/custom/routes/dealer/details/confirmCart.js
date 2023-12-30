function openModal(cartID, clientID, debt) {
    $("#form-searchClientProducts input[name='id']").val(clientID);
    $("#form-confirm input[name='Cart.ID']").val(cartID);

    let form = $("#form-searchClientProducts");
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            $("#clientProductsTable tbody").empty();
            response.data.forEach(product => {
                $("#clientProductsTable tbody").append(`
                    <tr data-id="${product.id}">
                        <td>${product.name}</td>
                        <td>$${product.price}</td>
                        <td><input type="number" class="form-control" name="quantity" value=""></td>
                    </tr>
                `);
            });
            $("#modalConfirmation").modal('show');
            $("input[name='quantity']").on("input", function () {
                let quantity = $(this).val();
                let price = $(this).closest("tr").find("td:eq(1)").text().replace('$', '');
                let total = quantity * price;
                $("#totalCart").text(`Total pedido: $${total}`);
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
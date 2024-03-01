function getClientHistory(clientID, clientName) {
    let form = $("#form-searchClientHistory");
    form.find("input[name='id']").val(clientID);
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            let table = `
            <table class="table table-bordered table-striped">
                <thead>
                    <tr>
                        <th>Fecha</th>
                        <th>Producto</th>
                        <th>Acción</th>
                        <th>Cantidad</th>
                    </tr>
                </thead>
                <tbody>
            `;

            //Crear las filas
            response.data.forEach(function (item) {
                table += `
                <tr>
                    <td>${item.date}</td>
                    <td>${item.productType}</td>
                    <td>${item.actionType}</td>
                    <td>${item.quantity}</td>
                </tr>`;
            });

            table += `
                </tbody>
            </table>
            `;

            Swal.fire({
                html: table,
                confirmButtonColor: '#1e88e5',
                allowOutsideClick: false,
            });
        },
        error: function (errorThrown) {
            Swal.fire({
                icon: 'error',
                title: errorThrown.responseJSON.message,
                confirmButtonColor: '#1e88e5',
            });
        }
    });
}
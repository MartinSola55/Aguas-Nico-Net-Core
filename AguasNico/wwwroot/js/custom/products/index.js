
$(document).ready(function () {
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
});
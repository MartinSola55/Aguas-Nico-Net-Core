
$(document).ready(function () {
    $("#clientsDay").on('change', function () {
        $("#clientsTable tbody").empty();
        $("#searchClientsTotal").text("");
        let loadingRow = `<tr>
            <td>
                <div class="spinner-border text-primary" role="status">
                    <span class="sr-only">Cargando...</span>
                </div>
            </td>
            <td></td>
        </tr>`;
        $('#clientsTable tbody').append($(loadingRow));
        let form = $('#form-clientsByDay');
        $("#form-clientsByDay input[name='day']").val($(this).val());
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                let total = 0;
                $('#clientsTable tbody').empty();
                response.data.forEach(client => {
                    total += client.debt;
                    let row = `<tr>
                        <td>${client.name}</td>
                        <td>$${client.debt >= 0 ? client.debt : client.debt + " a favor"}</td>
                    </tr>`;
                    $('#clientsTable tbody').append($(row));
                });
                $("#searchClientsTotal").text(`Total: $${total}`);
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

    // Clientes no visitados
    $('#dateFromClientsNotVisited').bootstrapMaterialDatePicker({
        maxDate: new Date(),
        time: false,
        format: 'DD/MM/YYYY',
        cancelText: "Cancelar",
        weekStart: 1,
        lang: 'es',
    });
    $("#dateFromClientsNotVisited").on("change", function () {
        $("#divDateToClientsNotVisited").show();
        $('#dateToClientsNotVisited').bootstrapMaterialDatePicker({
            minDate: $("#dateFromClientsNotVisited").val(),
            maxDate: new Date(),
            time: false,
            format: 'DD/MM/YYYY',
            cancelText: "Cancelar",
            weekStart: 1,
            lang: 'es',
        });
    });
    $("#dateToClientsNotVisited").on("change", function () {
        if ($("#dateFromClientsNotVisited").val() !== "" && $("#dateToClientsNotVisited").val() !== "") {
            $("#btnClientsNotVisited").show();
        }
    });
    $("#btnClientsNotVisited").on("click", function () {
        if ($("#dateFromClientsNotVisited").val() === "" || $("#dateToClientsNotVisited").val() === "") {
            Swal.fire({
                icon: 'warning',
                title: 'Error',
                text: 'Debe seleccionar un rango de fechas',
                confirmButtonColor: '#1e88e5',
            });
            return;
        }
        $("#form-clientsNotVisited input[name='dateFromString']").val($("#dateFromClientsNotVisited").val());
        $("#form-clientsNotVisited input[name='dateToString']").val($("#dateToClientsNotVisited").val());
        
        $("#clientsNotVisitedTable tbody").empty();
        let loadingRow = `<tr>
            <td>
                <div class="spinner-border text-primary" role="status">
                    <span class="sr-only">Cargando...</span>
                </div>
            </td>
            <td></td>
        </tr>`;
        $('#clientsNotVisitedTable tbody').append($(loadingRow));
        let form = $('#form-clientsNotVisited');
        $("#form-clientsNotVisited input[name='day']").val($(this).val());
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                $('#clientsNotVisitedTable tbody').empty();
                response.data.forEach(client => {
                    let row = `<tr>
                        <td>${client.name}</td>
                        <td>${client.address}</td>
                    </tr>`;
                    $('#clientsNotVisitedTable tbody').append($(row));
                });
            },
            error: function (error) {
                $('#clientsNotVisitedTable tbody').empty();
                let row = `<tr>
                    <td>
                        <h6 class="text-danger">No se pudo cargar la información</h6>
                    </td>
                    <td></td>
                </tr>`;
                $('#clientsNotVisitedTable tbody').append($(row));
            }
        });
    });

});
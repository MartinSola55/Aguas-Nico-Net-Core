function parseDate(date) {
    return moment(date).format("DD/MM/YYYY").toUpperCase()
}

function parseTime(time) {
    return moment(time).format("HH:mm").toUpperCase()
}

$("#datepicker").on("apply.daterangepicker", function (ev, picker) {
    let date_from = picker.startDate.format('YYYY-MM-DD');
    let date_to = picker.endDate.format('YYYY-MM-DD');
    let dates = [date_from, date_to];
    $("#dataTable").html("");
    $.get("../Sells/ShowSales/?dates=" + dates, function (data) {
        createTable(data);
    })
});

function createTable(data) {
    let content = "";
    content += `
        <table id="dataTable" class="container table table-striped table-bordered table-hover pt-3">
            <thead class='table-dark'>
                <tr class="fw-bold text-center">
                    <td>
                        Fecha venta
                    </td>
                </tr>
            </thead>
            <tbody>`;
    for (let i = 0; i < data.length; i++) {
        content += `
                    <tr>
                        <td class='clickable-row text-center' data-href='/Sells/DetailsSale/`+ data[i].id +`'>
                            Día: `+ parseDate(data[i].date) +`
                            <br>
                            Hora: `+ parseTime(data[i].date) +`
                        </td>
                   </tr>`;
    }
    content += "</tbody>";
    content += "</table>";
    $("#tableContainer").html(content);

    $(".clickable-row").on("click", function () {
        window.location = $(this).data("href");
    });

    $('#dataTable').DataTable(
        {
            searching: false,
            "language": {
                paginate: {
                    "first": "Primera",
                    "last": "Última",
                    "next": "Siguiente",
                    "previous": "Anterior"
                },
                emptyTable: "No existen ventas que coincidan con la búsqueda",
                info: "Mostrando _START_ a _END_ de _TOTAL_ ventas",
                infoEmpty: "Mostrando 0 ventas",
                lengthMenu: "Mostrar _MENU_ ventas",
            },
            order: [[0, 'desc']],
        }
    )
}

moment.locale('es');
$(function () {
    $("#datepicker").daterangepicker({
        "locale": {
            "applyLabel": "Aplicar",
            "cancelLabel": "Cancelar",
            "fromLabel": "Hasta",
            "toLabel": "Desde",
        },
        singleDatePicker: false,
        opens: 'right',
        autoUpdateInput: true,
        autoApply: false
    })
});
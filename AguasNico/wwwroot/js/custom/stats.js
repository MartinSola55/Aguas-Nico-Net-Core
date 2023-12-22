function parseDate(date) {
    return moment(date).format("DD/MM/YYYY").toUpperCase()
}

let hoy = new Date();
let dd = String(hoy.getDate()).padStart(2, '0');
let mm = String(hoy.getMonth() + 1).padStart(2, '0');
let yyyy = hoy.getFullYear();
hoy = dd + '/' + mm + '/' + yyyy;
$("#date").removeAttr("data-val-date");


$("#datepicker").on("apply.daterangepicker", function (ev, picker) {
    let date_from = picker.startDate.format('YYYY-MM-DD');
    let date_to = picker.endDate.format('YYYY-MM-DD');
    let dates = [date_from, date_to];
    $("#tablesContainer").html("");
    $.get("../../Sells/GetBusinessIDS/", function (business) {
        for (var i = 0; i < business.length; i++) {
            $.get("../../Sells/MostSoldProducts/?dates=" + dates + "&id_business=" + business[i], function (data) {
                createTable(data);
            });
        }
    })
});

function createTable(data) {
    let cont = 1;
    let content = "";
    content += data.length == 0 ? "" : `
        <h2 class="text-center mt-5"><b>` + data[0].business_name + `</b><h2/>
        <div class="table100 ver1 m-b-110">
            <div class="table100-head">
                <table>
                    <thead>
                        <tr class="row100 head">
                            <th class="text-center py-3 column10">Top</th>
                            <th class="text-center py-3 column20">Producto</th>
                            <th class="text-center py-3 column10">Total</th>
                            <th class="text-center py-3 column10">Cantidad</th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div class="table100-body js-pscroll ps ps--active-y">
                <table>
                    <tbody>`;
                        for (let i = 0; i < data.length; i++) {
                            content += `
                            <tr class='clickable-row row100 body'>
                                <td class='column10 text-center'>` + cont++ + `</td>
                                <td class='column20'>` + data[i].description + `</td>
                                <td class='column10 text-center'>` + data[i].liters + ` litros</td>
                                <td class='column10 text-center'>` + data[i].quantity + `</td>
                            </tr>`;
                        }
        content += `</tbody>
                </table>
            </div>
        </div>`
    $("#tablesContainer").append(content);
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
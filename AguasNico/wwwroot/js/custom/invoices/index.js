$(document).ready(function () {
    moment.locale('es');
    $('#dateRange').daterangepicker({
        locale: {
            format: 'DD/MM/YYYY',
            separator: ' - ',
            applyLabel: 'Aplicar',
            cancelLabel: 'Cancelar',
            fromLabel: 'Desde',
            toLabel: 'Hasta',
            customRangeLabel: 'Rango personalizado',
            weekLabel: 'S',
        },
        ranges: {
            'Este mes': [moment().startOf('month'), moment().endOf('month')],
            'Mes anterior': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
        },
        applyClass: "btn-info",
    });

    $("#print").click(function() {
        let mode = 'iframe';
        let close = mode == "popup";
        let options = {
            mode: mode,
            popClose: close,
        };
        $("div.printableArea").printArea(options);
    });

    const formattedNumber = (number) => {
        return number.toLocaleString('es-AR', { minimumFractionDigits: 2 });
    }

    function calculateTotal () {
        let subtotal = 0;
        $('#tables_container .productTotal').each(function() {
            const valor = $(this).text().replace('$', '').replace('.', '').replace(',', '.').trim();
            subtotal += parseFloat(valor);
        });
        $("#IVAAmount").html("IVA (21%) : $" + formattedNumber(subtotal*0.21))
        $("#totalAmount").html("<b>Total: </b>$" + formattedNumber(subtotal))
    }

    $("#btnSearchInvoices").click(function () {
        let dateRange = $("#dateRange").val();
        let invoiceDay = $("#InvoiceDay").val();
        let invoiceDealer = $("#InvoiceDealer").val();

        $("#invoiceDealerSelected").text($("#InvoiceDealer option:selected").text());
        $("#invoiceDaySelected").text($("#InvoiceDay option:selected").text());
        $("#dateRangeSelected").text(dateRange);

        let data = {
            dateRange: dateRange,
            invoiceDay: invoiceDay,
            invoiceDealer: invoiceDealer
        };

        $.ajax({
            url: "/Invoices/Show",
            type: "GET",
            data: data,
            success: function (response) {             
                let content = "";
                let total = 0;
                response.data.forEach(({ client, products }) => {
                    content +=
                    `<h3 class='text-start my-0'>${client.name} / Tipo de factura: ${client.invoice_type ?? "Sin cargar"} - CUIT: ${client.cuit ?? "Sin cargar"}</h3>
                    <table class="table table-hover mb-1" style="font-size: 0.75rem !important" >
                        <thead>
                            <tr>
                                <th class="col-6 p-1">Descripción</th>
                                <th class="col-3 p-1 text-right">Cantidad</th>
                                <th class="col-3 p-1 text-right">Subtotal</th>
                            </tr>
                        </thead>
                        <tbody>`;
                    let sum = 0;
                    //item.abonos.forEach((item) => {
                    //    sum += 1 * item.price;
                    //    content +=
                    //    `<tr>
                    //        <td class='p-0'>${item.name}</td>
                    //        <td class='p-0 text-right'>1</td>
                    //        <td class='p-0 text-right'>$${formattedNumber(parseInt(item.price))}</td>
                    //        <td class='p-0 text-right'>${item.date}</td>
                    //        <td class='p-0 text-right productTotal'>$${formattedNumber(parseInt(item.price))}</td>
                    //    </tr>`;
                    //});
                    products.forEach((product) => {
                        sum += product.total;
                        content +=
                        `<tr>
                            <td class='p-0'>${product.type}</td>
                            <td class='p-0 text-right'>${product.quantity}</td>
                            <td class='p-0 text-right productTotal'>$${formatNumber(product.total)}</td>
                        </tr>`;
                    });
                    content +=
                    `<tr>
                        <td class="p-0"></td>
                        <td class="p-0"></td>
                        <td class='p-0 text-right'><b style='font-weight: bold'>Total: $${formatNumber(sum)}</b></td>
                    </tr>
                    </tbody>
                    </table>
                    <hr class='mb-2'>`;
                    total += sum;
                });
                $("#tables_container").html(content);

                $("#IVAAmount").text(`IVA (21%): $${formatNumber(total * 0.21)}`);
                $("#totalAmount").text(`Total: $${formatNumber(total)}`);
            },
            error: function(errorThrown) {
                $("#tables_container").html("");
                if (errorThrown.exception !== null) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        html: 'Se agotó el tiempo de espera, por favor intente nuevamente.<br>Si el problema persiste, intente seleccionar un intervalo de fechas más corto.',
                        confirmButtonColor: '#1e88e5',
                    });
                    return;
                }
                Response('error', errorThrown.responseJSON.title, errorThrown.responseJSON.message);
            }
        });
    });
});

function formatNumber(number) {
    if (isNaN(number)) {
        return "Número inválido";
    }
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
}
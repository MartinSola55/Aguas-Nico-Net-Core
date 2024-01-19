
$(document).ready(function () {
    moment.locale('es');
    $('#dateRange').daterangepicker({
        opens: 'left',
        locale: {
            format: 'DD/MM/YYYY',
            separator: ' - ',
            applyLabel: 'Aplicar',
            cancelLabel: 'Cancelar',
            fromLabel: 'Desde',
            toLabel: 'Hasta',
            customRangeLabel: 'Rango personalizado',
            weekLabel: 'S',
        }
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

        let data = {
            dateRange: dateRange,
            invoiceDay: invoiceDay,
            invoiceDealer: invoiceDealer
        };

        $.ajax({
            url: "/Invoices/Show",
            type: "GET",
            data: data,
            success: function (result) {
                //$("#invoiceDaySelected").text(result.data.invoiceDay);
                //$("#invoiceDealerSelected").text(result.data.invoiceDealer);
                //$("#dateRangeSelected").text(result.data.dateRange);
                
                let content = "";
                response.data.clients.forEach((client) => {
                    content +=
                    `<h3 class='text-start my-0'>${client.name} / Tipo de factura: ${client.invoice_type ?? "Sin cargar"} - CUIT: ${client.cuit ?? "Sin cargar"}</h3>
                    <table class="table table-hover mb-1" style="font-size: 0.75rem !important" >
                        <thead>
                            <tr>
                                <th class="p-1">Descripción</th>
                                <th class="p-1 text-right">Cantidad</th>
                                <th class="p-1 text-right">Precio Unitario</th>
                                <th class="p-1 text-right">Fecha</th>
                                <th class="p-1 text-right">Subtotal</th>
                            </tr>
                        </thead>
                        <tbody>`;
                    let sum = 0;
                    client.abonos.forEach((item) => {
                        sum += 1 * item.price;
                        content +=
                        `<tr>
                            <td class='p-0'>${item.name}</td>
                            <td class='p-0 text-right'>1</td>
                            <td class='p-0 text-right'>$${formattedNumber(parseInt(item.price))}</td>
                            <td class='p-0 text-right'>${item.date}</td>
                            <td class='p-0 text-right productTotal'>$${formattedNumber(parseInt(item.price))}</td>
                        </tr>`;
                    });
                    client.products.forEach((item) => {
                        sum += item.quantity * item.price;
                        content +=
                        `<tr>
                            <td class='p-0'>${item.name}</td>
                            <td class='p-0 text-right'>${item.quantity}</td>
                            <td class='p-0 text-right'>$${formattedNumber(parseInt(item.price))}</td>
                            <td class='p-0 text-right'>-</td>
                            <td class='p-0 text-right productTotal'>$${formattedNumber(item.quantity * item.price)}</td>
                        </tr>`;
                    });
                    content +=
                    `<tr>
                        <td class="p-0"></td>
                        <td class="p-0"></td>
                        <td class="p-0"></td>
                        <td class="p-0"></td>
                        <td class='p-0 text-right'><b style='font-weight: bold'>Total: $${formattedNumber(sum)}</b></td>
                    </tr>
                    </tbody>
                    </table>
                    <hr class='mb-2'>`;
                });
                $("#tables_container").html(content);
                calculateTotal();
                console.log(result.message);
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
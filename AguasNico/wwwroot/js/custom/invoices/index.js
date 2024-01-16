
$(document).ready(function () {
    moment.locale('es');
    $('#dateRange').daterangepicker({
        opens: 'left',
        locale: {
            format: 'DD/MM/YYYY' 
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
                //$("#IVAAmount").text(result.data.IVAAmount);
                //$("#totalAmount").text(result.data.totalAmount);
                
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
                
                console.log(result.message);
            },
            error: function (error) {
                console.error(error);
            }
        });
    });
});
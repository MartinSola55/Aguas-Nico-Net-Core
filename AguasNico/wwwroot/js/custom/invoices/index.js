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
                                <th class="p-1">Descripción</th>
                                <th class="p-1 text-right">Cantidad</th>
                                <th class="p-1 text-right">Subtotal</th>
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
            error: function (error) {
                console.error(error);
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
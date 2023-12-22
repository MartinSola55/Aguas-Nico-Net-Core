function createTable(data) {
    let contenido = "";
    for (let i = 0; i < data.length; i++) {
        contenido += "<tr class='row100 body'>";
        contenido += "<td class='column5 ps-3'>" + data[i].internal_code ?? "" + "</td>";
        contenido += "<td class='column20'>" + data[i].description + "</td>";
        contenido += "<td class='column10'>" + data[i].brand + "</td>";
        let category = data[i].category != null ? data[i].category : " - ";
        contenido += "<td class='column10'>" + category + "</td>";
        let subcategory = data[i].subcategory != null ? data[i].subcategory : " - ";
        contenido += "<td class='column10'>" + subcategory + "</td>";
        if (data[i].color != null) {
            contenido += "<td class='cell100 column10'>";
            contenido += "<div class='d-flex flex-row justify-content-center align-items-center' >";
            contenido += data[i].color;
            contenido += "<span class='dot ms-2' style='background-color: " + data[i].rgb_hex_code + "'></span>";
            contenido += "</div>";
            contenido += "</td>";
        } else {
            contenido += "<td class='cell100 column10 text-center'>-";
            contenido += "</td>";
        }
        let capacity = data[i].capacity != null ? data[i].capacity : " - ";
        contenido += "<td class='cell100 column10 text-center'>" + capacity + "</td>";
        contenido += "<td class='text-center cell100 column10'>" + data[i].stock + "</td>";
        contenido += "<td class='cell100 column20'>";
        contenido += "<div class='d-flex flex-row justify-content-center'>";
        contenido += "<div class='value-button' onclick='decreaseValue(" + data[i].product_id + ")'>-</div>";
        contenido += "<input type='number' id='number" + data[i].product_id + "' class='number' value='0' />";
        contenido += "<div class='value-button' onclick='increaseValue(" + data[i].product_id + ", " + data[i].stock + ")' value='Increase Value'>+</div>";
        contenido += "</div>";
        contenido += "</td>";
        contenido += "<td class='cell100 column10 pe-3'>";
        contenido += "<div class='d-flex justify-content-center'>";
        contenido += `<form action='/Sells/AddToSale' id='formAddToSale_` + data[i].product_id + `'>`;
        contenido += "<input type='hidden' name='id_prod' id='id_prod_" + data[i].product_id + "' value='" + data[i].product_id + "' />";
        contenido += "<input type='hidden' name='quant' id='quant_" + data[i].product_id + "' />";
        contenido += "<button class='btn' style='background-color: yellowgreen' type='button' onClick='addToSale(" + data[i].product_id + ")'><i class='bi bi-plus-circle'></i></button>";
        contenido += "</form>";
        contenido += "</div>";
        contenido += "</td>";
        contenido += "</tr>";
    }
    $("#contentTable").html(contenido);
}

function increaseValue(id, quant) {
    let value = parseInt(document.getElementById('number' + id).value, 10);
    value = isNaN(value) ? 0 : value;
    value >= quant ? value = quant - 1 : '';
    value++;
    document.getElementById('number' + id).value = value;
}

function decreaseValue(id) {
    let value = parseInt(document.getElementById('number' + id).value, 10);
    value = isNaN(value) ? 0 : value;
    value < 1 ? value = 1 : '';
    value--;
    document.getElementById('number' + id).value = value;
}

$("#btnSearch").on("click", function () {
    let name = $("#txtSearch").val();
    $.get("../../Sells/FilterProductsByName/?name=" + name, function (data) {
        createTable(data);
    })
});

$("#btnFilter").on("click", function () {
    let id_brand = $('#id_brand').val();
    let id_category = $('#id_category').val();
    let id_subcategory = $('#id_subcategory').val();
    let id_color = $('#id_color').val();
    let id_capacity = $('#id_capacity').val();

    $.get("../../Sells/FilterProducts/?id_brand=" + id_brand + "&id_category=" + id_category + "&id_subcategory=" + id_subcategory +
        "&id_color=" + id_color + "&id_capacity=" + id_capacity,
        function (data) {
            createTable(data);
        });
});

$('#txtSearch').keypress(function (e) {
    if (e.which == 13) {
        $(this).blur();
        $('#btnSearch').click();
    }
});

function addToSale(id) {
    let quant = $("#number" + id).val();
    $("#quant_" + id).val(quant);
    $("#formAddToSale_" + id).submit();
};

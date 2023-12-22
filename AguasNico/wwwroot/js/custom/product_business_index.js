const id_business = window.location.pathname.split("/").pop();

$('#txtSearch').keypress(function (e) {
    if (e.which == 13) {
        $(this).blur();
        $('#btnSearch').click();
    }
});

$("#btnSearch").on("click", function () {
    let name = $("#txtSearch").val();
    $.get("../../Product_Business/FilterAllProductsByName/?name=" + name + "&id_business=" + id_business, function (data) {
        createTable(data);
    })
});

$("#btnFilter").on("click", function () {
    let id_brand = $('#id_brand').val();
    let id_category = $('#id_category').val();
    let id_subcategory = $('#id_subcategory').val();
    let id_color = $('#id_color').val();
    let id_capacity = $('#id_capacity').val();

    $.get("../../Product_Business/FilterAllProducts/?id_brand=" + id_brand + "&id_category=" + id_category + "&id_subcategory=" + id_subcategory +
        "&id_color=" + id_color + "&id_capacity=" + id_capacity + "&id_business=" + id_business,
        function (data) {
            createTable(data);
        });
});

function OpenModal(id, description, stock, minimum_stock) {
    $("#exampleModalLabel").text("Actualizar stock - " + description);
    $("#stock").val(stock);
    $("#id_product").val(id);
    if (minimum_stock != undefined) {
        $("#minimum_stock").val(minimum_stock);
    } else {
        $("#minimum_stock").val("");
    }
}

function createTable(data) {
    let content = "";
    for (let i = 0; i < data.length; i++) {
        content += "<tr class='text-center'>";
        content += "<td>" + (data[i].internal_code != null ? data[i].internal_code : "-") + "</td>";
        content += "<td>" + data[i].description + "</td>";
        content += "<td>" + data[i].brand + "</td>";
        content += "<td>" + (data[i].category != null ? data[i].category : "-") + "</td>";
        content += "<td>" + (data[i].subcategory != null ? data[i].subcategory : "-") + "</td>";
        if (data[i].color != null) {
            content += "<td>";
            content += "<div class='d-flex flex-row justify-content-between align-items-center' >";
            content += data[i].color;
            content += "<span class='dot' style='background-color: " + data[i].rgb_hex_code + "'></span>";
            content += "</div>";
            content += "</td>";
        } else {
            content += "<td>-</td>";
        }
        content += "<td>" + (data[i].capacity != null ? data[i].capacity : "-") + "</td>";
        content += "<td>" + (data[i].stock) + "</td>";
        content += "<td>";
        content += "<div class='d-flex flex-row justify-content-center'>";
        content += `<button type='button' style='background-color: #6c7ae0; border: none' onclick = 'OpenModal(` + data[i].product_id + `, "` + data[i].description + `", ` + data[i].stock + `, ` + data[i].minimum_stock + `);' class='btn btn-primary' data-bs-toggle='modal' data-bs-target='#exampleModal'>Seleccionar</button>`;
        content += `<button type="button" onclick="setDeleteID(` + data[i].product_id + `)" class="btn ms-3" style="background-color: crimson; color: white" data-bs-toggle="modal" data-bs-target="#modalConfirmation"><i class="bi bi-trash3"></i></button>`;
        content += "</div>";
        content += "</td>";
        content += "</tr>";
    }
    $("#contentTable").html(content);
}

function setDeleteID(id) {
    $("#id_product_delete").val(id);
    console.log($("#id_product_delete").val())
}

function increaseValue(id) {
    let value = parseInt(document.getElementById(id).value, 10);
    value = isNaN(value) ? 0 : value;
    value++;
    document.getElementById(id).value = value;
}

function decreaseValue(id) {
    let value = parseInt(document.getElementById(id).value, 10);
    value = isNaN(value) ? 0 : value;
    if (id === 'minimum_stock') {
        value < 2 ? value = 2 : '';
    } else {
        value < 1 ? value = 1 : '';
    }
    value--;
    document.getElementById(id).value = value;
}
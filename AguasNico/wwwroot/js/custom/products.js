$('#txtSearch').keypress(function (e) {
    if (e.which == 13) {
        $(this).blur();
        $('#btnSearch').click();
    }
});

$("#btnSearch").on("click", function () {
    let name = $("#txtSearch").val();
    $.get("../../Products/FilterProductsByName/?name=" + name, function (data) {
        createTable(data);
    })
});

$("#btnFilter").on("click", function () {
    let id_brand = $('#id_brand').val();
    let id_category = $('#id_category').val();
    let id_subcategory = $('#id_subcategory').val();
    let id_color = $('#id_color').val();
    let id_capacity = $('#id_capacity').val();

    $.get("../../Products/FilterProducts/?id_brand=" + id_brand + "&id_category=" + id_category + "&id_subcategory=" + id_subcategory +
        "&id_color=" + id_color + "&id_capacity=" + id_capacity,
        function (data) {
            createTable(data);
        });
});

function createTable(data) {
    let content = "";
    for (let i = 0; i < data.length; i++) {
        content += "<tr class='clickable-row text-center' data-href='/Products/Edit/" + data[i].product_id + "'>";
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
        content += "</tr>";
    }
    $("#contentTable").html(content);
    $(".clickable-row").click(function () {
        window.location = $(this).data("href");
    });
}
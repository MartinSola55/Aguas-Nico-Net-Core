$(document).ready(function () {
    $("#searchInput").on("input", function () {
        let searchText = $(this).val().toLowerCase();
        $(".timeline > li").each(function () {
            let nameElement = $(this).find(".name-element");
            let addressElement = $(this).find(".address-element");

            if (nameElement.text().toLowerCase().includes(searchText) ||
                addressElement.text().toLowerCase().includes(searchText)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });

    $("#estadoSelect").on("change", function () {
        let searchText = $(this).val().toLowerCase();
        $(".timeline > li").each(function () {
            let nameAndStateElement = $(this).find(".name-element");

            if (nameAndStateElement.text().toLowerCase().includes(searchText)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });

    function applyFilters() {
        let productText = $("#productSelect").val().toLowerCase();
        let typeText = $("#typeSelect").val().toLowerCase();
        let paymentMethod = $("#paymentMethodSelect").val().toLowerCase();

        $(".timeline > li").each(function () {
            let productElement = $(this).find(".product-element");
            let typeElement = $(this).find(".type-element");
            let paymentMethodElement = $(this).find(".paymentMethod-element");

            let productMatch = productText === "" || productElement.text().toLowerCase().includes(productText);
            let typeMatch = typeText === "" || typeElement.text().toLowerCase().includes(typeText);
            let paymentMethodMatch = paymentMethod === "" || typeElement.text().toLowerCase().includes(paymentMethod);

            if (productMatch && typeMatch) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    }

    function resetProductAndTypeSelect() {
        if ($("#productSelect").val() === "" && $("#typeSelect").val() === "" && $("#paymentMethodSelect").val() === "") {
            return; // No restablecer si los selectores de producto y tipo ya est�n vac�os
        }

        $("#productSelect, #typeSelect, #paymentMethodSelect").val(""); // Establece el valor de los selectores en blanco
        applyFilters(); // Aplica los filtros despu�s de restablecer los selectores
    }

    $("#estadoSelect, #searchInput").on("input", function () {
        resetProductAndTypeSelect(); // Restablece los selectores de producto y tipo
    });

    // Restablece los selectores de producto y tipo al cambiar estadoSelect
    $("#estadoSelect").on("change", function () {
        resetProductAndTypeSelect(); // Restablece los selectores de producto y tipo
    });

    $("#productSelect, #typeSelect, #paymentMethodSelect").on("change", applyFilters);

    applyFilters();
});
$(document).ready(function () {
    $("#searchInput").on("input", function () {
        resetProductAndTypeSelect();
        let searchText = $(this).val().toLowerCase();
        $(".col-timeline > .card").each(function () {
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
        resetProductAndTypeSelect();
        let searchText =  $(this).find("option:selected").val().toLowerCase();
        $(".col-timeline > .card").each(function () {
            let nameAndStateElement = $(this).find(".name-element");

            if (nameAndStateElement.text().toLowerCase().includes(searchText) || searchText === "") {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });

    // $("#paymentMethodSelect").on("change", function () {
    //     resetProductAndTypeSelect();
    //     let searchPaymentMethod = $(this).find("option:selected").val().toLowerCase();
    //     $(".col-timeline > .card").each(function () {
    //         let paymentMethodElement = $(this).find(".paymentMethod-element");
    //         let amountElement = $(this).find(".amount-value").text();
    //         let amountNumber = parseFloat(amountElement);
    //         if ((paymentMethodElement.text().toLowerCase().includes(searchPaymentMethod) || searchPaymentMethod === "") && amountNumber > 0) {
    //             $(this).show();
    //         } else {
    //             $(this).hide();
    //         }
    //         console.log(amountNumber);
    //     });
    // });

    $("#paymentStatusSelect").on("change", function () {
        resetProductAndTypeSelect();
        let searchPaymentStatus = $(this).val().toLowerCase();
    
        $(".col-timeline > .card").each(function () {
            let amountElement = $(this).find(".amount-value").text();
            let amountNumber = parseFloat(amountElement);
    
            if (searchPaymentStatus === "realizado" && amountNumber > 0) {
                $(this).show();
            } else if (searchPaymentStatus === "pendiente" && amountNumber === 0) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });

    $("#productSelect, #typeSelect").on("change", applyFilters);

    function applyFilters() {
        let productText = $("#productSelect").val().toLowerCase();
        let typeText = $("#typeSelect").val().toLowerCase();

        $(".col-timeline > .card").each(function () {
            let productElement = $(this).find(".product-element");
            let typeElement = $(this).find(".type-element");

            let productMatch = productText === "" || productElement.text().toLowerCase().includes(productText);
            let typeMatch = typeText === "" || typeElement.text().toLowerCase().includes(typeText);

            if (productMatch && typeMatch) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    }

    function resetProductAndTypeSelect() {
        if ($("#productSelect").val() === "" && $("#typeSelect").val() === "") {
            return; // No restablecer si los selectores de producto y tipo ya estén vacíos
        }

        $("#productSelect, #typeSelect").val(""); // Establece el valor de los selectores en blanco
        applyFilters(); // Aplica los filtros después de restablecer los selectores
    }
});
function print() {
    const mode = 'iframe';
    const close = mode == "popup";
    const options = {
        mode: mode,
        popClose: close,
    };
    $("div.printableArea").printArea(options);
}

function changeDay(control) {
    const value = control.value;
    if (value) {
        // hide divs not belongint to the selected day
        $(".printableArea").hide();
        $(`#${value}_section`).show();
        $('#emptySheets').show();
    } else {
        $(".printableArea").show();
        $('#emptySheets').show();
    }
}
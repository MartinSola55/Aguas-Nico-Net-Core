function print() {
    const mode = 'iframe';
    const close = mode == "popup";
    const options = {
        mode: mode,
        popClose: close,
    };
    $("div.printableArea").printArea(options);
}
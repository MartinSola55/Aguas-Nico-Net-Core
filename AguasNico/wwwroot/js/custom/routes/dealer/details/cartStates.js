function cancelState(cartID) {
    Swal.fire({
        title: "Esta acción no se puede revertir",
        text: '¿Seguro deseas restablecer el estado de la bajada?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Confirmar',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-danger waves-effect waves-light px-3 py-2',
            cancelButton: 'btn btn-default waves-effect waves-light px-3 py-2'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            let form = $("#form-resetState");
            form.find("input[name='cartID']").val(cartID);
            $.ajax({
                url: $(form).attr('action'), // Utiliza la ruta del formulario
                method: $(form).attr('method'), // Utiliza el método del formulario
                data: $(form).serialize(), // Utiliza los datos del formulario
                success: function (response) {
                    Swal.fire({
                        icon: 'success',
                        title: response.message,
                        confirmButtonColor: '#1e88e5',
                        allowOutsideClick: false,
                    }).then((result) => {
                        if (result.isConfirmed) {
                            location.reload();
                        }
                    });
                },
                error: function (errorThrown) {
                    Swal.fire({
                        icon: 'error',
                        title: errorThrown.responseJSON.message,
                        confirmButtonColor: '#1e88e5',
                    });
                }
            });
        }
    })
}

function setState(cartID, state, text) {
    if (text == "de vacaciones") text = "estaba de vacaciones";
    Swal.fire({
        title: `¿Está seguro que el cliente ${text}?`,
        icon: 'question',
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: 'Confirmar',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-success waves-effect waves-light px-3 py-2',
            cancelButton: 'btn btn-default waves-effect waves-light px-3 py-2'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            let form = $("#form-setState");
            form.find("input[name='cartID']").val(cartID);
            form.find("input[name='state']").val(state);
            
            $.ajax({
                url: $(form).attr('action'),
                method: $(form).attr('method'),
                data: $(form).serialize(),
                success: function (response) {
                    Swal.fire({
                        title: response.message,
                        icon: 'success',
                        showCancelButton: false,
                        confirmButtonColor: '#1e88e5',
                        confirmButtonText: 'OK',
                        allowOutsideClick: false,
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.reload();
                        }
                    })
                },
                error: function (errorThrown) {
                    SwalError(errorThrown.responseJSON.message);
                }
            });
        }
    })
}
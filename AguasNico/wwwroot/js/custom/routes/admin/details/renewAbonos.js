function renewByRoute() {
    Swal.fire({
        title: "Esta acción no se puede revertir",
        html: '¿Seguro deseas renovar <b>TODOS</b> los abonos? Esto sólo incluye los clientes de esta planilla.</br>Si un abono ya se renovó, no se volverá a renovar',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Confirmar',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-success waves-effect waves-light px-3 py-2',
            cancelButton: 'btn btn-default waves-effect waves-light px-3 py-2'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            let form = $("#form-renewByRoute");
            $.ajax({
                url: $(form).attr('action'),
                method: $(form).attr('method'),
                data: $(form).serialize(),
                success: function (response) {
                    Swal.fire({
                        icon: 'success',
                        title: response.message,
                        confirmButtonColor: '#1e88e5',
                        allowOutsideClick: true,
                    });
                },
                error: function (errorThrown) {
                    Swal.fire({
                        icon: 'error',
                        title: errorThrown.responseJSON.title,
                        html: errorThrown.responseJSON.message + "<br/>" + errorThrown.responseJSON.error,
                        confirmButtonColor: '#1e88e5',
                    });
                }
            });
        }
    })
}
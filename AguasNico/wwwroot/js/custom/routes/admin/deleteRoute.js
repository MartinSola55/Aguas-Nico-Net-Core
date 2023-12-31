function deleteRoute(id) {
    $("#form-deleteRoute input[name='id']").val(id);

    Swal.fire({
        title: "Esta acción no se puede revertir",
        html: '¿Seguro deseas eliminar esta planilla?<br/>Esto restablecerá el stock y el saldo de TODOS los clientes',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Eliminar',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-danger waves-effect waves-light px-3 py-2',
            cancelButton: 'btn btn-default waves-effect waves-light px-3 py-2'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            let form = $("#form-deleteRoute");
            $.ajax({
                url: $(form).attr('action'),
                method: $(form).attr('method'),
                data: $(form).serialize(),
                success: function (response) {
                    Swal.fire({
                        icon: 'success',
                        title: response.message,
                        confirmButtonColor: '#1e88e5',
                        allowOutsideClick: false,
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.href = window.origin + "/Routes/";
                        }
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
};
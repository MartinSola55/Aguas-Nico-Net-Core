$(document).ready(function () {
    $('#btnEditClient').click(function () {
        if ($('#divSaveClient').is(':visible')) {
            $('#divSaveClient').hide();
            $('#form-editClient input').prop('disabled', true);
            $('#form-editClient textarea').prop('disabled', true);
            $('#form-editClient select').prop('disabled', true);
        } else {
            $('#divSaveClient').show();
            $('#form-editClient input').prop('disabled', false);
            $('#form-editClient textarea').prop('disabled', false);
            $('#form-editClient select').prop('disabled', false);
        }
    });

    $('#btnDeleteClient').click(function () {
        Swal.fire({
            title: '¿Seguro deseas eliminar el cliente?',
            text: "Esta acción no se puede deshacer",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#1e88e5',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si, estoy seguro',
            cancelButtonText: 'Cancelar',
            allowOutsideClick: false,
        }).then((result) => {
            if (result.isConfirmed) {
                let form = $("#form-deleteClient");
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
                                window.location.href = "/Clients";
                            }
                        });
                    },
                    error: function (errorThrown) {
                        Swal.fire({
                            icon: 'error',
                            title: errorThrown.responseJSON.title,
                            text: errorThrown.responseJSON.message,
                            confirmButtonColor: '#1e88e5',
                        });
                    }
                });
            }
        });
    });

    $('#form-editClient').submit(function (e) {
        e.preventDefault();
        let form = $(this);
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                $('#divSaveClient').hide();
                $('#form-editClient input').prop('disabled', true);
                $('#form-editClient textarea').prop('disabled', true);
                $('#form-editClient select').prop('disabled', true);
                Swal.fire({
                    title: response.message,
                    icon: 'success',
                    showCancelButton: false,
                    confirmButtonColor: '#1e88e5',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                });
            },
            error: function (errorThrown) {
                Swal.fire({
                    icon: 'error',
                    title: errorThrown.responseJSON.title,
                    text: errorThrown.responseJSON.message,
                    confirmButtonColor: '#1e88e5',
                });
            }
        });
    });
});
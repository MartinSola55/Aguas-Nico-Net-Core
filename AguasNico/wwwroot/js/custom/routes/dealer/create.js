$(document).ready(function () {
    $('.clickable').on("click", function () {
        Swal.fire({
            title: '¿Seguro deseas comenzar el reparto?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Comenzar',
            buttonsStyling: false,
            customClass: {
                confirmButton: 'btn btn-success waves-effect waves-light px-3 py-2',
                cancelButton: 'btn btn-default waves-effect waves-light px-3 py-2'
            }
        }).then((result) => {
            if (result.isConfirmed) {
                $("#routeID").val($(this).data('id'));
                let form = $("#form-create");
                $.ajax({
                    url: $(form).attr("action"),
                    method: $(form).attr("method"),
                    data: $(form).serialize(),
                    success: function (response) {
                        window.location.href = "/Routes/Details/" + response.id;
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
    });
});
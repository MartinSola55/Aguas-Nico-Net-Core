$(document).ready(function () {
    $('#btnEditAbonos').click(function () {
        if ($('#divSaveAbonos').is(':visible')) {
            $('#divSaveAbonos').hide();
            $('#TableAbonos input[type="checkbox"]').prop('disabled', true);
        } else {
            $('#divSaveAbonos').show();
            $('#TableAbonos input[type="checkbox"]').prop('disabled', false);
        }
    });

    $('#btnSaveAbonos').click(function () {
        let abonos = [];
        let rows = $('#TableAbonos tbody tr');
        for (let i = 0; i < rows.length; i++) {
            let row = rows[i];
            if (!row.children[2].children[0].checked) {
                continue;
            }
            abonos.push({
                AbonoID: parseInt(row.dataset.id),
                ClientID: parseInt($('#Client_ID').val())
            });

        }
        let abonosData = {
            abonos
        };

        let form = $("#form-updateAbonos");
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize() + "&" + $.param(abonosData),
            success: function (response) {
                $('#divSaveAbonos').hide();
                $('#TableAbonos input[type="checkbox"]').prop('disabled', true);
                Swal.fire({
                    title: response.message,
                    icon: 'success',
                    showCancelButton: false,
                    confirmButtonColor: '#1e88e5',
                    confirmButtonText: 'OK',
                    allowOutsideClick: true,
                });
            },
            error: function (errorThrown) {
                Swal.fire({
                    icon: 'error',
                    title: errorThrown.responseJSON.title,
                    html: errorThrown.responseJSON.message + "<br/>" + errorThrown.responseJSON.error != null ? errorThrown.responseJSON.error : "",
                    confirmButtonColor: '#1e88e5',
                });
            }
        });
    });
});
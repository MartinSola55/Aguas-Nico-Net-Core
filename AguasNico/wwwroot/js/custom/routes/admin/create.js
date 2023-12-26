function openModal(dealerID, dealerName) {
    $("#form-create input[name='Route.UserID']").val(dealerID);
    $(".modal-title").html("Crear reparto para " + dealerName);
    $("#form-create select").val("");
};

$(document).ready(function () {
    $("#btnCreateRoute").click(function () {
        let form = document.getElementById('form-create');
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                $("#btnCloseModal").click();
                Swal.fire({
                    icon: 'success',
                    title: response.message,
                    confirmButtonColor: '#1e88e5',
                    allowOutsideClick: false,
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = window.location.origin + "/Routes";
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
    });
});
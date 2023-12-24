function Response(type, title, message) {
    Swal.fire({
        icon: type,
        title: title,
        text: message,
        confirmButtonColor: '#1e88e5',
    });
}

<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
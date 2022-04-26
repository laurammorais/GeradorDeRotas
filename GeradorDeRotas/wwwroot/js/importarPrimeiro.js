Swal.fire({
    icon: 'error',
    title: 'Oops...',
    text: 'É necessário importar um arquivo primeiro!',
    timer: 2000,
    showConfirmButton: false,
}).then(() => location.href = "https://localhost:44312/Excel/Upload")

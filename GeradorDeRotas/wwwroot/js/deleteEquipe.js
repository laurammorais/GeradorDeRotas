function confirmDelete(id) {
    Swal.fire({
        title: 'Você tem certeza?',
        text: "Você não poderá reverter esta ação!",
        icon: 'warning',
        showCancelButton: false,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Confirmar'
    }).then((result) => {
        if (result.isConfirmed) {
            const url = "/Equipes/Delete";
            const data = { id: id };

            $.ajax({
                url: url,
                type: "DELETE",
                dataType: "json",
                data: data
            });

            Swal.fire({
                title: 'Removido!',
                text: 'A Equipe foi deletada.',
                icon: 'success',
                timer: 2000,
                showConfirmButton: false,
            }
            ).then(() => location.href = location.href)
        }
    })
}
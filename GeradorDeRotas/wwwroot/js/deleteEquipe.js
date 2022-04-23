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

			Swal.fire(
				'Removido!',
				'A Equipe foi deletada.',
				'success'
			).then(() => location.href = location.href)
		}
	})
}
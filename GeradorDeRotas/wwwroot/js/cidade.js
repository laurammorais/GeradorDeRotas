$(document).ready(function () {
    $("#Servico").change(function () {
        var value = $("#Servico option:selected").val();
        if (value !== "" || value !== undefined) {
            ListarCidades(value)
        }
    });
})

function ListarCidades(value) {
    var url = "/Equipes/ListarCidades";
    var data = { servico: value };

    $("#Cidade").empty();

    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        data: data
    }).done(function (data) {
        if (data.cidades.length > 0) {
            var dadosCidades = data.cidades;

            $("#Cidade").append('<option value="">Selecione...</option>');

            $.each(dadosCidades, function (indice, item) {
                var opt = "";

                opt = '<option value="' + item + '">' + item + '</option>';

                $('#Cidade').append(opt);
            });
        }
    })
}
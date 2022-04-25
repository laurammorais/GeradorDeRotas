$(document).ready(function () {
    $("#Cidade").change(function () {
        var value = $("#Cidade option:selected").val();
        if (value !== "" || value !== undefined) {
            ListarServicos(value)
        }
    });
})

function ListarServicos(value) {
    var url = "/Select/FiltrarServicosPorCidade";
    var data = { cidade: value };
    console.log("listar Servicos")

    $("#Servico").empty();

    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        data: data
    }).done(function (data) {
        if (data.servicos.length > 0) {
            var dadosServicos = data.servicos;

            $("#Servico").append('<option value="">Selecione...</option>');

            $.each(dadosServicos, function (indice, item) {
                var opt = "";

                opt = '<option value="' + item + '">' + item + '</option>';

                $('#Servico').append(opt);
            });
        }
    })
}
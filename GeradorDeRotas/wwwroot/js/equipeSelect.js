$(document).ready(function () {
    $("#Cidade").change(function () {
        var value = $("#Cidade option:selected").val();
        if (value == "" || value == undefined) {
            $("#multiSelectDropDown2").empty();
            $("#multiSelectDropDown2").trigger("chosen:updated");
            $("#multiSelectDropDown2").chosen();
        }
        else {
            ListarEquipes(value)
        }
    });
})



function ListarEquipes(value) {
    var url = "/Select/FiltrarEquipesPorCidade";
    var data = { cidade: value };

    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        data: data
    }).done(function (data) {
        if (data.equipes?.length > 0) {
            var dadosEquipes = data.equipes;
            $("#multiSelectDropDown2").empty();

            $.each(dadosEquipes, function (indice, item) {
                var opt = "";
                opt = '<option value="' + item.id + '">' + item.nomeEquipe + '</option>';
                $('#multiSelectDropDown2').append(opt);
            });
            $("#multiSelectDropDown2").trigger("chosen:updated");
            $("#multiSelectDropDown2").chosen();
        }
        else {
            $("#multiSelectDropDown2").empty();
            $("#multiSelectDropDown2").trigger("chosen:updated");
            $("#multiSelectDropDown2").chosen();
        }
    })
}
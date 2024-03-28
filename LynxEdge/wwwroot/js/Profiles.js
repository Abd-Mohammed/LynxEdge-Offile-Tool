$(document).ready(function () {

    $('#addVehicleRow').click(function () {
        var table = $('#vehiclesTable tbody');
        var newRow = table.find('tr:last').clone();
        newRow.find('input').val('');
        newRow.find('input').attr('required', 'required');
        table.append(newRow);
    });
});

$(document).ready(function () {
    $("#StartDepot").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/profiles/autocomplete",
                type: "GET",
                data: {
                    prefix: request.term
                },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 1,
        appendTo: "#ProfileModal",
        select: function (event, ui) {
            $("#StartDepot").val(ui.item.label); 
            return false;
        }
    });

    $("#EndDepot").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/profiles/autocomplete",
                type: "GET",
                data: {
                    prefix: request.term
                },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 1,
        appendTo: "#ProfileModal",
        select: function (event, ui) {
            $("#EndDepot").val(ui.item.label);
            return false;
        }
    });
});

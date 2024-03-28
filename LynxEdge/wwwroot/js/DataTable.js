$(function () {
    const table = $("#DataTable1").DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"]
    });

    const selectButton = $('#DownloadBtn');
    selectButton.prop('disabled', true);

    let selectedRow = null;

    table.on('click', 'tbody tr', function (e) {
        var rowData = table.row(this).data();
        var id = rowData[0];

        if (selectedRow !== null && $(this).hasClass('selected')) {
            table.rows('.selected').nodes().to$().removeClass('selected');
            selectedRow = null;
        } else {
            table.rows('.selected').nodes().to$().removeClass('selected');
            $(this).addClass('selected');
            selectedRow = +id;
        }

        selectButton.prop('disabled', selectedRow === null);
    });

    selectButton.on('click', function () {
        if (selectedRow !== null) {
            //$.ajax({
            //    type: 'GET',
            //    url: '/Profiles/DownloadCSV',
            //    data: { id: selectedRow },
            //    success: function (response) {
            //        console.log('Optimization request sent successfully');
            //    },
            //    error: function (error) {
            //        console.error('Error sending optimization request:', error);
            //    }
            //});
            var eleForm = $("<form method='get'></form>");

            eleForm.attr("action", "/Profiles/DownloadCsv");

            var input = $("<input>")
                .attr("type", "hidden")
                .attr("name", "id")
                .val(selectedRow);

            eleForm.append(input);
            $(document.body).append(eleForm);
            eleForm.submit();

            table.rows('.selected').nodes().to$().removeClass('selected');
            selectedRow = null;
            selectButton.prop('disabled', true);

        }
    });
});

$(document).ready(function () {
    $(document).on("click", ".btnExport", function () {
        var trainingId = $(this).data("id");
        var trainingName = $(this).data("name");

        $.ajax({
            type: "POST",
            url: "/Training/ExportSelected",
            data: { trainingId: trainingId, trainingName: trainingName },
            xhrFields: {
                responseType: 'blob' // Set the response type to blob
            },
            success: function (data, status, xhr) {
                // Create a Blob object from the response data
                var blob = new Blob([data], { type: xhr.getResponseHeader('content-type') });

                // Create a temporary URL for the Blob
                var url = window.URL.createObjectURL(blob);

                // Create a link element and simulate a click to trigger the download
                var a = document.createElement('a');
                a.href = url;
                a.download = trainingName + '.xlsx';
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);

                toastr.success("Selection details exported successfully");
            },
            failure: function () {
                toastr.error("Unable to make request");
            },
            error: function () {
                toastr.error("Export failed!");
            }
        });
    });
});
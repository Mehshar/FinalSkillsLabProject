$(document).ready(function () {
    $(document).on("click", ".btnExport", function () {
        var trainingId = $(this).data("id");
        var trainingName = $(this).data("name");

        $.ajax({
            type: "POST",
            url: "/Training/ExportSelected",
            data: { trainingId: trainingId, trainingName: trainingName },
            dataType: "json",
            success: function (response) {
                if (response.result) {
                    toastr.success("Selection details exported successfully");
                }

                else {
                    toastr.error("Export failed!");
                }
            },
            failure: function () {
                toastr.error("Unable to make request");
            },
            error: function () {
                toastr.error("Something went wrong");
            }
        });
    });
});
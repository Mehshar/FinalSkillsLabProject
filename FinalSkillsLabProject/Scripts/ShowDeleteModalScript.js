$(document).ready(function () {
    $(document).on("click", ".btnDeleteTraining", function () {
        var trainingId = $(this).data("id");
        var trainingName = $(this).data("name");

        $("#trainingName").text(trainingName);
        $("#deleteTrainingModal").modal("show");

        $("#modalDeleteBtn").click(function () {
            $.ajax({
                type: "POST",
                url: "/Training/Delete",
                data: { id: trainingId },
                dataType: "json",
                success: function (response) {
                    if (response.result) {
                        $("#deleteTrainingModal").modal("hide");
                        toastr.success("Training deleted successfully!");
                    }

                    else {
                        toastr.error("Failed to delete training");
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
});
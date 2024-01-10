$(document).ready(function () {
    $(document).on("click", ".btnDeleteTraining", function () {
        var trainingId = $(this).data("id");
        var trainingName = $(this).data("name");

        $.ajax({
            type: "POST",
            url: "/Training/ValidateDelete",
            data: { trainingId: trainingId },
            dataType: "json",
            success: function (response) {
                if (response.isEnrollment) {
                    toastr.error("Delete failed! Enrollments already exist.");
                    return false;
                }

                else {
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
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 2000);
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
                }
            },
            failure: function (response) {
                toastr.error("Unable to make request!");
            },
            error: function (response) {
                toastr.error("Something went wrong!");
            }
        });
    });
});
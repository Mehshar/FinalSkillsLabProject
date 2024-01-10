$(document).ready(function () {
    $(document).on("click", ".btnEdit", function () {
        var trainingId = $(this).data("id");

        $.ajax({
            type: "POST",
            url: "/Training/ValidateEdit",
            data: { id: trainingId },
            dataType: "json",
            success: function (response) {
                if (response.result) {
                    window.location = response.url;
                }

                else {
                    toastr.error(response.message);
                    return false;
                }
            }
        });
    });
});
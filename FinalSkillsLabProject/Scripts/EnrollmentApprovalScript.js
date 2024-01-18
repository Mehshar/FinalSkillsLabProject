$(function () {
    $('.btnApprove').click(function () {
        var enrollmentId = $('[data-enrollmentid]').data('enrollmentid');
        console.log(`Approved: ${enrollmentId}`);
        var isApproved = true;
        var declineReason = null;

        $.ajax({
            type: "POST",
            url: "/Enrollment/ManageEnrollment",
            data: { enrollmentId: enrollmentId, isApproved: isApproved, declineReason: declineReason },
            dataType: "json",

            success: function (response) {
                if (response.result) {
                    toastr.success("Enrollment Approved Successfully");
                    setTimeout(function () {
                        window.location.reload();
                    }, 2000);
                }

                else {
                    toastr.error("Approval Failed!");
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
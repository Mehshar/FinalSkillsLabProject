$(function () {
    $('.btnReject').click(function () {
        if ($('.declineReason').hasClass('d-none')) {
            $('.declineReason').removeClass('d-none');
            return;
        }

        else {
            var declineReasontextarea = $('.declineReasontextarea').val();
            if (declineReasontextarea == "" || declineReasontextarea.trim() == "") {
                toastr.error("Reason for Decline cannot be empty");
                return;
            }

            else {
                var enrollmentId = $('[data-enrollmentid]').data('enrollmentid');
                var isApproved = false;

                $.ajax({
                    type: "POST",
                    url: "/Enrollment/ManageEnrollment",
                    data: { enrollmentId: enrollmentId, isApproved: isApproved, declineReason: declineReasontextarea },
                    dataType: "json",

                    success: function (response) {
                        if (response.result) {
                            toastr.success("Enrollment Rejected Successfully");
                            setTimeout(function () {
                                window.location.reload();
                            }, 2000);
                        }

                        else {
                            toastr.error("Rejection Failed!");
                        }
                    },
                    failure: function (response) {
                        toastr.error("Unable to make request!");
                    },
                    error: function (response) {
                        toastr.error("Something went wrong!");
                    }
                });
            }
        }
    });
});
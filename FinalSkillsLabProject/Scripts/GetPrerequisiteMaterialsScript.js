$(document).ready(function () {
    //$('.accordion-button').on('click', function () {
    // Use event delegation for dynamically added elements
    $('#enrollmentAccordion').on('click', '.accordion-button', function () {
        if ($(this).attr('aria-expanded')) {
            var enrollmentId = $(this).data('enrollment-id');
            var url = '/Enrollment/TrainingEnrollmentsMaterials/' + enrollmentId;
            var managerDetails = $('#manager_' + enrollmentId).text();
            $('#body_' + enrollmentId).load(url);

            setTimeout(function () {
                $('.manager-details').text(managerDetails);
                var enrollmentStatus = $("#badge_" + enrollmentId).text();
                if (enrollmentStatus == "Approved") {
                    $('.btnApprove').prop('disabled', true).removeClass('btn-outline-success').addClass('btn-outline-secondary');
                }

                else if (enrollmentStatus == "Selected") {
                    $('.responseBtnGroup').addClass('d-none');
                }

                else if (enrollmentStatus == "Declined") {
                    $('.btnReject').prop('disabled', true).removeClass('btn-outline-danger').addClass('btn-outline-secondary');
                    $('.declineReasontextarea').prop('readonly', true);
                    $('.declineReason').removeClass('d-none');

                    $.ajax({
                        type: "GET",
                        url: "/Enrollment/GetDeclineReasonByEnrollment",
                        data: { enrollmentId: enrollmentId },
                        dataType: "json",
                        success: function (response) {
                            $('.declineReasontextarea').val(response.result);
                        }
                    });
                }
            }, 100);
        }
    });
});
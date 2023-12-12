$(function () {
    function validateFiles() {
        var isValid = true;
        $('input[type="file"]').each(function () {
            if ($(this).get(0).files.length === 0) {
                toastr.error("Please select a file");
                isValid = false;
                return false;
            } else {
                var allowedTypes = ['application/pdf', 'image/jpeg', 'image/png'];
                if (!allowedTypes.includes($(this).get(0).files[0].type)) {
                    toastr.error("Please select a JPEG, PNG, or PDF file");
                    isValid = false;
                    return false;
                }
            }
        });
        return isValid;
    }

    $("#btnSubmit").click(function () {
        if (!validateFiles()) {
            return false;
        }
    });

    $('#enrollmentForm').submit(function (event) {
        event.preventDefault();

        validateFiles();
        if (!validateFiles()) {
            return false;
        }

        $('#loadingSpinner').removeClass('d-none');
        $('#loadingBackdrop').removeClass('d-none');

        var formData = new FormData($(this)[0]);

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: formData,
            async: true,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.result) {
                    toastr.success("Enrollment request submitted successfully");
                    setTimeout(function () {
                        window.location = response.url;
                    }, 2000);
                }

                else {
                    toastr.error("Enrollment failed!");
                    return false;
                }
            },
            complete: function () {
                $('#loadingSpinner').addClass('d-none');
                $('#loadingBackdrop').addClass('d-none');
            },
            failure: function (response) {
                toastr.error("Unable to make request!");
            },
            error: function (response) {
                toastr.error("Error submitting form!");
            }
        });

    });
});
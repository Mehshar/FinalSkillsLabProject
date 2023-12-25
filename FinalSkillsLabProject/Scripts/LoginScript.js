$(function () {
    let form = document.querySelector('form');

    form.addEventListener('submit', (e) => {
        e.preventDefault();
        return false;
    });

    function extractPlaceholder(fieldName) {
        var input = document.querySelector("#" + fieldName);
        return input.getAttribute("placeholder") || "";
    }

    function validateFieldValues() {
        var fields = ["username", "password"];
        var isValid = true;
        fields.forEach(function (field) {
            var value = $("#" + field).val();
            if (value = "" || value == null || value.trim() === "") {
                var placeholder = extractPlaceholder(field);
                toastr.error(placeholder + " cannot be empty");
                isValid = false;
            }
        });
        return isValid;
    }

    $("#btnLogin").click(function () {
        if (!validateFieldValues()) {
            return false;
        }

        var username = $("#username").val().trim();
        var password = $("#password").val().trim();

        // create object to map LoginModel
        var authObj = { Username: username, Password: password };

        $.ajax({
            type: "POST",
            url: "/Account/Authenticate",
            data: authObj,
            dataType: "json",

            success: function (response) {
                if (response.result) {
                    toastr.success("Authentication Successful!");
                    setTimeout(function () {
                        window.location = response.url;
                    }, 1000);
                }

                else {
                    toastr.error("Invalid username or password");
                    return false;
                }
            },
            failure: function (response) {
                toastr.error("Unable to make request!");
            },
            error: function (response) {
                toastr.error("Something went wrong");
            }
        });
    });
});
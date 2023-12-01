$(function () {
    let form = document.querySelector('form');

    form.addEventListener('submit', (e) => {
        e.preventDefault();
        return false;
    });

    // click event for login button
    $("#btnLogin").click(function () {
        // read username and password input
        var username = $("#username").val().trim();
        var password = $("#password").val().trim();

        if (username == '' || password == '') {
            toastr.error("Username and Password cannot be empty");
            return false;
        }

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
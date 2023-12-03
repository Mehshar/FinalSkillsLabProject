$(function () {
    let form = document.querySelector('form');

    form.addEventListener('submit', (e) => {
        e.preventDefault();
        return false;
    });

    // add click event for Signup button
    $("#btnSignup").click(function () {
        var nic = $("#nic").val().trim();
        var firstName = $("#firstName").val().trim();
        var lastName = $("#lastName").val().trim();
        var email = $("#email").val().trim();
        var mobileNum = $("#mobileNum").val().trim();
        var departmentId = parseInt($("#departmentDropdown").val().trim());
        var managerId = parseInt($("#managerDropdown").val().trim());
        var username = $("#username").val().trim();
        var password = $("#password").val().trim();

        var signupObj = { NIC: nic, FirstName: firstName, LastName: lastName, Email: email, MobileNum: mobileNum, DepartmentId: departmentId, ManagerId: managerId, Username: username, Password: password };

        $.ajax({
            type: "POST",
            url: "/Account/Signup",
            data: signupObj,
            dataType: "json",

            success: function (response) {
                if (response.result.toLowerCase().includes("success")) {
                    toastr.success(response.result);
                    //window.location = response.url;
                    // Set a timeout to redirect after 2 seconds
                    setTimeout(function () {
                        window.location = response.url;
                    }, 1000);
                }

                else {
                    toastr.error(response.result);
                    return false;
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
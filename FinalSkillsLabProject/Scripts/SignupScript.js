﻿$(function () {
    let form = document.querySelector('form');

    form.addEventListener('submit', (e) => {
        e.preventDefault();
        return false;
    });

    //function extractPlaceholder(fieldName) {
    //    var input = document.querySelector("#" + fieldName);
    //    return input.getAttribute("placeholder") || "";
    //}

    //function extractLabelValue(fieldName) {
    //    var label = $("Label[for='" + fieldName + "']");
    //    return label.length > 0 ? label.text().trim() : "";
    //}

    function validateFieldDropdownValues() {
        var dropdowns = ["departmentDropdown", "managerDropdown"];
        var inputFields = ["nic", "firstName", "lastName", "email", "mobileNum", "username", "password"];
        var isValid = true;

        inputFields.forEach(function (field) {
            var value = $("#" + field).val();
            if (value = "" || value == null || value.trim() === "") {
                var placeholder = extractPlaceholder(field);
                toastr.error(placeholder + " cannot be empty");
                isValid = false;
            }
        });

        dropdowns.forEach(function (field) {
            var value = $("#" + field).val();
            if (value == "" || value == null || value.trim() == "") {
                var label = extractLabelValue(field);
                toastr.error(label + " cannot be empty");
                isValid = false;
            }
        });
        return isValid;
    }

    function isValidEmail(email) {
        var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    $("#btnSignup").click(function () {
        if (!validateFieldDropdownValues()) {
            return false;
        }
        var email = $("#email").val().trim();

        if (!isValidEmail(email)) {
            toastr.error("Invalid email!");
            return false;
        }

        var nic = $("#nic").val().trim();
        var firstName = $("#firstName").val().trim();
        var lastName = $("#lastName").val().trim();
        var mobileNum = $("#mobileNum").val().trim();
        var departmentId = parseInt($("#departmentDropdown").val());
        var managerId = parseInt($("#managerDropdown").val());
        var username = $("#username").val().trim();
        var password = $("#password").val().trim();

        var signupObj = {
            NIC: nic,
            FirstName: firstName,
            LastName: lastName,
            Email: email,
            MobileNum: mobileNum,
            DepartmentId: departmentId,
            ManagerId: managerId,
            Username: username,
            Password: password
        };

        $.ajax({
            type: "POST",
            url: "/Account/Signup",
            data: signupObj,
            dataType: "json",

            success: function (response) {
                if (response.result.toLowerCase().includes("success")) {
                    toastr.success(response.result);
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
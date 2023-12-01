function togglePasswordVisibility() {
    var passwordInput = document.getElementById("password");
    var passwordCheckbox = document.getElementById("passwordCheckbox");

    if (passwordCheckbox.checked) {
        passwordInput.type = "text";
    }

    else {
        passwordInput.type = "password";
    }
}
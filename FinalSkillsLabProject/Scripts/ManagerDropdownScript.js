$(function () {
    $("#departmentDropdown").change(function () {
        var managerDropdown = $("#managerDropdown");
        managerDropdown.empty();
        managerDropdown.prop("disabled", false);

        var department = $("#departmentDropdown").val();

        $.ajax({
            type: "POST",
            url: "/Account/GetDepartmentManagers",
            data: { departmentId: department },  
            dataType: "json",

            success: function (response) {
                var placeholder = "<option disabled selected hidden>Choose your manager...</option>";
                managerDropdown.append(placeholder);
                $.each(response.managers, function (index, manager) {
                    var managerOptions = "<option value=" + manager.UserId + ">" + manager.FirstName + " " + manager.LastName + "</option>";
                    managerDropdown.append(managerOptions);
                });
            },

            error: function (xhr, status, error) {
                console.error("AJAX error:", status, error);
            }
        });
    });
});
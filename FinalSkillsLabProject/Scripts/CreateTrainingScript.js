$(function () {
    let form = document.querySelector('form');

    form.addEventListener('submit', (e) => {
        e.preventDefault();
        return false;
    });

    //function extractLabelValue(fieldName) {
    //    var label = $("Label[for='" + fieldName + "']");
    //    return label.length > 0 ? label.text().trim() : "";
    //}

    //function validateFieldValues() {
    //    var fields = ["trainingName", "description", "deadline", "capacity", "departmentDropdown"];
    //    var isValid = true;
    //    fields.forEach(function (field) {
    //        var value = $("#" + field).val();
    //        if (value == "" || value == null || value.trim() == "") {
    //            var label = extractLabelValue(field);
    //            toastr.error(label + " cannot be empty");
    //            isValid = false;
    //        }
    //    });
    //    return isValid;
    //}

    $("#btnSubmit").click(function () {
        var fields = ["trainingName", "description", "deadline", "capacity", "departmentDropdown"];
        if (!validateFieldValues(fields)) {
            return false;
        }

        var trainingName = $("#trainingName").val().trim();
        var description = $("#description").val().trim();
        var deadlineInput = $("#deadline").val();
        var capacity = parseInt($("#capacity").val().trim());
        var priorityDepartment = parseInt($("#departmentDropdown").val().trim());
        var selectedPrerequisites = []
        $(".prerequisite-checkbox:checked").each(function () {
            selectedPrerequisites.push($(this).val());
        });

        var parsedDeadline = new Date(deadlineInput);
        if (!isNaN(parsedDeadline.getTime())) {
            var formattedDeadline = parsedDeadline.toISOString();
        }

        var trainingObj = {
            TrainingName: trainingName,
            Description: description,
            Deadline: formattedDeadline,
            Capacity: capacity,
            PriorityDepartment: priorityDepartment
        };

        $.ajax({
            type: "POST",
            url: "/Training/Create",
            data: { training: trainingObj, prerequisitesList: selectedPrerequisites },
            dataType: "json",
            success: function (response) {
                if (response.result.toLowerCase().includes("success")) {
                    toastr.success(response.result);
                    setTimeout(function () {
                        window.location = response.url;
                    }, 2000);
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
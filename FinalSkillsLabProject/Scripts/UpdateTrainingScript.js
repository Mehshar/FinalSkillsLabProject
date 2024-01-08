$(function () {
    let form = document.querySelector('form');

    form.addEventListener('submit', (e) => {
        e.preventDefault();
        return false;
    });

    function extractLabelValues(fieldName) {
        var label = $("label[for='" + fieldName + "']");
        return label.length > 0 ? label.text().trim() : "";
    }

    function validateFieldValues() {
        var fields = ["trainingName", "description", "deadline", "capacity", "departmentDropdown"];
        var isValid = true;

        fields.forEach(function (field) {
            var value = $("#" + field).val();
            if (value == '' || value == null || value.trim() == '') {
                var label = extractLabelValues(field);
                toastr.error(label + " cannot be empty");
                isValid = false;
            }
        });
        return isValid;
    }

    function validateDeadline(parsedDeadline) {
        var currentDate = new Date();
        var isValid = true;

        if (isNaN(parsedDeadline.getTime())) {
            toastr.error("Invalid date format");
            isValid = false;
        }

        if (parsedDeadline < currentDate) {
            toastr.error("Deadline cannot be in the past");
            isValid = false;
        }
        return isValid;
    }

    function validateCapacity(capacity) {
        var isValid = true;
        if (capacity < 1) {
            toastr.error("Capacity must be greater than 1");
            isValid = false;
        }
        return isValid;
    }

    $("#btnSubmit").click(function () {
        if (!validateFieldValues()) {
            return false;
        }

        var trainingId = parseInt($("#trainingId").val());
        var trainingName = $("#trainingName").val().trim();
        var description = $("#description").val().trim();
        var deadlineInput = $("#deadline").val().trim();
        var capacity = parseInt($("#capacity").val().trim());
        var priorityDepartment = parseInt($("#departmentDropdown").val().trim());
        var selectedPrerequisites = [];

        $(".prerequisite-checkbox:checked").each(function () {
            selectedPrerequisites.push($(this).val());
        });

        if (!validateCapacity(capacity)) { return false; }

        var parsedDeadline = new Date(deadlineInput);
        if (!validateDeadline(parsedDeadline)) { return false; }
        var formattedDeadline = parsedDeadline.toISOString();

        var trainingObj = {
            TrainingId: trainingId,
            TrainingName: trainingName,
            Description: description,
            Deadline: formattedDeadline,
            Capacity: capacity,
            PriorityDepartment: priorityDepartment,
            PrerequisiteIds: selectedPrerequisites
        };

        $.ajax({
            type: "POST",
            url: "/Training/SaveEdit",
            data: trainingObj,
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
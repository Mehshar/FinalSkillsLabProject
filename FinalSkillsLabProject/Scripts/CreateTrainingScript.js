$(function () {
    let form = document.querySelector('form');

    form.addEventListener('submit', (e) => {
        e.preventDefault();
        return false;
    });

    //function getSelectedPrerequisites() {
    //    const reqCheckboxes = document.querySelectorAll('.prerequisite-checkbox');
    //    const selectedPrerequisites = [];

    //    reqCheckboxes.forEach(checkbox => {
    //        if (checkbox.checked) {
    //            selectedPrerequisites.push(checkbox.value);
    //        }
    //    });
    //    return selectedPrerequisites;
    //}

    $("#btnSubmit").click(function () {
        var trainingName = $("#trainingName").val().trim();
        var description = $("#description").val().trim();
        var deadlineInput = $("#deadline").val();
        var capacity = parseInt($("#capacity").val().trim());
        var priorityDepartment = parseInt($("#departmentDropdown").val().trim());
        //var selectedPrerequisites = getSelectedPrerequisites();
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
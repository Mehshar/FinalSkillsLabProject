$(function () {
    let form = document.querySelector('form');

    form.addEventListener('submit', (e) => {
        e.preventDefault();
        return false;
    });

    $("#btnSubmit").click(function () {
        var trainingId = parseInt($("#trainingId").val());
        var trainingName = $("#trainingName").val().trim();
        var description = $("#description").val().trim();
        var deadlineInput = $("#deadline").val().trim();
        var capacity = parseInt($("#capacity").val().trim());
        var priorityDepartment = parseInt($("#departmentDropdown").val().trim());

        var parsedDeadline = new Date(deadlineInput);
        if (!isNaN(parsedDeadline.getTime())) {
            // Convert the parsed date to a format suitable for sending to the server
            var formattedDeadline = parsedDeadline.toISOString();
        }

        var trainingObj = {
            TrainingId: trainingId,
            TrainingName: trainingName,
            Description: description,
            Deadline: formattedDeadline,
            Capacity: capacity,
            PriorityDepartment: priorityDepartment
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
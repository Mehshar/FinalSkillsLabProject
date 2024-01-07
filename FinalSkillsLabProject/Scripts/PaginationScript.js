$(document).ready(function () {
    $("#pageSizeForm").submit(function (e) {
        e.preventDefault();
        return false;
    });

    $("#pageNumForm").submit(function (e) {
        e.preventDefault();
        return false;
    });

    $("#pageSizeBtn").click(function () {
        var pageSize = $("#pageSizeInput").val();

        if (pageSize < 5 || pageSize > 20) {
            toastr.error("Page size must be between 5 and 20!");
            return false;
        }

        $.ajax({
            type: "POST",
            url: "/Training/Index",
            data: { pageSize: pageSize },
            dataType: "json",

            success: function (response) {
                toastr.success("success");
                updateTable(response.result);
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    });

    $("#pageNumBtn").click(function () {
        var pageNumber = $("#pageNumInput").val();
        var totalPages = $("#pageCountSpan").text();

        if (pageNumber < 1 || pageNumber > totalPages) {
            toastr.error(`Page number must between 1 and ${totalPages}!`);
            return false;
        }
    });

    function updateTable(trainings) {
        var tableBody = $("table tbody");
        tableBody.empty();

        $.each(trainings, function (index, training) {
            var row =
                $(`<tr class="clickable-row font-1" data-href="${detailsBaseUrl}${training.TrainingId}">
                    <td>${training.TrainingName}</td>
                    <td>${training.Description}</td>
                    <td>${new Date(parseInt(training.Deadline.match(/\d+/)[0])).toLocaleDateString()}</td>
                    <td>${training.Capacity}</td>
                    <td>Priority department name</td>
                    <td>
                        <a href="${editBaseUrl}${training.TrainingId}" class="btn btnCustomSquare d-none btnEdit" data-toggle="tooltip" title="Edit Training">
                            <i class="fa-solid fa-edit"></i>
                        </a>
                        ${enrollBaseUrl}${training.TrainingId}
                    </td>
                </tr>`);
            tableBody.append(row);
        });
    }
});
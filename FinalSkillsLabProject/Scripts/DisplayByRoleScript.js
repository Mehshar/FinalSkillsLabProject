$(document).ready(function () {
    var btnCreateTraining = $("#btnCreateTraining");
    var btnEdit = $(".btnEdit");
    var btnEnroll = $(".btnEnroll");
    var enrollmentsNavLink = $("#enrollmentsNavLink");
    var userTrainingsNavLink = $("#userTrainingsNavLink");
    var managerDetailsCard = $(".manager-details-card");
    var hangfireCard = $(".hangfireCard");
    var myenrollmentsBtn = $("#myenrollmentsBtn");
    var btnReject = $(".btnReject");
    var btnApprove = $(".btnApprove");
    var btnDeleteTraining = $(".btnDeleteTraining");

    $.ajax({
        type: "GET",
        url: "/Account/GetCurrentRole",
        dataType: "json",
        success: function (response) {
            var currentRole = response.currentRole;

            if ((btnCreateTraining.length || btnEdit.length || managerDetailsCard.length || hangfireCard.length) && currentRole === "Admin") {
                btnCreateTraining.removeClass("d-none");
                btnEdit.removeClass("d-none");
                managerDetailsCard.removeClass("d-none");
                hangfireCard.removeClass("d-none");
            }

            if (btnDeleteTraining.length && currentRole === "Admin") {
                btnDeleteTraining.removeClass("d-none");
            }

            if ((btnEnroll.length || userTrainingsNavLink.length || myenrollmentsBtn.length) && currentRole === "Employee") {
                btnEnroll.removeClass("d-none");
                userTrainingsNavLink.removeClass("d-none");
                myenrollmentsBtn.removeClass("d-none");
            }

            if (enrollmentsNavLink.length && (currentRole === "Manager" || currentRole === "Admin")) {
                enrollmentsNavLink.removeClass("d-none");
            }

            if ((btnReject.length || btnApprove.length) && currentRole === "Employee") {
                btnReject.addClass("d-none");
                btnApprove.addClass("d-none");
            }
        }
    });
});
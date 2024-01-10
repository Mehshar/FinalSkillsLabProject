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
    var btnExport = $(".btnExport");
    var btnExportDiv = $(".btnExportDiv");
    var btnEnrollDiv = $(".btnEnrollDiv");
    var tablePadding = $(".tablePadding");

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

            if ((btnDeleteTraining.length || btnExport.length || btnExportDiv.length) && currentRole === "Admin") {
                btnDeleteTraining.removeClass("d-none");
                btnExport.removeClass("d-none");
                btnExportDiv.removeClass("d-none");
            }

            if ((btnEnroll.length || userTrainingsNavLink.length || myenrollmentsBtn.length || btnEnrollDiv.length) && currentRole === "Employee") {
                btnEnroll.removeClass("d-none");
                userTrainingsNavLink.removeClass("d-none");
                myenrollmentsBtn.removeClass("d-none");
                btnEnrollDiv.removeClass("d-none");
            }

            if (enrollmentsNavLink.length && (currentRole === "Manager" || currentRole === "Admin")) {
                enrollmentsNavLink.removeClass("d-none");
            }

            if ((btnReject.length || btnApprove.length) && currentRole === "Employee") {
                btnReject.addClass("d-none");
                btnApprove.addClass("d-none");
            }

            if (tablePadding.length && (currentRole === "Admin" || currentRole === "Employee")) {
                tablePadding.addClass("pe-4");
            }
        }
    });
});
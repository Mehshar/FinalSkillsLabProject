﻿$(document).ready(function () {
    var btnCreateTraining = $("#btnCreateTraining");
    var btnEdit = $(".btnEdit");
    var btnEnroll = $(".btnEnroll");

    $.ajax({
        type: "GET",
        url: "/Account/GetCurrentRole",
        dataType: "json",
        success: function (response) {
            var currentRole = response.currentRole;

            if ((btnCreateTraining.length || btnEdit.length) && currentRole === "Admin") {
                btnCreateTraining.removeClass("d-none");
                btnEdit.removeClass("d-none");
            }

            if (btnEnroll.length && currentRole === "Employee") {
                btnEnroll.removeClass("d-none");
            }
        }
    });
});
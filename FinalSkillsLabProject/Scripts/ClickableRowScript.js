jQuery(document).ready(function ($) {
    $("body").on("click", ".clickable-row", function (e) {
        if ((!$(e.target).hasClass("btnDeleteTraining") && !$(e.target).parents(".btnDeleteTraining").length)
            && (!$(e.target).hasClass("btnEdit") && !$(e.target).parents(".btnEdit").length)) {
            window.location = $(this).data("href");
        }
    });
});
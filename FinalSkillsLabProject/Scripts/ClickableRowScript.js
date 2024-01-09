jQuery(document).ready(function ($) {
    $("body").on("click", ".clickable-row", function (e) {
        if (!$(e.target).hasClass("btnDeleteTraining") && !$(e.target).parents(".btnDeleteTraining").length) {
            window.location = $(this).data("href");
        }
    });
});
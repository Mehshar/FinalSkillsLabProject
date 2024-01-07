jQuery(document).ready(function ($) {
    //$(".clickable-row").click(function () {
    $("body").on("click", ".clickable-row", function () {
        window.location = $(this).data("href");
    });
});
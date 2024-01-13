$(document).ready(function ($) {
    $("body").on("click", ".clickable-row", function (e) {
        var excludeElements = [
            ".btnDeleteTraining",
            ".btnEdit",
            ".btnExport",
            ".btnEnroll"
        ];

        if (!excludeClickedElements(e, excludeElements)) {
            window.location = $(this).data("href");
        }
    });

    function excludeClickedElements(event, excludeSelectors) {
        var clickedElement = event.target;

        for (var i = 0; i < excludeSelectors.length; i++) {
            if (
                $(clickedElement).is(excludeSelectors[i]) ||
                $(clickedElement).parents(excludeSelectors[i]).length
            ) {
                return true; // Element is in the exclude list, do not redirect
            }
        }
        return false; // Element is not in the exclude list, proceed with the redirect
    }
});
function extractLabelValue(fieldName) {
    var label = $("label[for='" + fieldName + "']");
    return label.length > 0 ? label.text().trim() : "";
}

function extractPlaceholder(fieldName) {
    var input = document.querySelector("#" + fieldName);
    return input.getAttribute("placeholder") || "";
}

function validateFieldValues(fields) {
    var isValid = true;
    fields.forEach(function (field) {
        var value = $("#" + field).val();
        if (value == "" || value == null || value.trim() == "") {
            var label = extractLabelValue(field);
            toastr.error(label + " cannot be empty");
            isValid = false;
        }
    });
    return isValid;
}
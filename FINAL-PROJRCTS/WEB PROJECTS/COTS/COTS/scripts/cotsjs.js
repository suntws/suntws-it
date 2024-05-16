function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function splCharNotAllowed(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode == 34 || charCode == 38 || charCode == 42 || charCode == 44 || charCode == 46 || charCode == 47 || charCode == 58 || charCode == 59 ||
       charCode == 60 || charCode == 62 || charCode == 63 || charCode == 92 || charCode == 124)
        return false;

    return true;
}

function CheckMaxLength(textBox, maxLength) {
    if (textBox.value.length > maxLength) {
        alert("Max characters allowed are " + maxLength);
        textBox.value = textBox.value.substr(0, maxLength);
    }
}

function isNumberWithoutDecimal(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
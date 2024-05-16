function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function isNumberAndMinusKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31 && charCode != 45
            && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function isNumberWithoutDecimal(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function splCharNotAllowed(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode == 38 || charCode == 47 || charCode == 92)
        return false;

    return true;
}

function loadPopupBox(popupbox, textbox) {    // To Load the Popupbox
    $('#' + popupbox).css({ left: $('#' + textbox).offset().left - 15, top: $('#' + textbox).offset().top + 25, width: $('#' + textbox).width() + 26 })
    $('#' + popupbox).fadeIn("slow");
    $("#container").css({ // this is just for style
        "opacity": "0.3"
    });
}

function unloadPopupBox(popupbox) {    // TO Unload the Popupbox
    $('#' + popupbox).fadeOut("slow");
    $("#container").css({ // this is just for style       
        "opacity": "1"
    });
}

function upanddownKey(e, cont) {
    if (e.keyCode == 40 || e.which == 40) { // down arrow key code 
        if (liPossition < $("div[id*='" + cont + "'] ul li").length - 1) {
            liPossition++;
        }
        $("div[id*='" + cont + "'] ul li").removeClass('current');
        $("div[id*='" + cont + "'] ul li:eq(" + liPossition + ")").addClass('current');
    } else if (e.keyCode == 38 || e.which == 38) { // up arrow key code 
        if (liPossition == -1) {
            liPossition = $("div[id*='" + cont + "'] ul li").length - 1;
        } else {
            liPossition--;
        }
        $("div[id*='" + cont + "'] ul li").removeClass('current');
        $("div[id*='" + cont + "'] ul li:eq(" + liPossition + ")").addClass('current');
    }
}

function popupEnterKey(popupid, valCont) {
    if ($("div[id*='" + popupid + "'] ul li:eq(" + liPossition + ")").text() != "")
        $('#' + valCont).val($("div[id*='" + popupid + "'] ul li:eq(" + liPossition + ")").text());
    unloadPopupBox(popupid);
}

function popupHover(popupid, valCont) {
    $('ul.popupUL > li').mouseenter(function () {
        $(this).addClass('current').siblings().removeClass('current');
    });
    $('ul.popupUL li').click(function () {
        $('#' + valCont).val($(this).html());
        unloadPopupBox(popupid);
    });
}

function showProgress() {
    document.getElementById("progress").style.display = "block";
    document.getElementById("progress").setAttribute("style", "display:block");
}
function hideProgress() {
    document.getElementById("progress").style.display = "none";
    document.getElementById("progress").setAttribute("style", "display:none");
}

function nullValueDivShow(displayCss) {
    $('#divNullValuePopup').css({ "display": displayCss });
}

function NullValueCloseFunc() {
    $('#divNullValuePopup').hide();
}

function priceGvEditColumnHide() {
    $("#MainContent_gv_PriceGrid td:nth-child(7),th:nth-child(7)").hide();
    $("#MainContent_gv_PriceGrid td:nth-child(17),th:nth-child(17)").hide();
    $("#MainContent_gv_PriceGrid td:nth-child(18),th:nth-child(18)").hide();
    PriceGridHeadFixed();
}

function priceGvEditColumnUnitPrice() {
    $("#MainContent_gv_PriceGrid td:nth-child(7),th:nth-child(7)").hide();
    $("#MainContent_gv_PriceGrid td:nth-child(17),th:nth-child(17)").show();
    $("#MainContent_gv_PriceGrid td:nth-child(18),th:nth-child(18)").hide();
    PriceGridHeadFixed();
}

function priceGvEditColumnRmcb() {
    $("#MainContent_gv_PriceGrid td:nth-child(7),th:nth-child(7)").hide();
    $("#MainContent_gv_PriceGrid td:nth-child(17),th:nth-child(17)").hide();
    $("#MainContent_gv_PriceGrid td:nth-child(18),th:nth-child(18)").show();
    PriceGridHeadFixed();
}

function loadCustName(popupDivID) {
    $.ajax({ type: "POST", url: "" + $('#hdnVirtualStr').val() + "GetPopupRecords.aspx?type=getAllCustList", context: document.body, success: function (data) {
        if (data != '') {
            $('#' + popupDivID).html(data); $("div[id*='" + popupDivID + "'] ul li").first().addClass('current');
        }
        else {
            $('#' + popupDivID).html(''); $('#' + popupDivID).hide();
        }
    }
    });
}

function loadProspectCustName(popupDivID) {
    $.ajax({ type: "POST", url: "" + $('#hdnVirtualStr').val() + "GetPopupRecords.aspx?type=getAllProspectCust", context: document.body, success: function (data) {
        if (data != '') {
            $('#' + popupDivID).html(data); $("div[id*='" + popupDivID + "'] ul li").first().addClass('current');
        }
        else {
            $('#' + popupDivID).html(''); $('#' + popupDivID).hide();
        }
    }
    });
}

function loadUserWiseProspectCustName(popupDivID) {
    $.ajax({ type: "POST", url: "" + $('#hdnVirtualStr').val() + "GetPopupRecords.aspx?type=getUserWiseProspectCust", context: document.body, success: function (data) {
        if (data != '') {
            $('#' + popupDivID).html(data); $("div[id*='" + popupDivID + "'] ul li").first().addClass('current');
        }
        else {
            $('#' + popupDivID).html(''); $('#' + popupDivID).hide();
        }
    }
    });
}

function loadPriceSheetRefNo(popupDivID, custCode) {
    $.ajax({ type: "POST", url: "" + $('#hdnVirtualStr').val() + "GetPopupRecords.aspx?type=getAllPriceSheetCustWise&cCode=" + custCode + "", context: document.body, success: function (data) {
        if (data != '') {
            $('#' + popupDivID).html(data); $("div[id*='" + popupDivID + "'] ul li").first().addClass('current');
        }
        else {
            $('#' + popupDivID).html(''); $('#' + popupDivID).hide();
        }
    }
    });
}

function loadPriceSheetRefNo_AuthorizeOnly(popupDivID, custCode) {
    $.ajax({ type: "POST", url: "" + $('#hdnVirtualStr').val() + "BindRecords.aspx?type=getPriceRefAutorize&cCode=" + custCode + "&priceref=", context: document.body, success: function (data) {
        if (data != '') {
            $('#' + popupDivID).html(data); $("div[id*='" + popupDivID + "'] ul li").first().addClass('current');
        }
        else {
            $('#' + popupDivID).html(''); $('#' + popupDivID).hide();
        }
    }
    });
}

function loadRatesID(popupDivID) {
    $.ajax({ type: "POST", url: "" + $('#hdnVirtualStr').val() + "GetPopupRecords.aspx?type=getAllRatesID", context: document.body, success: function (data) {
        if (data != '') {
            $('#' + popupDivID).html(data); $("div[id*='" + popupDivID + "'] ul li").first().addClass('current');
        }
        else {
            $('#' + popupDivID).html(''); $('#' + popupDivID).hide();
        }
    }
    });
}

function LoadCompareCustList(popupDivID, strCustName, strCurrency) {
    $.ajax({ type: "POST", url: "" + $('#hdnVirtualStr').val() + "GetPopupRecords.aspx?type=getAllCompareCust&cname=" + strCustName + "&curr=" + strCurrency + "", context: document.body, success: function (data) {
        if (data != '') {
            $('#' + popupDivID).html(data); $("div[id*='" + popupDivID + "'] ul li").first().addClass('current');
        }
        else {
            $('#' + popupDivID).html(''); $('#' + popupDivID).hide();
        }
    }
    });
}

function loadPriceSheetNameWise(popupDivID, strCustName, strCategory) {
    $.ajax({ type: "POST", url: "" + $('#hdnVirtualStr').val() + "GetPopupRecords.aspx?type=getPriceSheetNameWise&cname=" + strCustName + "&category=" + strCategory + "", context: document.body, success: function (data) {
        if (data != '') {
            $('#' + popupDivID).html(data); $("div[id*='" + popupDivID + "'] ul li").first().addClass('current');
        }
        else {
            $('#' + popupDivID).html(''); $('#' + popupDivID).hide();
        }
    }
    });
}

function PriceGridHeadFixed() {
    if ($('#divPriceGrid').scrollTop() > 0) {
        $('#headVal').html($('#MainContent_gv_PriceGrid th').parent().html());
        $('#headVal th:nth-child(1)').width($('#MainContent_gv_PriceGrid th:nth-child(1)').width() + 2)
        $('#headVal th:nth-child(2)').width($('#MainContent_gv_PriceGrid th:nth-child(2)').width() + 2)
        $('#headVal th:nth-child(3)').width($('#MainContent_gv_PriceGrid th:nth-child(3)').width() + 2)
        $('#headVal th:nth-child(4)').width($('#MainContent_gv_PriceGrid th:nth-child(4)').width() + 2)
        $('#headVal th:nth-child(5)').width($('#MainContent_gv_PriceGrid th:nth-child(5)').width() + 2)
        $('#headVal th:nth-child(6)').width($('#MainContent_gv_PriceGrid th:nth-child(6)').width())
        $('#headVal th:nth-child(8)').width($('#MainContent_gv_PriceGrid th:nth-child(8)').width() + 2)
        $('#headVal th:nth-child(9)').width($('#MainContent_gv_PriceGrid th:nth-child(9)').width() + 2)
        $('#headVal th:nth-child(10)').width($('#MainContent_gv_PriceGrid th:nth-child(10)').width() + 2)
        $('#headVal th:nth-child(11)').width($('#MainContent_gv_PriceGrid th:nth-child(11)').width())
        $('#headVal th:nth-child(12)').width($('#MainContent_gv_PriceGrid th:nth-child(12)').width())
        $('#headVal th:nth-child(13)').width($('#MainContent_gv_PriceGrid th:nth-child(13)').width())
        $('#headVal th:nth-child(14)').width($('#MainContent_gv_PriceGrid th:nth-child(14)').width())
        $('#headVal th:nth-child(15)').width($('#MainContent_gv_PriceGrid th:nth-child(15)').width())
        $('#headVal th:nth-child(17)').width($('#MainContent_gv_PriceGrid th:nth-child(17)').width())
        $('#headVal th:nth-child(18)').width($('#MainContent_gv_PriceGrid th:nth-child(18)').width())
    }
}


function AddNewCountry() {
    $('#divNewCountry').css({ 'display': 'none' }); $('#divNewCity').css({ 'display': 'none' }); $('#divExistCity').css({ 'display': 'block' });
    $('#hdnCountryName').val($("#ddlCountry option:selected").text())
    if ($('#ddlCountry option:selected').text() == 'Add New Country') {
        $('#divNewCountry').css({ 'display': 'block' });
        $('#divNewCity').css({ 'display': 'block' });
        $('#hdnCityName').val('Add New City');
        $('#divExistCity').css({ 'display': 'none' });
    }
}

function AddNewCity() {
    $('#divNewCity').css({ 'display': 'none' });
    $('#hdnCityName').val($("#ddlCity option:selected").text())
    if ($('#ddlCity option:selected').text() == 'Add New City')
        $('#divNewCity').css({ 'display': 'block' });
}

function CheckMaxLength(textBox, maxLength) { if (textBox.value.length > maxLength) { alert("Max characters allowed are " + maxLength); textBox.value = textBox.value.substr(0, maxLength); } }

function displaycon(ctrl) { $('#' + ctrl).css({ 'display': 'none' }); }

function blinkStencil() { $('#divBlinkStencil').css({ 'background-color': '#AF9AFB', 'color': '#000' }); setTimeout("setblinkStencilNo()", 1000) }

function setblinkStencilNo() { $('#divBlinkStencil').css({ 'background-color': '#660053', 'color': '#fff' }); setTimeout("blinkStencil()", 1000) }

function gotoClaimDiv(ctrlID) { $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200); }
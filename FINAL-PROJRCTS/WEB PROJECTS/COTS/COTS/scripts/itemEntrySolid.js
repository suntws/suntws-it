function bind_AllSolidDesign() {
    $('#ContentPlaceHolder1_gvSolidCopy tr:eq(3)').find('td').css({ 'background-color': '#EFEFEF' });
    $('#ContentPlaceHolder1_gvSolidCopyFreeze tr:eq(3)').find('td').css({ 'height': '20px', 'background-color': '#EFEFEF' });
    $('#ContentPlaceHolder1_gvSolidCopy tr:eq(3)').find('td').find("input[id*='ContentPlaceHolder1_gvSolid_txt_']").attr('disabled', 'disabled').css({ 'width': '100px', 'font-weight': 'bold', 'border': '0px', 'height': '15px' });
    if ($('#ContentPlaceHolder1_gvSolid').width() > 1180) {
        $('#ContentPlaceHolder1_gvSolidFreeze tr').each(function () { $(this).find("td").css({ 'background-color': '#ccc' }); });
        $('#ContentPlaceHolder1_gvSolidPanelHeader').css({ 'width': '1165px' });
    }
    else {
        $('#ContentPlaceHolder1_gvSolid tr').each(function () { $(this).find("td") });
        $('#ContentPlaceHolder1_gvSolid tr').find('td:eq(0)').css('background-color', '#CCC');
        $('#ContentPlaceHolder1_gvSolid tr').find('td:eq(1)').css('background-color', '#CCC');
        $('#ContentPlaceHolder1_gvSolid tr').find('td:eq(2)').css('background-color', '#CCC');
        $('#ContentPlaceHolder1_gvSolid tr').find('td:eq(3)').css('background-color', '#CCC');
        $('#ContentPlaceHolder1_gvSolidPanelItem').css({ 'width': $('#ContentPlaceHolder1_gvSolid').width() + 15 });
        $('#ContentPlaceHolder1_gvSolidPanelHeader').css({ 'width': $('#ContentPlaceHolder1_gvSolid').width() });
    }

    if ($('#ContentPlaceHolder1_gvSolid tr').length > 0) { getSolidNullDataFromDB(); }
    if ($('#hdnQType').val() == 'old')
        if ($('#ContentPlaceHolder1_gvSolid tr').length > 0) { bind_InCompleteItems_Solid(); }
}

function getSolidNullDataFromDB() {
    if ($('#hdnCustCode').val().length > 0) { var strCategory = 'Solid'; $.ajax({ type: "POST", url: "" + $('#hdnVirtualStr').val() + "frmitementry.aspx/getNullDataFromDB_WebMethod", data: '{strCustCode:"' + $('#hdnCustCode').val() + '",strcategory:"' + strCategory + '",strStdCustCode:"' + $('#hdnStdCustCode').val() + '"}', contentType: "application/json; charset=utf-8", dataType: "json", success: OnSolidSuccessNullValues, failure: function (response) { alert(response.d); }, error: function (response) { alert(response.d); } }); }
}

function OnSolidSuccessNullValues(response) {
    var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var listSolidNullVals = xml.find("T1");
    $("span[id*='ContentPlaceHolder1_gvSolid_lbl_TyreSize_']").each(function (sizeRow) {
        var rowCount = sizeRow + 3; var gvsizeVal = $('#ContentPlaceHolder1_gvSolid_lbl_TyreSize_' + sizeRow).html() + "_" + $('#ContentPlaceHolder1_gvSolid_lbl_RimSize_' + sizeRow).html();
        $("#ContentPlaceHolder1_gvSolid tr:eq(" + rowCount + ")").find("input[id*='ContentPlaceHolder1_gvSolid_txt_']").each(function (txtRow) {
            var id1 = this.id; var splitID = id1.split('_'); var gvtypeExactID = splitID[3].toString() + "_" + splitID[4].toString() + "_" + splitID[5].toString() + "_" + splitID[6].toString()
            $(listSolidNullVals).each(function (e) {
                var nullValuetxtID = $(listSolidNullVals[e]).find("maingvid").text(); var nullSizeID = $(listSolidNullVals[e]).find("sizeid").text(); nullValuetxtID = nullValuetxtID.toString().replace(" ", "~"); if (nullValuetxtID.toString() == gvtypeExactID && nullSizeID == gvsizeVal) { $('#' + id1).val("NA").attr("disabled", "disabled").css({ 'font-size': '9px' }); return false; }
            });
        });
    });
}

function bind_InCompleteItems_Solid() {
    if ($('#hdnCustCode').val().length > 0) {
        var strCategory = 'Solid';
        $.ajax({ type: "POST", url: "" + $('#hdnVirtualStr').val() + "frmitementry.aspx/getIncompleteItems_WebMethod", data: '{strcsutcode:"' + $('#hdnCustCode').val() + '",strcategory:"' + strCategory + '",orderno:"' + $('#lblOrderRefNo').html() + '"}',
            contentType: "application/json; charset=utf-8", dataType: "json", success: OnSuccessSolidIncompleteItems,
            failure: function (response) { alert(response.d); },
            error: function (response) { alert(response.d); }
        });
    }
}
function OnSuccessSolidIncompleteItems(response) {
    var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var itemsSolidNullVals = xml.find("T1");
    $("span[id*='ContentPlaceHolder1_gvSolid_lbl_TyreSize_']").each(function (sizeRow) {
        var rowCount = sizeRow + 3;
        var gvsizeVal = $('#ContentPlaceHolder1_gvSolid_lbl_TyreSize_' + sizeRow).html() + "_" + $('#ContentPlaceHolder1_gvSolid_lbl_RimSize_' + sizeRow).html();
        $("#ContentPlaceHolder1_gvSolid tr:eq(" + rowCount + ")").find("input[id*='ContentPlaceHolder1_gvSolid_txt_']").each(function () {
            var id1 = this.id; var splitID = id1.split('_');
            var gvtypeExactID = splitID[3].toString() + "_" + splitID[4].toString() + "_" + splitID[5].toString() + "_" + splitID[6].toString()
            $(itemsSolidNullVals).each(function (e) {
                var nullValuetxtID = $(itemsSolidNullVals[e]).find("maingvid").text(); var nullSizeID = $(itemsSolidNullVals[e]).find("sizeid").text();
                if (nullValuetxtID.toString().replace(" ", "~") == gvtypeExactID && nullSizeID == gvsizeVal) { $('#' + id1).val($(itemsSolidNullVals[e]).find("itemqty").text()); return false; }
            });
        });
    });

    $("input[id*='ContentPlaceHolder1_gvSolid_txt_']").each(function () {
        var id1 = this.id; var splitID = id1.split('_');
        var gvMainCol = splitID[3].toString() + "_" + splitID[4].toString() + "_" + splitID[5].toString() + "_" + splitID[6].toString();
        var gvMainRow = splitID[7].toString();
        sumSolidValues(gvMainCol, gvMainRow);
    });
}

function solidEntry() {
    $("input[id*='ContentPlaceHolder1_gvSolid_txt_']").blur(function (e) {
        $(this).css('background-color', '#fff'); var id1 = this.id; var splitID = id1.split('_');
        var gvMainCol = splitID[3].toString() + "_" + splitID[4].toString() + "_" + splitID[5].toString() + "_" + splitID[6].toString();
        var gvMainRow = splitID[7].toString();
        sumSolidValues(gvMainCol, gvMainRow);
    }).focus(function () {
        $(this).css('background-color', '#4AECFA'); var j = 0;
        var id1 = this.id; var splitID = id1.split('_'); var gvMainRow = splitID[7].toString(); var gvFreezeRow = parseInt(gvMainRow) + 3;
        $("#ContentPlaceHolder1_gvSolidFreeze tr:eq(" + gvFreezeRow + ")").find('td').css('background-color', '#4AECFA');
        $("#ContentPlaceHolder1_gvSolid tr:eq(" + gvFreezeRow + ")").find('td').each(function () { if (j < 4) { $("#ContentPlaceHolder1_gvSolid tr:eq(" + gvFreezeRow + ")").find("td:eq(" + j + ")").css('background-color', '#4AECFA'); } j++; });
        $('#btnComplete').css({ "display": "none" }); $('#tempInsert').css({ "display": "block" });
    });
    $('#btnComplete').css({ "display": "none" }); $('#tempInsert').css({ "display": "block" });
}

function sumSolidValues(gvMainCol, gvMainRow) {
    var sumCol = 0; var sumRow = 0; var j = 0;
    $("#ContentPlaceHolder1_gvSolid_txt_" + gvMainCol + "_0_Copy").val('');
    $('.css' + gvMainCol).each(function () { if ($(this).val() != 'NA') sumCol += Number($(this).val()); });
    if (sumCol > 0) { $("#ContentPlaceHolder1_gvSolid_txt_" + gvMainCol + "_0_Copy").val(sumCol); }

    var gvRow = parseInt(gvMainRow); var gvFreezeRow = parseInt(gvMainRow) + 3;
    $("#ContentPlaceHolder1_gvSolid_lbl_TotPcs_" + gvRow).html(''); $("#ContentPlaceHolder1_gvSolidFreeze tr:eq(" + gvFreezeRow + ")").find("td:eq(1)").find('span').html('');
    $("#ContentPlaceHolder1_gvSolid tr:eq(" + gvFreezeRow + ")").find("input[type='text']").each(function () { if ($(this).val() != 'NA') sumRow += Number($(this).val()); });
    if (sumRow > 0) { $("#ContentPlaceHolder1_gvSolid_lbl_TotPcs_" + gvRow).html(sumRow); $("#ContentPlaceHolder1_gvSolidFreeze tr:eq(" + gvFreezeRow + ")").find("td:eq(1)").find('span').html(sumRow); }

    var gvRow = parseInt(gvMainRow); var gvFreezeRow = parseInt(gvMainRow) + 3; var totP = 0; var totFW = 0;
    $("#ContentPlaceHolder1_gvSolid tr:eq(" + gvFreezeRow + ")").find("input[type='text']").each(function () {
        if ($(this).val() > 0) {
            var txtQty = $(this).val(); var splitID = this.id.split('_'); var _Config = priceJsonSolid["" + splitID[3].toString() + ""];
            var _brand = _Config["" + splitID[4].toString() + ""]; var _sidewall = _brand["" + splitID[5].toString() + ""]; var _type = _sidewall["" + splitID[6].toString() + ""];
            $(_type).each(function (e) {
                if (_type[e].size == $('#ContentPlaceHolder1_gvSolid_lbl_TyreSize_' + gvRow).html() && _type[e].rim == $('#ContentPlaceHolder1_gvSolid_lbl_RimSize_' + gvRow).html()) {
                    var sumUnit = parseFloat(_type[e].price) * parseInt(txtQty); totP += Number(sumUnit);
                    var sumFW = parseFloat(_type[e].finishedwt) * parseInt(txtQty); totFW += Number(sumFW);
                }
            });
        }
    });
    $('#ContentPlaceHolder1_gvSolid_lbl_TotFinishedWT_' + gvRow).html(''); $("#ContentPlaceHolder1_gvSolidFreeze tr:eq(" + gvFreezeRow + ")").find("td:eq(0)").find('span').html('');
    if (totFW > 0) { $('#ContentPlaceHolder1_gvSolid_lbl_TotFinishedWT_' + gvRow).html(totFW.toFixed(2)); $("#ContentPlaceHolder1_gvSolidFreeze tr:eq(" + gvFreezeRow + ")").find("td:eq(0)").find('span').html(totFW.toFixed(2)); }

    $("#ContentPlaceHolder1_gvSolidFreeze tr:eq(" + gvFreezeRow + ")").find('td').css('background-color', '#CCC');
    $("#ContentPlaceHolder1_gvSolid tr:eq(" + gvFreezeRow + ")").find('td').each(function () { if (j < 4) { $("#ContentPlaceHolder1_gvSolid tr:eq(" + gvFreezeRow + ")").find("td:eq(" + j + ")").css('background-color', '#CCC'); } j++; });
}
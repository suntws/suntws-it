function bind_AllSolidDesign() {
    $('#MainContent_gvSolidCopy tr:eq(3)').find('td').css({ 'background-color': '#EFEFEF' });
    $('#MainContent_gvSolidCopyFreeze tr:eq(3)').find('td').css({ 'height': '20px', 'background-color': '#EFEFEF' });
    $('#MainContent_gvSolidCopy tr:eq(3)').find('td').find("input[id*='MainContent_gvSolid_txt_']").attr('disabled', 'disabled').css({ 'width': '100px', 'font-weight': 'bold', 'border': '0px', 'height': '15px' });
    if ($('#MainContent_gvSolid').width() > 1180) {
        $('#MainContent_gvSolidFreeze tr').each(function () { $(this).find("td").css({ 'background-color': '#ccc' }); });
        $('#MainContent_gvSolidPanelHeader').css({ 'width': '1165px' });
    }
    else {
        $('#MainContent_gvSolid tr').each(function () { $(this).find("td") });
        $('#MainContent_gvSolid tr').find('td:eq(0)').css('background-color', '#CCC');
        $('#MainContent_gvSolid tr').find('td:eq(1)').css('background-color', '#CCC');
        $('#MainContent_gvSolid tr').find('td:eq(2)').css('background-color', '#CCC');
        $('#MainContent_gvSolid tr').find('td:eq(3)').css('background-color', '#CCC');
        $('#MainContent_gvSolidPanelItem').css({ 'width': $('#MainContent_gvSolid').width() + 15 });
        $('#MainContent_gvSolidPanelHeader').css({ 'width': $('#MainContent_gvSolid').width() });
    }
    if ($('#hdnQType').val() == 'old')
        if ($('#MainContent_gvSolid tr').length > 0) { bind_InCompleteItems_Solid(); }
}

function bind_InCompleteItems_Solid() {
    if ($('#hdnCustCode').val().length > 0) {
        var strCategory = 'Solid';
        $.ajax({ type: "POST", url: "" + $('#hdnVirtualStr').val() + "exportmanual3.aspx/getIncompleteItems_WebMethod", data: '{strcsutcode:"' + $('#hdnCustCode').val() + '",strcategory:"' + strCategory + '",orderno:"' + $('#txtOrderRefNo').val() + '"}',
            contentType: "application/json; charset=utf-8", dataType: "json", success: OnSuccessSolidIncompleteItems,
            failure: function (response) { alert(response.d); },
            error: function (response) { alert(response.d); }
        });
    }
}
function OnSuccessSolidIncompleteItems(response) {
    var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var itemsSolidNullVals = xml.find("T1");
    $("span[id*='MainContent_gvSolid_lbl_TyreSize_']").each(function (sizeRow) {
        var rowCount = sizeRow + 3;
        var gvsizeVal = $('#MainContent_gvSolid_lbl_TyreSize_' + sizeRow).html() + "_" + $('#MainContent_gvSolid_lbl_RimSize_' + sizeRow).html();
        $("#MainContent_gvSolid tr:eq(" + rowCount + ")").find("input[id*='MainContent_gvSolid_txt_']").each(function () {
            var id1 = this.id; var splitID = id1.split('_');
            var gvtypeExactID = splitID[3].toString() + "_" + splitID[4].toString() + "_" + splitID[5].toString() + "_" + splitID[6].toString()
            $(itemsSolidNullVals).each(function (e) {
                var nullValuetxtID = $(itemsSolidNullVals[e]).find("maingvid").text(); var nullSizeID = $(itemsSolidNullVals[e]).find("sizeid").text();
                if (nullValuetxtID.toString().replace(" ", "~") == gvtypeExactID && nullSizeID == gvsizeVal) { $('#' + id1).val($(itemsSolidNullVals[e]).find("itemqty").text()); $('#' + id1).focus(); return false; }
            });
        });
    });
    $('#MainContent_gvSolid tr:eq(4)').find('td:eq(4)').find("input[id*='MainContent_gvSolid_txt_']").focus();
}

function solidEntry() {
    $("input[id*='MainContent_gvSolid_txt_']").blur(function (e) {
        $(this).css('background-color', '#fff'); var id1 = this.id; var splitID = id1.split('_');
        var gvMainCol = splitID[3].toString() + "_" + splitID[4].toString() + "_" + splitID[5].toString() + "_" + splitID[6].toString();
        var gvMainRow = splitID[7].toString(); var sumCol = 0; var sumRow = 0; var sumPerVol = 0;
        //Column calculation
        $("#MainContent_gvSolid_txt_" + gvMainCol + "_0_Copy").val('');
        $('.css' + gvMainCol).each(function () { if ($(this).val() != 'NA') sumCol += Number($(this).val()); });
        if (sumCol > 0) { $("#MainContent_gvSolid_txt_" + gvMainCol + "_0_Copy").val(sumCol); }

        //Row calculation
        var gvRow = parseInt(gvMainRow); var gvFreezeRow = parseInt(gvMainRow) + 3;
        $("#MainContent_gvSolid_lbl_TotPcs_" + gvRow).html(''); $("#MainContent_gvSolidFreeze tr:eq(" + gvFreezeRow + ")").find("td:eq(1)").find('span').html('');
        $("#MainContent_gvSolid tr:eq(" + gvFreezeRow + ")").find("input[type='text']").each(function () { if ($(this).val() != 'NA') sumRow += Number($(this).val()); });
        if (sumRow > 0) { $("#MainContent_gvSolid_lbl_TotPcs_" + gvRow).html(sumRow); $("#MainContent_gvSolidFreeze tr:eq(" + gvFreezeRow + ")").find("td:eq(1)").find('span').html(sumRow); }
        //_freezeitem
        sumAllTotQty();
        calcSolidPriceVol(gvMainRow); //calc each type,size unitprice
        sumAllTotFinishedWt();

        $("#MainContent_gvSolidFreeze tr:eq(" + gvFreezeRow + ")").find('td').css('background-color', '#CCC');
        var j = 0;
        $("#MainContent_gvSolid tr:eq(" + gvFreezeRow + ")").find('td').each(function () { if (j < 4) { $("#MainContent_gvSolid tr:eq(" + gvFreezeRow + ")").find("td:eq(" + j + ")").css('background-color', '#CCC'); } j++; });
    }).focus(function () {
        $(this).css('background-color', '#4AECFA'); var j = 0;
        var id1 = this.id; var splitID = id1.split('_'); var gvMainRow = splitID[7].toString(); var gvFreezeRow = parseInt(gvMainRow) + 3;
        $("#MainContent_gvSolidFreeze tr:eq(" + gvFreezeRow + ")").find('td').css('background-color', '#4AECFA');
        $("#MainContent_gvSolid tr:eq(" + gvFreezeRow + ")").find('td').each(function () { if (j < 4) { $("#MainContent_gvSolid tr:eq(" + gvFreezeRow + ")").find("td:eq(" + j + ")").css('background-color', '#4AECFA'); } j++; });
    });
    $('#moveproformadiv').css({ "display": "none" }); $('#tempInsert').css({ "display": "block" });
}

function calcSolidPriceVol(gvMainRow) {
    var gvRow = parseInt(gvMainRow); var gvFreezeRow = parseInt(gvMainRow) + 3; var totP = 0; var totFW = 0;
    $("#MainContent_gvSolid tr:eq(" + gvFreezeRow + ")").find("input[type='text']").each(function () {
        if ($(this).val() > 0) {
            var txtQty = $(this).val(); var splitID = this.id.split('_'); var _Config = priceJsonSolid["" + splitID[3].toString() + ""];
            var _brand = _Config["" + splitID[4].toString() + ""]; var _sidewall = _brand["" + splitID[5].toString() + ""]; var _type = _sidewall["" + splitID[6].toString() + ""];
            $(_type).each(function (e) {
                if (_type[e].size == $('#MainContent_gvSolid_lbl_TyreSize_' + gvRow).html() && _type[e].rim == $('#MainContent_gvSolid_lbl_RimSize_' + gvRow).html()) {
                    var sumUnit = parseFloat(_type[e].price) * parseInt(txtQty); totP += Number(sumUnit);
                    var sumFW = parseFloat(_type[e].finishedwt) * parseInt(txtQty); totFW += Number(sumFW);
                }
            });
        }
    });
    var gvRightRow = parseInt(gvRow) - 1;
    $('#MainContent_gvSolid_lbl_TotFinishedWT_' + gvRow).html(''); $("#MainContent_gvSolidFreeze tr:eq(" + gvFreezeRow + ")").find("td:eq(0)").find('span').html('');
    if (totFW > 0) { $('#MainContent_gvSolid_lbl_TotFinishedWT_' + gvRow).html(totFW.toFixed(2)); $("#MainContent_gvSolidFreeze tr:eq(" + gvFreezeRow + ")").find("td:eq(0)").find('span').html(totFW.toFixed(2)); }
}

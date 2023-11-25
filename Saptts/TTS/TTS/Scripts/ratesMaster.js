function getLipCost(ratesid, strCol) {
    $.ajax({ type: "POST", url: "RatesMaster.aspx/GetCost_WebMethod", data: '{ratesid:"' + ratesid + '",strCol:"' + strCol + '"}', contentType: "application/json; charset=utf-8",
        dataType: "json", success: OnSuccessLip, failure: function (response) { alert(response.d); },
        error: function (response) { alert(response.d); }
    });
}

function OnSuccessLip(response) {
    var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var lipVals = xml.find("T1"); var txtLipCost = $("input[id*='MainContent_gv_Lip_txtLipCost']");
    for (var k = 0; k < txtLipCost.length; k++) {
        $('#MainContent_gv_Lip_txtLipCost_' + k).val('0');
        for (var j = 0; j < lipVals.length; j++) {
            if ($(lipVals[j]).find("lipgum").text() == $('#MainContent_gv_Lip_lblLip_' + k).html()) {
                $('#MainContent_gv_Lip_lblLip_' + k).html($(lipVals[j]).find("lipgum").text());
                $('#MainContent_gv_Lip_txtLipCost_' + k).val($(lipVals[j]).find("IDValue").text());
                break;
            }
        }
    }
}

function getBaseCost(ratesid, strCol) {
    $.ajax({ type: "POST", url: "RatesMaster.aspx/GetCost_WebMethod", data: '{ratesid:"' + ratesid + '",strCol:"' + strCol + '"}', contentType: "application/json; charset=utf-8",
        dataType: "json", success: OnSuccessBase, failure: function (response) { alert(response.d); },
        error: function (response) { alert(response.d); }
    });
}

function OnSuccessBase(response) {
    var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var baseVals = xml.find("T1"); var txtBaseCost = $("input[id*='MainContent_gv_Base_txtBaseCost']");
    for (var k = 0; k < txtBaseCost.length; k++) {
        $('#MainContent_gv_Base_txtBaseCost_' + k).val('0');
        for (var j = 0; j < baseVals.length; j++) {
            if ($(baseVals[j]).find("base").text() == $('#MainContent_gv_Base_lblBase_' + k).html()) {
                $('#MainContent_gv_Base_lblBase_' + k).html($(baseVals[j]).find("base").text());
                $('#MainContent_gv_Base_txtBaseCost_' + k).val($(baseVals[j]).find("IDValue").text());
                break;
            }
        }
    }
}

function getCenterCost(ratesid, strCol) {
    $.ajax({ type: "POST", url: "RatesMaster.aspx/GetCost_WebMethod", data: '{ratesid:"' + ratesid + '",strCol:"' + strCol + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json", success: OnSuccessCenter, failure: function (response) { alert(response.d); },
        error: function (response) { alert(response.d); }
    });
}

function OnSuccessCenter(response) {
    var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var CenterVals = xml.find("T1"); var txtCenterCost = $("input[id*='MainContent_gv_Center_txtCenterCost']");
    for (var k = 0; k < txtCenterCost.length; k++) {
        $('#MainContent_gv_Center_txtCenterCost_' + k).val('0');
        for (var j = 0; j < CenterVals.length; j++) {
            if ($(CenterVals[j]).find("center").text() == $('#MainContent_gv_Center_lblCenter_' + k).html()) {
                $('#MainContent_gv_Center_lblCenter_' + k).html($(CenterVals[j]).find("center").text());
                $('#MainContent_gv_Center_txtCenterCost_' + k).val($(CenterVals[j]).find("IDValue").text());
                break;
            }
        }
    }
}

function getTreadCost(ratesid, strCol) {
    $.ajax({ type: "POST", url: "RatesMaster.aspx/GetCost_WebMethod", data: '{ratesid:"' + ratesid + '",strCol:"' + strCol + '"}', contentType: "application/json; charset=utf-8",
        dataType: "json", success: OnSuccessTread, failure: function (response) { alert(response.d); },
        error: function (response) { alert(response.d); }
    });
}

function OnSuccessTread(response) {
    var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var TreadVals = xml.find("T1"); var txtTreadCost = $("input[id*='MainContent_gv_Tread_txtTreadCost']");
    for (var k = 0; k < txtTreadCost.length; k++) {
        $('#MainContent_gv_Tread_txtTreadCost_' + k).val('0');
        for (var j = 0; j < TreadVals.length; j++) {
            if ($(TreadVals[j]).find("tread").text() == $('#MainContent_gv_Tread_lblTread_' + k).html()) {
                $('#MainContent_gv_Tread_lblTread_' + k).html($(TreadVals[j]).find("tread").text());
                $('#MainContent_gv_Tread_txtTreadCost_' + k).val($(TreadVals[j]).find("IDValue").text());
                break;
            }
        }
    }
}

function getSolidSizeList(ratesid, strCategory) {
    $.ajax({ type: "POST", url: "RatesMaster.aspx/GetSizeBeadBand_WebMethod", data: '{ratesid:"' + ratesid + '",sizeCategory:"' + strCategory + '"}', contentType: "application/json; charset=utf-8",
        dataType: "json", success: OnSuccessSolid, failure: function (response) { alert(response.d); },
        error: function (response) { alert(response.d); }
    });
}

function OnSuccessSolid(response) {
    var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var SolidVals = xml.find("T1"); var txtSolidSizeValue = $("input[id*='MainContent_gv_SolidSize_txtSolidSizeValue']");
    for (var k = 0; k < txtSolidSizeValue.length; k++) {
        $('#MainContent_gv_SolidSize_txtSolidSizeValue_' + k).val('0');
        for (var j = 0; j < SolidVals.length; j++) {
            if ($(SolidVals[j]).find("TyreSize").text() == $('#MainContent_gv_SolidSize_lblSolidSize_' + k).html()) {
                $('#MainContent_gv_SolidSize_lblSolidSize_' + k).html($(SolidVals[j]).find("TyreSize").text());
                $('#MainContent_gv_SolidSize_txtSolidSizeValue_' + k).val($(SolidVals[j]).find("SizeVal").text());
                break;
            }
        }
    }
}

function getPobSizeList(ratesid, strCategory) {
    $.ajax({ type: "POST", url: "RatesMaster.aspx/GetSizeBeadBand_WebMethod", data: '{ratesid:"' + ratesid + '",sizeCategory:"' + strCategory + '"}', contentType: "application/json; charset=utf-8",
        dataType: "json", success: OnSuccessPob, failure: function (response) { alert(response.d); },
        error: function (response) { alert(response.d); }
    });
}

function OnSuccessPob(response) {
    var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var PobVals = xml.find("T1"); var txtPobSizeValue = $("input[id*='MainContent_gv_PobSize_txtPobSizeValue']");
    for (var k = 0; k < txtPobSizeValue.length; k++) {
        $('#MainContent_gv_PobSize_txtPobSizeValue_' + k).val('0');
        for (var j = 0; j < PobVals.length; j++) {
            if ($(PobVals[j]).find("TyreSize").text() == $('#MainContent_gv_PobSize_lblPobSize_' + k).html()) {
                $('#MainContent_gv_PobSize_lblPobSize_' + k).html($(PobVals[j]).find("TyreSize").text());
                $('#MainContent_gv_PobSize_txtPobSizeValue_' + k).val($(PobVals[j]).find("SizeVal").text());
                break;
            }
        }
    }
}

function getPneumaticSizeList(ratesid, strCategory) {
    $.ajax({ type: "POST", url: "RatesMaster.aspx/GetSizeBeadBand_WebMethod", data: '{ratesid:"' + ratesid + '",sizeCategory:"' + strCategory + '"}', contentType: "application/json; charset=utf-8",
        dataType: "json", success: OnSuccessPneumatic, failure: function (response) { alert(response.d); },
        error: function (response) { alert(response.d); }
    });
}

function OnSuccessPneumatic(response) {
    var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var PneumaticVals = xml.find("T1"); var txtPneumaticSizeValue = $("input[id*='MainContent_gv_PneumaticSize_txtPneumaticSizeValue']");
    for (var k = 0; k < txtPneumaticSizeValue.length; k++) {
        $('#MainContent_gv_PneumaticSize_txtPneumaticSizeValue_' + k).val('0');
        for (var j = 0; j < PneumaticVals.length; j++) {
            if ($(PneumaticVals[j]).find("TyreSize").text() == $('#MainContent_gv_PneumaticSize_lblPneumaticSize_' + k).html()) {
                $('#MainContent_gv_PneumaticSize_lblPneumaticSize_' + k).html($(PneumaticVals[j]).find("TyreSize").text());
                $('#MainContent_gv_PneumaticSize_txtPneumaticSizeValue_' + k).val($(PneumaticVals[j]).find("SizeVal").text());
                break;
            }
        }
    }
}

function getTypeList(ratesid) {
    $.ajax({ type: "POST", url: "RatesMaster.aspx/GetType_List_WebMethod", data: '{ratesid:"' + ratesid + '"}', contentType: "application/json; charset=utf-8",
        dataType: "json", success: OnSuccessType, failure: function (response) { alert(response.d); },
        error: function (response) { alert(response.d); }
    });
}

function OnSuccessType(response) {
    var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var TypeVals = xml.find("T1"); var txtTypeValue = $("span[id*='MainContent_gv_TypeRates_lblType']");
    for (var k = 0; k < txtTypeValue.length; k++) {
        $('#MainContent_gv_TypeRates_lblTypeRatesVal_' + k).html('0');
        for (var j = 0; j < TypeVals.length; j++) {
            if ($(TypeVals[j]).find("TyreType").text() == $('#MainContent_gv_TypeRates_lblType_' + k).html()) {
                $('#MainContent_gv_TypeRates_lblType_' + k).html($(TypeVals[j]).find("TyreType").text());
                $('#MainContent_gv_TypeRates_lblTypeRatesVal_' + k).html(parseFloat($(TypeVals[j]).find("Typecost").text()).toFixed(2));
                break;
            }
        }
    }
}

function getCurCostList(ratesid) {
    $.ajax({ type: "POST", url: "RatesMaster.aspx/GetCur_Cost_WebMethod", data: '{ratesid:"' + ratesid + '"}', contentType: "application/json; charset=utf-8",
        dataType: "json", success: OnSuccessCurCost, failure: function (response) { alert(response.d); },
        error: function (response) { alert(response.d); }
    });
}

function OnSuccessCurCost(response) {
    var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var CurVals = xml.find("T1"); var ddlCurType = $('#ddlCurrency option');
    $('#txtLoatFactor').val($(CurVals).find("LoadFact").text()); $('#txtConvCost').val($(CurVals).find("ConCost").text());
    for (var j = 0; j < ddlCurType.length; j++) {
        var curtype = $(ddlCurType[j]).text();
        if (curtype.substr(0, 3) == $(CurVals).find("Cur").text()) {
            $('#ddlCurrency').val(curtype);
            $('#txtCurRate').val(curtype.substr(0, 3).toUpperCase());
            $('#txtIndRate').val($(CurVals).find("CurValue").text());
            break;
        }
    }
}

function getCurrency_ChangeValue(ratesid, curtype) {
    $.ajax({ type: "POST", url: "RatesMaster.aspx/GetCurrency_ChangeValue_WebMethod", data: '{ratesid:"' + ratesid + '",curtype:"' + curtype + '"}', contentType: "application/json; charset=utf-8",
        dataType: "json", success: OnSuccessCurrencyChange, failure: function (response) { alert(response.d); },
        error: function (response) { alert(response.d); }
    });
}

function OnSuccessCurrencyChange(response) {
    var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var CurVals = xml.find("T1");
    $('#txtIndRate').val($(CurVals).find("CurValue").text());
}

function ctrlRatesMaster() {
    showProgress(); $('#ErrMsg').html(''); $('#lblErrMsg').html('');
    var txtRatesID = $('#txtRatesID').val();
    var txtCurRate = $('#txtCurRate').val();
    var txtIndRate = $('#txtIndRate').val();
    var txtConvCost = $('#txtConvCost').val();
    var txtLoatFactor = $('#txtLoatFactor').val();
    var errMsg = '';

    if (txtRatesID.length == 0)
        errMsg += "Enter Rates-ID, ";
    if (txtCurRate.length == 0 || txtIndRate.length == 0)
        errMsg += "Enter Rate Value, ";
    if (txtConvCost.length == 0)
        errMsg += "Enter Conversion Cost/Kg, ";
    if (txtLoatFactor.length == 0)
        errMsg += "Enter Load Factor";

    var validDec = /^[-+]?(?:\d+\.?\d*|\.\d+)$/;
    var j;
    //check lipcost
    var liperrmsg = '';
    var txtLipCost = $("input[id*='MainContent_gv_Lip_txtLipCost']");
    for (j = 0; j < txtLipCost.length; j++) {
        var value = $('#MainContent_gv_Lip_txtLipCost_' + j).val();
        if (value.length > 0) {
            if (!validDec.test(value))
                liperrmsg += ", Enter proper values for Lip Cost row-" + parseInt(j + 1);
        }
        else
            liperrmsg += ", Enter decimal values for Lip Cost row-" + parseInt(j + 1);
    }

    //check base cost
    var baseerrmsg = '';
    var txtBaseCost = $("input[id*='MainContent_gv_Base_txtBaseCost']");
    for (j = 0; j < txtBaseCost.length; j++) {
        var value = $('#MainContent_gv_Base_txtBaseCost_' + j).val();
        if (value.length > 0) {
            if (!validDec.test(value))
                baseerrmsg += ", Enter proper values for Base Cost row-" + parseInt(j + 1);
        }
        else
            baseerrmsg += ", Enter decimal values for Base Cost row-" + parseInt(j + 1);
    }

    //check center cost
    var centererrmsg = '';
    var txtCenterCost = $("input[id*='MainContent_gv_Center_txtCenterCost']");
    for (j = 0; j < txtCenterCost.length; j++) {
        var value = $('#MainContent_gv_Center_txtCenterCost_' + j).val();
        if (value.length > 0) {
            if (!validDec.test(value))
                centererrmsg += ", Enter proper values for Center Cost row-" + parseInt(j + 1);
        }
        else
            centererrmsg += ", Enter decimal values for Center Cost row-" + parseInt(j + 1);
    }

    //check tread cost
    var treaderrmsg = '';
    var txtTreadCost = $("input[id*='MainContent_gv_Tread_txtTreadCost']");
    for (j = 0; j < txtTreadCost.length; j++) {
        var value = $('#MainContent_gv_Tread_txtTreadCost_' + j).val();
        if (value.length > 0) {
            if (!validDec.test(value))
                treaderrmsg += ", Enter proper values for Tread Cost row-" + parseInt(j + 1);
        }
        else
            treaderrmsg += ", Enter decimal values for Tread Cost row-" + parseInt(j + 1);
    }

    //check Solid size beadband
    var SolidMsg = '';
    var txtSolidSizeValue = $("input[id*='MainContent_gv_SolidSize_txtSolidSizeValue']");
    for (var k = 0; k < txtSolidSizeValue.length; k++) {
        var value = $('#MainContent_gv_SolidSize_txtSolidSizeValue_' + k).val();
        if (value.length > 0) {
            if (!validDec.test(value))
                SolidMsg += ", Enter proper values for Solid size beadband row-" + parseInt(k + 1);
        }
        else
            SolidMsg += ", Enter decimal values for Solid size beadband row-" + parseInt(k + 1);
    }

    //check Pob size beadband
    var pobMsg = '';
    var txtPobSizeValue = $("input[id*='MainContent_gv_PobSize_txtPobSizeValue']");
    for (var k = 0; k < txtPobSizeValue.length; k++) {
        var value = $('#MainContent_gv_PobSize_txtPobSizeValue_' + k).val();
        if (value.length > 0) {
            if (!validDec.test(value))
                pobMsg += ", Enter proper values for Pob size beadband row-" + parseInt(k + 1);
        }
        else
            pobMsg += ", Enter decimal values for Pob size beadband row-" + parseInt(k + 1);
    }

    //check Pneumatic size beadband
    var pneumaticMsg = '';
    var txtPneumaticSizeValue = $("input[id*='MainContent_gv_PneumaticSize_txtPneumaticSizeValue']");
    for (var k = 0; k < txtPneumaticSizeValue.length; k++) {
        var value = $('#MainContent_gv_PneumaticSize_txtPneumaticSizeValue_' + k).val();
        if (value.length > 0) {
            if (!validDec.test(value))
                pneumaticMsg += ", Enter proper values for Pneumatic size beadband row-" + parseInt(k + 1);
        }
        else
            pneumaticMsg += ", Enter decimal values for Pneumatic size beadband row-" + parseInt(k + 1);
    }

    var finalErr = '';
    if (errMsg.length > 0)
        finalErr += errMsg;
    if (liperrmsg.length > 0)
        finalErr += liperrmsg;
    if (baseerrmsg.length > 0)
        finalErr += baseerrmsg;
    if (centererrmsg.length > 0)
        finalErr += centererrmsg;
    if (treaderrmsg.length > 0)
        finalErr += treaderrmsg;
    if (SolidMsg.length > 0)
        finalErr += SolidMsg;
    if (pobMsg.length > 0)
        finalErr += pobMsg;
    if (pneumaticMsg.length > 0)
        finalErr += pneumaticMsg;

    if (finalErr.length > 0) {
        $('#ErrMsg').html(finalErr);
        $('#ErrMsg').css({ "color": "red", "font-weight": "bold" });
        hideProgress();
        return false;
    }
    else
        return true;
}
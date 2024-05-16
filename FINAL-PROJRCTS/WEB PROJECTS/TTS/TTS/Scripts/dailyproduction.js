function ddlConfig_Change() {
    $("#hdnConfig").val($("#ddlConfig option:selected").text());
    $("#lblProcessID").text("");
    $("#hdnProcessID").val("");
    $.ajax({
        url: "dailyproductionupload.aspx/GetBrand",
        data: "{config:\" " + $("#ddlConfig option:selected").text() + " \"}",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "JSON",
        success: function (result) {
            if (result.d != "") {
                bindSelect("#ddlBrand", result.d);
                ddlBrand_Change();
                if ($("#ddlBrand option:selected").text() == "CHOOSE")
                    $("#ddlBrand").focus();
            }
        },
        failure: function (result) {
            var str = result;
        }
    });
}

function ddlBrand_Change() {
    $("#lblProcessID").text("");
    $("#hdnProcessID").val("");
    $("#hdnBrand").val($("#ddlBrand option:selected").text());
    $.ajax({
        url: "dailyproductionupload.aspx/GetSidewall",
        data: "{config:\"" + $("#ddlConfig option:selected").text() + "\"," +
                                              "brand:\"" + $("#ddlBrand option:selected").text() + "\"" +
                                              "}",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "JSON",
        success: function (result) {
            if (result.d != "") {
                bindSelect("#ddlSidewall", result.d);
                ddlSidewall_Change();
                if ($("#ddlSidewall option:selected").text() == "CHOOSE")
                    $("#ddlSidewall").focus();
            }
        },
        failure: function (result) {
            var str = result;
        }
    });
}

function ddlSidewall_Change() {
    $("#lblProcessID").text("");
    $("#hdnProcessID").val("");
    $("#hdnSidewall").val($("#ddlSidewall option:selected").text());
    $.ajax({
        url: "dailyproductionupload.aspx/GetType",
        data: "{config:\"" + $("#ddlConfig option:selected").text() + "\"," +
                                              "brand:\"" + $("#ddlBrand option:selected").text() + "\"," +
                                              "sidewall:\"" + $("#ddlSidewall option:selected").text() + "\"" +
                                              "}",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "JSON",
        success: function (result) {
            if (result.d != "") {
                bindSelect("#ddlType", result.d);
                ddlType_Change();
                if ($("#ddlType option:selected").text() == "CHOOSE")
                    $("#ddlType").focus();
            }
        },
        failure: function (result) {
            var str = result;
        }
    });
}

function ddlType_Change() {
    $("#lblProcessID").text("");
    $("#hdnProcessID").val("");
    $("#hdnType").val($("#ddlType option:selected").text());
    $.ajax({
        url: "dailyproductionupload.aspx/GetSize",
        data: "{config:\"" + $("#ddlConfig option:selected").text() + "\"," +
                                              "brand:\"" + $("#ddlBrand option:selected").text() + "\"," +
                                              "sidewall:\"" + $("#ddlSidewall option:selected").text() + "\"," +
                                              "type:\"" + $("#ddlType option:selected").text() + "\"" +
                                              "}",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "JSON",
        success: function (result) {
            if (result.d != "") {
                bindSelect("#ddlTyreSize", result.d);
                ddlTyresize_Change();
                if ($("#ddltyresize option:selected").text() == "choose")
                    $("#ddlTyreSize").focus();
            }
        },
        failure: function (result) {
            var str = result;
        }
    });
}

function ddlTyresize_Change() {
    $("#lblProcessID").text("");
    $("#hdnProcessID").val("");
    $("#hdnSize").val($("#ddlTyreSize option:selected").text());
    $.ajax({
        url: "dailyproductionupload.aspx/GetRim",
        data: "{config:\"" + $("#ddlConfig option:selected").text() + "\"," +
                                              "brand:\"" + $("#ddlBrand option:selected").text() + "\"," +
                                              "sidewall:\"" + $("#ddlSidewall option:selected").text() + "\"," +
                                              "type:\"" + $("#ddlType option:selected").text() + "\"," +
                                              "size:\"" + $("#ddlTyreSize option:selected").text() + "\"" +
                                              "}",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "JSON",
        success: function (result) {
            if (result.d != "") {
                bindSelect("#ddlRim", result.d);
                ddlRim_Change();
                if ($("#ddlRim option:selected").text() == "CHOOSE")
                    $("#ddlRim").focus();
            }
        },
        failure: function (result) {
            var str = result;
        }
    });
}

function ddlRim_Change() {
    $("#lblProcessID").text("");
    $("#hdnProcessID").val("");
    $("#hdnRim").val($("#ddlRim option:selected").text());

    $.ajax({
        url: "dailyproductionupload.aspx/GetProcessId",
        data: "{config:\"" + $("#ddlConfig option:selected").text() + "\"," +
                                              "brand:\"" + $("#ddlBrand option:selected").text() + "\"," +
                                              "sidewall:\"" + $("#ddlSidewall option:selected").text() + "\"," +
                                              "type:\"" + $("#ddlType option:selected").text() + "\"," +
                                              "size:\"" + $("#ddlTyreSize option:selected").text() + "\"," +
                                              "rim:\"" + $("#ddlRim option:selected").text() + "\"" +
                                              "}",
        type: "POST",
        contentType: "application/json",
        dataType: "text",
        success: function (result) {
            result = JSON.parse(result);
            if (result.d != "") {
                $("#hdnProcessId").val(result.d);
                $("#lblProcessID").html(result.d);
                if ($("#txtStencilNo").val().length > 8) $('#txtStencilNo').val($('#txtStencilNo').val().substring(0, 8));
                $("#ddlGrade").focus();
            }
        },
        failure: function (result) {
            var str = result;
        }
    });
}

function chkStencilExist(stencilNo) {
    var isExist = false;
    var preSelected = false;
    if ($("#hdnEditMode").val() == "1" && stencilNo == $("#hdnPrevStencilNo").val()) {
        prepareBarcode();
        return false;
    }

    $(".lblStencilNo").each(function (e) {
        // if stencil no in the selected row of gridview is equal to the stencil no in the text box then preSelected = true; - no need to check again for already "checked and added" stencil no.
        if ($("#hdnRowSelectedIndex").val() != "" && $("#hdnRowSelectedIndex").val() == e.toString()) {
            if ($(".lblStencilNo").eq(e).html() == $('#txtStencilNo').val()) {
                preSelected = true;
                return;
            }
        }
        else {
            if ($(this).text() == stencilNo) {
                $("#lblErrMsgcontent").text("Stencil No already exist in the Row " + (e + 1).toString());
                isExist = true;
                return false;
            }
        }
    });

    if (isExist == true) {
        $('#txtStencilNo').val($('#txtStencilNo').val().substring(0, 8));
        return false;
    }
    else if (preSelected == true) {
        return;
    }

    $.ajax({
        url: "dailyproductionupload.aspx/CheckStencilExist",
        data: "{strStencilNo:\"" + stencilNo + "\"}",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            if (result.d != "No") {
                $("#lblErrMsgcontent").text(result.d);
                $('#txtStencilNo').val($('#txtStencilNo').val().substring(0, 8));
                $('#txtStencilNo').focus();
            }
            else
                prepareBarcode();
        }
    });
}

function bindSelect(control, data) {
    data = JSON.parse(data);
    var lstOptions = "";
    if (data.length > 1)
        lstOptions += "<option value='0'>CHOOSE</option>";
    for (var i = 0; i < data.length; i++) {
        lstOptions += "<option value='" + data[i] + "'>" + data[i] + "</option>";
    }
    $(control).html(lstOptions);
    return data.length;
}
function CtrlEnable(Qstring, statusid, preparestatus, uploadstatus, holdstatus) {
    EnableDisableButton('#imgPrepare', '0.3');
    EnableDisableButton('#imgEmail', '0.3');
    EnableDisableButton('#imgdownload', '0.3');
    EnableDisableButton('#imgUpload', '0.3');
    EnableDisableButton('#btnMoveOrder', '0.3');
    $('input[type=checkbox],input:radio,input[type=text],#ddl_deliverypriority').prop('disabled', true);
    if (Qstring == 'proforma') {
        $('#tr_Proforma1_RefNoModeofTrans').show();
        $('#tr_Proforma2_PaymentOtherCharges').show();
    }
    else if (Qstring == "workorder")
        $('#tr_WorkOrder_WorkOrderDetails').show();
    else if (Qstring == "invoice")
        $('#tr_Invoice_InvoiceDetails').show();
    else if (Qstring == "tcs")
        $('#tr_Tcs_Details').show();

    if (holdstatus == 'true') {
        EnableDisableButton('#imgdownload', '1');
        $('#lblErrMsg').html('CREDIT HOLD').css({ 'font-size': '16px' });
    }
    else {
        if (statusid == '1') {
            if (preparestatus == 'false') {
                EnableDisableButton('#imgPrepare', '1');
                $('#lblPrepareMessage').html("Prepare proforma");
                $('input[type=checkbox],input:radio,input:text[id*=MainContent_gvOrderItemList_txtPartNo_]').prop('disabled', false);
                $('#tr_OtherCharges input[type=text]').prop('disabled', false)
                $('#txtPayterms').prop('disabled', false);
            }
            else if (preparestatus == 'true') {
                EnableDisableButton('#imgdownload', '1');
                EnableDisableButton('#imgEmail', '1');
                $('#lblPrepareMessage').html("Proforma Generated").css({ 'color': '#c3c305' });
            }
        }
        else if (statusid == '2') {
            EnableDisableButton('#imgdownload', '1');
            $('#lblPrepareMessage').html("Proforma sent").css({ 'color': 'green' });
            EnableDisableButton('#btnMoveOrder', '1');
        }
        else if (statusid == '3') {
            EnableDisableButton('#imgdownload', '1');
            if (preparestatus == 'false') {
                EnableDisableButton('#imgPrepare', '1');
                $('#lblPrepareMessage').html("Prepare workorder");
                $('input[type=text],input:checkbox,#ddl_deliverypriority').prop('disabled', false);
            }
            else if (preparestatus == 'true') {
                $('#lblPrepareMessage').html("Workorder prepared").css({ 'color': '#c3c305' });
                EnableDisableButton('#btnMoveOrder', '1');
            }
        }
        else if (statusid == "7") {
            EnableDisableButton('#imgdownload', '1');
            $('input[type=text]').prop('disabled', false);
            if (preparestatus == 'false')
                EnableDisableButton('#imgPrepare', '1');
            else if (preparestatus == 'true') {
                EnableDisableButton('#imgdownload', '1');
                EnableDisableButton('#btnMoveOrder', '1');
            }
            if (uploadstatus == 0) {
                EnableDisableButton('#imgUpload', '1');
                $('#lblUploadMessage').html('Upload LR File').css({ 'color': '#614126' });
            }
            else if (uploadstatus == "1")
                $('#lblUploadMessage').html('LR File Uploaded').css({ 'color': 'Green' });
        }
        else if (statusid == "8" || statusid == "21") {
            EnableDisableButton('#btnMoveOrder', '1');
        }
        else if (statusid == "43") {
            $("input:radio[id*=rdbTcsAmt]").prop('disabled', false);
            $("input:radio[id*=rdbTcsPan]").prop('disabled', false);
            EnableDisableButton('#btnMoveOrder', '1');
        }
    }
}

function EnableDisableButton(id, op) {
    $(id).fadeTo('slow', op);
    if (op != "1")
        $(id).prop('disabled', true).css({ 'cursor': 'no-drop' });
    else
        $(id).prop('disabled', false).css({ 'cursor': 'pointer' });
}

function gotoBottomDiv(ctrlID) {
    $('#' + ctrlID).css({ 'display': 'block' });
    $("html, body").stop().animate({
        scrollLeft: $('#' + ctrlID).offset().left,
        scrollTop: $('#' + ctrlID).offset().top
    }, 1200);
}
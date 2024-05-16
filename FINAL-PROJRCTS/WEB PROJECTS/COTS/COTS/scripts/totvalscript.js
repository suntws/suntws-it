function sumAllTotQty() {
    var sumQty = 0; $('#txtOrderQty').val('');
    $("#ContentPlaceHolder1_gvSolid tr").find('td:eq(1)').find("[id*='ContentPlaceHolder1_gvSolid_lbl_TotPcs_']").each(function () { sumQty += Number($(this).html()); });
    $("#ContentPlaceHolder1_gvPob tr").find('td:eq(1)').find("[id*='ContentPlaceHolder1_gvPob_lbl_TotPcs_']").each(function () { sumQty += Number($(this).html()); });
    if (sumQty > 0) { $('#txtOrderQty').val(sumQty); }
}

function sumAllTotVolume() {
    var sumTotVol = 0; $('#txtOrderVolume').val('');
    $('.cssTotVol').each(function () { sumTotVol += Number($(this).val()); });
    if (sumTotVol > 0) { $('#txtOrderVolume').val((parseInt(sumTotVol) / 0.9).toFixed()); }
}

function sumAllTotUPrice() {
    var sumTotPrice = 0; $('#txtOrderAmt').val('');
    $('.cssTotPrice').each(function () { sumTotPrice += Number($(this).val()); });
    if (sumTotPrice > 0) {
        if ($('#lblCurType').html().toLowerCase() == "inr") { $('#txtOrderAmt').val((parseFloat(sumTotPrice)).toFixed(2)); }
        else { $('#txtOrderAmt').val((parseFloat(sumTotPrice)).toFixed(2)); }
    }
}

function sumAllTotFinishedWt() {
    var sumTotPrice = 0; $('#txtOrderWt').val('');
    $("#ContentPlaceHolder1_gvSolid tr").find('td:eq(0)').find("[id*='ContentPlaceHolder1_gvSolid_lbl_TotFinishedWT_']").each(function () { sumTotPrice += Number($(this).html()); });
    $("#ContentPlaceHolder1_gvPob tr").find('td:eq(0)').find("[id*='ContentPlaceHolder1_gvPob_lbl_TotFinishedWT_']").each(function () { sumTotPrice += Number($(this).html()); });
    if (sumTotPrice > 0) { $('#txtOrderWt').val((parseFloat(sumTotPrice)).toFixed(2)); }
}

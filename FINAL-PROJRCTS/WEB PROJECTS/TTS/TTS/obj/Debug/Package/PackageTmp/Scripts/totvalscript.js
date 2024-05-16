function sumAllTotQty() {
    var sumQty = 0; $('#txtOrderQty').val('');
    $("#MainContent_gvSolid tr").find('td:eq(1)').find("[id*='MainContent_gvSolid_lbl_TotPcs_']").each(function () { sumQty += Number($(this).html()); });
    $("#MainContent_gvPob tr").find('td:eq(1)').find("[id*='MainContent_gvPob_lbl_TotPcs_']").each(function () { sumQty += Number($(this).html()); });
    if (sumQty > 0) { $('#txtOrderQty').val(sumQty); }
}
function sumAllTotFinishedWt() {
    var sumTotPrice = 0; $('#txtOrderWt').val('');
    $("#MainContent_gvSolid tr").find('td:eq(0)').find("[id*='MainContent_gvSolid_lbl_TotFinishedWT_']").each(function () { sumTotPrice += Number($(this).html()); });
    $("#MainContent_gvPob tr").find('td:eq(0)').find("[id*='MainContent_gvPob_lbl_TotFinishedWT_']").each(function () { sumTotPrice += Number($(this).html()); });
    if (sumTotPrice > 0) { $('#txtOrderWt').val((parseFloat(sumTotPrice)).toFixed(2)); }
}

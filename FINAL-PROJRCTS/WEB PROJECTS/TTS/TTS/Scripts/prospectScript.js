function BindFlagBgColor() {
    $("span[id*='_lblListFlag_']").each(function (elem) {
        if ($(this).html() == "On" || $(this).html() == "")
            $(this).parent('td').css({ 'background-color': '#ff0000', 'color': '#fff' });
        else
            $(this).parent('td').css({ 'background-color': '#2D920D', 'color': '#fff' });
    });
}

function gotoDiv(ctrlID) {
    //var target = $(this).attr("href");
    $("html, body").stop().animate({
        scrollLeft: $('#' + ctrlID).offset().left,
        scrollTop: $('#' + ctrlID).offset().top
    }, 1200);
}
function mouseX(evt) {
    if (evt.pageX) return evt.pageX;
    else if (evt.clientX)
        return evt.clientX + (document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft);
    else return null;
}
function mouseY(evt) {
    if (evt.pageY) return evt.pageY;
    else if (evt.clientY)
        return evt.clientY + (document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop);
    else return null;
}
function ShowToolTip() {
    $('.supplier').click(function (e) {
        var idd = this.id;
        $('#hdnpositionid').val(idd);
        var id = $(this).closest('tr').attr('id');
        var rowData = $("#jqGrid").getRowData(id);
        var custname = rowData['Custname'];
        left = ''; var modelInfo1 = ''; var top = ''; // mouseY(e);
        var modelInfo = ''; var json = JSON.parse(supplierhistory);
        $.each(json, function (i, data) {
            if (data.p_custcode == id)
                modelInfo += "<tr class='cssTip'><td class='cssTip' style='width:240px;'>" + data.sup_name + "</td><td class='cssTip' style='width:30px;'>" + data.sup_from + "</td><td class='cssTip' style='width:30px;'>" + data.sup_to + "</td><tr>";
        }); left = mouseX(e) - 300;
        if (modelInfo != '')
            modelInfo1 += "<table cellspacing='0' rules='all' border='1' class='tableCust cssTip' style='width:300px;'><tr class='cssTip'><th class='cssTip' style='width:240px;'>SUPPLIER NAME</th><th  class='cssTip' style='width:30px;'>FROM</th><th  class='cssTip' style='width:30px;'>TO</th></tr>" + modelInfo + "</table>";
        else
            modelInfo1 = 'NO RECORD'; $('#code').html(custname + ' - ' + id);
        top = $('#' + idd).position().top;
        // left = $('#' + idd).position().left;
        $('#myToolTip').css({ "top": top + 213, "left": left });
        $('#myToolTip1').html('');
        $('#myToolTip1').append(modelInfo1);
        $('#myToolTip').show();
    });
    $('.LeadStatus').click(function (e) {
        var idd = this.id;
        $('#hdnpositionid').val(idd);
        var id = $(this).closest('tr').attr('id');
        var rowData = $("#jqGrid").getRowData(id);
        var custname = rowData['Custname'];
        var left = ''; var modelInfo1 = ''; var top = ''; //mouseY(e); 
        var modelInfo = '';
        var json = JSON.parse(LeadStatus);
        $.each(json, function (i, data) {
            if (data.custcode == id)
                modelInfo += "<tr class='cssTip'><td class='cssTip' style='width:200px;'>" + data.leadName + "</td><td class='cssTip' style='width:70px;'>" + data.leadtime + "</td><tr>";
        }); left = mouseX(e) - 300;
        if (modelInfo != '')
            modelInfo1 += "<table cellspacing='0' rules='all' border='1' class='tableCust cssTip' style='width:300px;'><tr class='cssTip'><th class='cssTip' style='width:200px;'>LEAD NAME</th><th  class='cssTip' style='width:70px;'>TIME</th></tr>" + modelInfo + "</table>";
        else
            modelInfo1 = 'NO RECORD';
        $('#code').html(custname + ' - ' + id);
        top = $('#' + idd).position().top; // left = $('#' + idd).position().left;
        $('#myToolTip').css({ "top": top + 213, "left": left });
        $('#myToolTip1').html('');
        $('#myToolTip1').append(modelInfo1);
        $('#myToolTip').show();
    }); $('.ui-icon').click(function () { $('#myToolTip').hide(); }); $('.ui-state-default').click(function () { $('#myToolTip').hide(); });
}
function OnSuccessCountry(response) {
    var xmlDoc = $.parseXML(response.d); var xml = $(xmlDoc); var CityVals = xml.find("T1"); var concatStr = '';
    if (CityVals.length > 0) {
        for (var j = 0; j < CityVals.length; j++) { concatStr += "<option value='" + $(CityVals[j]).find("City").text() + "'>" + $(CityVals[j]).find("City").text() + "</option>" };
    } $('#ddlCity').html("<option value='ALL'>ALL</option>" + concatStr);
}
function SupplierData(cellvalue, options, rowObject) {
    var id = options.rowId; var SupplierInfo = ''; var Supplier = '';
    var json = JSON.parse(supplierhistory);
    $.each(json, function (i, data) { if (data.p_custcode == id) { SupplierInfo = "OK"; } });
    if (SupplierInfo == "OK")
        Supplier = '<span class="supplier" id="sup_' + id + '" rel="details" class="btn btn-small pull-right" data-toggle="popover" data-image-id="5" data-content="">Supplier</span>';
    else Supplier = "NA";
    return Supplier;
}
function LeadData(cellvalue, options, rowObject) {
    var id = options.rowId; var LeadInfo = ''; var Lead = '';
    var json1 = JSON.parse(LeadStatus);
    $.each(json1, function (i, data) { if (data.custcode == id) { LeadInfo = "OK"; } });
    if (LeadInfo == "OK")
        Lead = '<span class="LeadStatus" id="Lead_' + id + '" rel="details" class="btn btn-small pull-right" data-toggle="popover" data-image-id="5" data-content="">Lead</span>';
    else Lead = "NA";
    return Lead;
}

function radiobtn(cellvalue, options, rowObject) {
    var id = options.rowId; var btn = '<input type="radio" name="rdb" value="' + id + '" id="rdb_' + id + '" onclick=SelectSingleReview(this.value); />'
    return btn;
}
function leadFeedBack(cellvalue, options, rowObject) {
    var id = options.rowId; var leadFeed = ''; var splinst = rowObject.specialinstruction; var divspl = '';
    if (splinst != "Enter Special Instructions" && splinst != null) divspl = "<br/><div style='backgound-color:#D0A9F5'>" + splinst + "</div>";
    var json = JSON.parse($('#hdnLeadfeedback').val());
    $.each(json, function (i, data) {
        var lastfeedback = '';
        if (data.custcode == id) {
            lastfeedback = data.leadsfeedback;
            if (lastfeedback.length > 100)
                leadFeed = lastfeedback.substring(0, 100) + "..<span class='leadhistorymore' onclick=\"SelectSingleReview(\'" + id + "\');\">more</span><br/>Led by:<b> " + data.username + "</b>  " + data.createddate;
                else
                    leadFeed = lastfeedback + " by: " + data.username + " " + data.createddate;
        }
    }); return leadFeed + divspl;
}
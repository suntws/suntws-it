function blinkOrderHead() { $('#divOrderHead').css({ 'background-color': '#AF9AFB', 'color': '#000' }); setTimeout("setblinkOrderHead()", 1000) }

function setblinkOrderHead() { $('#divOrderHead').css({ 'background-color': '#660053', 'color': '#fff' }); setTimeout("blinkOrderHead()", 1000) }

function gotoPreviewDiv(ctrlID) { $('#' + ctrlID).css({ 'display': 'block' }); $("html, body").stop().animate({ scrollLeft: $('#' + ctrlID).offset().left, scrollTop: $('#' + ctrlID).offset().top }, 1200); }
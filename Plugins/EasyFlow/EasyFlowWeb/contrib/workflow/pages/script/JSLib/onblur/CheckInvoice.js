function CheckInvoice(id) {
    var val = $(id).val();
    //alert(val);
    if (!isNaN(val)) {
    } else {
        alert("请输入正确的发票号。");
        $(id).attr("value", "");
    }
}
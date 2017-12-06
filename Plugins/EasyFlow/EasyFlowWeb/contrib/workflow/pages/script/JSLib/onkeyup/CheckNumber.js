function CheckValue(id) {
    var val = $(id).val();
    //alert(val);
    if (!isNaN(val)) {
    } else {
        $(id).attr("value", "")
    }
}
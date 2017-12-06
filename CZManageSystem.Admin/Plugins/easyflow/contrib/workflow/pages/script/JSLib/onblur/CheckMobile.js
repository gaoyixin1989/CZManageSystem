function isMobil(id) {
    var c = $(id).val();
    var patrn = /^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$/;
    if (c == '')
        return;
    if (!patrn.exec(c)) {
        $(id).focus();
        //c.select();
        alert('非法的手机号码.');
        return false;
    }
    return true
}
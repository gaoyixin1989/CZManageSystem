function isPostalCode(id) {
    var val = $(id).val();
    if (val == '')
        return;
    if (val.length != 6) {
        $(id).select();
        $(id).focus();
        alert('非法的邮政编码格式.');
        return false;
    }
    var patrn = /^[0-9]+$/;
    if (!patrn.exec(val)) {
        $(id).select();
        $(id).focus();
        alert('非法的邮政编码格式.');
        return false;
    }
    return true
}

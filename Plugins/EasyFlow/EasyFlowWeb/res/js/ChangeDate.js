//格式2012-02-29 15:45:50 日期格式的 
function ConvertJosnDateToDatetime(strDate) {
    var d = null;
    if (strDate != null) {
        d = new Date(parseInt(strDate.substring(6, strDate.length - 2)));
    }
    if (d == null) {
        d = new Date();
    }
    var year = d.getFullYear();
    var month = d.getMonth() + 1;
    var day = d.getDate();
    var hour = d.getHours();
    var minute = d.getMinutes();
    var second = d.getSeconds();
    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    if (hour < 10)
        hour = "0" + hour;
    if (minute < 10)
        minute = "0" + minute;
    if (second < 10)
        second = "0" + second;


    return year + "-" + month + "-" + day + "   " + hour + ":" + minute + ":" + second;


}
//格式2012-02-29 日期格式的
function ConvertJosnDateToDate(strDate) {
    var d = null;
    if (strDate != null) {
        d = new Date(parseInt(strDate.substring(6, strDate.length - 2)));
    }
    if (d == null) {
        d = new Date();
    }
    var year = d.getFullYear();
    var month = d.getMonth() + 1;
    var day = d.getDate();


    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    return year + "-" + month + "-" + day;


}


//参数为字符串类型，转换为日期形式 2012-03-01 12:00:00
function ConvertStringToDatetime(str) {
    var temp = 0;
    var tempIndex = 0;
    var year;
    var day;
    var month;
    var hour;
    var second;
    var minute;
    temp = str.indexOf('-');
    year = str.substring(0, temp);
    tempIndex = str.lastIndexOf('-');
    month = str.substring(temp + 1, tempIndex);
    temp = str.lastIndexOf('-');
    tempIndex = str.indexOf(' ');
    day = str.substring(temp + 1, tempIndex);
    temp = str.indexOf(':');
    hour = str.substring(tempIndex + 1, temp);
    tempIndex = str.lastIndexOf(":");
    minute = str.substring(temp + 1, tempIndex);
    second = str.substring(tempIndex + 1);


    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    if (hour < 10)
        hour = "0" + parseInt(hour);
    if (minute < 10)
        minute = "0" + parseInt(minute);
    if (second < 10)
        second = "0" + parseInt(second);


    return year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;


}


//参数为字符串类型，转换为日期形式 2012-03-01
function ConvertStringToDate(str) {
    var temp = 0;
    var tempIndex = 0;
    var year;
    var day;
    var month;
    temp = str.indexOf('-');
    year = str.substring(0, temp);
    tempIndex = str.lastIndexOf('-');
    month = str.substring(temp + 1, tempIndex);
    temp = str.lastIndexOf('-');
    tempIndex = str.indexOf(' ');
    day = str.substring(temp + 1, tempIndex);




    if (month < 10)
        month = "0" + parseInt(month);
    if (day < 10)
        day = "0" + parseInt(day);


    return year + "-" + month + "-" + day;


}
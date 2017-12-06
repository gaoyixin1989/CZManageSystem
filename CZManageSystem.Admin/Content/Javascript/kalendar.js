$(function () {
    var data = [];
    /*************    方法     **************/
    //判断是否是闰年,计算每个月的天数
    function leapYear(year) {
        var isLeap = year % 100 == 0 ? (year % 400 == 0 ? 1 : 0) : (year % 4 == 0 ? 1 : 0);
        return new Array(31, 28 + isLeap, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
    }

    //获得某月第一天是周几
    function firstDay(day) {
        return day.getDay();
    }

    //获得当天的相关日期变量
    function dateNoneParam() {
        var day = new Date();
        var today = new Array();
        today['year'] = day.getFullYear();
        today['month'] = day.getMonth();
        today['date'] = day.getDate();
        today['hour'] = day.getHours();
        today['minute'] = day.getMinutes();
        today['second'] = day.getSeconds();
        today['week'] = day.getDay();
        today['firstDay'] = firstDay(new Date(today['year'], today['month'], 1));
        return today;
    }

    //获得所选日期的相关变量
    function dateWithParam(year, month) {
        var day = new Date(year, month);
        var date = new Array();
        date['year'] = day.getFullYear();
        date['month'] = day.getMonth();
        date['firstDay'] = firstDay(new Date(date['year'], date['month'], 1));
        return date;
    }

    //生成日历代码 的方法
    //参数依次为 年 月 日 当月第一天(是星期几)
    function kalendarCode(codeYear, codeMonth, codeDay, codeFirst) {
        kalendar_html = "<table id='kalendar'>\n<tr id='select' class='selectpage'>\n<td colspan=7>";
        kalendar_html += "<div id='year'>\n<ul>\n<li class='selectChange'><a id='yearPrev' href='javascript:void(0);' class='a-3'></a><select name='year'>";
        //年 选择
        //for (var i = 1970; i <= codeYear + yearfloor; i++) {//文档要求2014-2024
        for (var i = 2014; i <= 2024; i++) {
            if (i == codeYear) {
                kalendar_html += "<option value='" + i + "' selected='true'>" + i + "年</option>";
            } else {
                kalendar_html += "<option value='" + i + "'>" + i + "年</option>";
            }
        }

        kalendar_html += "</select><a id='yearNext' href='javascript:void(0);' class='a-4'></a></li></ul>\n</div>";

        //月 选择
        kalendar_html += "<div id='month'><ul><li class='selectChange'><a id='monthPrev' href='javascript:void(0);' class='a-3'></a><select name='year'>";

        for (var j = 0; j <= 11; j++) {
            if (j == codeMonth) {
                kalendar_html += "<option value='" + j + "' selected='true'>" + month[j] + "月</option>";
            } else {
                kalendar_html += "<option value='" + j + "'>" + month[j] + "月</option>";
            }
        }
        kalendar_html += "</select><a id='monthNext' href='javascript:void(0);' class='a-4'></a></li></ul></div></td></tr>";


        kalendar_html += "<tr id='week'>\n<td>\n<ul>\n<li class='weekend'>日</li>\n<li>一</li>\n<li>二</li>\n<li>三</li>\n<li>四</li>\n<li>五</li>\n<li class='weekend'>六</li>\n</ul>\n</td>\n</tr>\n\n<tr id='day'>\n<td colspan=7>\n";
        var strCurYearMonth = padLeft(codeYear, 4) + '-' + padLeft(parseInt(codeMonth) + 1, 2) + '-';//2016-02-
        //日 列表
        for (var m = 0; m < 6; m++) {//日期共 4-6 行
            if (m >= Math.ceil((codeFirst + monthDays[codeMonth]) / 7)) {//第五、六行是否隐藏				
                kalendar_html += "<ul class='dayList hide dayListHide" + m + "'>\n";
            } else {
                kalendar_html += "<ul class='dayList dayListHide" + m + "'>\n";
            }

            for (var n = 0; n < 7; n++) {//列
                if ((7 * m + n) < codeFirst || (7 * m + n) >= (codeFirst + monthDays[codeMonth])) {//某月日历中不存在的日期
                    kalendar_html += "<li></li>";
                } else {
                    var strDate = strCurYearMonth + padLeft((7 * m + n + 1 - codeFirst), 2);
                    if ((7 * m + n + 1 - codeFirst == codeDay) && (((7 * m + n) % 7 == 0) || ((7 * m + n) % 7 == 6))) {//当天是周末
                        kalendar_html += "<li class='todayWeekend' day='" + strDate + "'>" + (7 * m + n + 1 - codeFirst) + "</li>";
                    } else if (((7 * m + n) % 7 == 0) || ((7 * m + n) % 7 == 6)) {//仅是周末
                        kalendar_html += "<li class='weekend' day='" + strDate + "'>" + (7 * m + n + 1 - codeFirst) + "</li>";
                    } else if (7 * m + n + 1 - codeFirst == codeDay) {//仅是当天
                        kalendar_html += "<li class='today' day='" + strDate + "'>" + (7 * m + n + 1 - codeFirst) + "</li>";
                    } else {//其他日期
                        kalendar_html += "<li day='" + strDate + "'>" + (7 * m + n + 1 - codeFirst) + "</li>";
                    }
                }
            }
            kalendar_html += "</ul>\n";
        }
        kalendar_html += "</td>\n</tr>\n</table><div id='kalendar_detail'></div>";
        return kalendar_html;
    }

    //年-月select框改变数值 的方法
    //参数依次为 1、操作对象(年下拉菜单 或 月下拉菜单) 2、被选中的下拉菜单值
    function y_mChange(obj, stopId) {
        obj.val(stopId);
    }

    //修改日历列表 的方法
    //参数依次为 操作对象(每一天) 月份 修改后的第一天是星期几 修改后的总天数 当天的具体日期
    function dateChange(dateObj, dateMonth, dateFirstDay, dateTotalDays, dateCurrentDay) {
        //判断新日历有几行,需要显示或隐藏
        var newLine = Math.ceil((dateFirstDay + monthDays[dateMonth]) / 7);//新行数
        if (newLine > dateLine) {//增加行
            for (var i = dateLine; i < newLine; i++) {
                $('.dayListHide' + i).show();
            }
        } else if (newLine < dateLine) {//减少行
            for (var i = dateLine - 1; i >= newLine; i--) {
                $('.dayListHide' + i).hide();
            }
        }
        //重置日期排序
        dateLine = newLine;
        /*如果改变 月 后，选中月的总天数小于当前日期，
		*如当前3.31，选中2月，可2月最多29天，则将当前日期改为选中月的最后一天，即2.39
		*/
        if (dateTotalDays < dateCurrentDay) {
            dateCurrentDay = dateTotalDays;
        }
        var strCurYearMonth = padLeft(yearChange, 4) + '-' + padLeft(parseInt(monthChange) + 1, 2) + '-';//2016-02-
        for (var i = 0; i < 7 * newLine; i++) {
            if (i < dateFirstDay || i > (dateTotalDays + dateFirstDay - 1)) {//日历中 当月不存在的日期
                dateObj.eq(i).text('').removeClass();
                dateObj.eq(i)[0].setAttribute('day', '');
            } else {
                var strDate = strCurYearMonth + padLeft(i + 1 - dateFirstDay, 2);
                if ((i + 1 - dateFirstDay == dateCurrentDay) && (i % 7 == 0 || i % 7 == 6)) {
                    dateObj.eq(i).removeClass().addClass('todayWeekend');
                } else if (i % 7 == 0 || i % 7 == 6) {//仅周末
                    dateObj.eq(i).removeClass().addClass('weekend');
                } else if (i + 1 - dateFirstDay == dateCurrentDay) {//仅当天
                    dateObj.eq(i).removeClass().addClass('today');
                } else {//其他日期
                    dateObj.eq(i).removeClass();
                }
                dateObj.eq(i).text(i + 1 - dateFirstDay);
                dateObj.eq(i)[0].setAttribute('day', strDate);
            }
        }
    }

    /*************    缓存节点和变量     **************/
    var rili_location = $('#rl');//日历代码的位置
    var kalendar_html = '';//记录日历自身代码 的变量
    var yearfloor = 10;//选择年份从1970到当前时间的后10年

    var someDay = dateNoneParam();//修改后的某一天,默认是当天
    var yearChange = someDay['year'];//改变后的年份，默认当年
    var monthChange = someDay['month'];//改变后的年份，默认当月

    /*************   将日历代码放入相应位置，初始时显示此处内容      **************/

    //获取时间，确定日历显示内容
    var today = dateNoneParam();//当天

    /*月-日 设置*/
    //var month = new Array('一', '二', '三', '四', '五', '六', '七', '八', '九', '十', '十一', '十二');
    var month = new Array('1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12');
    var monthDays = leapYear(today['year']);//返回数组，记录每月有多少天
    var weekDay = new Array('日', '一', '二', '三', '四', '五', '六');
    // alert('年:'+someDay['year']+'\n月:'+someDay['month']+'\n日:'+someDay['date']+'\n星期:'+someDay['week']+'\n本月第一天星期:'+someDay['firstDay']);return false;

    kalendar_html = kalendarCode(today['year'], today['month'], today['date'], today['firstDay']);
    rili_location.html(kalendar_html);

    /*************   js写的日历代码中出现的节点     **************/
    var yearPrev = $('#yearPrev');//上一年按钮
    var yearNext = $('#yearNext');//下一年按钮
    var monthPrev = $('#monthPrev');//上一月按钮
    var monthNext = $('#monthNext');//下一月按钮
    var selectYear = $('#year .selectChange select');//选择年份列表
    var listYear = $('#year .selectChange select option');//所有可选年份
    var selectMonth = $('#month .selectChange select');//选择月份列表
    var listMonth = $('#month .selectChange select option');//所有可选月份
    var dateLine = Math.ceil((monthDays[today['month']] + today['firstDay']) / 7);;//当前日历中有几行日期，默认是 当年当月
    var dateDay = $('#kalendar tr#day td ul.dayList li');//日历中的每一天
    var detailDay = $('#kalendar_detail');


    /***************************/
    //年 按钮事件
    yearPrev.bind('click', function () {
        yearChange--;
        if (yearChange < 2014) {
            yearChange = 2014;
            //alert('太前也没意义了...');
            return false;
        }
        //修改年份
        y_mChange(selectYear, yearChange);
        //重新获得 每月天数
        monthDays = leapYear(yearChange);//alert(monthDays);
        //新 年-月 下的对象信息
        someDay = dateWithParam(yearChange, monthChange);
        //修改 日期 列表
        dateChange(dateDay, someDay['month'], someDay['firstDay'], monthDays[someDay['month']], today['date']);
        renderingData();//重新渲染
        yearmonthChange(yearChange, parseInt(monthChange) + 1);//触发事件
    });

    yearNext.bind('click', function () {
        yearChange++;
        //if (yearChange >= today['year'] + yearfloor) {
        //    yearChange = today['year'] + yearfloor;
        //    alert('太后也没意义了...');
        //    return false;
        //}
        if (yearChange > 2024) {
            yearChange = 2024;
            return false;
        }
        //修改年份
        y_mChange(selectYear, yearChange);
        //重新获得 每月天数
        monthDays = leapYear(yearChange);//alert(monthDays);
        //新 年-月 下的对象信息
        someDay = dateWithParam(yearChange, monthChange);
        //修改 日期 列表
        dateChange(dateDay, someDay['month'], someDay['firstDay'], monthDays[someDay['month']], today['date']);
        renderingData();//重新渲染
        yearmonthChange(yearChange, parseInt(monthChange) + 1);//触发事件
    });

    // 月 按钮事件
    monthPrev.bind('click', function () {
        monthChange--;
        if (monthChange >= 0) {//仍在本年内
            //修改月份
            y_mChange(selectMonth, monthChange);
        } else {
            monthChange = 11;//前一年的最后一个月
            yearChange--;
            if (yearChange < 1970) {
                yearChange = 1970;
                alert('太久远也没意义了...');
                return false;
            }
            //修改月份
            y_mChange(selectMonth, monthChange);
            //修改年份
            y_mChange(selectYear, yearChange);
            //重新获得 每月天数
            monthDays = leapYear(yearChange);
        }
        //新 年-月 下的对象信息
        someDay = dateWithParam(yearChange, monthChange);
        //修改 日期 列表
        dateChange(dateDay, someDay['month'], someDay['firstDay'], monthDays[someDay['month']], today['date']);
        renderingData();//重新渲染
        yearmonthChange(yearChange, parseInt(monthChange) + 1);//触发事件
    });

    monthNext.bind('click', function () {
        monthChange++;
        if (monthChange <= 11) {//仍在本年内
            //修改月份
            y_mChange(selectMonth, monthChange);
        } else {
            monthChange = 0;//下一年的第一个月
            yearChange++;
            if (yearChange >= someDay['year'] + yearfloor) {
                yearChange = someDay['year'] + yearfloor;
                alert('太久远也没意义了...');
                return false;
            }
            //修改月份
            y_mChange(selectMonth, monthChange);
            //修改年份
            y_mChange(selectYear, yearChange);
            //重新获得 每月天数
            monthDays = leapYear(yearChange);
        }
        //新 年-月 下的对象信息
        someDay = dateWithParam(yearChange, monthChange);
        //修改 日期 列表
        dateChange(dateDay, someDay['month'], someDay['firstDay'], monthDays[someDay['month']], today['date']);
        renderingData();//重新渲染
        yearmonthChange(yearChange, parseInt(monthChange) + 1);//触发事件
    });

    // 年 选择事件
    selectYear.bind('change', function () {
        //获得年份
        yearChange = $(this).val();
        //修改年份
        y_mChange(selectYear, yearChange);
        //重新获得 每月天数
        monthDays = leapYear(yearChange);
        //新 年-月 下的对象信息
        someDay = dateWithParam(yearChange, monthChange);
        //修改 日期 列表
        dateChange(dateDay, someDay['month'], someDay['firstDay'], monthDays[someDay['month']], today['date']);
        renderingData();//重新渲染
        yearmonthChange(yearChange, parseInt(monthChange) + 1);//触发事件
    });

    // 月 选择事件
    selectMonth.bind('change', function () {
        //获得 月
        monthChange = $(this).val();
        //修改月份
        y_mChange(selectMonth, monthChange);
        //新 年-月 下的对象信息
        someDay = dateWithParam(yearChange, monthChange);
        //修改 日期 列表
        dateChange(dateDay, someDay['month'], someDay['firstDay'], monthDays[someDay['month']], today['date']);
        renderingData();//重新渲染
        yearmonthChange(yearChange, parseInt(monthChange) + 1);//触发事件
    });

    function resetData(obj) {
        //obj对象：data-数据，time-时间字符串，text-内容
        data = [];
        $.each(obj.data, function (i, item) {
            item.dateTime = new Date(item.Time.replace(/-/g, '/').replace(/T/g, ' '));
            data.push({
                dateTime: new Date(item[obj.time].replace(/-/g, '/').replace(/T/g, ' ')),
                Content: item[obj.text]
            });
        });
    }
    //将数据渲染到日历上
    function renderingData() {
        $.each(dateDay.filter('.hasData'), function (i, item) {
            jqRemoveClass(item, 'hasData');
        });
        //dateDay
        $.each(data, function (i, item) {
            if (yearChange == item.dateTime.getFullYear() && monthChange == item.dateTime.getMonth()) {
                var mm = dateDay.filter('[day=' + item.dateTime.Format('yyyy-MM-dd') + ']')[0];
                jqAddClass(mm, 'hasData');
            }
        })
    }
    //更新日程明细为某一天
    function updateDetail(date) {
        //kalendar_detail
        var curDateData = [];
        $.each(data, function (i, item) {
            if (item.dateTime.Format('yyyy-MM-dd') == date) {
                curDateData.push(item);
            }
        });
        var detail_html = '<div class="kalendar_detail_title">';
        detail_html += '<span>' + date + '</span>';
        detail_html += '<a href="javascript:void(0);"  onclick="editDay(\'' + date + '\');">编辑</a>';
        detail_html += '</div>';
        detail_html += '<div><ul>';
        $.each(curDateData, function (i, item) {
            detail_html += '<li><span style="color:#999;">' + item.dateTime.Format('HH:mm') + '</span style="color:#666;"><span>' + item.Content + '</span></li>';
        })
        detail_html += '</ul></div>';

        detailDay.html(detail_html);
        //detailDay.find('.kalendar_detail_title a').die('click').bind('click', function () {
        //    editDay(date);
        //});
    }

    /*日 鼠标事件*/
    dateDay.hover(function () {
        $(this).addClass('mouseFloat');
    }, function () {
        $(this).removeClass('mouseFloat');
    });

    dateDay.click(on_dayOnClick);
    dateDay.dblclick(on_dayOnDbCllick);
    //选中日期
    function on_dayOnClick() {
        var curDate = this.getAttribute('day');
        if (curDate == null || curDate == '')
            return;
        $.each($('.dateSelect'), function (i, item) {
            jqRemoveClass(item, 'dateSelect')
        });
        jqAddClass(this, 'dateSelect');
        //updateDetail(curDate);
        dayOnClick(curDate);
    }
    function on_dayOnDbCllick() {
        var curDate = this.getAttribute('day');
        if (curDate == null || curDate == '')
            return;
        dayOnDbCllick(curDate);
    }

    dayOnClick = function (date) { };
    dayOnDbCllick = function (date) { }
    yearmonthChange = function (year, month) { }
    editDay = function (date) { }

    kalendar_event = function (eventName, fnOrobj) {
        switch (eventName) {
            case 'dayOnClick': dayOnClick = fnOrobj; break;//日期选中事件
            case 'dayOnDbCllick': dayOnDbCllick = fnOrobj; break;//日期双击事件
            case 'yearmonthChange': yearmonthChange = fnOrobj; break;//年月变更事件
            case 'resetData': resetData(fnOrobj); renderingData(); break;//更新数据，重新渲染
            case 'editDay': editDay = fnOrobj;
            default: break;
        }
    }

});

//将num的长度补齐到n位，左侧加defaultValue，defaultValue默认0
function padLeft(num, n, defaultValue) {
    if (defaultValue == null || defaultValue == '')
        defaultValue = '0';
    return Array(n > ('' + num).length ? (n - ('' + num).length + 1) : 0).join(defaultValue) + num;
}
//判断是否为字符串
function isString(value) {
    return typeof value === 'string';
}
var trim = (function () {
    // native trim is way faster: http://jsperf.com/angular-trim-test
    // but IE doesn't have it... :-(
    // TODO: we should move this into IE/ES5 polyfill
    if (!String.prototype.trim) {
        return function (value) {
            return isString(value) ? value.replace(/^\s\s*/, '').replace(/\s\s*$/, '') : value;
        };
    }
    return function (value) {
        return isString(value) ? value.trim() : value;
    };
})();
function jqRemoveClass(element, cssClasses) {
    //gyx修改——20160614        
    //原代码对class的操作，setAttribute('class','abc')在ie7中不支持，修改为element.className
    if (cssClasses) {
        $.each(cssClasses.split(' '), function (i, cssClass) {
            element.className = trim(
              (" " + (element.className || '') + " ")
              .replace(/[\n\t]/g, " ")
              .replace(" " + trim(cssClass) + " ", " "));
        });
    }
}
function jqAddClass(element, cssClasses) {
    //gyx修改——20160614        
    //原代码对class的操作，setAttribute('class','abc')在ie7中不支持，修改为element.className
    if (cssClasses) {
        var existingClasses = (' ' + (element.className || '') + ' ')
                              .replace(/[\n\t]/g, " ");

        $.each(cssClasses.split(' '), function (i, cssClass) {
            cssClass = trim(cssClass);
            if (existingClasses.indexOf(' ' + cssClass + ' ') === -1) {
                existingClasses += cssClass + ' ';
            }
        });

        element.className = trim(existingClasses);
    }
}
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "H+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}


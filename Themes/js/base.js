$(function () {
    //heightFoot(); 
});
 
/**
* 替换字符串中的字段.
* @param {String} templ 模版字符串
* @param {Object} data 数据对象
* @param {RegExp} [regexp] 匹配字符串的正则表达式
*/
function substitute(templ, data, regexp) {
    return templ.replace(regexp || /\\?\{([^{}]+)\}/g,
                function (match, name) {
                    return (data[name] === undefined) ? '' : data[name];
                });
}
//日期比较
function checkEndTime(startTime, endTime) {
    var start = new Date(startTime.replace("-", "/").replace("-", "/"));
    var end = new Date(endTime.replace("-", "/").replace("-", "/"));
    if (end < start) {
        return false;
    }
    return true;
}

/**
* 时间格式
*/
function formatDate(now, type) {
    var now = new Date(now);
    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    var date = now.getDate();
    var hour = now.getHours();
    var minute = now.getMinutes();
    var second = now.getSeconds();
     

    if (type == "yyyy-mm-dd") {
        return year + "-" + (month < 10 ? "0" + month : month) + "-" + (date < 10 ? "0" + date : date);
    }
    else {
        return year + "-" + (month < 10 ? "0" + month : month) + "-" + (date < 10 ? "0" + date : date) + " " + (hour < 10 ? "0" + hour : hour) + ":" + (minute < 10 ? "0" + minute : minute) + ":" + (second < 10 ? "0" + second : second);
    }
}
 
/*
*js函数代码：字符串转换为时间戳 
*/
function getDateTimeStamp(dateStr) {
    return Date.parse(dateStr.replace(/-/gi, "/"));
}

/*
*查询时间段
*/
function getDateDiff(dateTimeStamp) {
    var minute = 1000 * 60;
    var hour = minute * 60;
    var day = hour * 24;
    var halfamonth = day * 15;
    var month = day * 30;


    var now = new Date().getTime();
    var diffValue = now - dateTimeStamp;
    if (diffValue < 0) {
        //若日期不符则弹出窗口告之
        //alert("结束日期不能小于开始日期！");
    }
    var monthC = diffValue / month;
    var weekC = diffValue / (7 * day);
    var dayC = diffValue / day;
    var hourC = diffValue / hour;
    var minC = diffValue / minute;

    if (dayC >= 1 || weekC >= 1 || monthC >= 1) {
        result = " " + formatDate(dateTimeStamp,"yyyy-MM-dd hh:mm:ss");
    }
    else if (hourC >= 1) {
        result = " " + parseInt(hourC) + "个小时前";
    }
    else if (minC >= 1) {
        result = " " + parseInt(minC) + "分钟前";
    } else
        result = "刚刚发布";
    return result;
}


/**
* 过滤只保留数字
*/
function formatNumber(str) {
    //str.replaceAll("[^0-9a-zA-Z\u4e00-\u9fa5.，,。？“”]+", "");
    var pattern = new RegExp("[^0-9]+", "g");
    return str.replace(pattern, '');
}


//给定出生日期，算出年龄
function birthdayToAge(str) {
    if (!str) {
        return '暂无';
    }
    var r = str.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
    if (r == null) return false;
    var d = new Date(r[1], r[3] - 1, r[4]);
    if (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]) {
        var Y = new Date().getFullYear();
        var M = new Date().getMonth();
        var iYear = Y - r[1];
        if (iYear > 0) {
            var iMonth = (M - r[3] + 1);
            if (iMonth < 0) {
                iYear = iYear - 1;
                iMonth = 12 - (-iMonth);
            }
            iYear = iYear + "岁";//+ str + "=" +iMonth;
            if (parseInt(iYear) < 3) {
                if (iMonth > 0) {
                    iYear = iYear + iMonth + "月";
                }
            }
            return iYear;
        }
        else {
            return (M - r[3] + 1) + "个月";
        }
    }
    return "error";
}
/** 
* 检查手机号
*/
function CheckPhone(phone) {
    //长度验证 
    if (!ChinaTelecom(phone) && !ChinaMobile(phone) && !ChinaUnicom(phone)) {
        return false;
    }
    else {
        return true;
    }
}
/**移动号码验证**/
/**134、135、136、137、138、139、147、150、151、152、157、158、159、182、188、170**/
function ChinaMobile(value) {
    /**移动号段：134-9，147，170,150-2，157-9，187-8**/
    var res = /^[1](3[4-9]{1}|47|70|5[0-2]{1}|5[7-9]{1}|8[2-3]{1}|8[4-8]{1}|78)\d{8}$/;
    var re = new RegExp(res);
    if (re.test(value)) {
        return true;
    }
    return false;
}

/**联通号码验证**/
function ChinaUnicom(value) {
    /**联通号段：130-2，155-6，185-6,76**/
    var res = /^[1](3[0-2]{1}|5[5-6]{1}|8[5-6]{1}|76)\d{8}$/;
    var re = new RegExp(res);
    if (re.test(value)) {
        return true;
    }
    return false;
}

/**电信号码验证**/
function ChinaTelecom(value) {
    /**电信号段：133，153，180，181,189, 177 **/
    var res = /^(([1](33|53|80|81|89|77|73)\d{8}))$/;
    var re = new RegExp(res);
    if (re.test(value)) {
        return true;
    }
    return false;
}
// 正则验证银行卡方法  
var IsBankCardNo = function (content) {
    var regex = /^(\d{16}|\d{19})$/;
    if (regex.test(content)) {
        return true;
    }
    return false;
}

/**身份证验证**/
function IsIdCardNo(idcard) {
    if (idcard == undefined || idcard == null || $.trim(idcard).length == 0)
        return false;
    idcard = idcard.toLocaleUpperCase();
    var Errors = new Array(
	"验证通过!",
	"身份证号码位数不对!",
	"身份证号码出生日期超出范围或含有非法字符!",
	"身份证号码校验错误!",
	"身份证地区非法!"
	);
    var area = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" }
    var idcard, Y, JYM;
    var S, M;
    var idcard_array = new Array();
    idcard_array = idcard.split("");
    /** 地区检验**/
    if (area[parseInt(idcard.substr(0, 2))] == null) return false;/** Errors[4];
/** 身份号码位数及格式检验**/
    switch (idcard.length) {
        case 15:
            if ((parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0 || ((parseInt(idcard.substr(6, 2)) + 1900) % 100 == 0 && (parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0)) {
                ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}$/;/** 测试出生日期的合法性**/
            } else {
                ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}$/;/** 测试出生日期的合法性**/
            }
            if (ereg.test(idcard))
                return true;/** Errors[0];**/
            else return false;/** Errors[2];**/
            break;
        case 18:
            /** 18位身份号码检测**/
            /** 出生日期的合法性检查**/
            /** 闰年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))**/
            /** 平年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))**/
            if (parseInt(idcard.substr(6, 4)) % 4 == 0 || (parseInt(idcard.substr(6, 4)) % 100 == 0 && parseInt(idcard.substr(6, 4)) % 4 == 0)) {
                ereg = /^[1-9][0-9]{5}(19|20)[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}[0-9Xx]$/;/** 闰年出生日期的合法性正则表达式**/
            } else {
                ereg = /^[1-9][0-9]{5}(19|20)[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}[0-9Xx]$/;/** 平年出生日期的合法性正则表达式**/
            }
            if (ereg.test(idcard)) {/**测试出生日期的合法性**/
                /** 计算校验位**/
                S = (parseInt(idcard_array[0]) + parseInt(idcard_array[10])) * 7
                + (parseInt(idcard_array[1]) + parseInt(idcard_array[11])) * 9
                + (parseInt(idcard_array[2]) + parseInt(idcard_array[12])) * 10
                + (parseInt(idcard_array[3]) + parseInt(idcard_array[13])) * 5
                + (parseInt(idcard_array[4]) + parseInt(idcard_array[14])) * 8
                + (parseInt(idcard_array[5]) + parseInt(idcard_array[15])) * 4
                + (parseInt(idcard_array[6]) + parseInt(idcard_array[16])) * 2
                + parseInt(idcard_array[7]) * 1
                + parseInt(idcard_array[8]) * 6
                + parseInt(idcard_array[9]) * 3;
                Y = S % 11;
                M = "F";
                JYM = "10X98765432";
                M = JYM.substr(Y, 1);/** 判断校验位**/
                if (M == idcard_array[17]) return true;/** Errors[0]; /**检测ID的校验位**/
                else return false;/** Errors[3];**/
            }
            else return false;/** Errors[2];**/
            break;
        default:
            return false;/** Errors[1];**/
            break;
    }
}

/****
**用户姓名验证
**
**/
function CheckUserName(value) {
    
    var res = /^([A-Za-z]|[\u4E00-\u9FA5])+$/;
    var re = new RegExp(res);
    if (re.test(value)) {
        return true;
    }
    return false;
}
 
/***
**
*纯数字
***/
function CheckNumber(value) {
    var res = /^[0-9]\d/;
    var re = new RegExp(res);
    if (re.test(value)) {
        return true;
    }
    return false;
}


//判断是否微信浏览器
function IsWeiXinBrowser() {
    var ua = window.navigator.userAgent.toLowerCase();
    if (ua.match(/MicroMessenger/i) == 'micromessenger') {//微信
        return 3;
    } else {
        if (/(iPhone|iPad|iPod|iOS)/i.test(navigator.userAgent)) {  //判断iPhone|iPad|iPod|iOS
            return 2;
        } else if (/(Android)/i.test(navigator.userAgent)) {   //判断Android
            return 1;
        } else {  //PC
            return 0;
        };
    }
}

;(function($){
//选项卡
$.fn.tabs=function(options){
  if(this.length == 0) return this;

  if(this.length > 1){
    this.each(function(){$(this).tabs(options)});
    return this;
  }
  if($(this).data('binds')=='yes') return false;
  $(this).data('binds','yes');
  var defaults={};
  var opts=$.extend(defaults,options || {});
  var $this=$(this),
  $hd=$this.children('div.tabs-hd').children('div'),
  $bd=$this.children('div.tabs-bd').children('div.tabs-bd-box');

  $hd.on('click',function(){
    var $el=$(this),
    index=$el.index();
    $el.addClass('curr').siblings().removeClass('curr');
    $bd.eq(index).addClass('curr').siblings().removeClass('curr');
    if(opts.callback){
      opts.callback(index);
    }
    //heightFoot();

  })
  }
})(jQuery);
window.GLOBAL_UPDATE_CYCLE = 1 * 60 * 1000;

//把字符串转换为json
function strToJson(strJson) {
    if (strJson == null || strJson == undefined || strJson == "") {
        return {};
    }
    return eval('(' + strJson + ')');
}

function Comget(url, params, successFunc, errorFunc) {
    $.ajax({
        url: url,
        data: params,
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {
            if (data.success) {
                if (typeof successFunc == 'function')
                    successFunc(data);
            } else {
                mui.toast(data.backMsg);
                loadingOut();
            }
        },
        error: function () {
            if (typeof errorFunc == 'functoin') {
                errorFunc();
            }
        }
    });
}

function Compost(url, params, successFunc, errorFunc) {
    $.ajax({
        url: url,
        data: params,
        type: 'POST',
        contentType: 'application/json',
        success: function (data) {
            if (data.success) {
                successFunc(data);
            } else {
                mui.toast(data.backMsg);
                loadingOut();
            }
        },
        error: function () {
            if (typeof errorFunc == 'functoin') {
                errorFunc();
            }
        }
    });
}

//时间戳转换为特定字符连接的字串
function dateFormate(stamp, lineStr) {
    var date = new Date(stamp);
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var lineStr = lineStr || '/';
    return year + lineStr + month + lineStr + day;
}

function throttle(method, context) {
    clearTimeout(method.tId);
    method.tId = setTimeout(function(){
        method.call(context);
    }, 100);
}

function loadingIn(text) {
    var _text = text || '';
    var _class = '';
    var append_html = '';
    if ($('.mui-loading').length > 0) {
        return false;
    }

    _class = ' mui-loading-text';
    append_html = '<div class="backup"></div><div class="mui-loading' + _class + '"><span class="mui-spinner mui-spinner-white"></span><p>' + _text + '</p></div>';
    $('body').append(append_html);
};

/**	
 * 隐藏加载中的图层
 */
function loadingOut(fast) {
    $('.mui-loading').addClass('fade');
    if (arguments.length == 1) {
        $('.mui-loading, .backup').remove();
        return;
    }
    setTimeout(function () {
        $('.mui-loading, .backup').remove();
    }, 350);
};
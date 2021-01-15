
//var baseUrl = 'http://localhost:56038' ;
var baseUrl = 'http://api.iot.lcvue.com';

//获取url参数
function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) { return pair[1]; }
    }
    return (false);
}

//post提交数据
function post(url, data, callback) {
    $.ajax({
        url: url,
        type: 'post',
        data: JSON.stringify(data),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: callback
    });
}

//post提交(带文件上传)
function postFile(url, data, callback) {
    $.ajax({
        url: url,
        type: 'post',
        data: data,
        async: true,
        contentType: false,
        processData: false,
        success: callback
    });
}

//获取表单信息
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};
var baseUrl = 'http://localhost:56038';
baseUrl = 'http://api.iot.lcvue.com';
//var _Request = {
//    createRequest: function () {
//        var request = {};
//        //request.url = '';
//        //request.data = '';

//        request.token = '';

//        //获取token
//        request.t = function (d) {
//            $.ajax({
//                url: _baseUrl + '/auth/requestToken',
//                type: 'post',
//                data: JSON.stringify(d),
//                dataType: 'json',
//                contentType: 'application/json; charset=utf-8',
//                success: function (res) {
//                    //将token放入缓存
//                    localStorage.setItem('token_obj', JSON.stringify(res.data));
//                }
//            });
//        };

//        //post请求数据
//        request.p = function (u, d, f) {
//            $.ajax({
//                url: _baseUrl + u,
//                type: 'post',
//                data: JSON.stringify(d),
//                dataType: 'json',
//                contentType: 'application/json; charset=utf-8',
//                headers: {
//                    "Authorization": "Bearer " + this.token + ""
//                },
//                success: f
//            });
//        }


//        request.rt = function (u, d) {

//        }

//        return request;
//    }
//};

class MyRequest{
    constructor(d) {
        var _tokenobj = localStorage.getItem('token_obj');
        console.log(_tokenobj);

        if (_tokenobj == null || _tokenobj == undefined) {
            this.init(d);
            var _o = $.parseJSON(_tokenobj);
            console.log(_o);
            this.token = _o.token;
        }
    }
    init(d) {
        $.ajax({
            url: baseUrl + '/auth/requestToken',
            type: 'post',
            data: JSON.stringify(d),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (res) {
                //将token放入缓存
                localStorage.setItem('token_obj', JSON.stringify(res.data));
            }
        });
    }
    p(url,data,callback) {
        $.ajax({
            url: _baseUrl + url,
            type: 'post',
            data: JSON.stringify(data),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            headers: {
                "Authorization": "Bearer " + this.token + ""
            },
            success: callback
        });
    }
    g(url, callback) {
        $.ajax({
            url: baseUrl + url,
            type: 'get',
            headers: {
                "Authorization": "Bearer " + this.token + ""
            },
            success: callback
        });
    }
}
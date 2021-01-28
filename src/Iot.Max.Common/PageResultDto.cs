using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Common
{
    /*
     * 返回自定义格式数据
     * 
     * **/
    /// <summary>
    /// 返回自定义格式数据
    /// </summary>
    public class PageResultDto
    {
        public int Code { get; set; } = (int)ResultCode.LAYUISUCCESS;  //状态码
        public string Msg { get; set; } = "Success";//提示消息
        public int Count { get; set; } = 0;//总记录数

        public dynamic Data { get; set; } = null;//返回数据

        public PageResultDto SetResult(int code, string msg, int count, dynamic datas)
        {
            Code = code;
            Msg = msg;
            Data = datas;
            Count = count;
            return this;
        }


        public PageResultDto Fun()
        {
            PageResultDto resultMsg = new PageResultDto();
            resultMsg = resultMsg.SetResult(
                        (int)ResultCode.SUCCESS //状态码200
                        ,ResultHelper.GetDescription(ResultCode.SUCCESS)  //状态码消息“成功”
                        ,Count
                        ,Data);//你要返回的数据object
            return resultMsg;
        }

    }

    public static class ResultHelper
    {

        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this object value)
        {
            if (value == null)
                return string.Empty;

            Type type = value.GetType();
            var fieldInfo = type.GetField(Enum.GetName(type, value));
            if (fieldInfo != null)
            {
                if (Attribute.IsDefined(fieldInfo, typeof(DescriptionAttribute)))
                {
                    var description =
                        Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;

                    if (description != null)
                        return description.Description;
                }
            }
            return string.Empty;
        }
    }

    /// <summary>
    /// 状态码
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// Layui数据显示成功
        /// </summary>
        [Description("Layui数据显示成功")]
        LAYUISUCCESS = 0,

        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        SUCCESS = 200,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        FAIL = 400,

        /// <summary>
        /// 请求不合理，服务器拒绝执行
        /// </summary>
        [Description("请求不合理，服务器拒绝执行")]
        UNAUTHORIZED = 403,

        /// <summary>
        /// 找不到所需数据
        /// </summary>
        [Description("找不到所需数据")]
        NOT_FOUND = 404,

        /// <summary>
        /// 服务器内部错误
        /// </summary>
        [Description("服务器内部错误")]
        INTERNAL_SERVER_ERROR = 500
    }

}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.WxMsg.Handler
{
    public interface IWxMsgHandler<T>
    {
        /// <summary>
        /// 初始化消息内容
        /// </summary>
        /// <returns></returns>
        string InitContent(T t);
    }
}
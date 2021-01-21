using Iot.Max.Wx.Models;
using Iot.Max.Wx.WxMsg.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Iot.Max.Api.Controllers.WeiXin.MsgHandler
{
    public class NewsMsg
    {
        readonly MsgReplyNewsModel content;
        readonly string eventKey;
        public NewsMsg(XDocument doc)
        {
            try
            {
                if (content == null)
                {
                    content = new MsgReplyNewsModel
                    {
                        //注意：给用户反馈回复消息touser和fromuser要对调  （所以没有用xml直接转实体）
                        FromUserName = doc.Element("xml").Element("ToUserName")?.Value,
                        ToUserName = doc.Element("xml").Element("FromUserName")?.Value,
                    };
                    eventKey = doc.Element("xml").Element("EventKey").Value;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string InitContent()
        {
            //根据用户的请求返回图文消息
            
            //根据菜单点击的具体按钮去处理数据
            content.Articles = InitItems(eventKey);
            content.ArticleCount = content.Articles.Count;

            return new MsgReplyNewsHandler().InitContent(content);
        }

        //item值最多10条。第一条图片不加链接
         static List<WxMsgReplayNewsItemModel> InitItems(string key)
        {
            //根据具体的Key,处理不同的业务数据
            List<WxMsgReplayNewsItemModel> items = new List<WxMsgReplayNewsItemModel>();
            #region 模拟数据
            for (int i = 0; i < 4; i++)
            {
                WxMsgReplayNewsItemModel item = new WxMsgReplayNewsItemModel
                {
                    Description = $"第{i}条Description",
                    Title = $"Title{i}"
                };
                if (i == 0)
                {
                    item.PicUrl = $"http://api.iot.lcvue.com/wx/1.jpg";
                }
                else
                {
                    item.PicUrl = $"http://api.iot.lcvue.com/wx/2.jpg";
                    item.Url = "http://iot.lcvue.com";
                }

                items.Add(item);
            }
            #endregion
            return items;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iot.Max.Api.Controllers.Pay
{
    public class AliPayConfig
    {
        // 应用ID,您的APPID
        public static string app_id = "2016092900625246";

        // 支付宝网关

        /// <summary>
        /// 沙箱测试环境的网关
        /// </summary>
        public static string _gatewayUrl = "https://openapi.alipaydev.com/gateway.do";

        /// <summary>
        /// 生产环境的网关
        /// </summary>
        public static string gatewayUrl = "https://openapi.alipay.com/gateway.do";

        // 商户私钥，您的原始格式RSA私钥

        /// <summary>
        /// 沙箱测试环境的原始格式RSA私钥
        /// </summary>
        public static string private_key = "MIIEowIBAAKCAQEAnjWh+Gh9r6PU6D6zLCQuB+lTqVwTeKVvs/fuCVi0ranNQ17x58Ufj6C9Uxyo/M5jmTCqLTKnpzHYl/wcpcDBIPywWChafqRV4malb8/shIz+ql9OLH3VYZ2HByIr9K7DGZoAK3TE6j6NVhW7hwQ/86dwGf+uCbE5UrqyY/5Qyez18N78G5UeHYs/UG2P5Ulu0uYDPzegzHSEeFfrypoua0I1WBftvR7z3gWaePaZJRWyR00dx3ivKRvOwCiJnCwOJAzFoEiUM1uDHiYdbCvfHmGEC0AEHv8cCAO0ZzIANafC8QZv56K5FqfuhUMqpWcQbjqb+Wb2ug9enTOxgfl2WQIDAQABAoIBAQCFNuXGM724+ftY/wqBHYnA0Z/HWCdxC+QtppLBPfcMz5BtrPGF1X7IwnEIHXbRP7uvjsGMQz3V02vsNbWtf8teykZ2RKxGPHuwofulVW9zAQGiVZOOdPoKMskyoXxfkBPpZ7nC3ZS9JKR/mtcHTfXHZPJIgfh6YsbAuUFiXw6JpuuJL/t9JYM7Bsr/PFfcx0hsp98tm2b5GiER6qGkK+yRtZSS+ta8hv3NmKFx49w03HdeEleiXuOgy2uQn53iFfKmFzoDEEaTsT0Wo1Aq17WfAd2lP4kUxPnZXRb/XJCf+oSrUrjfUi0tSrUQGVW9jzk3p/wDT7dET1ZtNB8JTSzxAoGBAPIiyPU0kaCzg2ydhVa/g0jYMk6/DwlQfm4J5W3kWwvN0juNj3Gkhv4CUT5jQpupFvv/yiJbLiBGwMlCLR6c+ZHnxtlGstNvjwogGgzppXrMdNgmVI93MFi6fjmRqYxsw+OHHM+x1XArImYnKf93o85rlVyMmgkIuUqW36nQ9G4VAoGBAKdEqMcFcQk9NRYz0S9ZZIkxDDq2ZXakQZCtKPy126tUzVrmJwlz4015b9gxRZtTpBARGwMCYMBsDarSDRvyo9HqPFafDxcv3O22g8vSjgj0PXQyUCyC462C52e1qOOVsmci4aG46YVS3Or+9lWXCcha2RPSFTLQue6AR1O5evw1AoGAOvWJZVu1kiHkAJCipSrg9t3d8hrHuIbnIjg6q3WNTeSCKoofwZZEYJmD2uMklu6ncBkqyQ7Wvnk7/EWm4utqQcLkQofah22EsPx8G2TRLIOYeAFtWbm2BgTzM2VqmYzqbf66X0B4LmXybFlg+pnRQzQjHHQIFecP5z+xmpIM0rkCgYBVF7y04rPKe3TSQrIcYGGg2W7bP4cGgDFKpIzBYtLJqm5926/oQyhTdX/Mf+RlTHueINWZBlCqE04wpIM+wIVOeNcRSgGnThYaaEtMGWVgcMACqMXYnw2sa3fFbsjoXnNPvJMEbsl9pdX1RpeSmy/C2VStnKZkH4M3LdMbaLSEMQKBgApPCh55sMcB4ZZJVxlmLffpFcGAPl5mHScuLq4A9F+C3UYKKZlYrFZrpKcox8dpggnbD5Yqu738FE8WHx1jO5bleRoIfm2SO0ZaDbLdQlGmWnptGt7TMq9BXW9FXbhqAMmYri8o4hhgu/s3wny5DbgwVL6/5lz5xY3m7bDulxW9";

        /// <summary>
        /// 真实环境的原始格式RSA私钥
        /// </summary>
        public static string _private_key = "";

        // 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。

        /// <summary>
        /// 沙箱测试环境的对应公钥
        /// </summary>
        public static string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2JbGN9bqgAmqzZFpxfYtS7CHn0nXyNUBFaVwjL5GsWzQ7IdsUJPHcX8CDzfytklTGL7kkiNjIh+Ves+SJ+HMnRjk1UL+u7ltjXDX23wNti4lA8OXj9pawJ0gF36SKq3Ywg0PT4Hc5SN3FsKCLsIzfEiZWEpOKrxYdzHVcV+7tUK+RZ63DD6NZXF8x9TgLFzGNLoE/lhXvWHUvFlolgRB0Ty8cim0/7qFRoej+19/d0w1ppWWXpomYif/uLd0FUSuB9NHDTwrH1qSahE9zAM+cOBYZyitXFS2xcuPBdDrxY21zly2VVew+NhD7lrED26sZ7rBKRBjHod82UgPpnrqpwIDAQAB";

        /// <summary>
        /// 真实环境的对应公钥
        /// </summary>
        public static string _alipay_public_key = "";

        // 签名方式
        public static string sign_type = "RSA2";

        // 编码格式
        public static string charset = "UTF-8";

        //版本号，无需修改
        public static string version = "1.0";


        ////合作伙伴ID：partnerID
        //public static string pid = "此处填写账号PID（partner id）";


        ////支付宝网关
        //public static string serverUrl = "https://openapi.alipay.com/gateway.do";
        //public static string mapiUrl = "https://mapi.alipay.com/gateway.do";
        //public static string monitorUrl = "http://mcloudmonitor.com/gateway.do";

        ////编码，无需修改
        //public static string charset = "utf-8";
        ////签名类型，支持RSA2（推荐！）、RSA
        ////public static string sign_type = "RSA2";
        //public static string sign_type = "RSA2";
        ////版本号，无需修改
        //public static string version = "1.0";


        ///// <summary>
        ///// 公钥文件类型转换成纯文本类型
        ///// </summary>
        ///// <returns>过滤后的字符串类型公钥</returns>
        //public static string getMerchantPublicKeyStr()
        //{
        //    StreamReader sr = new StreamReader(merchant_public_key);
        //    string pubkey = sr.ReadToEnd();
        //    sr.Close();
        //    if (pubkey != null)
        //    {
        //        pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
        //        pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
        //        pubkey = pubkey.Replace("\r", "");
        //        pubkey = pubkey.Replace("\n", "");
        //    }
        //    return pubkey;
        //}

        ///// <summary>
        ///// 私钥文件类型转换成纯文本类型
        ///// </summary>
        ///// <returns>过滤后的字符串类型私钥</returns>
        //public static string getMerchantPriveteKeyStr()
        //{
        //    StreamReader sr = new StreamReader(merchant_private_key);
        //    string pubkey = sr.ReadToEnd();
        //    sr.Close();
        //    if (pubkey != null)
        //    {
        //        pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
        //        pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
        //        pubkey = pubkey.Replace("\r", "");
        //        pubkey = pubkey.Replace("\n", "");
        //    }
        //    return pubkey;
        //}
    }
}

 
//using Microsoft.AspNetCore.Http;

//namespace AuthService.Common.Extensions
//{
//    public static class HttpContextExtention
//    {
//        public static string GetClientIP(this HttpContext context)
//        {
//            if (context == null)
//                return "";
//            return context.Connection.RemoteIpAddress.MapToIPv4().ToString();
//        }

//        public static DeviceInfo GetDeviceInfo(this HttpContext context)
//        {
//            DeviceInfo deviceInfo = new DeviceInfo();

//            if (context == null)
//                return deviceInfo;

//            try
//            {
//                deviceInfo.IP = context.GetClientIP();

//                string userAgent = context.Request.Headers["User-Agent"];
//                DeviceDetector dd = new DeviceDetector(userAgent);

//                dd.SetCache(new DictionaryCache());
//                dd.DiscardBotInformation();
//                dd.SkipBotDetection();
//                dd.Parse();

//                if (dd.IsBot())
//                {
//                    // handle bots,spiders,crawlers,...
//                    var botInfo = dd.GetBot();
//                }
//                else
//                {
//                    var clientInfo = dd.GetClient(); // holds information about browser, feed reader, media player, ...
//                    var osInfo = dd.GetOs();
//                    var device = dd.GetDeviceName();
//                    var model = dd.GetModel();

//                    deviceInfo.Browser = $"{clientInfo.Match.Name} v {clientInfo.Match.Version}";

//                    if (osInfo.Match.Name == "iOS" && !string.IsNullOrEmpty(model))
//                        deviceInfo.Device = $"{model}, ";

//                    deviceInfo.Device += $"{osInfo.Match.Name} {osInfo.Match.Version}";
//                    if (!string.IsNullOrEmpty(osInfo.Match.Platform))
//                        deviceInfo.Device += $", {osInfo.Match.Platform}";

//                    deviceInfo.Platform = osInfo.Match.Platform;
//                }
//            }
//            catch
//            {

//            }

//            return deviceInfo;
//        }
//    }
//}


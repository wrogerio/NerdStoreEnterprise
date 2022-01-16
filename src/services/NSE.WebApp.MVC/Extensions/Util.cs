namespace NSE.WebApp.MVC.Extensions
{
    public static class Util
    {
        public static string NormalizeReponse(this string response)
        {
            return response.Replace("{\"success\":true,\"data\":", string.Empty)
                .Replace("{\"success\":false,\"data\":", string.Empty).Replace("}}}", "}}");
        }
    }
}
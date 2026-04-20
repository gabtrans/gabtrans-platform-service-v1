using GabTrans.Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Services
{
    public class BaseService: IBaseService
    {
        //public string GetBrowser()
        //{
            // Access the HttpContext to get the Request object
        //    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

        //    // Example of a simple check for a specific browser (not robust for all cases):
        //    string browserName = "Unknown";
        //    if (userAgent.Contains("Chrome"))
        //    {
        //        browserName = "Chrome";
        //    }
        //    else if (userAgent.Contains("Firefox"))
        //    {
        //        browserName = "Firefox";
        //    }
        //    else if (userAgent.Contains("Edge"))
        //    {
        //        browserName = "Edge";
        //    }
        //    // Add more conditions for other browsers as needed

        //    return browserName;
        //}

        //public string GetIpAddress()
        //{
        //    var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        //    var serverRemoteIp = HttpContext.Request.Headers["X-Forwarded-For"].ToString();

        //    var requestIp = string.Empty;
        //    var finalIp = string.Empty;

        //    if (!string.IsNullOrWhiteSpace(serverRemoteIp))
        //    {
        //        requestIp = serverRemoteIp;
        //    }
        //    else
        //    {
        //        requestIp = remoteIp;
        //    }

        //    if (requestIp != null)
        //    {
        //        var iPList = requestIp.Split(',');
        //        if (iPList.Length > 0)
        //        {
        //            finalIp = iPList[0];
        //        }
        //    }

        //    return finalIp;
        //}

        //public long GetUserId(string accessToken)
        //{


        //    return 1;
        //}
    }
}
 
﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ags.Web.Framework.Infrastructure
{
    public class VisitorCounterMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public VisitorCounterMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            string visitorId = context.Request.Cookies["VisitorId"];
            if (visitorId == null)
            {
                context.Items.Add("newSession", "Y");
                context.Response.Cookies.Append("VisitorId", Guid.NewGuid().ToString(), new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = false,
                });
            }
            else
            {
                context.Items.Add("newSession", "N");
            }
            await _requestDelegate(context);
        }
    }
}
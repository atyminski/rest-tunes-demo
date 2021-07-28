using System;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Gevlee.RestTunes.Utils
{
    public static class UrlHelperExtensions
    {
        public static string GetCurrentRouteName(this IUrlHelper urlHelper)
        {
            object value;
            var dataTokens = urlHelper.ActionContext.HttpContext.GetRouteData().DataTokens;

            return dataTokens.TryGetValue("RouteName", out value)
                ? value.ToString()
                : null;
        }

        private static object CombineValues(Action<dynamic> factory)
        {
            dynamic result = new ExpandoObject();
            factory(result);
            return result;
        }

        public static string Link(this IUrlHelper urlHelper, string routeName, Action<dynamic> valuesFactory)
        {
            return urlHelper.Link(routeName, CombineValues(valuesFactory));
        }

        public static string LinkForCurrentRoute(this IUrlHelper urlHelper, object values)
        {
            return urlHelper.Link(urlHelper.GetCurrentRouteName(), values);
        }

        public static string LinkForCurrentRoute(this IUrlHelper urlHelper, Action<dynamic> valuesFactory)
        {
            return Link(urlHelper, urlHelper.GetCurrentRouteName(), valuesFactory);
        }
    }
}
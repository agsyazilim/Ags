using Ags.Data.Common;
using Ags.Data.Core;
using Ags.Services.Seo;
using Ags.Services.Topics;
using Ags.Web.Framework.Extensions;
using Ags.Web.Framework.UI.Paging;
using Ags.Web.Models.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Ags.Web.Extensions
{
    public static class HtmlExtensions
    {
        //we have two pagers:
        //The first one can have custom routes
        //The second one just adds query string parameter
        public static IHtmlContent Pager<TModel>(this IHtmlHelper<TModel> html, PagerModel model)
        {
            if (model.TotalRecords == 0)
            {
                return new HtmlString("");
            }

            StringBuilder links = new StringBuilder();
            if (model.ShowTotalSummary && (model.TotalPages > 0))
            {
                links.Append("<li class=\"total-summary\">");
                links.Append(string.Format(model.CurrentPageText, model.PageIndex + 1, model.TotalPages, model.TotalRecords));
                links.Append("</li>");
            }
            if (model.ShowPagerItems && (model.TotalPages > 1))
            {
                if (model.ShowFirst)
                {
                    //first page
                    if ((model.PageIndex >= 3) && (model.TotalPages > model.IndividualPagesDisplayedCount))
                    {
                        model.RouteValues.pageNumber = 1;

                        links.Append("<li class=\"first-page\">");
                        if (model.UseRouteLinks)
                        {
                            IHtmlContent link = html.RouteLink(model.FirstButtonText, model.RouteActionName, model.RouteValues, new { title = "ilk Sayfa" });
                            links.Append(link.ToHtmlString());
                        }
                        else
                        {
                            IHtmlContent link = html.ActionLink(model.FirstButtonText, model.RouteActionName, model.RouteValues, new { title = "ilk Sayfa" });
                            links.Append(link.ToHtmlString());
                        }
                        links.Append("</li>");
                    }
                }
                if (model.ShowPrevious)
                {
                    //previous page
                    if (model.PageIndex > 0)
                    {
                        model.RouteValues.pageNumber = (model.PageIndex);

                        links.Append("<li class=\"previous-page\">");
                        if (model.UseRouteLinks)
                        {
                            IHtmlContent link = html.RouteLink(model.PreviousButtonText, model.RouteActionName, model.RouteValues, new { title = "Önceki Sayfa" });
                            links.Append(link.ToHtmlString());
                        }
                        else
                        {
                            IHtmlContent link = html.ActionLink(model.PreviousButtonText, model.RouteActionName, model.RouteValues, new { title = "Önceki Sayfa" });
                            links.Append(link.ToHtmlString());
                        }
                        links.Append("</li>");
                    }
                }
                if (model.ShowIndividualPages)
                {
                    //individual pages
                    int firstIndividualPageIndex = model.GetFirstIndividualPageIndex();
                    int lastIndividualPageIndex = model.GetLastIndividualPageIndex();
                    for (int i = firstIndividualPageIndex; i <= lastIndividualPageIndex; i++)
                    {
                        if (model.PageIndex == i)
                        {
                            links.AppendFormat("<li class=\"current-page\"><span>{0}</span></li>", (i + 1));
                        }
                        else
                        {
                            model.RouteValues.pageNumber = (i + 1);

                            links.Append("<li class=\"individual-page\">");
                            if (model.UseRouteLinks)
                            {
                                IHtmlContent link = html.RouteLink((i + 1).ToString(), model.RouteActionName, model.RouteValues, new
                                {
                                    title = $"Sayfa {(i + 1)}"
                                });
                                links.Append(link.ToHtmlString());
                            }
                            else
                            {
                                IHtmlContent link = html.ActionLink((i + 1).ToString(), model.RouteActionName, model.RouteValues, new { title = string.Format("Sayfa{0}", (i + 1)) });
                                links.Append(link.ToHtmlString());
                            }
                            links.Append("</li>");
                        }
                    }
                }
                if (model.ShowNext)
                {
                    //next page
                    if ((model.PageIndex + 1) < model.TotalPages)
                    {
                        model.RouteValues.pageNumber = (model.PageIndex + 2);

                        links.Append("<li class=\"next-page\">");
                        if (model.UseRouteLinks)
                        {
                            IHtmlContent link = html.RouteLink(model.NextButtonText, model.RouteActionName, model.RouteValues, new { title = "Sonraki Sayfa" });
                            links.Append(link.ToHtmlString());
                        }
                        else
                        {
                            IHtmlContent link = html.ActionLink(model.NextButtonText, model.RouteActionName, model.RouteValues, new { title = "Sonraki Sayfa" });
                            links.Append(link.ToHtmlString());
                        }
                        links.Append("</li>");
                    }
                }
                if (model.ShowLast)
                {
                    //last page
                    if (((model.PageIndex + 3) < model.TotalPages) && (model.TotalPages > model.IndividualPagesDisplayedCount))
                    {
                        model.RouteValues.pageNumber = model.TotalPages;

                        links.Append("<li class=\"last-page\">");
                        if (model.UseRouteLinks)
                        {
                            IHtmlContent link = html.RouteLink(model.LastButtonText, model.RouteActionName, model.RouteValues, new { title = "Son Sayfa" });
                            links.Append(link.ToHtmlString());
                        }
                        else
                        {
                            IHtmlContent link = html.ActionLink(model.LastButtonText, model.RouteActionName, model.RouteValues, new { title = "Son Sayfa" });
                            links.Append(link.ToHtmlString());
                        }
                        links.Append("</li>");
                    }
                }
            }
            string result = links.ToString();
            if (!string.IsNullOrEmpty(result))
            {
                result = "<ul>" + result + "</ul>";
            }
            return new HtmlString(result);
        }


        public static Pager Pager(this IHtmlHelper helper, IPageableModel pagination)
        {
            return new Pager(pagination, helper.ViewContext);
        }

        /// <summary>
        /// Get topic SEO name
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="html">HTML helper</param>
        /// <param name="systemName">System name</param>
        /// <returns>Topic SEO Name</returns>
        public static string GetTopicSeName<T>(this IHtmlHelper<T> html, string systemName)
        {
            IStoreContext storeContext = ServiceProviderFactory.ServiceProvider.GetRequiredService<IStoreContext>();
            ITopicService topicService = ServiceProviderFactory.ServiceProvider.GetRequiredService<ITopicService>();
            var topic = topicService.GetTopicBySystemName(systemName, storeContext.CurrentStore.Id);
            if (topic == null)
            {
                return "";
            }

            IUrlRecordService urlRecordService = ServiceProviderFactory.ServiceProvider.GetRequiredService<IUrlRecordService>();
            return urlRecordService.GetSeName(topic);
        }

        /// <summary>
        /// Get topic title
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="html">HTML helper</param>
        /// <param name="systemName">System name</param>
        /// <returns>Topic SEO Name</returns>
        public static string GetTopicTitle<T>(this IHtmlHelper<T> html, string systemName)
        {
            IStoreContext storeContext = ServiceProviderFactory.ServiceProvider.GetRequiredService<IStoreContext>();
            ITopicService topicService = ServiceProviderFactory.ServiceProvider.GetRequiredService<ITopicService>();
            var topic = topicService.GetTopicBySystemName(systemName, storeContext.CurrentStore.Id);
            if (topic == null)
            {
                return "";
            }

            return topic.Title;

        }
    }
}
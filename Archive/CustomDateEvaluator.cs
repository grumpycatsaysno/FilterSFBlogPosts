using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Web;
using Telerik.Sitefinity.Web.UrlEvaluation;

namespace SFBlog.CustomControls.Archive
{
    public class CustomDateEvaluator : DateEvaluator
    {
        public override string BuildUrl(DateTime date, DateBuildOptions options, UrlEvaluationMode urlEvaluationMode, string urlKeyPrefix)
        {
            var qString = QueryStringBuilder.Current.Reset();

            if (urlEvaluationMode == UrlEvaluationMode.QueryString)
            {
                var yearFullUrlKey = String.Concat(urlKeyPrefix, "year");
                var monthFullUrlKey = String.Concat(urlKeyPrefix, "month");
                var dayFullUrlKey = String.Concat(urlKeyPrefix, "day");

                qString.Add(yearFullUrlKey, date.Year.ToString(), true);
                if (options != DateBuildOptions.Year)
                {
                    qString.Add(monthFullUrlKey, date.Month.ToString("00"), true);
                    if (options == DateBuildOptions.YearMonthDay)
                    {
                        qString.Add(dayFullUrlKey, date.Day.ToString("00"), true);
                    }
                }
            }
            return qString.ToString();          
        }
    }
}
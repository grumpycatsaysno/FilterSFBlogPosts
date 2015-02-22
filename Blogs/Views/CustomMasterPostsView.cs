using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Sitefinity.Data.Linq.Dynamic;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.Web.UI;
using Telerik.Sitefinity.Web.UI.ContentUI.Contracts;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Blogs.Model;
using Telerik.Sitefinity.Web.UI.Fields;
using Telerik.Sitefinity.Modules.Blogs;
using Telerik.Sitefinity.Data.Metadata;
using Telerik.Sitefinity.Metadata.Model;
using ENDFund2013.CustomControls.Archive;
using System.Collections.Specialized;
using Telerik.Sitefinity.Modules.News.Web.UI;

namespace SFBlog.CustomControls.Blogs.Views
{
    public class CustomMasterPostsView : Telerik.Sitefinity.Modules.Blogs.Web.UI.Public.MasterPostsView
    {
        #region Methods

        protected override void InitializeListView(IQueryable<BlogPost> query, int? totalCount)
        {
            string sYear = string.Empty;
            string sMonth = string.Empty;
            string sDay = string.Empty;

            sYear = System.Web.HttpContext.Current.Request.QueryString["year"];
            sMonth = System.Web.HttpContext.Current.Request.QueryString["month"];

            if (!string.IsNullOrEmpty(sYear) || !string.IsNullOrEmpty(sMonth))
            {
                query = this.FilterByCustomDateTimeCriteria(query, sYear, sMonth, ref totalCount);
            }

            base.InitializeListView(query, totalCount);
        }

        /// <summary>
        /// Filters the Blog Posts by custom filter built from query string parameters passed to the url.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="sYear">The s year.</param>
        /// <param name="sMonth">The s month.</param>
        /// <param name="totalCount">The total count.</param>
        /// <returns></returns>
        private IQueryable<BlogPost> FilterByCustomDateTimeCriteria(IQueryable<BlogPost> query, string sYear, string sMonth, ref int? totalCount)
        {
            var values = new List<object>();
            var filterValues = new List<object>();

            var years = sYear.Split(',');
            var months = sMonth.Split(',');

            //if the url does not contain a pair of month and year the query will not be filtered
            if (years.Count() != months.Count())
            {
                return query;
            }

            string filter = null;

            var j = 0;
            var k = 1;
            for (int i = 0; i < years.Count(); i++)
            {
                if (i > 0)
                {
                    filter += "||";
                    ++k;
                    j = k;
                    ++k;
                }

                filter += this.SetDates(years[i], months[i], null, out values, "PublicationDate", j, k);
                filterValues.AddRange(values);
            }

            if (filter == null)
            {
                return query;
            }

            query = query.Where(filter, filterValues.ToArray());

            totalCount = query.Count();
            return query;
        }

        /// <summary>
        /// Constructs a filter query with the start and end period by month and year passed as parameters.
        /// </summary>
        /// <param name="sYear">The s year.</param>
        /// <param name="sMonth">The s month.</param>
        /// <param name="sDay">The s day.</param>
        /// <param name="values">The values.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private string SetDates(string sYear, string sMonth, string sDay, out System.Collections.Generic.List<object> values, string propertyName, int index1, int index2)
        {
            System.DateTime time;
            System.DateTime time2;
            int num = int.Parse(sYear);
            if (string.IsNullOrEmpty(sMonth))
            {
                time = new System.DateTime(num, 1, 1, 0, 0, 0);
                time2 = new System.DateTime(num, 12, System.DateTime.DaysInMonth(num, 12), 0x17, 0x3b, 0x3b);
            }
            else if (string.IsNullOrEmpty(sDay))
            {
                int num2 = int.Parse(sMonth);
                time = new System.DateTime(num, num2, 1, 0, 0, 0);
                time2 = new System.DateTime(num, num2, System.DateTime.DaysInMonth(num, num2), 0x17, 0x3b, 0x3b);
            }
            else
            {
                int num3 = int.Parse(sMonth);
                int num4 = int.Parse(sDay);
                time = new System.DateTime(num, num3, num4, 0, 0, 0);
                time2 = new System.DateTime(num, num3, num4, 0x17, 0x3b, 0x3b);
            }
            values = new System.Collections.Generic.List<object>();

            values.Add(time);
            values.Add(time2);
            return new System.Text.StringBuilder().Append("(").Append(propertyName).Append(" >= @" + index1 + " && ").Append(propertyName).Append(" <= @" + index2 + ")").ToString();
        }

        protected override IQueryable<BlogPost> GetItemsList(ref int? totalCount)
        {
            //this setting prevents the inner logic to read the query string from the url and perform additional filter
            //as by default the url can contain only one pair of year and month
            this.AllowUrlQueries = false;
            return base.GetItemsList(ref totalCount);
        }

        protected override void OnPreRender(EventArgs e)
        {
            int? totalCount = 0;
            IQueryable<BlogPost> query = this.GetItemsList(ref totalCount);
            this.InitializeListView(query, totalCount);
            base.OnPreRender(e);
        }

        #endregion
    }
}
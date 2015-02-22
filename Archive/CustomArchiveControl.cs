using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Sitefinity.Blogs.Model;
using Telerik.Sitefinity.Modules;
using Telerik.Sitefinity.Modules.Blogs;
using Telerik.Sitefinity.Modules.GenericContent;
using Telerik.Sitefinity.Modules.GenericContent.Archive;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Web;
using Telerik.Sitefinity.Web.UI;
using Telerik.Sitefinity.Web.UI.PublicControls;
using Telerik.Sitefinity.Web.UrlEvaluation;
using Telerik.Web.UI;

namespace SFBlog.CustomControls.Archive
{
    public class CustomArchiveControl : ArchiveControl
    {
        public override string LayoutTemplatePath
        {
            get
            {
                return this.layoutTemplatePath;
            }
            set
            {
                this.layoutTemplatePath = value;
            }
        }

        public virtual CheckBox ArchiveFilter
        {
            get
            {
                return this.Container.GetControl<CheckBox>("archiveFilter", false);
            }
        }

        public virtual Button FilterButton
        {
            get
            {
                return this.Container.GetControl<Button>("filterButton", false);
            }
        }

        protected override void InitializeControls(GenericContainer container)
        {
            if (!this.IsDesignMode())
            {
                this.FilterButton.Click += FilterButton_Click;
                this.ArchiveRepeater.PreRender += ArchiveRepeater_PreRender;
            }
            base.InitializeControls(container);
        }

        protected void FilterButton_Click(object sender, EventArgs e)
        {
            var repeaterItems = this.ArchiveRepeater.Items;
            var archiveDates = new List<DateTime>();

            //cache is invalidated
            ObjectCache.CheckboxesCache.Clear();

            foreach (RepeaterItem item in repeaterItems)
            {
                CheckBox checkbox = item.FindControl("archiveFilter") as CheckBox;
                if (checkbox.Checked)
                {
                    DateTime dateToFilter = DateTime.Now;
                    DateTime.TryParse(checkbox.Text, out dateToFilter);
                    archiveDates.Add(dateToFilter);

                    //need to preserve the checked state of checkboxes after page navigation
                    //for that purpose an object cache is substituted
                    ObjectCache.CheckboxesCache.Add(checkbox.Text);
                }
            }
            if (archiveDates.Count > 0)
            {
                var urlToNavigate = this.ResolveUrl(archiveDates.ToArray());
                SystemManager.CurrentHttpContext.Response.Redirect(urlToNavigate);
            }
            else
            {
                SystemManager.CurrentHttpContext.Response.Redirect(this.GetBaseUrl());
            }
        }

        protected override void ArchiveRepeater_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ArchiveItem archive = e.Item.DataItem as ArchiveItem;
                CheckBox checkbox = e.Item.FindControl("archiveFilter") as CheckBox;

                if (checkbox != null)
                {
                    checkbox.Text = ResolveDisplayText(archive.Date);

                    if (this.ShowItemCount)
                    {
                        checkbox.Text += string.Format("  <span class='sfCount'>({0})</span>", archive.ItemsCount);
                    }
                }
            }
        }

        protected void ArchiveRepeater_PreRender(object sender, EventArgs e)
        {
            var repeater = sender as Repeater;
            foreach (RepeaterItem item in repeater.Items)
            {
                CheckBox checkbox = item.FindControl("archiveFilter") as CheckBox;
                if (ObjectCache.CheckboxesCache.Contains(checkbox.Text))
                {
                    checkbox.Checked = true;
                }
            }
        }

        private string GetBaseUrl()
        {
            var url = this.BaseUrl;

            if (string.IsNullOrEmpty(url))
            {
                var siteMap = SiteMapBase.GetCurrentProvider();
                if (siteMap == null || (siteMap != null && siteMap.CurrentNode == null))
                {
                    return string.Empty;
                }

                url = siteMap.CurrentNode.Url;
            }

            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("BaseUrl property could not be resolved.");

            if (VirtualPathUtility.IsAppRelative(url))
                url = VirtualPathUtility.ToAbsolute(url);
            return url;
        }

        protected string ResolveUrl(params DateTime[] archiveDates)
        {
            var url = this.GetBaseUrl();
            var evaluator = new CustomDateEvaluator();
            StringBuilder evaluatedResult = new StringBuilder();
            string urlToEvaluate = null;

            for (int i = 0; i < archiveDates.Length; i++)
            {
                urlToEvaluate = evaluator.BuildUrl(archiveDates[i], this.DateBuildOptions, this.GetUrlEvaluationMode(), this.UrlKeyPrefix);
                if (i > 0)
                {
                    urlToEvaluate = urlToEvaluate.Replace('?', '&');
                }
                evaluatedResult.Append(urlToEvaluate);
            }

           return string.Concat(url, evaluatedResult.ToString());
        }

        #region Private fields and constants

        private const string selectedCssClass = "sfSel";
        private string layoutTemplatePath = "~/CustomControls/Archive/CustomArchiveTemplate.ascx";

        #endregion
    }
}
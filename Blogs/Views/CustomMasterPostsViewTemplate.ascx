<%@ Control Language="C#" %>
<%@ Register TagPrefix="sf" Namespace="Telerik.Sitefinity.Web.UI.ContentUI" Assembly="Telerik.Sitefinity" %>
<%@ Register TagPrefix="sf" Namespace="Telerik.Sitefinity.Web.UI.Comments" Assembly="Telerik.Sitefinity" %>
<%@ Register TagPrefix="sf" Namespace="Telerik.Sitefinity.Web.UI" Assembly="Telerik.Sitefinity" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="sf" Namespace="Telerik.Sitefinity.Web.UI.PublicControls.BrowseAndEdit" Assembly="Telerik.Sitefinity" %>

<script type="text/javascript">
    $(document).ready(function () {

        $('body').on('click', '.blog-filters .filterTrigger', function (e) {
            $(this).parent().parent().addClass('filter-opened');
            $('.blog-filters .filter-values').hide();
            console.log($(this));
            $(this).siblings().show();
        });
        $('body').on('click', function (e) {
            if ($('.filter-opened').length) {
                if (!$(e.target).closest('input.filterTrigger').length && !($(e.target).parents('.filter-values').length)) {
                    $('.blog-filters .filter-values').hide();
                    $('.blog-filters.filter-opened').removeClass('filter-opened');
                }
            };
        });

        $('div.blog-item-tbn img').each(function () {
            if ($(this).attr('src') == '') {
                $(this).parent().parent().addClass("no-image");
                $(this).remove();
            }
        });
    });
</script>

<div class="blog-filters-wrap">
    <div class="blog-filters">
        <span>
            <asp:Button runat="server" ID="authorFilterBtn" Text="Author" OnClientClick="return false;" ClientIDMode="Static" class="filterTrigger" />
            <div class="filter-values">
                <asp:CheckBoxList runat="server" ID="authorsList"></asp:CheckBoxList>
            </div>
        </span>
        <span>
            <asp:Button runat="server" ID="topicFilterBtn" Text="Topic" OnClientClick="return false;" ClientIDMode="Static" class="filterTrigger" />
            <div class="filter-values">
                <asp:CheckBoxList runat="server" ID="topicsList"></asp:CheckBoxList>
            </div>
        </span>

        <span>
            <asp:Button runat="server" ID="diseaseFilterBtn" Text="Disease" OnClientClick="return false;" ClientIDMode="Static" class="filterTrigger" />
            <div class="filter-values">
                <asp:CheckBoxList runat="server" ID="diseasesList"></asp:CheckBoxList>
            </div>
        </span>
    </div>
    <asp:Button runat="server" ID="applyFilterBtn" Text="Apply Filter" class="filter-apply-btn" />
</div>


<ul class="blog-list">
    <telerik:RadListView ID="Repeater" ItemPlaceholderID="ItemsContainer" runat="server" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false">
        <LayoutTemplate>
            <sf:ContentBrowseAndEditToolbar ID="MainBrowseAndEditToolbar" runat="server" Mode="Add"></sf:ContentBrowseAndEditToolbar>
            <asp:PlaceHolder ID="ItemsContainer" runat="server" />
        </LayoutTemplate>
        <ItemTemplate>
            <li>
                <div class="blog-item-tbn">
                    <img src='<%# Eval("Image") != null && Eval("Image") != "" ? new Uri(Eval("Image").ToString()).PathAndQuery : ""  %>' />
                </div>
                <div class="blog-item-info">
                    <h4>
                        <sf:DetailsViewHyperLink ID="DetailsViewHyperLink1" TextDataField="Title" ToolTipDataField="Description" runat="server" />
                    </h4>
                    <span class="date">
                        <sf:FieldListView ID="PublicationDate" runat="server" Format="{PublicationDate.ToLocal():MMM dd, yyyy}" />
                    </span>
                    <div class="blog-item-summary">
                        <sf:FieldListView ID="summary" runat="server" Text="{0}" Properties="Summary" />
                    </div>
                    <span class="author">
                        <sitefinity:TextField ID="TextField1" runat="server" DisplayMode="Read" Value='<%# Eval("Author")%>' />
                    </span>
                    <sf:ContentBrowseAndEditToolbar ID="BrowseAndEditToolbar" runat="server" Mode="Edit,Delete,Unpublish"></sf:ContentBrowseAndEditToolbar>
                </div>
            </li>

        </ItemTemplate>
    </telerik:RadListView>
</ul>

<sf:Pager ID="pager" runat="server"></sf:Pager>

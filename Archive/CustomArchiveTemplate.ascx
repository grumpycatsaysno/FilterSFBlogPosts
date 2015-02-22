<%@ Control Language="C#" %>
<%@ Register TagPrefix="sitefinity" Assembly="Telerik.Sitefinity" Namespace="Telerik.Sitefinity.Web.UI" %>

<asp:PlaceHolder ID="plhTitle" runat="server" Visible="false">
    <h2 class="sfarchiveTitle">
        <asp:Literal ID="titleLabel" runat="server" />
    </h2>
</asp:PlaceHolder>

<script type="text/javascript">

    function pageLoad() {
        $('#archiveFilterBtn').click(function () {
            $('[id*="repeaterContainer"]').show();
        });
    };
</script>

<asp:Button ID="archiveFilterBtn" runat="server" Text="Archive" OnClientClick="return false;" ClientIDMode="Static" />
<div id="repeaterContainer" style="display: none;" class="archive-filter">
    <asp:Repeater ID="rptArchive" runat="server">
        <HeaderTemplate>
            <ul class="sfarchiveList">
        </HeaderTemplate>
        <ItemTemplate>
            <li class="sfarchiveListItem">
                <sitefinity:SitefinityHyperLink ID="linkArchive" runat="server" CssClass="selectCommand sfarchiveLink" Visible="false"></sitefinity:SitefinityHyperLink>
                <asp:CheckBox ID="archiveFilter" runat="server" />
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
    <a class="show-more">Show more...</a>
    <asp:Button ID="filterButton" runat="server" Text="Filter By Date" />
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QuickCheckin.ascx.cs" Inherits="Plugins_org_hope_QuickCheckin" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>
        <h2 id="labelAttendance" runat="server" />
        <div class="grid grid-panel">
            <Rock:Grid ID="gridGroupsList" runat="server" DisplayType="Full" RowItemText="Group" AllowSorting="true" OnRowSelected="gridGroupsList_Selected" DataKeyNames="ID">
                <Columns>
                    <Rock:RockBoundField DataField="Name" HeaderText="Name" SortExpression="Name"/>
		            <Rock:RockBoundField DataField="GroupType" HeaderText="Group Type" SortExpression="GroupType"/>
               </Columns>
            </Rock:Grid>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

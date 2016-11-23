using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Plugins_org_hope_QuickCheckin : RockBlock
{
    private class GroupMet
    {
        public Group MetGroup { get; set; }
        public List<ScheduleOccurrence> MetSchedule { get; set; }

        public GroupMet(Group metGroup, List<ScheduleOccurrence> metSched)
        {
            MetGroup = metGroup;
            MetSchedule = metSched;
        }
    }

    private List<GroupMet> MetGroups = new List<GroupMet>();

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (!Page.IsPostBack)
        {
            using (RockContext context = new RockContext())
            {
                var userGroups = new GroupService(context).Queryable("GroupType,Schedule").AsNoTracking()
                    .Where(grp => grp.Members.Count > 1 && grp.Members
                        .Any(p => p.PersonId == this.CurrentPersonId && p.GroupRole.IsLeader)).ToList();

                gridGroupsList.DataSource = userGroups.Select(x => new
                {
                    Name = x.Name,
                    ID = x.Id,
                    GroupType = x.GroupType
                }).ToList();

                gridGroupsList.DataBind();
            }

        }
    }

    protected void gridGroupsList_Selected(object sender, RowEventArgs e)
    {
        int groupID = (int)e.RowKeyValues["ID"];
        Response.Redirect(string.Format("~/page/363?GroupId={0}", groupID), false);

        Context.ApplicationInstance.CompleteRequest();
        return;
    }
}
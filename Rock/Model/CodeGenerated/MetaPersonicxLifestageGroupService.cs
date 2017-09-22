//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// MetaPersonicxLifestageGroup Service class
    /// </summary>
    public partial class MetaPersonicxLifestageGroupService : Service<MetaPersonicxLifestageGroup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetaPersonicxLifestageGroupService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public MetaPersonicxLifestageGroupService(RockContext context) : base(context)
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( MetaPersonicxLifestageGroup item, out string errorMessage )
        {
            errorMessage = string.Empty;
 
            if ( new Service<MetaPersonicxLifestageCluster>( Context ).Queryable().Any( a => a.MetaPersonicxLifestyleGroupId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", MetaPersonicxLifestageGroup.FriendlyTypeName, MetaPersonicxLifestageCluster.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<Person>( Context ).Queryable().Any( a => a.MetaPersonicxLifestageGroupId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", MetaPersonicxLifestageGroup.FriendlyTypeName, Person.FriendlyTypeName );
                return false;
            }  
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class MetaPersonicxLifestageGroupExtensionMethods
    {
        /// <summary>
        /// Clones this MetaPersonicxLifestageGroup object to a new MetaPersonicxLifestageGroup object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static MetaPersonicxLifestageGroup Clone( this MetaPersonicxLifestageGroup source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as MetaPersonicxLifestageGroup;
            }
            else
            {
                var target = new MetaPersonicxLifestageGroup();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another MetaPersonicxLifestageGroup object to this MetaPersonicxLifestageGroup object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this MetaPersonicxLifestageGroup target, MetaPersonicxLifestageGroup source )
        {
            target.Id = source.Id;
            target.Children = source.Children;
            target.Description = source.Description;
            target.DetailsUrl = source.DetailsUrl;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.HomeOwnership = source.HomeOwnership;
            target.Income = source.Income;
            target.IncomeLevel = source.IncomeLevel;
            target.IncomeRank = source.IncomeRank;
            target.LifeStage = source.LifeStage;
            target.LifestyleGroupCode = source.LifestyleGroupCode;
            target.LifestyleGroupName = source.LifestyleGroupName;
            target.MaritalStatus = source.MaritalStatus;
            target.NetWorth = source.NetWorth;
            target.NetWorthLevel = source.NetWorthLevel;
            target.NetworthRank = source.NetworthRank;
            target.OrganizationHouseholdCount = source.OrganizationHouseholdCount;
            target.OrganizationIndividualCount = source.OrganizationIndividualCount;
            target.PercentOrganization = source.PercentOrganization;
            target.PercentUS = source.PercentUS;
            target.Summary = source.Summary;
            target.Urbanicity = source.Urbanicity;
            target.UrbanicityRank = source.UrbanicityRank;
            target.CreatedDateTime = source.CreatedDateTime;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            target.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }
    }
}

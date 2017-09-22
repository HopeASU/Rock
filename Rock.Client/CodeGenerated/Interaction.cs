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
using System.Collections.Generic;


namespace Rock.Client
{
    /// <summary>
    /// Base client model for Interaction that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class InteractionEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public int? EntityId { get; set; }

        /// <summary />
        public Guid? ForeignGuid { get; set; }

        /// <summary />
        public string ForeignKey { get; set; }

        /// <summary />
        public int InteractionComponentId { get; set; }

        /// <summary />
        public string InteractionData { get; set; }

        /// <summary />
        public DateTime InteractionDateTime { get; set; }

        /// <summary />
        public int? InteractionSessionId { get; set; }

        /// <summary />
        public string InteractionSummary { get; set; }

        /// <summary>
        /// If the ModifiedByPersonAliasId is being set manually and should not be overwritten with current user when saved, set this value to true
        /// </summary>
        public bool ModifiedAuditValuesAlreadyUpdated { get; set; }

        /// <summary />
        public string Operation { get; set; }

        /// <summary />
        public int? PersonalDeviceId { get; set; }

        /// <summary />
        public int? PersonAliasId { get; set; }

        /// <summary>
        /// Leave this as NULL to let Rock set this
        /// </summary>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// This does not need to be set or changed. Rock will always set this to the current date/time when saved to the database.
        /// </summary>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Leave this as NULL to let Rock set this
        /// </summary>
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary>
        /// If you need to set this manually, set ModifiedAuditValuesAlreadyUpdated=True to prevent Rock from setting it
        /// </summary>
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public int? ForeignId { get; set; }

        /// <summary>
        /// Copies the base properties from a source Interaction object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( Interaction source )
        {
            this.Id = source.Id;
            this.EntityId = source.EntityId;
            this.ForeignGuid = source.ForeignGuid;
            this.ForeignKey = source.ForeignKey;
            this.InteractionComponentId = source.InteractionComponentId;
            this.InteractionData = source.InteractionData;
            this.InteractionDateTime = source.InteractionDateTime;
            this.InteractionSessionId = source.InteractionSessionId;
            this.InteractionSummary = source.InteractionSummary;
            this.ModifiedAuditValuesAlreadyUpdated = source.ModifiedAuditValuesAlreadyUpdated;
            this.Operation = source.Operation;
            this.PersonalDeviceId = source.PersonalDeviceId;
            this.PersonAliasId = source.PersonAliasId;
            this.CreatedDateTime = source.CreatedDateTime;
            this.ModifiedDateTime = source.ModifiedDateTime;
            this.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            this.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;

        }
    }

    /// <summary>
    /// Client model for Interaction that includes all the fields that are available for GETs. Use this for GETs (use InteractionEntity for POST/PUTs)
    /// </summary>
    public partial class Interaction : InteractionEntity
    {
        /// <summary />
        public InteractionComponent InteractionComponent { get; set; }

        /// <summary />
        public InteractionSession InteractionSession { get; set; }

        /// <summary />
        public PersonAlias PersonAlias { get; set; }

        /// <summary>
        /// NOTE: Attributes are only populated when ?loadAttributes is specified. Options for loadAttributes are true, false, 'simple', 'expanded' 
        /// </summary>
        public Dictionary<string, Rock.Client.Attribute> Attributes { get; set; }

        /// <summary>
        /// NOTE: AttributeValues are only populated when ?loadAttributes is specified. Options for loadAttributes are true, false, 'simple', 'expanded' 
        /// </summary>
        public Dictionary<string, Rock.Client.AttributeValue> AttributeValues { get; set; }
    }
}

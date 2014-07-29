﻿// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
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
using System.Linq;

using Rock.Data;
using Rock.Web.Cache;

namespace Rock.Model
{
    /// <summary>
    /// Service/Data access class for <see cref="Rock.Model.FinancialBatch"/> entity objects.
    /// </summary>
    public partial class FinancialBatchService
    {
        public FinancialBatch Get( string namePrefix, DefinedValueCache currencyType, DefinedValueCache creditCardType,
            DateTime transactionDate, TimeSpan batchTimeOffset, List<FinancialBatch> batches = null )
        {
            // Use the credit card type's batch name suffix, or if that doesn't exist, use the currency type value
            string ccSuffix = string.Empty;
            
            if (creditCardType != null )
            {
                ccSuffix = creditCardType.GetAttributeValue( "BatchNameSuffix" );
                if ( string.IsNullOrWhiteSpace( ccSuffix ) )
                {
                    ccSuffix = creditCardType.Name;
                }
            }

            if ( string.IsNullOrWhiteSpace( ccSuffix ) && currencyType != null )
            {
                ccSuffix = currencyType.Name;
            }

            string batchName = namePrefix.Trim() + ( string.IsNullOrWhiteSpace( ccSuffix ) ? "" : " " + ccSuffix );

            FinancialBatch batch = null;

            // If a list of batches was passed, search those first
            if ( batches != null )
            {
                batch = batches
                    .Where( b =>
                        b.Status == BatchStatus.Open &&
                        b.BatchStartDateTime <= transactionDate &&
                        b.BatchEndDateTime > transactionDate &&
                        b.Name == batchName )
                    .OrderByDescending( b => b.BatchStartDateTime )
                    .FirstOrDefault();

                if ( batch != null )
                {
                    return batch;
                }
            }

            // If batch was not found in existing list, search database
            batch = Queryable()
                .Where( b =>
                    b.Status == BatchStatus.Open &&
                    b.BatchStartDateTime <= transactionDate &&
                    b.BatchEndDateTime > transactionDate &&
                    b.Name == batchName )
                .OrderByDescending( b => b.BatchStartDateTime )
                .FirstOrDefault();

            // If still no batch, create a new one
            if ( batch == null )
            {
                batch = new FinancialBatch();
                batch.Guid = Guid.NewGuid();
                batch.Name = batchName;
                batch.Status = BatchStatus.Open;
                batch.BatchStartDateTime = transactionDate.Date.Add( batchTimeOffset );
                if ( batch.BatchStartDateTime > transactionDate )
                {
                    batch.BatchStartDateTime.Value.AddDays( -1 );
                }

                batch.BatchEndDateTime = batch.BatchStartDateTime.Value.AddDays( 1 );
                batch.ControlAmount = 0;
                Add( batch );
            }

            // Add the batch to the list
            if ( batches != null )
            {
                batches.Add( batch );
            }

            return batch;
        }
    }
}
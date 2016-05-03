﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Newtonsoft.Json;
using Rock.Attribute;
using Rock.UniversalSearch.IndexModels;

namespace Rock.UniversalSearch.IndexComponents
{
    [Description( "ElasticSearch Universal Search Index" )]
    [Export( typeof( IndexComponent ) )]
    [ExportMetadata( "ComponentName", "ElasticSearch" )]

    [TextField( "Node URL", "The URL of the ElasticSearch node (http://myserver:9200)", true, key: "NodeUrl" )]
    public class ElasticSearch : IndexComponent
    {
        private static string _indexName = "rock-globalcatalog";
        private ElasticLowLevelClient _client;

        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public override bool IsConnected
        {
            get
            {
                if ( _client != null )
                {
                    var results = _client.ClusterState<object>();

                    if (results != null )
                    {
                        return results.Success;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the index location.
        /// </summary>
        /// <value>
        /// The index location.
        /// </value>
        public override string IndexLocation
        {
            get
            {
                return GetAttributeValue( "NodeUrl" );
            }
        }

        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        /// <value>
        /// The name of the index.
        /// </value>
        public override string IndexName
        {
            get
            {
                return _indexName;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticSearch" /> class.
        /// </summary>
        public ElasticSearch()
        {
            var node = new Uri( GetAttributeValue( "NodeUrl" ) );
            var config = new ConnectionConfiguration( node );
            _client = new ElasticLowLevelClient( config );
        }

        /// <summary>
        /// Indexes the document.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="document">The document.</param>
        public override void IndexDocument( string typeName, IndexModelBase document )
        {
            string documentJson = JsonConvert.SerializeObject( document );
            _client.IndexAsync<object>( _indexName, typeName, document.Id.ToString(), documentJson );
        }

        public void DeleteDocumentsByType(string typeName )
        {
            //_client.DeleteByQuery(_indexName, )
        }

    }
}


// forbidden characters in field names _ . , #

// cluster state: http://localhost:9200/_cluster/state?filter_nodes=false&filter_metadata=true&filter_routing_table=true&filter_blocks=true&filter_indices=true
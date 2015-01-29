using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using DynamoDbDemo.Entities;


namespace DynamoDbDemo
{
    public class DvdMaker
    {
        private readonly AmazonDynamoDBClient _client;
        public DvdMaker()
        {
            _client = new AmazonDynamoDBClient();

        }

        public List<DVD> Dvds
        {
            get
            {
                return new List<DVD>
                {
                    new DVD
                    {
                        Title = "The Godfather",
                        ReleaseYear = 1972 ,
                        ActorNames = new List<string>
                        {
                            "Marlon Brando",
                            "Al Pacino",
                            "James Caan"

                        },
                        Director = "Frank Darabont",
                        Producer = "Robert Evans"

                    },

                    new DVD
                    {
                        Title = "The Dark Knight",
                        ReleaseYear = 2008,
                        ActorNames = new List<string>
                        {
                            "Christian Bale",
                            "Heath Ledger",
                            "Aaron Eckhart"
                            
                        },
                        Director = "Christopher Nolan",
                        Producer = "Thomas Tull"

                    },
                    new DVD
                    {
                        Title = "Fight Club",
                        ReleaseYear = 1999,
                        ActorNames = new List<string>
                        {
                            "Edward Norton",
                            "Brad Pitt",
                            "Helena Carter"
                        },
                        Director = "Robert Zemeckis",
                        Producer = "Arnon Milchan"

                    },
                   
                };
            }
        }
        public void Init()
        {
            List<string> currentTables = _client.ListTables().TableNames;
            if (!currentTables.Contains("DVD"))
            {
                var createTableRequest = new CreateTableRequest
                {
                    TableName = "DVD",
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 1,
                        WriteCapacityUnits = 1
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "Title",
                            KeyType = "HASH"
                        },
                        new KeySchemaElement
                        {
                            AttributeName = "ReleaseYear",
                            KeyType = "RANGE"
                        },
                    },
                    AttributeDefinitions = new List<AttributeDefinition>()
                    {
                        new AttributeDefinition()
                        {
                            AttributeName = "Title",AttributeType = "S"
                        },
                        new AttributeDefinition()
                        {
                            AttributeName = "ReleaseYear",AttributeType = "N"
                        }
                    }
                };

                CreateTableResponse createTableResponse = _client.CreateTable(createTableRequest);

                while (createTableResponse.TableDescription.TableStatus != "ACTIVE")
                {
                    System.Threading.Thread.Sleep(5000);
                }
            }

        }
    }
}
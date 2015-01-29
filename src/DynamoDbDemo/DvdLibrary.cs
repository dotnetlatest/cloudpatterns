using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using CloudPatterns.AWS.DynamoDb;
using DynamoDbDemo.Entities;

namespace DynamoDbDemo
{
    public class DvdLibrary
    {
        private readonly DynamoService _dynamoService;

        public DvdLibrary()
        {
            _dynamoService = new DynamoService();
        }

        /// <summary>
        ///  AddDVD will accept a DVD object and creates an Item on Amazon DynamoDB
        /// </summary>
        /// <param name="dvd"></param>
        public void AddDvd(DVD dvd)
        {
            _dynamoService.Store(dvd);
        }

        /// <summary>
        /// ModifyDVD  tries to load an existing DVD, modifies and saves it back. If the Item doesn’t exist, it raises an exception
        /// </summary>
        /// <param name="dvd"></param>
        public void ModifyDvd(DVD dvd)
        {
            _dynamoService.UpdateItem(dvd);
        }

        /// <summary>
        /// GetALllDvds will perform a Table Scan operation to return all the DVDs
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DVD> GetAllDvds()
        {
            return _dynamoService.GetAll<DVD>();
        }

        public IEnumerable<DVD> SearchDvds(string title, int releaseYear)
        {
            IEnumerable<DVD> filteredDvds = _dynamoService.DbContext.Query<DVD>(title, QueryOperator.Equal, releaseYear);

            return filteredDvds;
        }

        
        /// <summary>
        /// Delete DVD will remove an item from DynamoDb
        /// </summary>
        /// <param name="dvd"></param>
        public void DeleteDvd(DVD dvd)
        {
            _dynamoService.DeleteItem(dvd);
        }

        #region TODO
        //public List<DVD> SearchDvdByTitle(string title)
        //{
        //    // Define item hash-key
        //    var hashKey = new AttributeValue { S = title };

        //    // Create the key conditions from hashKey
        //    var keyConditions = new Dictionary<string, Condition>
        //    {
        //        // Hash key condition. ComparisonOperator must be "EQ".
        //        { 
        //            "Title",
        //            new Condition
        //            {
        //                ComparisonOperator = "EQ",
        //                AttributeValueList = new List<AttributeValue> { hashKey }
        //            }
        //        }
        //    };

        //    // Define marker variable
        //    Dictionary<string, AttributeValue> startKey = null;

        //    do
        //    {
        //        // Create Query request
        //        var request = new QueryRequest
        //        {
        //            TableName = "DVD",
        //            ExclusiveStartKey = startKey,
        //            KeyConditions = keyConditions
        //        };

        //        // Issue request
        //        var result = _dynamoService.DynamoClient.Query(request);

        //        // View all returned items
        //        List<Dictionary<string, AttributeValue>> items = result.Items;
        //        foreach (Dictionary<string, AttributeValue> item in items)
        //        {
        //            foreach (var keyValuePair in item)
        //            {
        //                Console.WriteLine("{0} : S={1}, N={2}, SS=[{3}], NS=[{4}]",
        //                    keyValuePair.Key,
        //                    keyValuePair.Value.S,
        //                    keyValuePair.Value.N,
        //                    string.Join(", ", keyValuePair.Value.SS ?? new List<string>()),
        //                    string.Join(", ", keyValuePair.Value.NS ?? new List<string>()));
        //            }
        //        }

        //        // Set marker variable
        //        startKey = result.LastEvaluatedKey;
        //    } while (startKey != null && startKey.Count > 0);

        //    return new List<DVD>();

        //}
        #endregion

    }
}
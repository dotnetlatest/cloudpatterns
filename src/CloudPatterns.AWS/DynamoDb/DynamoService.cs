using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using CloudPatterns.AWS.Contracts;

namespace CloudPatterns.AWS.DynamoDb
{
    public class DynamoService : ITableDataService
    {
        public readonly DynamoDBContext DbContext;
        public AmazonDynamoDBClient DynamoClient;

        public DynamoService()
        {
             DynamoClient = new AmazonDynamoDBClient();

            DbContext = new DynamoDBContext(DynamoClient, new DynamoDBContextConfig
            {
                //Setting the Consistent property to true ensures that you'll always get the latest 
                ConsistentRead = true,
                SkipVersionCheck = true
            });
        }

        /// <summary>
        /// The Store method allows you to save a POCO to DynamoDb
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public void Store<T>(T item) where T : new()
        {
            DbContext.Save(item);
        }

        /// <summary>
        /// The BatchStore Method allows you to store a list of items of type T to dynamoDb
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        public void BatchStore<T>(IEnumerable<T> items) where T : class
        {
            var itemBatch = DbContext.CreateBatchWrite<T>();

            foreach (var item in items)
            {
                itemBatch.AddPutItem(item);
            }

            itemBatch.Execute();
        }
        /// <summary>
        /// Uses the scan operator to retrieve all items in a table
        /// <remarks>[CAUTION] This operation can be very expensive if your table is large</remarks>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>() where T : class
        {
            IEnumerable<T> items = DbContext.Scan<T>();
            return items;
        }

        /// <summary>
        /// Retrieves an item based on a search key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetItem<T>(string key) where T : class
        {
            return DbContext.Load<T>(key);
        }

        /// <summary>
        /// Method Updates and existing item in the table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public void UpdateItem<T>(T item) where T : class
        {
            T savedItem = DbContext.Load(item);

            if (savedItem == null)
            {
                throw new AmazonDynamoDBException("The item does not exist in the Table");
            }

            DbContext.Save(item);
        }

        /// <summary>
        /// Deletes an Item from the table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public void DeleteItem<T>(T item)
        {
            var savedItem = DbContext.Load(item);

            if (savedItem == null)
            {
                throw new AmazonDynamoDBException("The item does not exist in the Table");
            }

            DbContext.Delete(item);
        }

    }
}
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq; 

namespace Application.Infrastructure.DAL
{
    public class DocumentDbRepository<T> where T : class 
    {
        private static readonly string DatabaseId = "";
        private static readonly string CollectionId = "";
        private static Database database;
        private static DocumentClient client;

        public static  void Initialize(string endPoint,string authKey,string databaseId,string collectionId)
        {
            client=new DocumentClient(new Uri(endPoint), authKey);
            CreateDatabaseIfNotExistAsync().Wait();
            CreateCollectionIfNotExistsAsync(databaseId,collectionId).Wait();
        }
        

        private static async Task CreateDatabaseIfNotExistAsync()
        {
            try
            {
                 database = await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch(DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                   database= await client.CreateDatabaseAsync(new Database {Id = DatabaseId});
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync(string databaseId,string collectionId)
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                     UriFactory.CreateDatabaseUri(DatabaseId),new DocumentCollection { Id= collectionId},
                        new RequestOptions{ OfferThroughput = 1000}
                    );
                }
                else
                {
                    throw;
                }
            }
        }


        public static  Document GetDocument(string id)
        {
            var coll = client.CreateDocumentCollectionQuery(database.CollectionsLink).ToList().First();
            var docs = client.CreateDocumentQuery(coll.DocumentsLink);
            return client.CreateDocumentQuery(database.CollectionsLink).Where(x => x.Id == id).AsEnumerable().FirstOrDefault();
         
        }
        
        public static async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId)).Where(predicate).AsDocumentQuery();
            
            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }


        public static async Task<Document> CreateItemAsync(T item)
        {
            try
            {
                return await client.CreateDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static async Task<Document> UpdateItemAsync(T item)
        {
            try
            {
                return await client.UpsertDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static async Task<Document> DeleteDocumentAsync(Document document)
        {
            try
            {
                return await client.DeleteDocumentAsync(document.SelfLink);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
    }   
}
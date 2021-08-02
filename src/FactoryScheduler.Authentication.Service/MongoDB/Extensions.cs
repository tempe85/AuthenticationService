using FactoryScheduler.Authentication.Service.Interfaces;
using FactoryScheduler.Authentication.Service.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using FactoryScheduler.Authentication.Service.Entities;
using FactoryScheduler.Authentication.Service.Repositories;

namespace FactoryScheduler.Authentication.Service.MongoDB
{
    public static class MongoDbExtensions
    {
        public static IServiceCollection AddMongoDB(
            this IServiceCollection services,
            FactorySchedulerSettings settings)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddSingleton(CreateMongoDatabase(settings));

            return services;
        }

        // public static IServiceCollection AddMongoBaseDbRepository<T>(this IServiceCollection services, string collectionName)
        // {
        //     services.AddSingleton<IMongoBaseRepository
        // }


        public static IServiceCollection AddMongoDbRepository<TRepository, TModel>(
            this IServiceCollection services, string collectionName)
            where TModel : IMongoEntity
            where TRepository : MongoBaseRepository<TModel>
        {
            services.AddSingleton(serviceProvider =>
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                return database.GetCollection<TModel>(collectionName);
            });
            services.AddSingleton<IMongoBaseRepository<TModel>>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                var collection = database.GetCollection<TModel>(collectionName);
                return new MongoBaseRepository<TModel>(collection);
            });
            return services;
        }

        // public static async void AddMongoDBCollectionUniqueKeys(IMongoDatabase database, FactorySchedulerDatabaseSettings settings)
        // {
        //     var workStationUsersBuilder = Builders<WorkStation_Users>.IndexKeys;
        //     var indexModel = new CreateIndexModel<WorkStation_Users>(workStationUsersBuilder.);      
        //     var workAreaUsersCollection = database.GetCollection<WorkStation_Users>(settings.WorkAreaCollectionName);
        //     var options = new CreateIndexModel { Unique = true };
        //     await workAreaUsersCollection.Indexes.CreateOneAsync("{}", options);
        // }

        private static IMongoDatabase CreateMongoDatabase(FactorySchedulerSettings settings)
        {
            var mongoClient = new MongoClient(settings.ConnectionString);
            return mongoClient.GetDatabase(settings.DatabaseName);
        }

    }

}
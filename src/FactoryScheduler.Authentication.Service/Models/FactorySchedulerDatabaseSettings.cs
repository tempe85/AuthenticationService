using FactoryScheduler.Authentication.Service.Interfaces;

namespace FactoryScheduler.Authentication.Service.Models
{
    public class FactorySchedulerDatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; init; }
        public string DatabaseName { get; init; }
        public string WorkBuildingCollectionName { get; init; }
        public string AuthenticationCollectionName { get; init; }
        public string UserCertificationCollectionName { get; init; }
        public string WorkStationUsersCollectionName { get; init; }
        public string WorkAreaCollectionName { get; init; }
    }
}
namespace FactoryScheduler.Authentication.Service.Interfaces
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; init; }
        string DatabaseName { get; init; }
    }

}

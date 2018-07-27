
using Dragon.Core; 

namespace Dragon.Infrastructure.Services
{
    public interface IEventBusProvider : IDependency
    {
        string EventName { get; }

        object Execute(object[] parameters);
    }

    public interface IEventBusService
    {
        object[] PublishRequest(string eventName, params object[] parameters);
    }

}
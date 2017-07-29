using Chatazon.Models;
using System.Threading.Tasks;

namespace Chatazon.Hubs
{
    // Client side methods to be invoked by Broadcaster Hub
    public interface IBroadcaster
    {
        Task SetConnectionId(string connectionId);
        Task AddChatMessage(MessageViewModel message);
    }
}

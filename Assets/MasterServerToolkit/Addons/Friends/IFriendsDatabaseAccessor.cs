using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasterServerToolkit.MasterServer
{
    public interface IFriendsDatabaseAccessor:IDatabaseAccessor
    {
        Task<IFriendsInfoData> RestoreFriends(string userId);
    }
}
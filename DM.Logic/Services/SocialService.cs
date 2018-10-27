using DM.Models.ViewModels;
using DM.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class SocialService
    {
        public SocialService()
        {

        }

        public async Task<IndexedResult<IEnumerable<UserFriendsVM>>> GetUserFriends(Guid userId, IndexedResult<UserFriendsVM> lastReturned)
        {

        }

        public async Task<IndexedResult<IEnumerable<FriendActivityVM>>> GetFriendsActivitiesFeed(Guid userId, IndexedResult<FriendActivityVM> lastReturned)
        {

        }
    }
}

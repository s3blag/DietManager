using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.Exceptions;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using DM.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class FriendService : IFriendService
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IActivityService _activityService;
        private readonly IMapper _mapper;
        private readonly IAchievementService _achievementService;

        public FriendService(IFriendRepository friendRepository, IActivityService activityService, 
            IMapper mapper, IAchievementService achievementService)
        {
            _friendRepository = friendRepository;
            _activityService = activityService;
            _mapper = mapper;
            _achievementService = achievementService;
        }

        public async Task<IndexedResult<IEnumerable<UserVM>>> GetUserFriendsAsync(
            Guid userId, 
            IndexedResult<UserVM> lastReturned, 
            int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return null;
            }

            var friends = await _friendRepository.GetUserFriendsAsync(userId, lastReturned?.Index ?? 0, takeAmount);

            return new IndexedResult<IEnumerable<UserVM>>()
            {
                Result = _mapper.Map<IEnumerable<UserVM>>(friends),
                Index = lastReturned?.Index ?? 0 + friends.Count,
                IsLast = friends.Count != takeAmount
            };
        }

        public async Task<IndexedResult<IEnumerable<UserActivityVM>>> GetFriendsActivitiesFeedAsync(
            Guid userId, 
            IndexedResult<UserActivityVM> lastReturned,
            int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE)
        {
            var friends = await _friendRepository.GetUserFriendsAsync(userId, 0, int.MaxValue);
            var indexedFriendsActivities = await _activityService.GetUsersActivitiesFeedAsync(friends.Select(f => f.Id).ToList(), lastReturned, takeAmount);

            return new IndexedResult<IEnumerable<UserActivityVM>>()
            {
                Result = _mapper.Map<IEnumerable<UserActivityVM>>(indexedFriendsActivities.Result),
                Index = lastReturned?.Index ?? 0 + indexedFriendsActivities.Result.Count,
                IsLast = indexedFriendsActivities.Result.Count != takeAmount
            };
        }

        public async Task<IndexedResult<IEnumerable<AwaitingFriendInvitationVM>>> GetFriendInvitationsAsync(
           Guid userId,
           IndexedResult<AwaitingFriendInvitationVM> lastReturned,
           int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return null;
            }

            var friends = await _friendRepository.GetUserFriendsAsync(userId, lastReturned?.Index ?? 0, takeAmount, Models.Enums.FriendInvitationStatus.Awaiting);

            return new IndexedResult<IEnumerable<AwaitingFriendInvitationVM>>()
            {
                Result = _mapper.Map<IEnumerable<AwaitingFriendInvitationVM>>(friends),
                Index = lastReturned?.Index ?? 0 + friends.Count,
                IsLast = friends.Count != takeAmount
            };
        }

        public async Task SendFriendInvitationAsync(FriendInvitationCreationVM friendInvitation)
        {
            if (!await _friendRepository.AddAsync(_mapper.Map<Friend>(friendInvitation)))
            {
                throw new DataAccessException($"Sending friend invitation failed for model: {JsonConvert.SerializeObject(friendInvitation)}");
            }
        }

        public async Task AcceptFriendInvitationAsync(Guid invitingUserId, Guid invitedUserId)
        {
            if (!await _friendRepository.SetFriendInvitationStatusAsync(invitingUserId, invitedUserId, Models.Enums.FriendInvitationStatus.Accepted))
            {
                throw new DataAccessException($"Accepting friend invitation failed for model: {JsonConvert.SerializeObject(new { invitedUserId, invitingUserId })}");
            }

            var checkInvitersNumberOfFriendsTask = _achievementService.CheckForNumberOfFriendsAsync(invitingUserId);
            var checkReceiversNumberOfFriendsTask = _achievementService.CheckForNumberOfFriendsAsync(invitedUserId);

            await Task.WhenAll(checkInvitersNumberOfFriendsTask, checkReceiversNumberOfFriendsTask);
        }

        public async Task IgnoreFriendInvitationAsync(Guid invitingUserId, Guid invitedUserId)
        {
            if (!await _friendRepository.SetFriendInvitationStatusAsync(invitingUserId, invitedUserId, Models.Enums.FriendInvitationStatus.Ignored))
            {
                throw new DataAccessException($"Ignoring friend invitation failed for model: {JsonConvert.SerializeObject(new { invitedUserId, invitingUserId })}");
            }
        }

        public async Task RemoveFromFriends(Guid friendId, Guid userId)
        {
            if (!await _friendRepository.RemoveFriendAsync(userId, friendId))
            {
                throw new DataAccessException($"Ignoring friend invitation failed for model: {JsonConvert.SerializeObject(new { friendId, userId })}");
            }
        }
    }
}

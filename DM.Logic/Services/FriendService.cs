using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.Exceptions;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using DM.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class FriendService : IFriendService
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IMapper _mapper;
        private readonly IAchievementService _achievementService;
        private readonly ILogger<FriendService> _logger;

        public FriendService(IFriendRepository friendRepository, IActivityRepository activityRepository, 
            IMapper mapper, IAchievementService achievementService, ILogger<FriendService> logger)
        {
            _friendRepository = friendRepository;
            _activityRepository = activityRepository;
            _mapper = mapper;
            _achievementService = achievementService;
            _logger = logger;
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

            var usersVM = _mapper.Map<ICollection<UserVM>>(await _friendRepository.GetFriendsAsync(userId, lastReturned?.Index ?? 0, takeAmount));

            foreach (var user in usersVM)
            {
                user.IsFriend = true;
            }

            return new IndexedResult<IEnumerable<UserVM>>()
            {
                Result = usersVM,
                Index = lastReturned?.Index ?? 0 + usersVM.Count,
                IsLast = usersVM.Count != takeAmount
            };
        }

        public async Task<IndexedResult<IEnumerable<UserActivityVM>>> GetFriendsActivitiesFeedAsync(
            Guid userId, 
            IndexedResult<UserActivityVM> lastReturned,
            int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE)
        {

            var indexedFriendsActivities = await _activityRepository.GetUsersFriendsActivitiesAsync(userId, lastReturned?.Index ?? 0, takeAmount);

            return new IndexedResult<IEnumerable<UserActivityVM>>()
            {
                Result = _mapper.Map<IEnumerable<UserActivityVM>>(indexedFriendsActivities),
                Index = lastReturned?.Index ?? 0 + indexedFriendsActivities.Count,
                IsLast = indexedFriendsActivities.Count != takeAmount
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

            var friends = await _friendRepository.GetFriendsAsync(userId, lastReturned?.Index ?? 0, takeAmount, Models.Enums.FriendInvitationStatus.Awaiting);

            return new IndexedResult<IEnumerable<AwaitingFriendInvitationVM>>()
            {
                Result = _mapper.Map<IEnumerable<AwaitingFriendInvitationVM>>(friends),
                Index = lastReturned?.Index ?? 0 + friends.Count,
                IsLast = friends.Count != takeAmount
            };
        }

        public async Task<bool> SendFriendInvitationAsync(FriendInvitationCreationVM friendInvitation)
        {
            bool addedSuccessfully = false;

            try
            {
                addedSuccessfully = await _friendRepository.AddAsync(_mapper.Map<Friend>(friendInvitation));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }

            if (!addedSuccessfully)
            {
                throw new DataAccessException($"Sending friend invitation failed for model: {JsonConvert.SerializeObject(friendInvitation)}");
            }

            return true;
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

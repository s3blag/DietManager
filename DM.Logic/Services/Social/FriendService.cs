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
        private readonly IActivityRepository _activityRepository;
        private readonly IMapper _mapper;

        public FriendService(IFriendRepository friendRepository, IActivityRepository activityRepository, IMapper mapper)
        {
            _friendRepository = friendRepository;
            _activityRepository = activityRepository;
            _mapper = mapper;
        }

        public async Task<IndexedResult<IEnumerable<UserFriendsVM>>> GetUserFriendsAsync(
            Guid userId, 
            IndexedResult<UserFriendsVM> lastReturned, 
            int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return null;
            }

            var friends = await _friendRepository.GetUserFriendsAsync(userId, lastReturned?.Index ?? 0, takeAmount);

            return new IndexedResult<IEnumerable<UserFriendsVM>>()
            {
                Result = _mapper.Map<IEnumerable<UserFriendsVM>>(friends),
                Index = lastReturned?.Index ?? 0 + friends.Count,
                IsLast = friends.Count != takeAmount
            };
        }

        //TODO
        public async Task<IndexedResult<IEnumerable<FriendActivityVM>>> GetFriendsActivitiesFeedAsync(
            Guid userId, 
            IndexedResult<FriendActivityVM> lastReturned,
            int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return null;
            }

            var friends = await _friendRepository.GetUserFriendsAsync(userId, 0, int.MaxValue);
            var friendsActivities = await _activityRepository.GetUsersActivitiesAsync(friends.Select(f => f.Id).ToList(), lastReturned.Index, takeAmount);

            return new IndexedResult<IEnumerable<FriendActivityVM>>()
            {
                Result = _mapper.Map<IEnumerable<FriendActivityVM>>(friendsActivities),
                Index = lastReturned?.Index ?? 0 + friendsActivities.Count,
                IsLast = friendsActivities.Count != takeAmount
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

        public async Task AcceptFriendInvitationAsync(AwaitingFriendInvitationVM friendInvitationVM, Guid receiverId)
        {
            if (!await _friendRepository.SetFriendInvitationStatusAsync(friendInvitationVM.UserId, receiverId, Models.Enums.FriendInvitationStatus.Accepted))
            {
                throw new DataAccessException($"Accepting friend invitation failed for model: {JsonConvert.SerializeObject(friendInvitationVM)}");
            }
        }

        public async Task IgnoreFriendInvitationAsync(AwaitingFriendInvitationVM friendInvitationVM, Guid receiverId)
        {
            if (!await _friendRepository.SetFriendInvitationStatusAsync(friendInvitationVM.UserId, receiverId, Models.Enums.FriendInvitationStatus.Ignored))
            {
                throw new DataAccessException($"Ignoring friend invitation failed for model: {JsonConvert.SerializeObject(friendInvitationVM)}");
            }
        }
    }
}

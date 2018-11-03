using AutoMapper;
using DM.Database;
using DM.Models;
using DM.Models.Enums;
using DM.Models.Models;
using DM.Models.ViewModels;
using System;

namespace DM.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MealScheduleEntry, MealScheduleEntryVM>();
            CreateMap<MealWithIngredients, MealVM>();
            CreateMap<MealIngredientWithQuantityVM, MealIngredientWithQuantity>().ReverseMap();
            CreateMap<MealPreview, MealPreviewVM>().ReverseMap();
            CreateMap<NutritionsVM, Nutrition>().
                ForMember(target => target.Id, config => config.MapFrom(src => Guid.NewGuid())).
                ReverseMap(); 
            CreateMap<MealVM, Meal>().
                ForMember(target => target.Id, config => config.MapFrom(src => Guid.NewGuid())).
                ReverseMap();
            CreateMap<MealCreationVM, Meal>().
                ForMember(target => target.Id, config => config.MapFrom(src => Guid.NewGuid())).
                ForMember(target => target.CreationDate, config => config.MapFrom(src => DateTimeOffset.Now)).
                ReverseMap();
            CreateMap<MealIngredientVM, MealIngredient>().
                ForMember(target => target.Id, config => config.MapFrom(source => Guid.NewGuid())).
                ReverseMap();
            CreateMap<MealIngredientCreationVM, MealIngredient>().
                ForMember(target => target.Id, config => config.MapFrom(source => Guid.NewGuid())).
                ReverseMap();
            CreateMap<NewMealVM, Meal>().
                ForMember(target => target.Id, config => config.MapFrom(source => Guid.NewGuid())).
                ReverseMap();
            CreateMap<ImageCreation, Image>().
                ForMember(target => target.Id, config => config.MapFrom(source => Guid.NewGuid())).
                ReverseMap();
            CreateMap<UserAchievement, UserAchievementVM>().ReverseMap();
            CreateMap<Achievement, AchievementVM>().ReverseMap();
            CreateMap<User, UserVM>().ReverseMap();
            CreateMap<UserAchievementCreation, UserAchievement>().
                ForMember(target => target.Id, config => config.MapFrom(source => Guid.NewGuid()));
            CreateMap<User, AwaitingFriendInvitationVM>().
                ForMember(target => target.UserId, config => config.MapFrom(src => src.Id)).
                ForMember(target => target.Name, config => config.MapFrom(src => src.Name)).
                ForMember(target => target.Surname, config => config.MapFrom(src => src.Surname)).
                ForMember(target => target.ImageId, config => config.MapFrom(src => src.ImageId)).
                ReverseMap();
            CreateMap<FriendInvitationCreationVM, Friend>().
                ForMember(target => target.User1Id, config => config.MapFrom(src => src.InvitingUserId)).
                ForMember(target => target.User2Id, config => config.MapFrom(src => src.InvitedUserId)).
                ForMember(target => target.Status, config => config.MapFrom(src => FriendInvitationStatus.Awaiting)).
                ForMember(target => target.CreationDate, config => config.MapFrom(_ => DateTimeOffset.Now));
            CreateMap<UserActivity, FriendActivityVM>().
                ForMember(target => target.Activity, config => config.MapFrom(src => (ActivityType)Enum.Parse(typeof(ActivityType), src.ActivityType, true))).
                ReverseMap().
                ForPath(target => target.ActivityType, config => config.MapFrom(src => src.Activity.ToString()));
        }
    }
}

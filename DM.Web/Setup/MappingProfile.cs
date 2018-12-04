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
            CreateMap<CompleteMealScheduleEntry, MealScheduleEntryVM>();
            CreateMap<MealVM, MealWithIngredients>().ReverseMap();
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
            CreateMap<MealScheduleEntryCreationVM, MealScheduleEntry>().
                ForMember(target => target.Id, config => config.MapFrom(source => Guid.NewGuid()));
            CreateMap<ImageCreation, Image>().
                ForMember(target => target.Id, config => config.MapFrom(source => Guid.NewGuid())).
                ReverseMap();
            CreateMap<UserAchievement, UserAchievementVM>().
                ForMember(target => target.Category, config => config.MapFrom(source => source.Achievement.Category)).
                ForMember(target => target.Type, config => config.MapFrom(source => source.Achievement.Type)).
                ForMember(target => target.Value, config => config.MapFrom(source => source.Achievement.Value)).
                ReverseMap();
            CreateMap<Achievement, AchievementVM>().ReverseMap();
            CreateMap<User, UserVM>().ReverseMap();
            CreateMap<User, UserWithPasswordVM>();
            CreateMap<User, LoggedInUserVM>();
            CreateMap<UserCreationVM, User>().
                ForMember(target => target.Id, config => config.MapFrom(src => Guid.NewGuid())).
                ForMember(target => target.UserName, config => config.MapFrom(source => source.Username)).
                ForMember(target => target.FullName, config => config.MapFrom(source => source.Name + " " + source.Surname)).
                ForMember(target => target.CreationDate, config => config.MapFrom(source => DateTimeOffset.Now)).
                ForMember(target => target.LastLoginDate, config => config.MapFrom(source => DateTimeOffset.Now));
            CreateMap<UserAchievementCreation, UserAchievement>();
            CreateMap<User, AwaitingFriendInvitationVM>().
                ForMember(target => target.UserId, config => config.MapFrom(src => src.Id)).
                ForMember(target => target.Name, config => config.MapFrom(src => src.Name)).
                ForMember(target => target.Surname, config => config.MapFrom(src => src.Surname)).
                ForMember(target => target.ImageId, config => config.MapFrom(src => src.ImageId)).
                ReverseMap();
            CreateMap<FriendInvitationCreationVM, Friend>().
                ForMember(target => target.InvitingUserId, config => config.MapFrom(src => src.InvitingUserId)).
                ForMember(target => target.InvitedUserId, config => config.MapFrom(src => src.InvitedUserId)).
                ForMember(target => target.Status, config => config.MapFrom(src => FriendInvitationStatus.Awaiting)).
                ForMember(target => target.CreationDate, config => config.MapFrom(_ => DateTimeOffset.Now));
            CreateMap<UserActivity, UserActivityVM>();
            CreateMap<FavouriteCreationVM, Favourite>().
                ForMember(target => target.Id, config => config.MapFrom(src => Guid.NewGuid())).
                ForMember(target => target.CreationDate, config => config.MapFrom(source => DateTimeOffset.Now));
            CreateMap<FavouriteVM, Favourite>().ReverseMap();
            CreateMap<Meal, MealPreviewVM>().ReverseMap();
            //!
            CreateMap<Favourite, MealPreviewVM>().
                ForMember(target => target.Id, config => config.MapFrom(src => src.Meal.Id)).
                ForMember(target => target.ImageId, config => config.MapFrom(src => src.Meal.ImageId)).
                ForMember(target => target.Name, config => config.MapFrom(src => src.Meal.Name)).
                ForMember(target => target.Calories, config => config.MapFrom(src => src.Meal.Calories)).
                ForMember(target => target.CreationDate, config => config.MapFrom(src => src.CreationDate)).
                ForMember(target => target.Creator, config => config.MapFrom(src => src.Meal.Creator)).
                ForMember(target => target.NumberOfFavouriteMarks, config => config.MapFrom(src => src.Meal.NumberOfFavouriteMarks)).
                ForMember(target => target.NumberOfUses, config => config.MapFrom(src => src.Meal.NumberOfUses)).
                ForMember(target => target.IsFavourite, config => config.MapFrom(_ => true));
            CreateMap<ActivityCreation, UserActivity>();
            CreateMap<MealScheduleEntryUpdateVM, MealScheduleEntry>().
                ForMember(target => target.Date, config => config.MapFrom(src => src.NewDate));
        }
    }
}

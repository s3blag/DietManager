using AutoMapper;
using DM.Database;
using DM.Models;
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
            CreateMap<UserAchievementCreation, UserAchievement>().
                ForMember(target => target.Id, config => config.MapFrom(source => Guid.NewGuid()));

        }
    }
}

using AutoMapper;
using DM.Database;
using DM.Models;
using DM.Models.ViewModels;
using System;

namespace DM.Web.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MealScheduleEntry, MealScheduleEntryVM>();
            CreateMap<MealWithIngredients, MealVM>();
            CreateMap<NutritionsVM, Nutrition>().
                ForMember(target => target.Id, config => config.MapFrom(src => Guid.NewGuid())).
                ReverseMap(); ;
            CreateMap<MealVM, Meal>().
                ForMember(target => target.Id, config => config.MapFrom(src => Guid.NewGuid())).
                ReverseMap();
            CreateMap<MealCreationVM, Meal>().
                ForMember(target => target.Id, config => config.MapFrom(src => Guid.NewGuid())).
                ReverseMap();
            CreateMap<MealIngredientVM, MealIngredient>().
                ForMember(target => target.Id, config => config.MapFrom(source => Guid.NewGuid())).
                ReverseMap();
            CreateMap<MealCreationVM, MealIngredient>().
                ForMember(target => target.Id, config => config.MapFrom(source => Guid.NewGuid())).
                ReverseMap();
            CreateMap<NewMealVM, Meal>().
                ForMember(target => target.Id, config => config.MapFrom(source => Guid.NewGuid())).
                ReverseMap();
            CreateMap<ImageCreation, Image>().
                ForMember(target => target.Id, config => config.MapFrom(source => Guid.NewGuid())).
                ReverseMap();
        }
    }
}

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
            CreateMap<MealWithIngredients, MealVM>().ReverseMap();
            CreateMap<Nutrition, NutritionsVM>().ReverseMap();
            CreateMap<MealVM, Meal>().
                ForMember(target => target.Id, config => config.MapFrom(src => Guid.NewGuid())).
                ReverseMap();
            CreateMap<MealIngredientVM, MealIngredient>().
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

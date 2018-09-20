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
            CreateMap<MealVM, Meal>().
                ForMember(target => target.Id, config => config.MapFrom(src => Guid.NewGuid())).
                ReverseMap();
            CreateMap<MealWithIngredients, MealVM>().ReverseMap();
            CreateMap<Nutrition, NutritionsVM>().ReverseMap();
            CreateMap<MealIngredient, MealIngredientVM>().ReverseMap();
            CreateMap<NewMealVM, Meal>().
                ForMember(target => target.Id, config => config.MapFrom(src => Guid.NewGuid())).
                ReverseMap();
            CreateMap<ImageCreation, Image>().
                ForMember(target => target.Id, config => config.MapFrom(src => Guid.NewGuid())).
                ReverseMap();
        }
    }
}

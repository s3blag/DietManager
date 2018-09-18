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
            CreateMap<Meal, MealVM>();
            CreateMap<MealWithIngredients, MealVM>();
            CreateMap<Nutrition, NutritionsVM>();
            CreateMap<MealIngredient, MealIngredientVM>();
            CreateMap<NewMealVM, Meal>().
                ForMember(target => target.Id, config => config.MapFrom(src => Guid.NewGuid()));

        }
    }
}

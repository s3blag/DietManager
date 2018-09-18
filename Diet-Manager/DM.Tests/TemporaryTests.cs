using AutoMapper;
using DM.Database;
using DM.Logic.Services;
using DM.Models.ViewModels;
using DM.Repositories;
using DM.Web.Config;
using LinqToDB.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DM.Tests
{
    public class TemporaryTests
    {
        [Fact]
        public async Task Test1()
        {
            DataConnection.DefaultSettings = new DBConnectionSettings();

            var userRepository = new UserRepository();

            var guid = new Guid("629fe72b-4bb0-4dc8-aa51-bfe9d65139a9");

            await userRepository.DeleteUserAsync(guid);

            await userRepository.AddUserAsync(new User()
            { 
                Id = guid,
                Email = "121313",
                Password = "p@ssw0rd",
                UserName = "seblag"
            });

            var user = await userRepository.GetUserAsync(guid);
        }

        [Fact]
        public async Task Test2()
        {
            //var guid = Guid.NewGuid();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            Mapper.Configuration.CompileMappings();
            var mapper = Mapper.Instance;

            var mealService = new MealService(mapper, new MealRepository());

            var newMeal = new NewMealVM()
            {
                Name = "Nowy posi³ek",
                PhotoId = new Guid("beb5afef-d709-4f50-b280-a45068518718"),
                Calories = "220",
                Ingredients = Enumerable.Empty<MealIngredientVM>(),
            };

            DataConnection.DefaultSettings = new DBConnectionSettings();

            var result = await mealService.AddMealAsync(newMeal);
        }

    }
}

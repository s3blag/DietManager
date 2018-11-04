using AutoMapper;
using DM.Database;
using DM.Logic.Services;
using DM.Models.Config;
using DM.Models.Enums;
using DM.Models.Models;
using DM.Models.ViewModels;
using DM.Repositories;
using DM.Repositories.Extensions;
using DM.Repositories.Interfaces;
using DM.Repositories.Repositories;
using DM.Tests.TestsData;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DM.Tests
{
    public class TemporaryTests
    {
        //[Fact]
        //public async Task Test1()
        //{
        //    ObjectsFactory.InitDbConnection(Constants.ConnectionString);

        //    var userRepository = new UserRepository();

        //    var guid = new Guid("629fe72b-4bb0-4dc8-aa51-bfe9d65139a9");

        //    await userRepository.DeleteUserAsync(guid);

        //    await userRepository.AddUserAsync(new User()
        //    { 
        //        Id = guid,
        //        Email = "121313",
        //        Password = "p@ssw0rd",
        //        UserName = "seblag"
        //    });

        //    var user = await userRepository.GetUserAsync(guid);
        //}

        //[Fact]
        //public async Task Test2()
        //{
        //    ObjectsFactory.InitDbConnection(Constants.ConnectionString);

        //    var mealService = new MealService(ObjectsFactory.GetMapperInstance(), new MealRepository(), new MealIngredientRepository());

        //    var newMeal = new NewMealVM()
        //    {
        //        Name = "Nowy posi�ek",
        //        ImageId = new Guid("beb5afef-d709-4f50-b280-a45068518718"),
        //        Calories = "220",
        //        Ingredients = Enumerable.Empty<MealIngredientVM>(),
        //    };

        //    var result = await mealService.AddMealAsync(newMeal);
        //}

        //[Fact]
        //public async Task Test3()
        //{
        //    ObjectsFactory.InitDbConnection(Constants.ConnectionString);
        //    var imageService = new ImageService(new ImageRepository(), ObjectsFactory.GetMapperInstance(), ObjectsFactory.GetImageServiceIOptions(Constants.NumberOfImagesInSubdirectory, Constants.TestImageDirectoryPath));

        //    string imagePath = Constants.TestImageDirectoryPath + "104647_1.jpg";
        //    byte[] imageToSave = await File.ReadAllBytesAsync(imagePath);
        //    string bytesAsString = JsonConvert.SerializeObject(imageToSave);

        //    var result = await imageService.AddImageAsync(imageToSave);
        //}

        //[Fact]
        //public async Task AddMeal()
        //{
        //    ObjectsFactory.InitDbConnection(Constants.ConnectionString);
        //    var mapper = ObjectsFactory.GetMapperInstance();

        //    var mealIngredientRepo = new MealIngredientRepository();
        //    var mealRepo = new MealRepository();

        //    var mealService = new MealService(mapper, mealRepo, mealIngredientRepo);

        //    var mealVM = new NewMealVM()
        //    {
        //        Name = "newMealVM",
        //        ImageId = null,
        //        Calories = "235",
        //        Ingredients = new List<MealIngredientVM>()
        //        {
        //            new MealIngredientVM()
        //            {
        //                Name = "mealIngredient1",
        //                Calories = "35",
        //                Nutrition = new NutritionsVM()
        //                {
        //                    Protein = 23,
        //                    Carbohydrates = 11,
        //                    Fats = 22,
        //                    VitaminA = 13
        //                }
        //            },
        //            new MealIngredientVM()
        //            {
        //                Name = "mealIngredient2",
        //                Calories = "150",
        //                Nutrition = new NutritionsVM()
        //                {
        //                    Protein = 53,
        //                    Carbohydrates = 1,
        //                    Fats = 78,
        //                    VitaminA = 16
        //                }
        //            },
        //            new MealIngredientVM()
        //            {
        //                Name = "mealIngredient3",
        //                Calories = "50",
        //                Nutrition = new NutritionsVM()
        //                {
        //                    Protein = 13,
        //                    Carbohydrates = 12,
        //                    Fats = 25,
        //                    VitaminB6 = 6
        //                }
        //            },
        //        }
        //    };

        //    var result = await mealService.AddMealAsync(mealVM);
        //}

        [Fact]
        public async Task DeleteMeal()
        {
            ObjectsFactory.InitDbConnection(Constants.ConnectionString);
            using (var db = new DietManagerDB())
            {
                db.InlineParameters = true;
                var date = DateTimeOffset.Now;
                var mealPreviewsQuery = db.Meals.
                    Where(m => m.CreationDate.GreaterThanOrEqual(date));
                

                var result = await mealPreviewsQuery.ToListAsync();

            }
        }

        [Fact]
        public async Task Load()
        {
            ObjectsFactory.InitDbConnection(Constants.ConnectionString);

            //var obj = new AchievementsSetup();

            //await obj.Setup(new AchievementsConfig()
            //{
            //    FriendAchievements = new Dictionary<Achievements.FriendAchievement, int[]>()
            //    {
            //        [Achievements.FriendAchievement.NumberOfFriends] = new [] { 1, 5, 10 }
            //    },
            //    MealScheduleAchievements = new Dictionary<Achievements.MealScheduleAchievement, int[]>()
            //    {
            //        [Achievements.MealScheduleAchievement.ConsequentScheduleUpdates] = new [] {2, 4, 6}
            //    }
            //}, new AchievementRepository(new AchievementsContainer()));
        }

        [Fact]
        public async Task Mapper()
        {
            var mapper = ObjectsFactory.GetMapperInstance();

            var user = new User
            {
                Id = new Guid("d887c539-741d-482d-a031-806830bcb1e2"),
                Name = "Sebastian",
                Surname = "Łągiewski" 
            };

            var activity = new UserActivity()
            {
                ActivityDate = DateTimeOffset.Now,
                ActivityType = "NewMealIngredientAdded",
                ContentId= new Guid("d887c539-741d-0000-a031-806830bcb1e2"),
                Id = new Guid("d887c539-741d-1111-a031-806830bcb1e2"),
                User = user,
                UserId = user.Id
            };

            var afterTransform = mapper.Map<UserActivityVM>(activity);
            var again = mapper.Map<UserActivity>(afterTransform);
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class FavouriteVM
    {

        public Guid? Id { get; set; }


        public UserVM User { get; set; }


        public MealVM Meal { get; set; }
    }
}

﻿





//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/linq2db).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

using LinqToDB;
using LinqToDB.Mapping;

namespace DM.Database
{
	/// <summary>
	/// Database       : DietManager
	/// Data Source    : tcp://localhost:5432
	/// Server Version : 10.4
	/// </summary>
	public partial class DietManagerDB : LinqToDB.Data.DataConnection
	{
		public ITable<Achievement>                  Achievements                  { get { return this.GetTable<Achievement>(); } }
		public ITable<Favourite>                    Favourites                    { get { return this.GetTable<Favourite>(); } }
		public ITable<Friend>                       Friends                       { get { return this.GetTable<Friend>(); } }
		public ITable<Image>                        Images                        { get { return this.GetTable<Image>(); } }
		public ITable<Meal>                         Meals                         { get { return this.GetTable<Meal>(); } }
		public ITable<MealCompleteMealIngredient>   MealCompleteMealIngredients   { get { return this.GetTable<MealCompleteMealIngredient>(); } }
		public ITable<MealIngredient>               MealIngredients               { get { return this.GetTable<MealIngredient>(); } }
		public ITable<MealIngredientsWithNutrition> MealIngredientsWithNutritions { get { return this.GetTable<MealIngredientsWithNutrition>(); } }
		public ITable<MealMealIngredient>           MealMealIngredients           { get { return this.GetTable<MealMealIngredient>(); } }
		public ITable<MealScheduleEntry>            MealScheduleEntries           { get { return this.GetTable<MealScheduleEntry>(); } }
		public ITable<Nutrition>                    Nutritions                    { get { return this.GetTable<Nutrition>(); } }
		public ITable<User>                         Users                         { get { return this.GetTable<User>(); } }
		public ITable<UserAchievement>              UserAchievements              { get { return this.GetTable<UserAchievement>(); } }
		public ITable<UserActivity>                 UserActivities                { get { return this.GetTable<UserActivity>(); } }

		public DietManagerDB()
		{
			InitDataContext();
		}

		public DietManagerDB(string configuration)
			: base(configuration)
		{
			InitDataContext();
		}

		partial void InitDataContext();
	}

	[Table(Schema="Socials", Name="Achievement")]
	public partial class Achievement
	{
		[PrimaryKey, NotNull] public Guid   Id       { get; set; } // uuid
		[Column,     NotNull] public string Category { get; set; } // text
		[Column,     NotNull] public string Type     { get; set; } // text
		[Column,     NotNull] public int    Value    { get; set; } // integer

		#region Associations

		/// <summary>
		/// fk_userachievement_achievement_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="AchievementId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<UserAchievement> fkuserachievementachievements { get; set; }

		/// <summary>
		/// fk_useractivity_achievement_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="AchievementId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<UserActivity> fkuseractivityachievements { get; set; }

		#endregion
	}

	[Table(Schema="Meals", Name="Favourite")]
	public partial class Favourite
	{
		[PrimaryKey, NotNull] public Guid           Id           { get; set; } // uuid
		[Column,     NotNull] public Guid           MealId       { get; set; } // uuid
		[Column,     NotNull] public Guid           UserId       { get; set; } // uuid
		[Column,     NotNull] public DateTimeOffset CreationDate { get; set; } // timestamp (6) with time zone

		#region Associations

		/// <summary>
		/// fk_favourites_mealid
		/// </summary>
		[Association(ThisKey="MealId", OtherKey="Id", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="fk_favourites_mealid", BackReferenceName="fkfavouritesmealids")]
		public Meal Meal { get; set; }

		/// <summary>
		/// fk_favourites_userid
		/// </summary>
		[Association(ThisKey="UserId", OtherKey="Id", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="fk_favourites_userid", BackReferenceName="fkfavouritesuserids")]
		public User User { get; set; }

		#endregion
	}

	[Table(Schema="Socials", Name="Friend")]
	public partial class Friend
	{
		[PrimaryKey(1), NotNull] public Guid           InvitingUserId { get; set; } // uuid
		[PrimaryKey(2), NotNull] public Guid           InvitedUserId  { get; set; } // uuid
		[Column,        NotNull] public string         Status         { get; set; } // character varying(50)
		[Column,        NotNull] public DateTimeOffset CreationDate   { get; set; } // timestamp (6) with time zone

		#region Associations

		/// <summary>
		/// fk_friend_inviteduserid
		/// </summary>
		[Association(ThisKey="InvitedUserId", OtherKey="Id", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="fk_friend_inviteduserid", BackReferenceName="fkfriendinviteduserids")]
		public User InvitedUser { get; set; }

		/// <summary>
		/// fk_friend_invitinguserid
		/// </summary>
		[Association(ThisKey="InvitingUserId", OtherKey="Id", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="fk_friend_invitinguserid", BackReferenceName="fkfriendinvitinguserids")]
		public User InvitingUser { get; set; }

		#endregion
	}

	[Table(Schema="Images", Name="Image")]
	public partial class Image
	{
		[PrimaryKey, NotNull] public Guid   Id   { get; set; } // uuid
		[Column,     NotNull] public string Path { get; set; } // text

		#region Associations

		/// <summary>
		/// fk_user_image_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="ImageId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<User> fkuserimages { get; set; }

		/// <summary>
		/// FK_MealIngredient_Image_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="ImageId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<MealIngredient> MealIngredients { get; set; }

		/// <summary>
		/// FK_Meal_Image_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="ImageId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<Meal> Meals { get; set; }

		#endregion
	}

	[Table(Schema="Meals", Name="Meal")]
	public partial class Meal
	{
		[PrimaryKey, NotNull    ] public Guid           Id                     { get; set; } // uuid
		[Column,     NotNull    ] public DateTimeOffset CreationDate           { get; set; } // timestamp (6) with time zone
		[Column,     NotNull    ] public Guid           CreatorId              { get; set; } // uuid
		[Column,        Nullable] public Guid?          ImageId                { get; set; } // uuid
		[Column,     NotNull    ] public string         Name                   { get; set; } // character varying(50)
		[Column,     NotNull    ] public string         Description            { get; set; } // text
		[Column,     NotNull    ] public int            Calories               { get; set; } // integer
		[Column,     NotNull    ] public int            NumberOfFavouriteMarks { get; set; } // integer
		[Column,     NotNull    ] public int            NumberOfUses           { get; set; } // integer

		#region Associations

		/// <summary>
		/// FK_Meal_User
		/// </summary>
		[Association(ThisKey="CreatorId", OtherKey="Id", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="FK_Meal_User", BackReferenceName="Meals")]
		public User Creator { get; set; }

		/// <summary>
		/// fk_favourites_mealid_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="MealId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<Favourite> fkfavouritesmealids { get; set; }

		/// <summary>
		/// fk_useractivity_favourite_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="FavouriteId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<UserActivity> fkuseractivityfavourites { get; set; }

		/// <summary>
		/// fk_useractivity_meal_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="MealId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<UserActivity> fkuseractivitymeals { get; set; }

		/// <summary>
		/// fk_usermeal_mealid_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="MealId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<MealScheduleEntry> fkusermealmealids { get; set; }

		/// <summary>
		/// FK_Meal_Image
		/// </summary>
		[Association(ThisKey="ImageId", OtherKey="Id", CanBeNull=true, Relationship=Relationship.ManyToOne, KeyName="FK_Meal_Image", BackReferenceName="Meals")]
		public Image Image { get; set; }

		/// <summary>
		/// FK_MealMealIngredient_Meal_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="MealId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<MealMealIngredient> MealMealIngredients { get; set; }

		#endregion
	}

	[Table(Schema="Meals", Name="Meal-CompleteMealIngredient", IsView=true)]
	public partial class MealCompleteMealIngredient
	{
		[Column(SkipOnUpdate=true), Nullable] public Guid?   MealId                 { get; set; } // uuid
		[Column(SkipOnUpdate=true), Nullable] public double? Quantity               { get; set; } // double precision
		[Column(SkipOnUpdate=true), Nullable] public Guid?   MealIngredientId       { get; set; } // uuid
		[Column(SkipOnUpdate=true), Nullable] public string  MealIngredientName     { get; set; } // character varying(30)
		[Column(SkipOnUpdate=true), Nullable] public Guid?   MealIngredientImageId  { get; set; } // uuid
		[Column(SkipOnUpdate=true), Nullable] public int?    MealIngredientCalories { get; set; } // integer
		[Column(SkipOnUpdate=true), Nullable] public double? Protein                { get; set; } // double precision
		[Column(SkipOnUpdate=true), Nullable] public double? Carbohydrates          { get; set; } // double precision
		[Column(SkipOnUpdate=true), Nullable] public double? Fats                   { get; set; } // double precision
		[Column(SkipOnUpdate=true), Nullable] public double? VitaminA               { get; set; } // double precision
		[Column(SkipOnUpdate=true), Nullable] public double? VitaminC               { get; set; } // double precision
		[Column(SkipOnUpdate=true), Nullable] public double? VitaminB6              { get; set; } // double precision
		[Column(SkipOnUpdate=true), Nullable] public double? VitaminD               { get; set; } // double precision
	}

	[Table(Schema="Meals", Name="MealIngredient")]
	public partial class MealIngredient
	{
		[PrimaryKey, NotNull    ] public Guid   Id           { get; set; } // uuid
		[Column,        Nullable] public Guid?  CreatorId    { get; set; } // uuid
		[Column,        Nullable] public Guid?  ImageId      { get; set; } // uuid
		[Column,        Nullable] public string Name         { get; set; } // character varying(30)
		[Column,     NotNull    ] public int    Calories     { get; set; } // integer
		[Column,     NotNull    ] public Guid   NutritionsId { get; set; } // uuid
		[Column,     NotNull    ] public int    NumberOfUses { get; set; } // integer

		#region Associations

		/// <summary>
		/// FK_MealIngredient_Creator
		/// </summary>
		[Association(ThisKey="CreatorId", OtherKey="Id", CanBeNull=true, Relationship=Relationship.ManyToOne, KeyName="FK_MealIngredient_Creator", BackReferenceName="MealIngredientCreators")]
		public User Creator { get; set; }

		/// <summary>
		/// fk_useractivity_mealingredient_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="MealIngredientId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<UserActivity> fkuseractivitymealingredients { get; set; }

		/// <summary>
		/// FK_MealIngredient_Image
		/// </summary>
		[Association(ThisKey="ImageId", OtherKey="Id", CanBeNull=true, Relationship=Relationship.ManyToOne, KeyName="FK_MealIngredient_Image", BackReferenceName="MealIngredients")]
		public Image Image { get; set; }

		/// <summary>
		/// FK_MealMealIngredient_MealIngredient-_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="MealIngredientId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<MealMealIngredient> MealMealIngredients { get; set; }

		/// <summary>
		/// FK_MealIngredient_Nutritions
		/// </summary>
		[Association(ThisKey="NutritionsId", OtherKey="Id", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="FK_MealIngredient_Nutritions", BackReferenceName="MealIngredients")]
		public Nutrition Nutrition { get; set; }

		#endregion
	}

	[Table(Schema="Meals", Name="MealIngredientsWithNutritions", IsView=true)]
	public partial class MealIngredientsWithNutrition
	{
		[Column(SkipOnUpdate=true), Nullable] public Guid?   Id            { get; set; } // uuid
		[Column(SkipOnUpdate=true), Nullable] public string  Name          { get; set; } // character varying(30)
		[Column(SkipOnUpdate=true), Nullable] public Guid?   ImageId       { get; set; } // uuid
		[Column(SkipOnUpdate=true), Nullable] public int?    Calories      { get; set; } // integer
		[Column(SkipOnUpdate=true), Nullable] public Guid?   CreatorId     { get; set; } // uuid
		[Column(SkipOnUpdate=true), Nullable] public int?    NumberOfUses  { get; set; } // integer
		[Column(SkipOnUpdate=true), Nullable] public double? Protein       { get; set; } // double precision
		[Column(SkipOnUpdate=true), Nullable] public double? Carbohydrates { get; set; } // double precision
		[Column(SkipOnUpdate=true), Nullable] public double? Fats          { get; set; } // double precision
		[Column(SkipOnUpdate=true), Nullable] public double? VitaminA      { get; set; } // double precision
		[Column(SkipOnUpdate=true), Nullable] public double? VitaminC      { get; set; } // double precision
		[Column(SkipOnUpdate=true), Nullable] public double? VitaminB6     { get; set; } // double precision
		[Column(SkipOnUpdate=true), Nullable] public double? VitaminD      { get; set; } // double precision
	}

	[Table(Schema="Meals", Name="Meal-MealIngredient")]
	public partial class MealMealIngredient
	{
		[PrimaryKey(1), NotNull] public Guid   MealIngredientId { get; set; } // uuid
		[PrimaryKey(2), NotNull] public Guid   MealId           { get; set; } // uuid
		[Column,        NotNull] public double Quantity         { get; set; } // double precision

		#region Associations

		/// <summary>
		/// FK_MealMealIngredient_Meal
		/// </summary>
		[Association(ThisKey="MealId", OtherKey="Id", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="FK_MealMealIngredient_Meal", BackReferenceName="MealMealIngredients")]
		public Meal Meal { get; set; }

		/// <summary>
		/// FK_MealMealIngredient_MealIngredient-
		/// </summary>
		[Association(ThisKey="MealIngredientId", OtherKey="Id", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="FK_MealMealIngredient_MealIngredient-", BackReferenceName="MealMealIngredients")]
		public MealIngredient MealIngredient { get; set; }

		#endregion
	}

	[Table(Schema="Meals", Name="MealScheduleEntry")]
	public partial class MealScheduleEntry
	{
		[PrimaryKey, NotNull] public Guid           Id     { get; set; } // uuid
		[Column,     NotNull] public Guid           UserId { get; set; } // uuid
		[Column,     NotNull] public Guid           MealId { get; set; } // uuid
		[Column,     NotNull] public DateTimeOffset Date   { get; set; } // timestamp (6) with time zone

		#region Associations

		/// <summary>
		/// fk_usermeal_mealid
		/// </summary>
		[Association(ThisKey="MealId", OtherKey="Id", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="fk_usermeal_mealid", BackReferenceName="fkusermealmealids")]
		public Meal Meal { get; set; }

		/// <summary>
		/// fk_usermeal_userid
		/// </summary>
		[Association(ThisKey="UserId", OtherKey="Id", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="fk_usermeal_userid", BackReferenceName="fkusermealuserids")]
		public User User { get; set; }

		#endregion
	}

	[Table(Schema="Meals", Name="Nutritions")]
	public partial class Nutrition
	{
		[PrimaryKey, NotNull    ] public Guid    Id            { get; set; } // uuid
		[Column,     NotNull    ] public double  Protein       { get; set; } // double precision
		[Column,     NotNull    ] public double  Carbohydrates { get; set; } // double precision
		[Column,     NotNull    ] public double  Fats          { get; set; } // double precision
		[Column,        Nullable] public double? VitaminA      { get; set; } // double precision
		[Column,        Nullable] public double? VitaminC      { get; set; } // double precision
		[Column,        Nullable] public double? VitaminB6     { get; set; } // double precision
		[Column,        Nullable] public double? VitaminD      { get; set; } // double precision

		#region Associations

		/// <summary>
		/// FK_MealIngredient_Nutritions_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="NutritionsId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<MealIngredient> MealIngredients { get; set; }

		#endregion
	}

	[Table(Schema="Users", Name="User")]
	public partial class User
	{
		[PrimaryKey, NotNull    ] public Guid           Id                          { get; set; } // uuid
		[Column,        Nullable] public string         UserName                    { get; set; } // character varying(20)
		[Column,     NotNull    ] public string         Name                        { get; set; } // character varying(20)
		[Column,     NotNull    ] public string         Surname                     { get; set; } // character varying(35)
		[Column,     NotNull    ] public string         FullName                    { get; set; } // character varying(56)
		[Column,     NotNull    ] public string         City                        { get; set; } // character varying(35)
		[Column,     NotNull    ] public int            CreatedMealsCount           { get; set; } // integer
		[Column,     NotNull    ] public int            CreatedMealIngredientsCount { get; set; } // integer
		[Column,     NotNull    ] public string         Password                    { get; set; } // character varying(100)
		[Column,     NotNull    ] public DateTimeOffset CreationDate                { get; set; } // timestamp (6) with time zone
		[Column,     NotNull    ] public DateTimeOffset LastLoginDate               { get; set; } // timestamp (6) with time zone
		[Column,        Nullable] public Guid?          ImageId                     { get; set; } // uuid
		[Column,     NotNull    ] public bool           Deleted                     { get; set; } // boolean

		#region Associations

		/// <summary>
		/// fk_favourites_userid_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="UserId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<Favourite> fkfavouritesuserids { get; set; }

		/// <summary>
		/// fk_friend_inviteduserid_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="InvitedUserId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<Friend> fkfriendinviteduserids { get; set; }

		/// <summary>
		/// fk_friend_invitinguserid_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="InvitingUserId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<Friend> fkfriendinvitinguserids { get; set; }

		/// <summary>
		/// fk_userachievement_user_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="UserId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<UserAchievement> fkuserachievementusers { get; set; }

		/// <summary>
		/// fk_useractivity_friend_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="FriendId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<UserActivity> fkuseractivityfriends { get; set; }

		/// <summary>
		/// fk_useractivity_user_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="UserId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<UserActivity> fkuseractivityusers { get; set; }

		/// <summary>
		/// fk_usermeal_userid_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="UserId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<MealScheduleEntry> fkusermealuserids { get; set; }

		/// <summary>
		/// fk_user_image
		/// </summary>
		[Association(ThisKey="ImageId", OtherKey="Id", CanBeNull=true, Relationship=Relationship.ManyToOne, KeyName="fk_user_image", BackReferenceName="fkuserimages")]
		public Image Image { get; set; }

		/// <summary>
		/// FK_MealIngredient_Creator_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="CreatorId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<MealIngredient> MealIngredientCreators { get; set; }

		/// <summary>
		/// FK_Meal_User_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="CreatorId", CanBeNull=true, Relationship=Relationship.OneToMany, IsBackReference=true)]
		public IEnumerable<Meal> Meals { get; set; }

		#endregion
	}

	[Table(Schema="Socials", Name="User-Achievement")]
	public partial class UserAchievement
	{
		[PrimaryKey(1), NotNull] public Guid AchievementId { get; set; } // uuid
		[PrimaryKey(2), NotNull] public Guid UserId        { get; set; } // uuid
		[Column,        NotNull] public bool Seen          { get; set; } // boolean

		#region Associations

		/// <summary>
		/// fk_userachievement_achievement
		/// </summary>
		[Association(ThisKey="AchievementId", OtherKey="Id", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="fk_userachievement_achievement", BackReferenceName="fkuserachievementachievements")]
		public Achievement Achievement { get; set; }

		/// <summary>
		/// fk_userachievement_user
		/// </summary>
		[Association(ThisKey="UserId", OtherKey="Id", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="fk_userachievement_user", BackReferenceName="fkuserachievementusers")]
		public User User { get; set; }

		#endregion
	}

	[Table(Schema="Socials", Name="UserActivity")]
	public partial class UserActivity
	{
		[PrimaryKey, Identity   ] public int   Id               { get; set; } // integer
		[Column,     NotNull    ] public Guid  UserId           { get; set; } // uuid
		[Column,        Nullable] public Guid? MealId           { get; set; } // uuid
		[Column,        Nullable] public Guid? MealIngredientId { get; set; } // uuid
		[Column,        Nullable] public Guid? FavouriteId      { get; set; } // uuid
		[Column,        Nullable] public Guid? FriendId         { get; set; } // uuid
		[Column,        Nullable] public Guid? AchievementId    { get; set; } // uuid

		#region Associations

		/// <summary>
		/// fk_useractivity_achievement
		/// </summary>
		[Association(ThisKey="AchievementId", OtherKey="Id", CanBeNull=true, Relationship=Relationship.ManyToOne, KeyName="fk_useractivity_achievement", BackReferenceName="fkuseractivityachievements")]
		public Achievement Achievement { get; set; }

		/// <summary>
		/// fk_useractivity_favourite
		/// </summary>
		[Association(ThisKey="FavouriteId", OtherKey="Id", CanBeNull=true, Relationship=Relationship.ManyToOne, KeyName="fk_useractivity_favourite", BackReferenceName="fkuseractivityfavourites")]
		public Meal Favourite { get; set; }

		/// <summary>
		/// fk_useractivity_friend
		/// </summary>
		[Association(ThisKey="FriendId", OtherKey="Id", CanBeNull=true, Relationship=Relationship.ManyToOne, KeyName="fk_useractivity_friend", BackReferenceName="fkuseractivityfriends")]
		public User Friend { get; set; }

		/// <summary>
		/// fk_useractivity_meal
		/// </summary>
		[Association(ThisKey="MealId", OtherKey="Id", CanBeNull=true, Relationship=Relationship.ManyToOne, KeyName="fk_useractivity_meal", BackReferenceName="fkuseractivitymeals")]
		public Meal Meal { get; set; }

		/// <summary>
		/// fk_useractivity_mealingredient
		/// </summary>
		[Association(ThisKey="MealIngredientId", OtherKey="Id", CanBeNull=true, Relationship=Relationship.ManyToOne, KeyName="fk_useractivity_mealingredient", BackReferenceName="fkuseractivitymealingredients")]
		public MealIngredient MealIngredient { get; set; }

		/// <summary>
		/// fk_useractivity_user
		/// </summary>
		[Association(ThisKey="UserId", OtherKey="Id", CanBeNull=false, Relationship=Relationship.ManyToOne, KeyName="fk_useractivity_user", BackReferenceName="fkuseractivityusers")]
		public User User { get; set; }

		#endregion
	}

	public static partial class TableExtensions
	{
		public static Achievement Find(this ITable<Achievement> table, Guid Id)
		{
			return table.FirstOrDefault(t =>
				t.Id == Id);
		}

		public static Favourite Find(this ITable<Favourite> table, Guid Id)
		{
			return table.FirstOrDefault(t =>
				t.Id == Id);
		}

		public static Friend Find(this ITable<Friend> table, Guid InvitingUserId, Guid InvitedUserId)
		{
			return table.FirstOrDefault(t =>
				t.InvitingUserId == InvitingUserId &&
				t.InvitedUserId  == InvitedUserId);
		}

		public static Image Find(this ITable<Image> table, Guid Id)
		{
			return table.FirstOrDefault(t =>
				t.Id == Id);
		}

		public static Meal Find(this ITable<Meal> table, Guid Id)
		{
			return table.FirstOrDefault(t =>
				t.Id == Id);
		}

		public static MealIngredient Find(this ITable<MealIngredient> table, Guid Id)
		{
			return table.FirstOrDefault(t =>
				t.Id == Id);
		}

		public static MealMealIngredient Find(this ITable<MealMealIngredient> table, Guid MealIngredientId, Guid MealId)
		{
			return table.FirstOrDefault(t =>
				t.MealIngredientId == MealIngredientId &&
				t.MealId           == MealId);
		}

		public static MealScheduleEntry Find(this ITable<MealScheduleEntry> table, Guid Id)
		{
			return table.FirstOrDefault(t =>
				t.Id == Id);
		}

		public static Nutrition Find(this ITable<Nutrition> table, Guid Id)
		{
			return table.FirstOrDefault(t =>
				t.Id == Id);
		}

		public static User Find(this ITable<User> table, Guid Id)
		{
			return table.FirstOrDefault(t =>
				t.Id == Id);
		}

		public static UserAchievement Find(this ITable<UserAchievement> table, Guid AchievementId, Guid UserId)
		{
			return table.FirstOrDefault(t =>
				t.AchievementId == AchievementId &&
				t.UserId        == UserId);
		}

		public static UserActivity Find(this ITable<UserActivity> table, int Id)
		{
			return table.FirstOrDefault(t =>
				t.Id == Id);
		}
	}
}

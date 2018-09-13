DROP SCHEMA "Users" CASCADE;
DROP SCHEMA "Images" CASCADE;
DROP SCHEMA "Meals" CASCADE;

CREATE SCHEMA "Users";
CREATE SCHEMA "Images";
CREATE SCHEMA "Meals";

CREATE TABLE "Users"."Role" (
    "Id" UUID PRIMARY KEY,
    "RoleName" TEXT NOT NULL
);

CREATE TABLE "Images"."Image" (
    "Id" UUID PRIMARY KEY,
    "Path" TEXT NOT NULL
);

CREATE TABLE "Users"."User" (
    "Id" UUID PRIMARY KEY,
    "Email" varchar(254) UNIQUE NOT NULL,
    "UserName" varchar(20) UNIQUE NOT NULL,
    "Password" TEXT NOT NULL,
    "CreationDate" TIMESTAMPTZ NOT NULL,
    "PhotoId" UUID NOT NULL,
    "RoleId" UUID NOT NULL
);
ALTER TABLE "Users"."User" 
ADD CONSTRAINT FK_User_Photo FOREIGN KEY ("PhotoId") REFERENCES "Images"."Image"("Id"),
ADD CONSTRAINT FK_User_Role FOREIGN KEY ("RoleId") REFERENCES "Users"."Role"("Id");

CREATE TABLE "Users"."Friend" (
    "Id" UUID PRIMARY KEY,
    "User1Id" UUID NOT NULL,
    "User2Id" UUID NOT NULL
);
ALTER TABLE "Users"."Friend" 
ADD CONSTRAINT FK_Friend_User1 FOREIGN KEY ("User1Id") REFERENCES "Users"."User"("Id"),
ADD CONSTRAINT FK_Friend_User2 FOREIGN KEY ("User2Id") REFERENCES "Users"."User"("Id"),
ADD CONSTRAINT Friend_UserIdsShouldBeDifferent CHECK ("User1Id" != "User2Id");

CREATE TABLE "Meals"."Meal" (
    "Id" UUID PRIMARY KEY,
    "PhotoId" UUID UNIQUE NOT NULL,
    "Name" TEXT NOT NULL,
    "Calories" REAL NOT NULL
);
ALTER TABLE "Meals"."Meal" 
ADD CONSTRAINT "FK_Meal_Photo" FOREIGN KEY ("PhotoId") REFERENCES  "Images"."Image"("Id");

CREATE TABLE "Meals"."MealIngredient" (
    "Id" UUID PRIMARY KEY,
    "PhotoId" UUID NOT NULL,
    "Name" TEXT,
    "Calories" INTEGER NOT NULL
);
ALTER TABLE "Meals"."MealIngredient" 
ADD CONSTRAINT "FK_MealIngredient_Photo" FOREIGN KEY ("PhotoId") REFERENCES  "Images"."Image"("Id");

CREATE TABLE "Meals"."Meal_MealIngredient" (
    "Id" UUID PRIMARY KEY,
    "MealIngredientId" UUID NOT NULL,
    "MealId" UUID NOT NULL
);
ALTER TABLE "Meals"."Meal_MealIngredient" 
ADD CONSTRAINT "FK_MealIngredientMeal_MealIngredient" FOREIGN KEY ("MealIngredientId") REFERENCES  "Meals"."MealIngredient"("Id"),
ADD CONSTRAINT "FK_MealIngredientMeal_Meal" FOREIGN KEY ("MealId") REFERENCES  "Meals"."Meal"("Id");

CREATE TABLE "Meals"."Nutritions" (
    "MealIngredientId" UUID PRIMARY KEY,
    "ProteinAmount" REAL NOT NULL,
    "Carbohydrates" REAL NOT NULL,
    "Fats" REAL NOT NULL,
    "VitaminA" REAL,
    "VitaminC" REAL,
    "VitaminB6" REAL,
    "VitaminD" REAL
);
ALTER TABLE "Meals"."Nutritions" 
ADD CONSTRAINT "FK_Nutritions_MealIngredient" FOREIGN KEY ("MealIngredientId") REFERENCES "Meals"."MealIngredient"("Id");
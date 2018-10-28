DROP SCHEMA "Users" CASCADE;
DROP SCHEMA "Images" CASCADE;
DROP SCHEMA "Meals" CASCADE;
DROP SCHEMA "Socials" CASCADE;

CREATE SCHEMA "Users";
CREATE SCHEMA "Images";
CREATE SCHEMA "Meals";
CREATE SCHEMA "Socials";


CREATE TABLE "Images"."Image" (
    "Id"    UUID    PRIMARY KEY,
    "Path"  TEXT    NOT NULL
);

CREATE TABLE "Users"."Role" (
    "Id"        UUID    PRIMARY KEY,
    "RoleName"  TEXT    NOT NULL
);

CREATE TABLE "Users"."User" (
    "Id"            UUID            PRIMARY KEY,
    "Email"         VARCHAR(254)    UNIQUE NULL,
    "UserName"      VARCHAR(20)     UNIQUE NULL,
    "Name"          VARCHAR(20)     NOT NULL,
    "Surname"       VARCHAR(35)     NOT NULL,
    "FullName"      VARCHAR(56)     NOT NULL,
    "Password"      TEXT            NOT NULL,
    "CreationDate"  TIMESTAMPTZ     NOT NULL,
    "ImageId"       UUID            NULL,
    "RoleId"        UUID            NOT NULL
);
ALTER TABLE "Users"."User" 
ADD CONSTRAINT FK_User_Image      FOREIGN KEY ("ImageId")     REFERENCES "Images"."Image"("Id"),
ADD CONSTRAINT FK_User_Role       FOREIGN KEY ("RoleId")      REFERENCES "Users"."Role"("Id"),
ADD CONSTRAINT UserName_Or_Email  CHECK (("Email" IS NOT NULL) OR ("UserName" IS NOT NULL));

CREATE TABLE "Socials"."Friend" (
    PRIMARY KEY ("User1Id", "User2Id"),
    "User1Id"       UUID        NOT NULL,
    "User2Id"       UUID        NOT NULL,
    "Confirmed"     BOOLEAN     NOT NULL DEFAULT FALSE,
    "CreationDate"  TIMESTAMPTZ NOT NULL
);
ALTER TABLE "Socials"."Friend" 
ADD CONSTRAINT FK_Friend_User1 FOREIGN KEY ("User1Id") REFERENCES "Users"."User"("Id"),
ADD CONSTRAINT FK_Friend_User2 FOREIGN KEY ("User2Id") REFERENCES "Users"."User"("Id"),
ADD CONSTRAINT Friend_FriendsIdsShouldBeDifferent CHECK ("User1Id" != "User2Id");

CREATE TABLE "Socials"."UserActivity" (
    "Id"            UUID        PRIMARY KEY,
    "UserId"        UUID        NOT NULL,
    "ActivityType"  TEXT        NOT NULL,
    "ContentId"     UUID        NOT NULL,
    "ActivityDate"  TIMESTAMPTZ NOT NULL
);
ALTER TABLE "Socials"."UserActivity" 
ADD CONSTRAINT FK_UserActivity_User FOREIGN KEY ("UserId") REFERENCES "Users"."User"("Id");

CREATE TABLE "Socials"."Achievement" (
    "Id"                UUID    PRIMARY KEY,
    "Category"          TEXT    NOT NULL,
    "Type"              TEXT    NOT NULL,
    "AchievementValue"  INTEGER NOT NULL
);

CREATE TABLE "Meals"."Meal" (
    "Id"            UUID        PRIMARY KEY,
    "CreationDate"  TIMESTAMPTZ NOT NULL,
    "CreatorId"     UUID        NULL,
    "ImageId"       UUID        NULL,
    "Name"          TEXT        NOT NULL,
    "Description"   TEXT        NULL,
    "Calories"      REAL        NOT NULL,
    -- races!
    "NumberOfUses"  INTEGER     NOT NULL DEFAULT 0
);
ALTER TABLE "Meals"."Meal" 
ADD CONSTRAINT "FK_Meal_Image" FOREIGN KEY ("ImageId") REFERENCES  "Images"."Image"("Id"),
ADD CONSTRAINT "FK_Meal_User" FOREIGN KEY ("CreatorId") REFERENCES  "Users"."User"("Id");

CREATE TABLE "Meals"."Favourites" (
    "Id"            UUID        PRIMARY KEY,
    "MealId"        UUID        NOT NULL,
    "UserId"        UUID        NOT NULL
);
ALTER TABLE "Meals"."Favourites"
ADD CONSTRAINT FK_Favourites_MealId FOREIGN KEY ("MealId") REFERENCES "Meals"."Meal"("Id") ON DELETE CASCADE,
ADD CONSTRAINT FK_Favourites_UserId FOREIGN KEY ("UserId") REFERENCES "Users"."User"("Id") ON DELETE CASCADE;

CREATE TABLE "Meals"."MealScheduleEntry" (
    "Id"        UUID            PRIMARY KEY,
    "UserId"    UUID            NOT NULL,
    "MealId"    UUID            NOT NULL,
    "Date"      TIMESTAMPTZ     NOT NULL
);
ALTER TABLE "Meals"."MealScheduleEntry"
ADD CONSTRAINT FK_UserMeal_MealId FOREIGN KEY ("MealId") REFERENCES "Meals"."Meal"("Id") ON DELETE CASCADE,
ADD CONSTRAINT FK_UserMeal_UserId FOREIGN KEY ("UserId") REFERENCES "Users"."User"("Id") ON DELETE CASCADE;

CREATE TABLE "Meals"."Nutritions" (
    "Id"            UUID    PRIMARY KEY,
    "Protein"       REAL    NOT NULL,
    "Carbohydrates" REAL    NOT NULL,
    "Fats"          REAL    NOT NULL,
    "VitaminA"      REAL,
    "VitaminC"      REAL,
    "VitaminB6"     REAL,
    "VitaminD"      REAL
);

CREATE TABLE "Meals"."MealIngredient" (
    "Id"            UUID        PRIMARY KEY,
    "ImageId"       UUID        NULL,
    "Name"          TEXT        UNIQUE,
    "Calories"      INTEGER     NOT NULL,
    "NutritionsId"  UUID        NOT NULL
);
ALTER TABLE "Meals"."MealIngredient" 
ADD CONSTRAINT "FK_MealIngredient_Image" FOREIGN KEY ("ImageId") REFERENCES  "Images"."Image"("Id"),
ADD CONSTRAINT "FK_MealIngredient_Nutritions" FOREIGN KEY ("NutritionsId") REFERENCES  "Meals"."Nutritions"("Id");

CREATE TABLE "Meals"."Meal-MealIngredient" (
    "Id"                UUID    PRIMARY KEY,
    "MealIngredientId"  UUID    NOT NULL,
    "MealId"            UUID    NOT NULL,
    "Quantity"          INTEGER NOT NULL
);
ALTER TABLE "Meals"."Meal-MealIngredient" 
ADD CONSTRAINT "FK_MealMealIngredient_MealIngredient-" FOREIGN KEY ("MealIngredientId") REFERENCES  "Meals"."MealIngredient"("Id") ON DELETE CASCADE,
ADD CONSTRAINT "FK_MealMealIngredient_Meal" FOREIGN KEY ("MealId") REFERENCES  "Meals"."Meal"("Id") ON DELETE CASCADE,
ADD CONSTRAINT "Quantity_MustBeGreaterThan0" CHECK ("Quantity" > 0);

CREATE VIEW "Meals"."MealIngredientsWithNutritions" AS
SELECT
    m."Id",
    m."Name",
    m."ImageId",
    m."Calories",
    n."Protein",
    n."Carbohydrates",
    n."Fats",
    n."VitaminA",
    n."VitaminC",
    n."VitaminB6",
    n."VitaminD"
FROM "Meals"."MealIngredient" m
JOIN "Meals"."Nutritions" n ON n."Id" = m."NutritionsId";

CREATE VIEW "Meals"."Meal-FullMealIngredient" AS
SELECT 
 	mmi."MealId",
    mmi."Quantity" as "Quantity",
    min."Id" as "MealIngredientId",
    min."Name" as "MealIngredientName",
    min."ImageId" as "MealIngredientImageId",
    min."Calories" as "MealIngredientCalories",
    min."Protein",
    min."Carbohydrates",
    min."Fats",
    min."VitaminA",
    min."VitaminC",
    min."VitaminB6",
    min."VitaminD"
FROM "Meals"."Meal-MealIngredient" mmi
JOIN "Meals"."MealIngredientsWithNutritions" min ON mmi."MealIngredientId" = min."Id";

-- built in values

INSERT INTO "Users"."Role"(
    "Id",
    "RoleName"
)
VALUES (
    '00000000-0000-0000-0000-000000000000',
    'DefaultRole'
);

INSERT INTO "Users"."User"(
    "Id", 
    "Email", 
    "UserName", 
    "Name",
    "Surname",
    "FullName",
    "Password", 
    "CreationDate", 
    "RoleId"
)
VALUES (
    '00000000-0000-0000-0000-000000000000',
    'ad@m.in',
    'ad@m.in',
    'Sebastian',
    'Łągiewski',
    'Sebastian Łągiewski',
    'password',
    '25.10.2018 20:58:57 +02:00',
    '00000000-0000-0000-0000-000000000000'
);

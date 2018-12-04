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

CREATE TABLE "Users"."User" (
    "Id"                            UUID            PRIMARY KEY,
    "UserName"                      VARCHAR(20)     UNIQUE,
    "Name"                          VARCHAR(20)     NOT NULL,
    "Surname"                       VARCHAR(35)     NOT NULL,
    "FullName"                      VARCHAR(56)     NOT NULL,
    "City"                          VARCHAR(35)     NOT NULL,
    "CreatedMealsCount"             INTEGER         NOT NULL DEFAULT 0,
    "CreatedMealIngredientsCount"   INTEGER         NOT NULL DEFAULT 0,
    "Password"                      VARCHAR(100)    NOT NULL,
    "CreationDate"                  TIMESTAMPTZ     NOT NULL,
    "LastLoginDate"                 TIMESTAMPTZ     NOT NULL,
    "ImageId"                       UUID            NULL,
    "IsAdmin"                       BOOL            NOT NULL DEFAULT FALSE,
    "Deleted"                       BOOL            NOT NULL DEFAULT FALSE
);
ALTER TABLE "Users"."User" 
ADD CONSTRAINT FK_User_Image      FOREIGN KEY ("ImageId")     REFERENCES "Images"."Image"("Id");

CREATE TABLE "Socials"."Friend" (
    PRIMARY KEY ("InvitingUserId", "InvitedUserId"),
    "InvitingUserId"    UUID            NOT NULL,
    "InvitedUserId"     UUID            NOT NULL,
    "Status"            VARCHAR(50)     NOT NULL,
    "CreationDate"      TIMESTAMPTZ     NOT NULL
);
ALTER TABLE "Socials"."Friend" 
ADD CONSTRAINT FK_Friend_InvitingUserId FOREIGN KEY ("InvitingUserId") REFERENCES "Users"."User"("Id"),
ADD CONSTRAINT FK_Friend_InvitedUserId FOREIGN KEY ("InvitedUserId") REFERENCES "Users"."User"("Id"),
ADD CONSTRAINT Friend_FriendsIdsShouldBeDifferent CHECK ("InvitingUserId" != "InvitedUserId");

CREATE TABLE "Socials"."Achievement" (
    "Id"                UUID        PRIMARY KEY,
    "Category"          TEXT        NOT NULL,
    "Type"              TEXT        NOT NULL,
    "Value"             INTEGER     NOT NULL
);

CREATE TABLE "Socials"."User-Achievement" (
    PRIMARY KEY ("AchievementId", "UserId"),
    "AchievementId"     UUID    NOT NULL,
    "UserId"            UUID    NOT NULL,
    "Seen"              BOOL    NOT NULL DEFAULT FALSE
);
ALTER TABLE "Socials"."User-Achievement" 
ADD CONSTRAINT FK_UserAchievement_User FOREIGN KEY ("UserId") REFERENCES "Users"."User"("Id"),
ADD CONSTRAINT FK_UserAchievement_Achievement FOREIGN KEY ("AchievementId") REFERENCES "Socials"."Achievement"("Id");

CREATE TABLE "Meals"."Meal" (
    "Id"                        UUID            PRIMARY KEY,
    "CreationDate"              TIMESTAMPTZ     NOT NULL,
    "CreatorId"                 UUID            NOT NULL,
    "ImageId"                   UUID            NULL,
    "Name"                      VARCHAR(50)     NOT NULL,
    "Description"               TEXT            NOT NULL,
    "Calories"                  INTEGER         NOT NULL,
    "NumberOfFavouriteMarks"    INTEGER         NOT NULL DEFAULT 0,
    "NumberOfUses"              INTEGER         NOT NULL DEFAULT 0
);
ALTER TABLE "Meals"."Meal" 
ADD CONSTRAINT "FK_Meal_Image" FOREIGN KEY ("ImageId") REFERENCES  "Images"."Image"("Id"),
ADD CONSTRAINT "FK_Meal_User" FOREIGN KEY ("CreatorId") REFERENCES  "Users"."User"("Id"),
ADD CONSTRAINT "Description_Length" CHECK (char_length("Description") <= 1000);

CREATE TABLE "Meals"."Favourite" (
    "Id"            UUID            PRIMARY KEY,
    "MealId"        UUID            NOT NULL,
    "UserId"        UUID            NOT NULL,
    "CreationDate"  TIMESTAMPTZ     NOT NULL
);
ALTER TABLE "Meals"."Favourite"
ADD CONSTRAINT FK_Favourites_MealId FOREIGN KEY ("MealId") REFERENCES "Meals"."Meal"("Id") ON DELETE CASCADE,
ADD CONSTRAINT FK_Favourites_UserId FOREIGN KEY ("UserId") REFERENCES "Users"."User"("Id") ON DELETE CASCADE,
ADD CONSTRAINT UQ_User_Meal UNIQUE("UserId", "MealId");

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
    "Id"            UUID                PRIMARY KEY,
    "Protein"       DOUBLE PRECISION    NOT NULL,
    "Carbohydrates" DOUBLE PRECISION    NOT NULL,
    "Fats"          DOUBLE PRECISION    NOT NULL,
    "VitaminA"      DOUBLE PRECISION    NULL,
    "VitaminC"      DOUBLE PRECISION    NULL,
    "VitaminB6"     DOUBLE PRECISION    NULL,
    "VitaminD"      DOUBLE PRECISION    NULL
);

CREATE TABLE "Meals"."MealIngredient" (
    "Id"            UUID        PRIMARY KEY,
    "CreatorId"     UUID        NULL,
    "ImageId"       UUID        NULL,
    "Name"          VARCHAR(30) UNIQUE,
    "Calories"      INTEGER     NOT NULL,
    "NutritionsId"  UUID        UNIQUE NOT NULL,
    "NumberOfUses"  INTEGER     NOT NULL
);
ALTER TABLE "Meals"."MealIngredient" 
ADD CONSTRAINT "FK_MealIngredient_Image" FOREIGN KEY ("ImageId") REFERENCES  "Images"."Image"("Id"),
ADD CONSTRAINT "FK_MealIngredient_Nutritions" FOREIGN KEY ("NutritionsId") REFERENCES  "Meals"."Nutritions"("Id"),
ADD CONSTRAINT "FK_MealIngredient_Creator" FOREIGN KEY ("CreatorId") REFERENCES  "Users"."User"("Id");

CREATE TABLE "Meals"."Meal-MealIngredient" (
    PRIMARY KEY ("MealIngredientId", "MealId"),
    "MealIngredientId"  UUID                NOT NULL,
    "MealId"            UUID                NOT NULL,
    "Quantity"          DOUBLE PRECISION    NOT NULL
);
ALTER TABLE "Meals"."Meal-MealIngredient" 
ADD CONSTRAINT "FK_MealMealIngredient_MealIngredient-" FOREIGN KEY ("MealIngredientId") REFERENCES  "Meals"."MealIngredient"("Id") ON DELETE CASCADE,
ADD CONSTRAINT "FK_MealMealIngredient_Meal" FOREIGN KEY ("MealId") REFERENCES  "Meals"."Meal"("Id") ON DELETE CASCADE,
ADD CONSTRAINT "Quantity_MustBeGreaterThan0" CHECK ("Quantity" > 0);

CREATE TABLE "Socials"."UserActivity" (
    "Id"                SERIAL          PRIMARY KEY,
    "UserId"            UUID            NOT NULL,
    "MealId"            UUID            NULL,
    "MealIngredientId"  UUID            NULL,
    "FavouriteId"       UUID            NULL,
    "FriendId"          UUID            NULL,
    "AchievementId"     UUID            NULL,
    "SeenByAdmin"       BOOL            DEFAULT FALSE
);
ALTER TABLE "Socials"."UserActivity" 
ADD CONSTRAINT FK_UserActivity_User FOREIGN KEY ("UserId") REFERENCES "Users"."User"("Id"),
ADD CONSTRAINT FK_UserActivity_Meal FOREIGN KEY ("MealId") REFERENCES "Meals"."Meal"("Id"),
ADD CONSTRAINT FK_UserActivity_MealIngredient FOREIGN KEY ("MealIngredientId") REFERENCES "Meals"."MealIngredient"("Id"),
ADD CONSTRAINT FK_UserActivity_Favourite FOREIGN KEY ("FavouriteId") REFERENCES "Meals"."Meal"("Id"),
ADD CONSTRAINT FK_UserActivity_Friend FOREIGN KEY ("FriendId") REFERENCES "Users"."User"("Id"),
ADD CONSTRAINT FK_UserActivity_Achievement FOREIGN KEY ("AchievementId") REFERENCES "Socials"."Achievement"("Id"),
ADD CONSTRAINT FK_UserActivity_AnyContent CHECK (
    ("MealId"           IS NOT NULL) OR 
    ("MealIngredientId" IS NOT NULL) OR 
    ("FavouriteId"      IS NOT NULL) OR 
    ("FriendId"         IS NOT NULL) OR 
    ("AchievementId"    IS NOT NULL)); 

CREATE VIEW "Meals"."MealIngredientsWithNutritions" AS
SELECT
    m."Id",
    m."Name",
    m."ImageId",
    m."Calories",
    m."CreatorId",
    m."NumberOfUses",
    n."Protein",
    n."Carbohydrates",
    n."Fats",
    n."VitaminA",
    n."VitaminC",
    n."VitaminB6",
    n."VitaminD"
FROM "Meals"."MealIngredient" m
JOIN "Meals"."Nutritions" n ON n."Id" = m."NutritionsId";

CREATE VIEW "Meals"."Meal-CompleteMealIngredient" AS
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

INSERT INTO "Users"."User"(
    "Id", 
    "City",
    "UserName", 
    "Name",
    "Surname",
    "FullName",
    "Password", 
    "LastLoginDate",
    "CreationDate",
    "IsAdmin"
)
VALUES 
(
    '00000000-0000-0000-0000-000000000000',
    'Admin',
    'ad',
    'Admin',
    'Admin',
    'Admin Admin',
    '$hash$20$W3ojGDwYQkjInL+4r7CRO876Hc2ApD2sLJYGCr2s9gAOyAIR',
    '25.10.2018 20:58:57 +02:00',
    '25.10.2018 20:58:57 +02:00',
    'true'
),
(
    '10000000-0000-0000-0000-000000000000',
    'Wroclaw',
    'ad2@m.in',
    'Marcepan',
    'Marcepański',
    'Marcepan Marcepański',
    'password',
    '25.10.2018 20:58:57 +02:00',
    '25.10.2018 20:58:57 +02:00',
    'false'
),
(
    '20000000-0000-0000-0000-000000000000',
    'Wroclaw',
    'ad3@m.in',
    'Klaudia',
    'Łągiewska',
    'Klaudia Łągiewski',
    'password',
    '25.10.2018 20:58:57 +02:00',
    '25.10.2018 20:58:57 +02:00',
    'false'
),
(
    '30000000-0000-0000-0000-000000000000',
    'Wroclaw',
    'ad4@m.in',
    'Dawid',
    'Lorek',
    'Dawid Lorek',
    'password',
    '25.10.2018 20:58:57 +02:00',
    '25.10.2018 20:58:57 +02:00',
    'false'
),
(
    '40000000-0000-0000-0000-000000000000',
    'Wroclaw',
    'ad5@m.in',
    'Przemyslaw',
    'Salata',
    'Przemyslaw Salata',
    'password',
    '25.10.2018 20:58:57 +02:00',
    '25.10.2018 20:58:57 +02:00',
    'false'
),
(
    '50000000-0000-0000-0000-000000000000',
    'Wroclaw',
    'ad6@m.in',
    'Karolina',
    'Kozica',
    'Karolina Kozica',
    'password',
    '25.10.2018 20:58:57 +02:00',
    '25.10.2018 20:58:57 +02:00',
    'false'
);

INSERT INTO "Socials"."Friend"(
    "InvitingUserId", 
    "InvitedUserId",
    "Status", 
    "CreationDate"
)
VALUES 
(
    '00000000-0000-0000-0000-000000000000',
    '10000000-0000-0000-0000-000000000000',
    'Accepted',
    '28.10.2018 20:58:57 +02:00'
),
(
    '00000000-0000-0000-0000-000000000000',
    '20000000-0000-0000-0000-000000000000',
    'Awaiting',
    '10.11.2018 20:58:57 +02:00'
),
(
    '00000000-0000-0000-0000-000000000000',
    '30000000-0000-0000-0000-000000000000',
    'Awaiting',
    '11.11.2018 20:58:57 +02:00'
);
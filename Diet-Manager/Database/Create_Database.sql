DROP SCHEMA "Users" CASCADE;
DROP SCHEMA "Images" CASCADE;

CREATE SCHEMA "Users";
CREATE SCHEMA "Images";

CREATE TABLE "Users"."User" (
    "UserId" UUID PRIMARY KEY,
    "Email" varchar(254) UNIQUE NOT NULL,
    "UserName" varchar(20) UNIQUE NOT NULL,
    "Password" TEXT NOT NULL  
);

CREATE TABLE "Images"."Image" (
    "ImageId" UUID PRIMARY KEY,
    "Path" TEXT NOT NULL
);
import Vue from "vue";
import Router from "vue-router";
import Home from "./views/Home.vue";
import MealManager from "./components/meal/MealManager.vue";
import AddMeal from "@/components/meal/AddMeal.vue";
import MyMeals from "@/components/meal/MyMeals.vue";
import FavouriteMeals from "@/components/meal/FavouriteMeals.vue";
import SearchMeals from "@/components/meal/SearchMeals.vue";
import FriendManager from "./components/user/friends/FriendManager.vue";
import MyFriends from "@/components/user/friends/MyFriends.vue";
import FriendInvitations from "@/components/user/friends/Invitations.vue";
import SearchUsers from "@/components/user/friends/SearchUsers.vue";
import MealSchedule from "./components/meal-schedule/MealSchedule.vue";
import NewsFeed from "./components/user/friends/NewsFeed.vue";
import UserPanel from "@/components/user/user-panel/UserPanel.vue";
import AccountManager from "@/components/user/user-panel/AccountManager.vue";
import UserAchievements from "@/components/user/user-panel/UserAchievements.vue";
import Friend from "@/components/user/friends/Friend.vue";
import Meal from "@/components/meal/Meal.vue";
import Login from "@/components/account/Login.vue";
import Registration from "@/components/account/Register.vue";
Vue.use(Router);

export default new Router({
  mode: "history",
  routes: [
    {
      path: "/",
      name: "Home",
      component: Home,
      redirect: { name: "MySchedule" }
    },
    {
      path: "/user/:userId",
      name: "Friend",
      component: Friend
    },
    {
      path: "/meal/:mealId",
      name: "Meal",
      component: Meal
    },

    {
      path: "/auth/register",
      name: "Register",
      component: Registration
    },
    {
      path: "/auth/login",
      name: "Login",
      component: Login
    },

    {
      path: "/user-panel",
      name: "UserPanel",
      component: UserPanel,
      redirect: { name: "AccountManager" },
      children: [
        {
          path: "manage",
          name: "AccountManager",
          component: AccountManager
        },
        {
          path: "achievements",
          name: "UserAchievements",
          component: UserAchievements
        }
      ]
    },
    {
      path: "/meal-manager",
      name: "MealManager",
      redirect: { name: "AddMeal" },
      component: MealManager,
      children: [
        {
          path: "add",
          name: "AddMeal",
          component: AddMeal
        },
        {
          path: "browse",
          name: "BrowseMeals",
          component: SearchMeals
        },
        {
          path: "my-meals",
          name: "MyMeals",
          component: MyMeals
        },
        {
          path: "favourite-meals",
          name: "FavouriteMeals",
          component: FavouriteMeals
        }
      ]
    },
    {
      path: "/friend-manager",
      name: "FriendManager",
      redirect: { name: "NewsFeed" },
      component: FriendManager,
      children: [
        {
          path: "news-feed",
          name: "NewsFeed",
          component: NewsFeed
        },
        {
          path: "search",
          name: "SearchUsers",
          component: SearchUsers
        },
        {
          path: "my-friends",
          name: "MyFriends",
          component: MyFriends
        },
        {
          path: "invitations",
          name: "FriendInvitations",
          component: FriendInvitations
        }
      ]
    },
    {
      path: "/my-schedule",
      name: "MySchedule",
      component: MealSchedule
    }
  ]
});

import Vue from "vue";
import Router from "vue-router";
import Home from "./views/Home.vue";
import MealManager from "./components/meal/MealManager.vue";
import AddMeal from "@/components/meal/AddMeal.vue";
import MyMeals from "@/components/meal/MyMeals.vue";
import SearchMeals from "@/components/meal/SearchMeals.vue";
import FriendManager from "./components/friend/FriendManager.vue";
import MealSchedule from "./components/meal-schedule/MealSchedule.vue";

Vue.use(Router);

export default new Router({
  mode: "history",
  routes: [
    {
      path: "/",
      name: "Home",
      component: Home
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
          name: "NewsFeed"
        },
        {
          path: "search",
          name: "SearchFriends"
        },
        {
          path: "my-friends",
          name: "MyFriends"
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

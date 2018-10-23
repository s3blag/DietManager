import Vue from "vue";
import Router from "vue-router";
import Home from "./views/Home.vue";
import MealManager from "./components/meal/MealManager.vue";
import AddMeal from "@/components/meal/AddMeal.vue";
import MyMeals from "@/components/meal/MyMeals.vue";

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
          name: "BrowseMeals"
        },
        {
          path: "my-meals",
          name: "MyMeals",
          component: MyMeals
        }
      ]
    }
  ]
});

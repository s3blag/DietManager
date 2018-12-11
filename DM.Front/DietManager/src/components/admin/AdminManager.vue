<template>
  <side-menu>
    <router-link tag="li" class="link" :to="{ name: 'AdminActivities'}">
      <font-awesome-icon id="add-icon" class="option-icon" icon="newspaper"/>Activities
    </router-link>

    <router-link tag="li" class="link" :to="{ name: 'AdminMeals'}">
      <font-awesome-icon id="search-icon" class="option-icon" icon="search"/>Meals
    </router-link>

    <router-link tag="li" class="link" :to="{ name: 'AdminMealIngredients'}">
      <font-awesome-icon id="book-icon" class="option-icon" icon="search"/>Ingredients
    </router-link>

    <router-link tag="li" class="link" :to="{ name: 'AdminUsers'}">
      <font-awesome-icon id="favourites-icon" class="option-icon" icon="search"/>Users
    </router-link>
  </side-menu>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import SideMenu from "@/components/common/SideMenu.vue";

@Component({
  components: {
    "side-menu": SideMenu
  }
})
export default class MealsManager extends Vue {}
</script>

<style scoped>
#add-icon {
  position: relative;
  right: 20px;
}
#search-icon {
  position: relative;
  right: 30px;
}
#book-icon {
  position: relative;
  right: 10px;
}
#favourites-icon {
  position: relative;
  right: 30px;
}
</style>
<template>
  <side-menu>
    <router-link tag="li" class="link" :to="{ name: 'AddMeal'}">
      <font-awesome-icon id="add-icon" class="option-icon" icon="plus-circle" /> Add
    </router-link>

    <router-link tag="li" class="link" :to="{ name: 'BrowseMeals'}">
      <font-awesome-icon id="search-icon" class="option-icon" icon="search" /> Browse
    </router-link>

    <router-link tag="li" class="link" :to="{ name: 'MyMeals'}">
      <font-awesome-icon id="book-icon" class="option-icon" icon="book" /> My Meals
    </router-link>

    <router-link tag="li" class="link" :to="{ name: 'FavouriteMeals'}">
      <font-awesome-icon id="book-icon" class="option-icon" icon="star" /> Favourite
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
  right: 30px;
}
#search-icon {
  position: relative;
  right: 16px;
}
#book-icon {
  position: relative;
  right: 10px;
}
</style>

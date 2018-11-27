<template>
  <div class="list-container">
    <h1 class="main-color">My Meals</h1>
    <h3 v-if="mealPreviews.length === 0 && lastReturned">You haven't added any meals yet</h3>
    <meal-preview-item
      v-else
      class="meal"
      v-for="mealPreview in mealPreviews"
      :key="mealPreview.id"
      :mealPreview="mealPreview"
      :enableFavouriteMarkToggling="false"
    />
    <button
      @click="getMealPreviews"
      class="load-more-button main-background-color"
      v-if="elementsRemainingToLoad"
    >Load more...</button>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

import MealPreview from "@/ViewModels/meal/mealPreview";
import MealApiCaller from "@/services/api-callers/mealApi";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import ImageApiCaller from "@/services/api-callers/imageApi";
import MealPreviewItem from "./MealPreviewItem.vue";

Component.registerHooks(["beforeRouteEnter"]);

@Component({
  components: {
    "meal-preview-item": MealPreviewItem
  }
})
export default class MyMeals extends Vue {
  private mealPreviews: MealPreview[] = [];
  private lastReturned: IndexedResult<MealPreview> | null = null;

  beforeRouteEnter(
    to: any,
    from: any,
    next: (onBeforeRouteEnter: (instance: MyMeals) => void) => void
  ) {
    next(instance => {
      instance.getMealPreviews();
    });
  }

  get isMobile() {
    if (window.innerWidth < 860) {
      return true;
    } else {
      return false;
    }
  }

  get elementsRemainingToLoad() {
    if (!this.lastReturned) {
      return true;
    } else {
      return !this.lastReturned.isLast;
    }
  }

  getMealPreviews() {
    MealApiCaller.getMealPreviews(
      this.lastReturned,
      this.getMealPreviewsSuccessHandler
    );
  }

  getMealPreviewsSuccessHandler(
    indexedMealPreviews: IndexedResult<MealPreview[]>
  ) {
    if (
      !indexedMealPreviews.result !== null ||
      indexedMealPreviews.result.length > 0
    ) {
      this.mealPreviews.push(...indexedMealPreviews.result);

      this.lastReturned = {
        result:
          indexedMealPreviews.result[indexedMealPreviews.result.length - 1],
        index: indexedMealPreviews.index,
        isLast: indexedMealPreviews.isLast
      };
    }
  }
}
</script>

<style lang="less">
h1 {
  margin-bottom: 15px !important;
}
.meals-container {
  padding: 10px;
  display: block;
  background-color: #e6e4e4;
  border-radius: 10px;
  min-height: 800px;
}
.meal {
  text-align: left;
  background-color: white;
  margin-bottom: 10px;
  border-radius: 10px;
  padding: 5px;
  display: flex;
  justify-content: space-between;
  text-align: center;
}
.load-more-button {
  border-radius: 7px;
  padding: 5px 10px 28px 10px;
  height: 30px;
  text-align: center;
  color: white;
}
</style>

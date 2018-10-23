<template>
  <div class="meals-container">
    <div class="meal" v-for="mealPreview in mealPreviews" :key="mealPreview.Id">
      <img :src="'/api/image/' + mealPreview.imageId">
      <span>{{mealPreview.Name}} </span>
      <span>{{mealPreview.Calories}} </span>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

import MealPreview from "@/ViewModels/meal/mealPreview";
import MealApiCaller from "@/services/api-callers/mealApi";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";

Component.registerHooks(["beforeRouteEnter"]);

@Component
export default class MyMeals extends Vue {
  private mealPreviews: MealPreview[] = [];
  private lastReturned: MealPreview | null = null;
  private isLast: boolean = false;

  beforeRouteEnter(
    to: any,
    from: any,
    next: (onBeforeRouteEnter: (instance: MyMeals) => void) => void
  ) {
    next(instance => {
      instance.getMealPreviews();
    });
  }

  getMealPreviews() {
    MealApiCaller.getMealPreviews(
      this.lastReturned,
      this.getMealPreviewsSuccessHandler
    );
  }

  getMealPreviewsSuccessHandler(
    indexedMealPreviews: IndexedResult<MealPreview>
  ) {
    // eslint-disable-next-line no-console
    console.log(indexedMealPreviews);
    if (
      !indexedMealPreviews.result === null ||
      indexedMealPreviews.result.length === 0
    ) {
      this.handleNoMealsFound();
    } else {
      this.mealPreviews.push(...indexedMealPreviews.result);
      this.lastReturned =
        indexedMealPreviews.result[indexedMealPreviews.result.length - 1];
      this.isLast = indexedMealPreviews.isLast;
    }
  }

  handleNoMealsFound() {
    // eslint-disable-next-line no-console
    console.log("No meals found");
  }
}
</script>

<style>
.meals-container {
  padding: 10px;
}
.meal {
  margin: 5px;
}
</style>

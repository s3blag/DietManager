// todo: reuse MyMealsComponent
<template>
  <div class="list-container">
    <h1 class="main-color">Favourite Meals</h1>
    <h3 v-if="mealPreviews.length === 0">You have no favourite meals</h3>
    <meal-preview-item
      v-else
      class="meal"
      v-for="mealPreview in mealPreviews"
      :key="mealPreview.id"
      :mealPreview="mealPreview"
      :enableFavouriteMarkToggling="asEmittingComponent ? false : true"
      @deleted-from-favourites="onDeletedFromFavourites"
      :emitEvents="asEmittingComponent"
      @selectedMeal="onMealSelected"
    />

    <button
      @click="getMealPreviews"
      class="load-more-button main-background-color"
      v-if="elementsRemainingToLoad && mealPreviews.length > 0"
    >Load more...</button>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

import MealPreview from "@/ViewModels/meal/mealPreview";
import FavouritesApiCaller from "@/services/api-callers/favouritesApi";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import ImageApiCaller from "@/services/api-callers/imageApi";
import MealPreviewItem from "./MealPreviewItem.vue";
import { Prop } from "vue-property-decorator";

Component.registerHooks(["beforeRouteEnter"]);

@Component({
  components: {
    "meal-preview-item": MealPreviewItem
  }
})
export default class MyMeals extends Vue {
  @Prop({ required: false, default: false })
  private asEmittingComponent!: boolean;
  private mealPreviews: MealPreview[] = [];
  private lastReturned: IndexedResult<MealPreview> | null = null;
  private selectedMealId: string | null = null;
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
    FavouritesApiCaller.get(
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

  onDeletedFromFavourites(guid: string) {
    const indexOfDeletedItem = this.mealPreviews.findIndex(m => m.id == guid);

    this.mealPreviews.splice(indexOfDeletedItem, 1);
  }

  onMealSelected(id: string) {
    const previousMeal = this.mealPreviews.find(
      m => m.id === this.selectedMealId
    );
    if (previousMeal) {
      previousMeal!.isSelected = false;
    }

    const meal = this.mealPreviews.find(m => m.id === id);
    meal!.isSelected = true;
    this.selectedMealId = meal!.id;

    this.$emit("meal-selected", meal!.id);
  }
}
</script>

<style lang="less" scoped>
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
h3 {
  margin-top: 100px !important;
  color: grey !important;
}
</style>

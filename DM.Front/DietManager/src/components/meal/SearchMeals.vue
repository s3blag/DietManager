<template>
  <div id="search-meals">
    <div id="search-input">
      <input @keyup.enter="searchMeals" type="text" class="form-control" id="mealSearchName" placeholder="Search..." v-model="searchQuery">
      <button id="searchMeals" class="btn main-background-color" @click="searchMeals">
        <font-awesome-icon id="search-icon" icon="search" />
      </button>
    </div>
    <meal-preview-item class="meal" v-for="mealPreview in mealPreviews" :key="mealPreview.id" :mealPreview="mealPreview" :enableFavouriteMarkToggling="true" />
    <button v-if="!isLast && lastReturned" @click="loadMore" class="load-more-button main-background-color">
      Load more...
    </button>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Watch } from "vue-property-decorator";
import MealPreviewItem from "./MealPreviewItem.vue";
import MealPreviewWithImage from "@/ViewModels/meal/mealPreviewWithImage";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import MealPreview from "@/ViewModels/meal/mealPreview";
import MealApiCaller from "@/services/api-callers/mealApi";
import ImageApiCaller from "@/services/api-callers/imageApi";
import MealSearch from "@/ViewModels/meal/mealSearch";

@Component({
  components: {
    "meal-preview-item": MealPreviewItem
  }
})
export default class SearchMeals extends Vue {
  private mealPreviews: MealPreviewWithImage[] = [];
  private lastReturned: IndexedResult<MealPreview> | null = null;
  private searchQuery: string = "";
  private previousSearchQuery: string = "";

  get isLast() {
    if (!this.lastReturned) {
      return false;
    } else {
      return this.lastReturned.isLast;
    }
  }

  get isMobile() {
    if (window.innerWidth < 860) {
      return true;
    } else {
      return false;
    }
  }

  get searchIndex() {
    if (!this.lastReturned) {
      return 0;
    } else {
      return this.lastReturned.index;
    }
  }

  get isQueryEmpty() {
    return this.searchQuery.length === 0 || !this.searchQuery.trim();
  }

  searchMeals() {
    if (
      this.isQueryEmpty ||
      this.searchQuery.length < 2 ||
      this.searchQuery === this.previousSearchQuery
    ) {
      return;
    }

    this.lastReturned = null;
    this.mealPreviews = [];
    this.previousSearchQuery = this.searchQuery;

    const lastReturnedSearch: IndexedResult<MealSearch> = {
      result: { query: this.searchQuery },
      isLast: this.isLast,
      index: this.searchIndex
    };

    MealApiCaller.search(
      lastReturnedSearch,
      this.getMealPreviewsSuccessHandler
    );
  }

  loadMore() {
    const lastReturnedSearch: IndexedResult<MealSearch> = {
      result: { query: this.previousSearchQuery },
      isLast: this.isLast,
      index: this.searchIndex
    };

    MealApiCaller.search(
      lastReturnedSearch,
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
      this.mealPreviews.push(
        ...indexedMealPreviews.result.map(mealPreview => {
          const mealPreviewWithImage = {
            mealPreview: mealPreview,
            imageData: null
          } as MealPreviewWithImage;
          this.getMealPreviewImage(mealPreviewWithImage, mealPreview.imageId);
          return mealPreviewWithImage;
        })
      );

      this.lastReturned = {
        result:
          indexedMealPreviews.result[indexedMealPreviews.result.length - 1],
        index: indexedMealPreviews.index,
        isLast: indexedMealPreviews.isLast
      };
    }
  }

  getMealPreviewImage(
    mealPreviewWithImage: MealPreviewWithImage,
    imageGuid: string | null
  ) {
    if (imageGuid !== null) {
      ImageApiCaller.get(imageGuid, imageData => {
        mealPreviewWithImage.imageData = imageData;
      });
    }
  }
}
</script>

<style lang="less" scoped>
#search-meals {
  padding: 10px;
  background-color: #e6e4e4;
  border-radius: 10px;
  min-height: 600px;
}
.load-more-button {
  border-radius: 7px;
  padding: 5px 10px 28px 10px;
  height: 30px;
  text-align: center;
  color: white;
}
#search-input {
  display: inline-flex;
  margin: 10px;
}
#searchMeals {
  border-radius: 0px 5px 5px 0px;
}
#mealSearchName {
  border-radius: 5px 0px 0px 5px;
}
#search-icon {
  color: white;
}
</style>

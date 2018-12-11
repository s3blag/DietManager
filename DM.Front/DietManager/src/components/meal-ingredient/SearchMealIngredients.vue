<template>
  <div id="search-meal-ingredients">
    <div>
      <h2 class="main-color">Ingredients Manager</h2>
      <modal v-if="showAddMealIngredientModal">
        <add-meal-ingredient
          id="add-meal-ingredient"
          @cancel="showAddMealIngredientModal = false"
          @meal-ingredient-added="mealIngredientAddedHandler"
        ></add-meal-ingredient>
      </modal>
      <div id="search-input">
        <input
          @keyup.enter="searchMealIngredients"
          type="text"
          class="form-control"
          id="mealSearchName"
          placeholder="Search..."
          v-model="searchQuery"
        >
        <button id="searchMealIngredients" class="button" @click="searchMealIngredients">
          <font-awesome-icon id="search-icon" icon="search"/>
        </button>
      </div>
      <h3 v-if="mealIngredients.length === 0 && lastReturned">No meal ingredients were found</h3>
      <div id="ingredients-container">
        <meal-ingredient-preview-item
          class="meal-ingredient"
          v-for="mealIngredient in mealIngredients"
          :key="mealIngredient.id"
          :mealIngredient="mealIngredient"
          @meal-ingredient-added="mealIngredientAddedHandler"
          @meal-ingredient-deleted="onMealIngredientDeleted"
        />
        <button
          v-if="!isLast && lastReturned"
          @click="loadMore"
          class="load-more-button main-background-color"
        >Load more...</button>
      </div>
    </div>
    <div>
      <div class="modal-buttons-container" v-if="!isAdmin">
        <button class="button" @click="$emit('cancel')">Back</button>
        <button class="button" @click="showAddMealIngredientModal = true">+</button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Watch, Prop } from "vue-property-decorator";
import MealIngredientPreviewItem from "./MealIngredientPreviewItem.vue";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import MealIngredientSearch from "@/ViewModels/meal-ingredient/mealIngredientSearch";
import MealIngredient from "@/ViewModels/meal-ingredient/mealIngredient";
import MealIngredientApiCaller from "@/services/api-callers/mealIngredientApi";
import AddMealIngredient from "@/components/meal-ingredient/AddMealIngredient.vue";
import MealIngredientWithQuantity from "@/ViewModels/meal-ingredient/mealIngredientWithQuantity";
import Modal from "@/components/common/Modal.vue";

@Component({
  components: {
    "meal-ingredient-preview-item": MealIngredientPreviewItem,
    "add-meal-ingredient": AddMealIngredient,
    modal: Modal
  }
})
export default class SearchMealIngredients extends Vue {
  private mealIngredients: MealIngredient[] = [];
  private lastReturned: IndexedResult<MealIngredient> | null = null;
  private searchQuery: string = "";
  private previousSearchQuery: string = "";
  private selectedMealIngredient: MealIngredientWithQuantity | null = null;
  private showAddMealIngredientModal = false;

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

  get isAdmin() {
    return this.$route.meta && this.$route.meta.asAdmin;
  }

  get isQueryEmpty() {
    return this.searchQuery.length === 0 || !this.searchQuery.trim();
  }

  searchMealIngredients() {
    if (
      this.isQueryEmpty ||
      this.searchQuery.length < 2 ||
      this.searchQuery === this.previousSearchQuery
    ) {
      return;
    }

    this.lastReturned = null;
    this.mealIngredients = [];
    this.previousSearchQuery = this.searchQuery;

    const lastReturnedSearch: IndexedResult<MealIngredientSearch> = {
      result: { query: this.searchQuery },
      isLast: this.isLast,
      index: this.searchIndex
    };

    MealIngredientApiCaller.search(
      lastReturnedSearch,
      this.getMealIngredientsSuccessHandler
    );
  }

  loadMore() {
    const lastReturnedSearch: IndexedResult<MealIngredientSearch> = {
      result: { query: this.previousSearchQuery },
      isLast: this.isLast,
      index: this.searchIndex
    };

    MealIngredientApiCaller.search(
      lastReturnedSearch,
      this.getMealIngredientsSuccessHandler
    );
  }

  getMealIngredientsSuccessHandler(
    indexedMealIngredientPreviews: IndexedResult<MealIngredient[]>
  ) {
    if (
      !indexedMealIngredientPreviews.result !== null ||
      indexedMealIngredientPreviews.result.length > 0
    ) {
      this.mealIngredients.push(...indexedMealIngredientPreviews.result);

      this.lastReturned = {
        result:
          indexedMealIngredientPreviews.result[
            indexedMealIngredientPreviews.result.length - 1
          ],
        index: indexedMealIngredientPreviews.index,
        isLast: indexedMealIngredientPreviews.isLast
      };
    }
  }

  mealIngredientAddedHandler(
    addedMealIngredientWithQuantity: MealIngredientWithQuantity
  ) {
    this.$emit("meal-ingredient-added", addedMealIngredientWithQuantity);
  }

  onMealIngredientDeleted(id: string) {
    const indexOfDeletedItem = this.mealIngredients.findIndex(m => m.id == id);
    this.mealIngredients.splice(indexOfDeletedItem, 1);
  }
}
</script>

<style lang="less" scoped>
#ingredients-container {
  height: 390px;
  overflow-y: auto;
}
#search-meal-ingredients {
  padding: 10px;
  background-color: #e6e4e4;
  border-radius: 10px;
  min-height: 600px;
  width: 100%;
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
.add-mealingredient-modal {
  width: 100%;
}
h3 {
  color: grey;
  margin-top: 50px;
}
.modal-buttons-container {
  button {
    width: 75px;
    height: 35px;
  }
}
.meal-ingredient {
  width: 50%;
  max-width: 750px;
  text-align: left;
  background-color: white;
  margin: 0px auto 10px auto;
  border-radius: 10px;
  padding: 5px;
  display: flex;
  justify-content: space-between;
  text-align: center;
}
</style>

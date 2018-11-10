<template>
  <div class="add-meal">
    <modal v-if="showAddMealIngredientModal" class="modal-window">
      <add-meal-ingredient id="add-meal-ingredient" @cancel="showAddMealIngredientModal = false" @meal-ingredient-added="mealIngredientAddedHandler"></add-meal-ingredient>
    </modal>
    <div class="form-container content-background">
      <div class="column left">
        <form>
          <div class="form-group">
            <label for="mealName">Name</label>
            <input type="text" class="form-control" id="mealName" placeholder="Enter meal name..." v-model="mealFormData.name">
          </div>
          <div class="form-group">
            <label for="mealDescription">Description</label>
            <textarea class="form-control" height="200" width="200" id="mealDescription" placeholder="Enter meal description..." rows="10" v-model="mealFormData.description"></textarea>
          </div>
        </form>
      </div>
      <div class="column right">
        <div class="picture-input">
          <picture-input ref="pictureInput" @change="onPictureChange" width="400" height="200" radius="5" accept="image/jpeg,image/png" size="10" :removable="true" :customStrings="{
            drag: '+'
          }">
          </picture-input>
        </div>
        <div class="meal-ingredients-container">
          <div>
            <label for="mealName">Add Meal Ingredients</label>
            <div class="meal-ingredients-search">
              <input type="text" class="form-control" id="mealIngredientSearchName" placeholder="Search..." v-model="mealIngredientSearchQuery">
              <button id="searchMealIngredients" class="btn main-background-color" @click="searchMealIngredients">
                <font-awesome-icon id="search-icon" icon="search" />
              </button>
            </div>
          </div>
          <ul class="added-meal-ingredients">
            <li class="meal-ingredient" v-for="mealIngredientWithQuantity in addedMealIngredients" :key="mealIngredientWithQuantity.mealIngredient.id">
              <div class="image"></div>
              <span class="name">{{mealIngredientWithQuantity.mealIngredient.name}}</span>
              <span class="quantity">{{mealIngredientWithQuantity.quantity}}</span>
            </li>
          </ul>
        </div>
      </div>
      <button id="addMealButton" type="submit" class="btn main-background-color" @click="submit">Add Meal</button>
    </div>
    <meal-summary class="summary" :mealIngredients="addedMealIngredients" />
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { Watch, Model, Component } from "vue-property-decorator";
import PictureInput from "vue-picture-input";
import _ from "lodash";

import MealCreation from "@/ViewModels/meal/mealCreation";
import MealIngredient from "@/ViewModels/meal-ingredient/mealIngredient";
import MealApiCaller from "@/services/api-callers/mealApi";
import imageApiCaller from "@/services/api-callers/imageApi";
import MealLookup from "@/ViewModels/meal/mealLookup";
import MealSummary from "@/components/meal/MealSummary.vue";
import AddMealIngredient from "@/components/meal-ingredient/AddMealIngredient.vue";
import MealIngredientWithQuantity from "@/ViewModels/meal-ingredient/mealIngredientWithQuantity";
import MealIngredientIdWithQuantity from "@/ViewModels/meal-ingredient/mealIngredientIdWithQuantity";
import MealIngredientApiCaller from "@/services/api-callers/mealIngredientApi";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import MealIngredientSearch from "@/ViewModels/meal-ingredient/mealIngredientSearch";
import Modal from "@/components/common/Modal.vue";

@Component({
  components: {
    modal: Modal,
    "meal-summary": MealSummary,
    "add-meal-ingredient": AddMealIngredient,
    "picture-input": PictureInput
  }
})
export default class AddMeal extends Vue {
  private showAddMealIngredientModal: boolean = false;
  private mealFormData: MealCreation = {
    name: "",
    description: ""
  } as MealCreation;
  private addedMealIngredients: MealIngredientWithQuantity[] = [];

  private mealIngredientSearchQuery: string = "";
  private lastReturnedMealIngredientSearch: IndexedResult<
    MealIngredientSearch
  > | null = null;
  private foundMealIngredients: MealIngredient[] = [];

  @Watch("mealIngredientSearchQuery")
  onSearchQueryChanged() {
    this.lastReturnedMealIngredientSearch = null;
  }

  get mealCalories() {
    return _.sum(
      this.addedMealIngredients.map(
        ingredientWithQuantity =>
          ingredientWithQuantity.mealIngredient.calories *
          ingredientWithQuantity.quantity
      )
    );
  }

  submit() {
    const { name, description, imageId } = this.mealFormData;
    const completeMealCreation = {
      calories: this.mealCalories,
      name: name,
      description: description,
      imageId: imageId,
      ingredientsIdsWithQuantity: this.addedMealIngredients.map(
        mealIngredient => {
          return {
            id: mealIngredient.mealIngredient.id,
            quantity: mealIngredient.quantity
          } as MealIngredientIdWithQuantity;
        }
      )
    } as MealCreation;

    MealApiCaller.add(completeMealCreation, this.addMealSuccessHandler);
  }

  addMealSuccessHandler(addedMealGuid: string) {
    // eslint-disable-next-line no-console
    console.log(addedMealGuid);
  }

  onPictureChange(base64ImageString: string) {
    imageApiCaller.add(base64ImageString, this.addImageSuccessHandler);
  }

  addImageSuccessHandler(imageId: string) {
    this.mealFormData.imageId = imageId;
  }

  mealIngredientAddedHandler(
    addedMealIngredientWithQuantity: MealIngredientWithQuantity
  ) {
    this.addedMealIngredients.push(addedMealIngredientWithQuantity);
    this.showAddMealIngredientModal = false;
  }

  searchMealIngredients() {
    this.showAddMealIngredientModal = true;
    // MealIngredientApiCaller.search(
    //   {
    //     isLast: false,
    //     index: 0,
    //     result: { query: this.mealIngredientSearchQuery }
    //   },
    //   this.mealIngredientFoundHandler
    // );
  }

  loadMoreSearchMealIngredientResults() {
    MealIngredientApiCaller.search(
      this.lastReturnedMealIngredientSearch,
      this.mealIngredientFoundHandler
    );
  }

  //on search text change delete lest returned
  mealIngredientFoundHandler(
    indexedSearchResults: IndexedResult<MealIngredient[]>
  ) {
    this.foundMealIngredients.push(...indexedSearchResults.result);
    this.lastReturnedMealIngredientSearch = {
      result: { query: this.mealIngredientSearchQuery },
      isLast: indexedSearchResults.isLast,
      index: indexedSearchResults.index
    };
  }
}
</script>

<style lang="less" scoped>
.add-meal {
  font-size: 18px;
  width: 100%;
  display: flex;
  float: left;
}
.form-container {
  margin: 0px 20px;
  padding: 10px;
  width: 82%;
  height: 750px;
  border-radius: 10px;
}
.summary {
  overflow: hidden;
  margin: 0px 20px;
  padding: 20px;
  width: 13%;
  height: fit-content;
}
.add-meal:after {
  content: "";
  display: table;
  clear: both;
}
button {
  color: white;
}
label {
  display: block;
  text-align: left;
}
#mealDescription {
  height: 500px;
  resize: none;
}
.column {
  float: left;
  height: 100%;
}
.column.left {
  width: 60%;
  padding: 10px;
}
.column.right {
  width: 40%;
  padding-left: 15px;
  padding-right: 15px;
}
.column.righ:after {
  content: "";
  display: table;
  clear: both;
}
.picture-input {
  margin: 15px auto 10px auto;
}
#addMealButton {
  display: block;
  position: relative;
  top: -50px;
  margin: 0 auto;
}
.meal-ingredients-container {
  padding-top: 15px;
}
.meal-ingredients-search {
  display: inline-flex;
  width: 100%;
}
#searchMealIngredients {
  border-radius: 0px 5px 5px 0px;
}
#mealIngredientSearchName {
  border-radius: 5px 0px 0px 5px;
}
.meal-ingredient {
  width: 100%;
  display: inline-flex;
  border-bottom-style: solid;
  border-bottom-width: 1px;
}
.meal-ingredient .quantity {
  text-align: right;
}
.meal-ingredient .name {
  width: 100%;
  text-align: left;
}
.added-meal-ingredients {
  padding-top: 50px;
  padding-left: 0px;
}
#add-meal-ingredient {
  width: 400px;
  min-width: 320px;
  height: 98%;
}
@media screen and (max-width: 860px) {
  .form-container {
    width: 98%;
  }
  .summary {
    width: 98%;
    margin-bottom: 20px;
  }

  .add-meal {
    display: flex;
    flex-wrap: wrap-reverse;
  }
  #add-meal-ingredient {
    max-width: 250px !important;
    overflow: scroll;
  }
}
</style>

<style lang="less">
.content-background {
  background-color: #e6e4e4;
}
</style>

<template>
  <div class="add-meal">
    <meal-ingredients-manager
      v-if="showMealIngredientsManager"
      @cancel="showMealIngredientsManager = false"
      @meal-ingredient-added="mealIngredientAddedHandler"
    />

    <div v-else class="form-container content-background">
      <h2 class="main-color">Add Meal</h2>
      <div class="column left">
        <form>
          <div class="form-group">
            <label for="mealName">Name</label>
            <input
              type="text"
              class="form-control"
              id="mealName"
              placeholder="Enter meal name..."
              v-model="mealFormData.name"
            >
          </div>
          <div class="form-group">
            <label for="mealDescription">Recipe</label>
            <textarea
              class="form-control"
              height="200"
              width="200"
              id="mealDescription"
              placeholder="Enter meal description..."
              rows="10"
              v-model="mealFormData.description"
            ></textarea>
          </div>
        </form>
      </div>
      <div class="column right">
        <div class="picture-input">
          <picture-input
            ref="pictureInput"
            @change="onPictureChange"
            width="400"
            height="200"
            radius="5"
            accept="image/jpeg, image/png"
            size="10"
            :removable="true"
            :custom-strings="{
            drag: '+'
          }"
          ></picture-input>
        </div>
        <div class="meal-ingredients-container">
          <div>
            <h5 class="soft-border top">Added Meal Ingredients:</h5>
          </div>
          <ul class="added-meal-ingredients">
            <li
              class="meal-ingredient"
              v-for="mealIngredientWithQuantity in addedMealIngredients"
              :key="mealIngredientWithQuantity.mealIngredient.id"
            >
              <div class="image"></div>
              <span class="name">{{mealIngredientWithQuantity.mealIngredient.name}}</span>
              <span class="quantity">{{mealIngredientWithQuantity.quantity}}</span>
            </li>
          </ul>
        </div>
      </div>
      <div class="buttons-container">
        <button
          id="addMealButton"
          type="submit"
          :class="!formValid ? 'disabled' : ''"
          class="button"
          @click="submit"
        >Add Meal</button>
        <button type="submit" class="button" @click="showMealIngredientsManager = true">Next</button>
      </div>
    </div>
    <meal-summary class="summary" :mealIngredients="addedMealIngredients"/>
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

import MealIngredientWithQuantity from "@/ViewModels/meal-ingredient/mealIngredientWithQuantity";
import MealIngredientIdWithQuantity from "@/ViewModels/meal-ingredient/mealIngredientIdWithQuantity";
import MealIngredientApiCaller from "@/services/api-callers/mealIngredientApi";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import MealIngredientSearch from "@/ViewModels/meal-ingredient/mealIngredientSearch";
import Modal from "@/components/common/Modal.vue";
import MealIngredientsManager from "@/components/meal-ingredient/SearchMealIngredients.vue";

@Component({
  components: {
    modal: Modal,
    "meal-summary": MealSummary,
    "meal-ingredients-manager": MealIngredientsManager,
    "picture-input": PictureInput
  }
})
export default class AddMeal extends Vue {
  private mealFormData: MealCreation = {
    name: "",
    description: ""
  } as MealCreation;
  private addedMealIngredients: MealIngredientWithQuantity[] = [];
  private showMealIngredientsManager = false;

  get mealCalories() {
    return _.sum(
      this.addedMealIngredients.map(
        ingredientWithQuantity =>
          ingredientWithQuantity.mealIngredient.calories *
          ingredientWithQuantity.quantity
      )
    );
  }

  get formValid() {
    return (
      this.addedMealIngredients.length > 0 &&
      this.mealFormData.name.length > 2 &&
      this.mealFormData.description.length > 5
    );
  }

  submit() {
    if (!this.formValid) {
      return;
    }
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
    this.$router.push({ path: "/meal/" + addedMealGuid });
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
    this.showMealIngredientsManager = false;
  }
}
</script>

<style lang="less" scoped>
h5 {
  margin-top: 5px;
  padding-top: 10px;
}
.add-meal {
  font-size: 18px;
  width: 100%;
  display: flex;
  float: left;
}
.form-container {
  margin: 0px 5px;
  padding: 10px;
  width: 100%;
  height: 780px;
  border-radius: 10px;
}
.summary {
  overflow: hidden;
  margin: 0px 15px;
  padding: 20px;
  min-width: 170px;
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
  color: rgb(119, 119, 119);
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
.buttons-container {
  position: relative;
  top: -105px;
  height: 65px;
  display: inline-flex;
  width: 300px;
  margin: 0px auto;

  button {
    height: 45px;
    width: 85px;
    font-size: 0.9em;
    margin: 0px 10px;
  }
}
.disabled {
  background-color: grey;
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

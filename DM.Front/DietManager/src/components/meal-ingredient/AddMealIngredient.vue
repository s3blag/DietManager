<template>
  <div id="add-meal-ingredient">
    <div id="add-meal-ingredient-form">
      <div class="form-input">
        <div class="label">Name</div>
        <input type="text" class="form-control" placeholder="Enter meal ingredient name..." v-model="mealIngredientFormData.mealIngredient.name">
      </div>
      <div class="form-input">
        <div class="label">Quantity</div>
        <input type="number" class="form-control" placeholder="Enter quantity..." v-model="mealIngredientFormData.quantity">
      </div>
      <div class="form-input">
        <div class="label">Calories</div>
        <input type="number" class="form-control" placeholder="Enter number of calories..." v-model="mealIngredientFormData.mealIngredient.calories">
      </div>
      <div class="form-input">
        <div class="label">Proteins</div>
        <input type="number" class="form-control" placeholder="Enter amount of proteins..." v-model="mealIngredientFormData.mealIngredient.nutrition.protein">
      </div>
      <div class="form-input">
        <div class="label">Carbohydrates</div>
        <input type="number" class="form-control" placeholder="Enter amount of carbohydrates..." v-model="mealIngredientFormData.mealIngredient.nutrition.carbohydrates">
      </div>
      <div class="form-input">
        <div class="label">Fats</div>
        <input type="number" class="form-control" placeholder="Enter amount of fats..." v-model="mealIngredientFormData.mealIngredient.nutrition.fats">
      </div>
      <div class="form-input">
        <div class="label">Vitamin A (optional)</div>
        <input type="number" class="form-control" placeholder="Enter amount of Vitamin A..." v-model="mealIngredientFormData.mealIngredient.nutrition.vitaminA">
      </div>
      <div class="form-input">
        <div class="label">Vitamin C (optional)</div>
        <input type="number" class="form-control" placeholder="Enter amount of Vitamin C..." v-model="mealIngredientFormData.mealIngredient.nutrition.vitaminC">
      </div>
      <div class="form-input">
        <div class="label">Vitamin B6 (optional)</div>
        <input type="number" class="form-control" placeholder="Enter amount of Vitamin B6..." v-model="mealIngredientFormData.mealIngredient.nutrition.vitaminB6">
      </div>
      <div class="form-input">
        <div class="label">Vitamin D (optional)</div>
        <input type="number" class="form-control" placeholder="Enter amount of Vitamin D..." v-model="mealIngredientFormData.mealIngredient.nutrition.vitaminD">
      </div>
    </div>
    <div id="buttons-container">
      <button class="btn main-background-color" @click="addMealIngredient">Add</button>
      <button class="btn main-background-color" @click="cancel">Close</button>
    </div>
  </div>
</template>
<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import MealIngredientApiCaller from "@/services/api-callers/mealIngredientApi";
import MealIngredient from "@/ViewModels/meal-ingredient/mealIngredient";
import MealIngredientCreationWithQuantity from "@/ViewModels/meal-ingredient/mealIngredientCreationWithQuantity";
import MealIngredientWithQuantity from "@/ViewModels/meal-ingredient/mealIngredientWithQuantity";
import { Emit } from "vue-property-decorator";

@Component
export default class AddMealIngredient extends Vue {
  private mealIngredientFormData: MealIngredientCreationWithQuantity = {
    quantity: 1,
    mealIngredient: {
      nutrition: {
        vitaminC: null,
        vitaminB6: null,
        vitaminA: null,
        vitaminD: null
      }
    }
  } as MealIngredientCreationWithQuantity;

  addMealIngredient() {
    MealIngredientApiCaller.add(
      this.mealIngredientFormData.mealIngredient,
      this.addMealIngredientSuccessHandler
    );
  }

  addMealIngredientSuccessHandler(guid: string) {
    const mealIngredientCreation = this.mealIngredientFormData;
    const addedMealIngredientWithQuantity = {
      mealIngredient: {
        id: guid,
        imageId: mealIngredientCreation.mealIngredient.imageId,
        name: mealIngredientCreation.mealIngredient.name,
        calories: mealIngredientCreation.mealIngredient.calories,
        nutrition: mealIngredientCreation.mealIngredient.nutrition
      } as MealIngredient,
      quantity: mealIngredientCreation.quantity
    } as MealIngredientWithQuantity;

    this.$emit("meal-ingredient-added", addedMealIngredientWithQuantity);
  }

  @Emit()
  cancel() {
    return;
  }
}
</script>
<style lang="less" scoped>
#add-meal-ingredient-form {
  height: 100%;
  flex-wrap: wrap;
  width: 90%;
  margin: 0 auto;
}
#buttons-container {
  margin: 15px 0px 5px 0px;
  width: 100%;
  height: 50px;
  justify-content: center;
  display: flex;

  button {
    margin: 0px 25px 0px 25px;
    width: 100px;
    color: white;
  }
}
#add-meal-ingredient {
  padding: 15px;
}
.form-input {
  height: 65px;
  margin-bottom: 10px;
  width: 100%;
}
.label {
  text-align: left;
}
</style>

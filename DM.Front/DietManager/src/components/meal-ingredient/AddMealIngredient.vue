<template>
  <div id="add-meal-ingredient">
    <div id="add-meal-ingredient-form">
      <div class="form-input">
        <div class="label">Name</div>
        <input type="text" class="form-control" placeholder="Enter meal ingredient name..." v-model="mealIngredientFormData.Name">
      </div>
      <div class="form-input">
        <div class="label">Calories</div>
        <input type="number" class="form-control" placeholder="Enter number of calories..." v-model="mealIngredientFormData.Calories">
      </div>
      <div class="form-input">
        <div class="label">Proteins</div>
        <input type="number" class="form-control" placeholder="Enter amount of proteins..." v-model="mealIngredientFormData.Nutritions.Proteins">
      </div>
      <div class="form-input">
        <div class="label">Carbohydrates</div>
        <input type="number" class="form-control" placeholder="Enter amount of carbohydrates..." v-model="mealIngredientFormData.Nutritions.Carbohydrates">
      </div>
      <div class="form-input">
        <div class="label">Fats</div>
        <input type="number" class="form-control" placeholder="Enter amount of fats..." v-model="mealIngredientFormData.Nutritions.Fats">
      </div>
      <div class="form-input">
        <div class="label">Vitamin A (optional)</div>
        <input type="number" class="form-control" placeholder="Enter amount of Vitamin A..." v-model="mealIngredientFormData.Nutritions.VitaminA">
      </div>
      <div class="form-input">
        <div class="label">Vitamin C (optional)</div>
        <input type="number" class="form-control" placeholder="Enter amount of Vitamin C..." v-model="mealIngredientFormData.Nutritions.VitaminC">
      </div>
      <div class="form-input">
        <div class="label">Vitamin B6 (optional)</div>
        <input type="number" class="form-control" placeholder="Enter amount of Vitamin B6..." v-model="mealIngredientFormData.Nutritions.VitaminB6">
      </div>
      <div class="form-input">
        <div class="label">Vitamin D (optional)</div>
        <input type="number" class="form-control" placeholder="Enter amount of Vitamin D..." v-model="mealIngredientFormData.Nutritions.VitaminD">
      </div>
    </div>
    <div id="buttons-container">
      <button class="btn main-background-color" @click="addMealIngredient">Add</button>
      <button class="btn main-background-color" @click="$modal.hide('addMealIngredientModal')">Close</button>
    </div>
  </div>
</template>
<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import MealIngredientCreation from "@/ViewModels/meal-ingredient/mealIngredientCreation";
import MealIngredientApiCaller from "@/services/api-callers/mealIngredientApi";

@Component
export default class AddMealIngredient extends Vue {
  private mealIngredientFormData: MealIngredientCreation = {
    Nutritions: {
      Proteins: 0.0,
      Carbohydrates: 0.0,
      Fats: 0.0
    }
  } as MealIngredientCreation;

  addMealIngredient() {
    MealIngredientApiCaller.add(
      this.mealIngredientFormData,
      this.addMealIngredientSuccessHandler,
      this.addMealIngredientErrorHandler
    );
  }

  addMealIngredientSuccessHandler(guid: string) {
    // eslint-disable-next-line no-console
    console.log("meal-ingredient guid: " + guid);
  }

  addMealIngredientErrorHandler(error: Error) {
    // eslint-disable-next-line no-console
    console.error(error);
  }
}
</script>
<style scoped>
#buttons-container {
  margin: 15px 0px 5px 0px;
  width: 100%;
  justify-content: center;
  display: flex;
}
#buttons-container > button {
  margin: 0px 25px 0px 25px;
  width: 100px;
  color: white;
}
#add-meal-ingredient {
  padding: 15px;
}
.form-input {
  margin-bottom: 10px;
}
</style>

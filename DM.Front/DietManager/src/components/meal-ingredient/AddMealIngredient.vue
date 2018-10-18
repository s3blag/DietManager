<template>
    <div>
      <div id="add-meal-ingredient-form">
        <div class="form-input">
          <div class="label">Name</div>
          <input type="text" class="form-control" placeholder="Enter meal ingredient name..." v-model="mealIngredientFormData.Name">
        </div>
        <div class="form-input">
          <div class="label">Calories</div>
          <input type="number" class="form-control" placeholder="Enter meal ingredient calories..." v-model="mealIngredientFormData.Calories">
        </div>
        <div class="form-input">
          <div class="label">Proteins</div>
          <input type="number" class="form-control" placeholder="Enter meal ingredient proteins..." v-model="mealIngredientFormData.Nutritions.Proteins">
        </div>
        <div class="form-input">
          <div class="label">Carbohydrates</div>
          <input type="number" class="form-control" placeholder="Enter meal ingredient carbohydrates..." v-model="mealIngredientFormData.Nutritions.Carbohydrates">
        </div>
        <div class="form-input">
          <div class="label">Fats</div>
          <input type="number" class="form-control" placeholder="Enter meal ingredient fats..." v-model="mealIngredientFormData.Nutritions.Fats">
        </div>
        <div class="form-input">
          <div class="label">Vitamin A</div>
          <input type="number" class="form-control" placeholder="Enter meal ingredient Vitamin A..." v-model="mealIngredientFormData.Nutritions.VitaminA">
        </div>
        <div class="form-input">
          <div class="label">Vitamin C</div>
          <input type="number" class="form-control" placeholder="Enter meal ingredient Vitamin C..." v-model="mealIngredientFormData.Nutritions.VitaminC">
        </div>
        <div class="form-input">
          <div class="label">Vitamin B6</div>
          <input type="number" class="form-control" placeholder="Enter meal ingredient Vitamin B6..." v-model="mealIngredientFormData.Nutritions.VitaminB6">
        </div>
        <div class="form-input">
          <div class="label">Vitamin D</div>
          <input type="number" class="form-control" placeholder="Enter meal ingredient Vitamin D..." v-model="mealIngredientFormData.Nutritions.VitaminD">
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
  private mealIngredientFormData: MealIngredientCreation = {} as MealIngredientCreation;

  addMealIngredient() {
    MealIngredientApiCaller.add(
      this.mealIngredientFormData,
      this.addMealIngredientSuccessHandler,
      this.addMealIngredientErroHandler
    );
  }

  addMealIngredientSuccessHandler(guid: string) {
    // eslint-disable-next-line no-console
    console.log("meal-ingredient guid: " + guid);
  }

  addMealIngredientErroHandler(error: Error) {
    // eslint-disable-next-line no-console
    console.error(error);
  }
}
</script>
<style scoped>
#buttons-container {
  display: flex;
  justify-content: center;
}
#buttons-container > button {
  width: 70px;
}
</style>

<template>
  <div id="add-meal-ingredient">
    <div id="add-meal-ingredient-form">
      <div class="form-input">
        <div class="label">Name</div>
        <input type="text" class="form-control" placeholder="Enter meal ingredient name..." v-model="mealIngredientFormData.MealIngredient.Name">
      </div>
      <div class="form-input">
        <div class="label">Quantity</div>
        <input type="number" class="form-control" placeholder="Enter quantity..." v-model="mealIngredientFormData.Quantity">
      </div>
      <div class="form-input">
        <div class="label">Calories</div>
        <input type="number" class="form-control" placeholder="Enter number of calories..." v-model="mealIngredientFormData.MealIngredient.Calories">
      </div>
      <div class="form-input">
        <div class="label">Proteins</div>
        <input type="number" class="form-control" placeholder="Enter amount of proteins..." v-model="mealIngredientFormData.MealIngredient.Nutrition.Protein">
      </div>
      <div class="form-input">
        <div class="label">Carbohydrates</div>
        <input type="number" class="form-control" placeholder="Enter amount of carbohydrates..." v-model="mealIngredientFormData.MealIngredient.Nutrition.Carbohydrates">
      </div>
      <div class="form-input">
        <div class="label">Fats</div>
        <input type="number" class="form-control" placeholder="Enter amount of fats..." v-model="mealIngredientFormData.MealIngredient.Nutrition.Fats">
      </div>
      <div class="form-input">
        <div class="label">Vitamin A (optional)</div>
        <input type="number" class="form-control" placeholder="Enter amount of Vitamin A..." v-model="mealIngredientFormData.MealIngredient.Nutrition.VitaminA">
      </div>
      <div class="form-input">
        <div class="label">Vitamin C (optional)</div>
        <input type="number" class="form-control" placeholder="Enter amount of Vitamin C..." v-model="mealIngredientFormData.MealIngredient.Nutrition.VitaminC">
      </div>
      <div class="form-input">
        <div class="label">Vitamin B6 (optional)</div>
        <input type="number" class="form-control" placeholder="Enter amount of Vitamin B6..." v-model="mealIngredientFormData.MealIngredient.Nutrition.VitaminB6">
      </div>
      <div class="form-input">
        <div class="label">Vitamin D (optional)</div>
        <input type="number" class="form-control" placeholder="Enter amount of Vitamin D..." v-model="mealIngredientFormData.MealIngredient.Nutrition.VitaminD">
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
import MealIngredientApiCaller from "@/services/api-callers/mealIngredientApi";
import MealIngredient from "@/ViewModels/meal-ingredient/mealIngredient";
import MealIngredientCreationWithQuantity from "@/ViewModels/meal-ingredient/mealIngredientCreationWithQuantity";
import MealIngredientWithQuantity from "@/ViewModels/meal-ingredient/mealIngredientWithQuantity";

@Component
export default class AddMealIngredient extends Vue {
  private mealIngredientFormData: MealIngredientCreationWithQuantity = {
    quantity: 1,
    mealIngredient: {
      nutrition: {
        protein: 0.0,
        carbohydrates: 0.0,
        fats: 0.0,
        vitaminC: null,
        vitaminB6: null,
        vitaminA: 0.0,
        vitaminD: null
      }
    }
  } as MealIngredientCreationWithQuantity;
  private counter: number = 0;

  addMealIngredient() {
    //this.addMealIngredientSuccessHandler((this.counter++).toString());
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
        nutritions: mealIngredientCreation.mealIngredient.nutrition
      } as MealIngredient,
      quantity: mealIngredientCreation.quantity
    } as MealIngredientWithQuantity;

    // eslint-disable-next-line no-console
    console.log("added meal-ingredient quid: " + guid);
    this.$emit("meal-ingredient-added", addedMealIngredientWithQuantity);
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

<template>
  <div class="add-meal">
    <div class="form-container">
      <div class="column left">
        <form>
          <div class="form-group">
            <label for="mealName">Name</label>
            <input type="text" class="form-control" id="mealName" placeholder="Enter meal name..." v-model="mealFormData.Name">
          </div>
          <div class="form-group">
            <label for="mealDescription">Description</label>
            <textarea class="form-control" height="200" width="200" id="mealDescription" placeholder="Enter meal description..." rows="10" v-model="mealFormData.Description"></textarea>
          </div>
        </form>
      </div>
      <div class="column right">
        <div class="picture-input">
          <picture-input ref="pictureInput" @change="onChange" width="400" height="200" radius="5" accept="image/jpeg,image/png" size="10" :removable="true" :customStrings="{
        upload: '<h1>Bummer!</h1>',
        drag: 'Drop an image or click to select meal avatar'
      }">
          </picture-input>
        </div>
      </div>
      <button id="addMealButton" type="submit" class="btn main-background-color" @click="submit">Add Meal</button>
    </div>
    <add-meal-summary class="summary" :mealIngredients="mealIngredients"></add-meal-summary>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { Watch, Model, Component } from "vue-property-decorator";
import PictureInput from "vue-picture-input";

import MealCreation from "@/ViewModels/meal/mealCreation";
import MealCreationFormData from "@/ViewModels/meal/mealCreationFormData";
import MealIngredient from "@/ViewModels/meal-ingredient/mealIngredient";
import MealApiCaller from "@/services/api-callers/mealApi";
import MealLookup from "@/ViewModels/meal/mealLookup";
import AddMealSummary from "@/components/meal/AddMealSummary.vue";

@Component({
  components: {
    "add-meal-summary": AddMealSummary,
    "picture-input": PictureInput
  }
})
export default class AddMeal extends Vue {
  private mealFormData: MealCreationFormData = {
    Name: "",
    Description: ""
  } as MealCreation;
  private mealIngredients: MealIngredient[] = [];

  submit() {
    const { Name, Description, ImageId } = this.mealFormData;
    let completeMealCreation = {
      Name: Name,
      Description: Description,
      ImageId: ImageId,
      Ingredients: this.mealIngredients.map(mealIngredient => mealIngredient.Id)
    } as MealCreation;

    MealApiCaller.add(
      completeMealCreation,
      this.addMealSuccessHandler,
      this.addMealErrorHandler
    );
  }

  addMealSuccessHandler(addedMealGuid: string) {
    // eslint-disable-next-line no-console
    console.log(addedMealGuid);
  }

  addMealErrorHandler(error: Error) {
    // eslint-disable-next-line no-console
    console.error(error);
  }

  onChange() {}
}
</script>

<style scoped>
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
  background-color: #e6e4e4;
  border-radius: 10px;
}
.summary {
  overflow: hidden;
  margin: 0px 20px;
  padding: 20px;
  width: 13%;
  height: fit-content;
  background-color: #e6e4e4;
  border-radius: 10px;
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
}
.column.righ:after {
  content: "";
  display: table;
  clear: both;
}
.picture-input {
  margin: 45px auto 10px auto;
}
#addMealButton {
  display: block;
  position: relative;
  top: -50px;
  margin: 0 auto;
}
</style>

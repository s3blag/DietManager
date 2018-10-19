<template>
  <ul>
    <li>
      <div class="element-name">Calories</div>
      <div class="element-value">{{caloriesSummary}}</div>
    </li>
    <li>
      <div class="element-name">Proteins</div>
      <div class="element-value">{{proteinsSummary}}</div>
    </li>
    <li>
      <div class="element-name">Carbohydrates</div>
      <div class="element-value">{{carbohydratesSummary}}</div>
    </li>
    <li>
      <div class="element-name">Fats</div>
      <div class="element-value">{{fatsSummary}}</div>
    </li>
    <li>
      <div class="element-name">Vitamin A</div>
      <div class="element-value">{{vitaminASummary}}</div>
    </li>
    <li>
      <div class="element-name">Vitamin C</div>
      <div class="element-value">{{vitaminCSummary}}</div>
    </li>
    <li>
      <div class="element-name">Vitamin B6</div>
      <div class="element-value">{{vitaminB6Summary}}</div>
    </li>
    <li>
      <div class="element-name">Vitamin D</div>
      <div class="element-value">{{vitaminDSummary}}</div>
    </li>
  </ul>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop } from "vue-property-decorator";
import _ from "lodash";

import MealIngredient from "@/ViewModels/meal-ingredient/mealIngredient";

@Component
export default class AddMealSummary extends Vue {
  @Prop({
    type: Array
  })
  private mealIngredients!: MealIngredient[];

  get caloriesSummary() {
    let result = _.sum(
      this.mealIngredients.map(ingredient => ingredient.Calories)
    );

    return this.finalizeResult(result, "kcal");
  }
  get proteinsSummary() {
    let result = _.sum(
      this.mealIngredients.map(ingredient => ingredient.Nutritions.Proteins)
    );

    return this.finalizeResult(result, "g");
  }
  get carbohydratesSummary() {
    let result = _.sum(
      this.mealIngredients.map(
        ingredient => ingredient.Nutritions.Carbohydrates
      )
    );

    return this.finalizeResult(result, "g");
  }
  get fatsSummary() {
    let result = _.sum(
      this.mealIngredients.map(ingredient => ingredient.Nutritions.Fats)
    );

    return this.finalizeResult(result, "g");
  }
  get vitaminASummary() {
    let result = _.sum(
      this.mealIngredients.map(ingredient => ingredient.Nutritions.VitaminA)
    );

    return this.finalizeResult(result, "mg");
  }
  get vitaminCSummary() {
    let result = _.sum(
      this.mealIngredients.map(ingredient => ingredient.Nutritions.VitaminC)
    );

    return this.finalizeResult(result, "mg");
  }
  get vitaminB6Summary() {
    let result = _.sum(
      this.mealIngredients.map(ingredient => ingredient.Nutritions.VitaminB6)
    );

    return this.finalizeResult(result, "mg");
  }
  get vitaminDSummary() {
    let result = _.sum(
      this.mealIngredients.map(ingredient => ingredient.Nutritions.VitaminD)
    );

    return this.finalizeResult(result, "mg");
  }

  private finalizeResult(result: number, suffix: string) {
    if (!result) {
      return `-- ${suffix}`;
    }

    return `${result} ${suffix}`;
  }
}
</script>

<style scoped>
ul {
  list-style: none;
}
li {
  justify-content: space-between;
  display: flex;
  border-bottom-width: 1px;
  border-bottom-style: solid;
  margin: 15px 0px;
  font-size: 16px;
}
.element-name {
  text-align: left;
}
.element-value {
  text-align: right;
}
</style>

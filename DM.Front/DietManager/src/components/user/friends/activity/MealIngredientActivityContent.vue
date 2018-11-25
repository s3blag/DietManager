<template>
  <router-link :to="'/meal-ingredients/' + mealIngredient.id" tag="div" class="link">
    <layout>
      <template slot="picture">
        <image-wrapper :imageId="mealIngredient.imageId" :asWheel="false">
          <template slot="placeholder">
            <font-awesome-icon class="main-color" icon="lemon" size="2x" />
          </template>
        </image-wrapper>
      </template>
      <template slot="details">
        <div>
          {{mealIngredient.name}}
        </div>
      </template>
    </layout>
  </router-link>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import Achievement from "@/ViewModels/achievement/achievement";
import { Prop } from "vue-property-decorator";
import ActivityContentLayout from "./ActivityContentLayout.vue";
import ImageWrapper from "@/components/image/ImageWrapper.vue";
import MealIngredient from "@/ViewModels/meal-ingredient/mealIngredient";

@Component({
  components: {
    layout: ActivityContentLayout,
    "image-wrapper": ImageWrapper
  }
})
export default class MealIngredientActivityContent extends Vue {
  @Prop({ required: true })
  private mealIngredient!: MealIngredient;
  // = {
  //   id: "id",
  //   imageId: null,
  //   name: "meal ingredient name",
  //   calories: 1,
  //   nutritions: {
  //     protein: 1,
  //     carbohydrates: 1,
  //     fats: 1,
  //     vitaminA: 1,
  //     vitaminC: null,
  //     vitaminB6: null,
  //     vitaminD: null
  //   }
  // };
}
</script>

<style lang="less" scoped>
</style>

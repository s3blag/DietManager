<template>
  <div>
    <layout>
      <template slot="picture">
        <image-wrapper :imageId="mealIngredient.imageId" :asWheel="false">
          <template slot="placeholder">
            <font-awesome-icon class="main-color" icon="lemon" size="2x"/>
          </template>
        </image-wrapper>
      </template>
      <template slot="details">
        <div>{{mealIngredient.name}}</div>
      </template>
    </layout>
  </div>
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
}
</script>

<style lang="less" scoped>
#activity-content-details {
  text-decoration: none !important;
}
</style>

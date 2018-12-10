<template>
  <router-link
    :to="{ name: 'Meal', params: { mealId: meal.id, asAdmin: isAdmin } }"
    tag="div"
    class="link"
  >
    <layout>
      <template slot="picture">
        <image-wrapper :imageId="meal.imageId" :asWheel="false">
          <template slot="placeholder">
            <font-awesome-icon class="main-color" icon="utensils" size="2x"/>
          </template>
        </image-wrapper>
      </template>
      <template slot="details">
        <div>{{meal.name}}</div>
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
import Meal from "@/ViewModels/meal/meal";
import ImageWrapper from "@/components/image/ImageWrapper.vue";

@Component({
  components: {
    layout: ActivityContentLayout,
    "image-wrapper": ImageWrapper
  }
})
export default class MealActivityContent extends Vue {
  @Prop({ required: true })
  private meal!: Meal;

  get isAdmin() {
    return this.$route.meta && this.$route.meta.asAdmin;
  }
}
</script>

<style lang="less" scoped>
</style>

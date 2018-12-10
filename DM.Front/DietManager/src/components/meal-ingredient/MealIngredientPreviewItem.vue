<template>
  <div id="preview-item">
    <modal v-if="showDeleteModal">
      <span class="modal-text">Are you sure you want to delete this meal ingredient?</span>
      <div class="modal-buttons-container">
        <button class="button" @click="deleteMealIngredient">Delete</button>
        <button class="button" @click="showDeleteModal = false">Cancel</button>
      </div>
    </modal>

    <modal v-if="showDetailsModal">
      <div class="modal-text">
        <div class="label">Name</div>
        <div>{{mealIngredient.name}}</div>

        <div class="label">Calories</div>
        <div>{{mealIngredient.calories}}</div>

        <div class="label">Proteins</div>
        <div>{{mealIngredient.nutrition.protein}}</div>

        <div class="label">Carbohydrates</div>
        <div>{{mealIngredient.nutrition.carbohydrates}}</div>

        <div class="label">Fats</div>
        <div>{{mealIngredient.nutrition.fats}}</div>

        <div class="label">Vitamin A</div>
        <div>{{mealIngredient.nutrition.vitaminA ? mealIngredient.nutrition.vitaminA : '--'}}</div>

        <div class="label">Vitamin C</div>
        <div>{{mealIngredient.nutrition.vitaminC ? mealIngredient.nutrition.vitaminC : '--'}}</div>

        <div class="label">Vitamin B6</div>
        <div>{{mealIngredient.nutrition.vitaminB6 ? mealIngredient.nutrition.vitaminB6 : '--'}}</div>

        <div class="label">Vitamin D</div>
        <div>{{mealIngredient.nutrition.vitaminD ? mealIngredient.nutrition.vitaminD : '--'}}</div>
      </div>
      <div class="modal-buttons-container">
        <button class="button" @click="showDetailsModal = false">Ok</button>
      </div>
    </modal>

    <modal v-if="showSetAmountModal">
      <div class="modal-text">
        <h4>Set amount [g]</h4>
        <div class="time-picker">
          <input type="number" class="amount-picker" min="0" v-model="amount">
        </div>
        <div class="modal-buttons-container">
          <button class="button" @click="onSubmit">Save</button>
          <button class="button" @click="showSetAmountModal = false">Cancel</button>
        </div>
      </div>
    </modal>

    <div class="meal-ingredient-info-element">
      <div class="label">Name</div>
      <div>{{mealIngredient.name}}</div>
    </div>
    <div class="meal-ingredient-info-element">
      <div class="label">Calories [100g]</div>
      <div class="value">{{mealIngredient.calories}}</div>
    </div>
    <button class="button item" @click="showDetailsModal = true">Details</button>
    <button v-if="!isAdmin" class="button item" @click="showSetAmountModal = true" id="add">
      <font-awesome-icon icon="plus-circle"/>
    </button>
    <button v-else class="button item" @click="showDeleteModal = true" id="add">Delete</button>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop } from "vue-property-decorator";
import ImageWrapper from "@/components/image/ImageWrapper.vue";
import MealIngredient from "@/ViewModels/meal-ingredient/mealIngredient";
import Modal from "@/components/common/Modal.vue";
import MealIngredientWithQuantity from "@/ViewModels/meal-ingredient/mealIngredientWithQuantity";
import AuthService from "@/services/authService";
import AdminApiCaller from "@/services/api-callers/adminApi";
Component.registerHooks(["beforeRouteEnter"]);

@Component({
  components: {
    modal: Modal
  }
})
export default class MealIngredientPreviewItem extends Vue {
  @Prop({ required: true })
  private mealIngredient!: MealIngredient;
  private showDetailsModal = false;
  private showSetAmountModal = false;
  private amount: number = 0;
  private showDeleteModal = false;

  onSubmit() {
    const addedMealIngredientWithQuantity = {
      mealIngredient: this.mealIngredient,
      quantity: this.amount
    } as MealIngredientWithQuantity;
    this.$emit("meal-ingredient-added", addedMealIngredientWithQuantity);
  }

  deleteMealIngredient() {
    AdminApiCaller.deleteMealIngredient(this.mealIngredient.id, () =>
      this.$emit("meal-ingredient-deleted", this.mealIngredient.id)
    );
  }

  get isAdmin() {
    return this.$route.meta && this.$route.meta.asAdmin;
  }
}
</script>

<style lang="less" scoped>
.meal-ingredient-info-element {
  margin-left: 10px;
  display: inline-block;
  margin-top: auto;
  margin-bottom: auto;
  width: 125px;
}
.meal-ingredient-name {
  overflow: hidden;
}
.label {
  color: #929191;
}
.item {
  height: 35px;
  margin: auto 0px;
}
#add {
  margin-right: 10px;
}
#modal-text {
  text-align: left;
}
.amount-picker {
  width: 200px;
  text-align: center;
  border-style: groove;
  border-radius: 7px;
}
</style>

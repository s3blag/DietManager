<template>
  <modal>
    <div id="add-schedule-entry">
      <div id="header" class="soft-border bottom" v-show="!datePicking">
        <span class="link" @click="favouritesTabSelected = false">Search</span>
        <span class="link" @click="favouritesTabSelected = true">Favourites</span>
      </div>
      <div id="date-picking" v-show="datePicking">
        <h4>Set a date</h4>
        <div class="time-picker">
          <input type="time" class="form-control" v-model="time">
        </div>
      </div>
      <div id="content" v-show="!datePicking">
        <favourite-meals
          v-show="favouritesTabSelected"
          :asEmittingComponent="true"
          @meal-selected="onMealSelected"
        />
        <meals-search
          v-show="!favouritesTabSelected"
          :asEmittingComponent="true"
          @meal-selected="onMealSelected"
        />
      </div>

      <div class="modal-buttons-container">
        <button
          class="button"
          :class="!this.selectedMealId ? 'disabled' : ''"
          @click="onNext"
        >{{datePicking ? 'Previous' : 'Next'}}</button>
        <button
          class="button"
          v-if="datePicking"
          @click="$emit('add-schedule-entry', {time: time, id: selectedMealId})"
        >Add</button>
        <button class="button" @click="$emit('close-modal')">Exit</button>
      </div>
    </div>
  </modal>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import Modal from "@/components/common/Modal.vue";
import FavouriteMeals from "@/components/meal/FavouriteMeals.vue";
import SearchMeals from "@/components/meal/SearchMeals.vue";
import MealPreview from "@/ViewModels/meal/mealPreview";
import { Prop } from "vue-property-decorator";

@Component({
  components: {
    modal: Modal,
    "favourite-meals": FavouriteMeals,
    "meals-search": SearchMeals
  }
})
export default class AddMealSchedule extends Vue {
  private favouritesTabSelected = false;
  private selectedMealId: string | null = null;
  private datePicking = false;
  private time: string = "00:00";

  onMealSelected(id: string) {
    this.selectedMealId = id;
  }

  onNext() {
    if (this.selectedMealId) {
      this.datePicking = !this.datePicking;
    }
  }
}
</script>

<style lang="less" scoped>
#add-schedule-entry {
  display: block;
  max-height: 85%;
  display: block;
  > * {
    width: 100%;
  }
}
#header {
  text-align: center;
  margin-bottom: 20px;
  > * {
    margin: 5px;
  }
}
#content {
  width: 750px;
  height: 500px;
  overflow-y: auto;
  border-width: 1px;
  border-style: solid;
  border-color: rgb(190, 190, 190);
  max-height: 95%;
}
#date-picking {
  width: 750px;
  height: 545px;
}
.button-container {
  margin-top: 10px;
}
.disabled {
  background-color: grey;
}
.time-picker {
  input {
    margin: 0 auto;
    width: 150px;
  }
}
h4 {
  height: 50px;
}
.button {
  width: 85px;
}
</style>


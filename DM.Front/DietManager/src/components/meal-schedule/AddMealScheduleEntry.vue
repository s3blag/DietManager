<template>
    <modal>
        <div id="add-schedule-entry">
            <div id="header" class="soft-border bottom">
                <span class="link" @click="favouritesTabSelected = false">
                    Search
                </span>
                <span class="link" @click="favouritesTabSelected = true">
                    Favourites
                </span>
            </div>
            <div id="content">
                <favourite-meals v-if="favouritesTabSelected" :asEmittingComponent="true" @meal-selected="onMealSelected"/>
                <meals-search v-else :asEmittingComponent="true" @meal-selected="onMealSelected"/>
            </div>
            <div>
                <button class="button" @click="onNext">
                    Next
                </button>
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

  onMealSelected(id: string) {
    this.selectedMealId = id;
  }

  onNext() {
    if (this.selectedMealId) {
      this.datePicking = true;
    }
  }
}
</script>

<style lang="less" scoped>
#add-schedule-entry {
  overflow: scroll;
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
</style>


<template>
  <div class="meal-schedule">
    <div class="meal-schedule-container content-background">
      <div class="schedule-header">
        <button @click="previousWeek" class="button">
          <font-awesome-icon icon="arrow-left"/>
        </button>
        {{parsedWeekStartDate}} - {{parsedWeekEndDate}}
        <button
          @click="nextWeek"
          class="button"
        >
          <font-awesome-icon icon="arrow-right"/>
        </button>
      </div>
      <div id="calendar">
        <meal-schedule-container
          class="daily-schedule"
          v-for="(day, index) in daysOfWeek"
          :key="index"
          :mealSchedule="mealSchedule[day]"
          :label="day"
          :class="{'current-day': index+1 === currentDayOfWeek}"
          :date="getFullDateFromDayOfWeek(index)"
          @on-schedule-changed="getMealSchedule"
        />
      </div>
      <div id="show-shopping-list-button">
        <button class="button" @click="showShoppingList = true">Shopping list</button>
      </div>
    </div>
    <meal-schedule-summary id="meal-schedule-summary" :mealIngredients="mealIngredients"/>

    <modal v-if="showShoppingList">
      <div id="modal-content">
        <h3>Shopping list</h3>
        <div id="shopping-list-items-wrapper">
          <div
            class="shopping-list-item soft-border bottom"
            v-for="item in shoppingList"
            :key="item.name"
          >
            <span class="name">{{item.name}}</span>
            <span class="quantity">{{item.quantity + 'x'}}</span>
          </div>
        </div>
      </div>
      <div class="modal-buttons-container">
        <button class="button" @click="showShoppingList = false">Ok</button>
      </div>
    </modal>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import WeeklyMealSchedule from "@/ViewModels/meal-schedule/weeklyMealSchedule";
import MealScheduleApi from "@/services/api-callers/mealScheduleApi";
import { Watch } from "vue-property-decorator";
import MealScheduleEntry from "@/ViewModels/meal-schedule/MealScheduleEntry";
import MealIngredientWithQuantity from "@/ViewModels/meal-ingredient/mealIngredientWithQuantity";
import MealScheduleSummary from "@/components/meal/MealSummary.vue";
import { DaysOfWeek } from "@/ViewModels/enums/daysOfWeek";
import MealScheduleContainer from "./MealScheduleContainer.vue";
import Modal from "@/components/common/Modal.vue";
import _ from "lodash";

@Component({
  components: {
    "meal-schedule-summary": MealScheduleSummary,
    "meal-schedule-container": MealScheduleContainer,
    modal: Modal
  }
})
export default class MealSchedule extends Vue {
  private mealSchedule: WeeklyMealSchedule = this.emptyMealSchedule;
  private selectedWeekStartDate: Date = this.getCurrentWeekStartDate();
  private showShoppingList = false;

  created() {
    this.mealSchedule = this.emptyMealSchedule;
    this.getMealSchedule();
  }

  get parsedWeekStartDate() {
    const date = this.selectedWeekStartDate;
    return `${date.getDate()}.${date.getMonth() + 1}.${date.getFullYear()}`;
  }

  get parsedWeekEndDate() {
    const date = this.selectedWeekEndDate!;
    return `${date.getDate()}.${date.getMonth() + 1}.${date.getFullYear()}`;
  }

  get selectedWeekEndDate() {
    const endDate = new Date(this.selectedWeekStartDate.getTime());
    endDate.setDate(endDate.getDate() + 6);
    endDate.setHours(0, 0, 0, 0);
    return endDate;
  }

  get currentDayOfWeek() {
    const currentDate = new Date();

    if (
      currentDate > this.selectedWeekEndDate ||
      currentDate < this.selectedWeekStartDate
    ) {
      return 8;
    }

    return currentDate.getDay();
  }

  getFullDateFromDayOfWeek(dayOfWeek: number) {
    const date = new Date(this.selectedWeekStartDate);
    date.setDate(date.getDate() + dayOfWeek);
    return date;
  }

  get emptyMealSchedule() {
    const mealSchedule = {
      monday: [],
      tuesday: [],
      wednesday: [],
      thursday: [],
      friday: [],
      saturday: [],
      sunday: []
    } as WeeklyMealSchedule;

    return mealSchedule;
  }

  get mealIngredients() {
    if (this.mealSchedule !== null) {
      const mealSchedule = this.mealSchedule;

      const mealScheduleEntries = Object.keys(mealSchedule).flatMap(
        key => (mealSchedule as any)[key] as MealScheduleEntry[]
      );

      const ingredients = mealScheduleEntries.flatMap(
        mealScheduleEntry =>
          mealScheduleEntry.meal.ingredients as MealIngredientWithQuantity[]
      );

      return ingredients;
    } else {
      return [];
    }
  }

  get daysOfWeek() {
    return Object.keys(DaysOfWeek).filter(
      k => typeof (DaysOfWeek as any)[k] === "number"
    ) as string[];
  }

  getCurrentWeekStartDate() {
    let currentWeekStartDate = new Date();

    const currentDay = currentWeekStartDate.getDay();

    currentWeekStartDate.setDate(
      currentWeekStartDate.getDate() - (currentDay - 1)
    );
    currentWeekStartDate.setHours(0, 0, 0, 0);

    return currentWeekStartDate;
  }

  nextWeek() {
    this.addDaysToTheSelectedWeekDate(+7);
  }

  previousWeek() {
    this.addDaysToTheSelectedWeekDate(-7);
  }

  addDaysToTheSelectedWeekDate(days: number) {
    const newDate = new Date(
      this.selectedWeekStartDate.setDate(
        this.selectedWeekStartDate.getDate() + days
      )
    );
    this.selectedWeekStartDate = newDate;
    this.getMealSchedule();
  }

  getMealSchedule() {
    MealScheduleApi.get(
      this.selectedWeekStartDate.toISOString(),
      this.getMealScheduleSuccessHandler
    );
  }

  getMealScheduleSuccessHandler(mealSchedule: WeeklyMealSchedule) {
    this.mealSchedule = mealSchedule;
  }

  get shoppingList() {
    return _(this.mealIngredients)
      .groupBy(m => m.mealIngredient.name)
      .map((value, key) => ({
        name: key,
        quantity: _.sumBy(value, v => v.quantity)
      }))
      .value();
  }
}
</script>

<style lang="less" scoped>
.meal-schedule {
  display: inline-flex;
  width: 98%;
  margin: 10px;
  > * {
    margin: 0px 10px 0px 10px;
  }
}
.meal-schedule-container {
  width: 90%;
  border-radius: 10px;
  height: 750px;
  min-height: fit-content;
}
#meal-schedule-summary {
  padding: 15px;
  width: 170px;
  min-width: 150px;
  height: 380px;
}
.schedule-header {
  margin: 10px;
  display: inline-flex;
  width: 98%;
  justify-content: space-between;
  align-items: center;
  font-weight: bold;
}
#calendar {
  display: inline-flex;
  width: 98%;
  justify-content: space-between;
}
.daily-schedule {
  background-color: white;
  flex-grow: 1;
  margin: 0px 5px 0px 5px;
  width: 100px;
  height: 650px;
  border-radius: 10px;
  padding: 5px;
}
.current-day {
  background-color: rgba(37, 102, 207, 0.534);
}
.button {
  width: 35px;
  height: 35px;
}
#show-shopping-list-button {
  margin: 20px 0px 10px 0px;
  height: 50px;
  width: 100%;
  > * {
    height: 80%;
  }
}
#modal-content {
  width: 100%;

  h3 {
    font-weight: bold;
    margin-bottom: 25px;
  }
}
.shopping-list-item {
  display: flex;
  justify-content: space-between;
  margin-bottom: 20px;
}
.shopping-list-item:last-child {
  margin-bottom: 5px;
}
.modal-buttons-container {
  button {
    width: 75px;
    height: 40px;
  }
}
</style>
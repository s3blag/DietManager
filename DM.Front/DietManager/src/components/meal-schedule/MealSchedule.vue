<template>
  <div class="meal-schedule">
    <div class="meal-schedule-container content-background">
      <div class="schedule-header">
        <button @click="previousWeek" class="btn main-background-color">
          <font-awesome-icon icon="arrow-left" />
        </button>
        {{parsedWeekStartDate}} - {{parsedWeekEndDate}}
        <button @click="nextWeek" class="btn main-background-color">
          <font-awesome-icon icon="arrow-right" />
        </button>
      </div>
      <div id="calendar">
        <meal-schedule-container class="daily-schedule" v-for="(day, index) in daysOfWeek" :key="index" :mealSchedule="mealSchedule[day]" :label="day" :class="{'current-day': index+1 === currentDayOfWeek}" />
      </div>
    </div>
    <meal-schedule-summary id="meal-schedule-summary" :mealIngredients="mealIngredients" />
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

@Component({
  components: {
    "meal-schedule-summary": MealScheduleSummary,
    "meal-schedule-container": MealScheduleContainer
  }
})
export default class MealSchedule extends Vue {
  mealSchedule: WeeklyMealSchedule = this.emptyMealSchedule;
  selectedWeekStartDate: Date = this.getCurrentWeekStartDate();

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
button {
  color: white;
}
</style>
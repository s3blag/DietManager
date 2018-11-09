<template>
  <div>
    {{parsedWeekStartDate}}-{{parsedWeekEndDate}}
    <button @click="nextWeek">next</button>
    <button @click="previousWeek">previous</button>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import WeeklyMealSchedule from "@/ViewModels/meal-schedule/weeklyMealSchedule";
import MealScheduleApi from "@/services/api-callers/mealScheduleApi";
import { Watch } from "vue-property-decorator";

@Component
export default class MealSchedule extends Vue {
  mealSchedule: WeeklyMealSchedule | null = null;
  selectedWeekStartDate: Date = this.getCurrentWeekStartDate();

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

  created() {
    this.getMealSchedule();
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
</style>
2018-11-05 23:32:49.234453+01
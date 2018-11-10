<template>
  <div id="schedule-container">
    <div class="label">
      <span id="label">{{label}}</span>
      <span id="add-icon" @click="addMealScheduleEntry">
        <font-awesome-icon class="option-icon" icon="plus-circle" size="lg" />
      </span>
    </div>
    <div class="content">
      <div class="schedule-entry" v-for="(scheduleEntry, index) in sortedMealScheduleEntries" :key="index">
        <meal-schedule-item :mealScheduleEntry="scheduleEntry" @deleteMealScheduleEntry="deleteMealScheduleEntry" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop } from "vue-property-decorator";
import MealScheduleEntry from "@/ViewModels/meal-schedule/mealScheduleEntry";
import MealScheduleItem from "./MealScheduleItem.vue";
import { DaysOfWeek } from "@/ViewModels/enums/daysOfWeek";
import _ from "lodash";

@Component({
  components: {
    "meal-schedule-item": MealScheduleItem
  }
})
export default class MealScheduleContainer extends Vue {
  @Prop({
    required: true
  })
  mealSchedule: MealScheduleEntry[] = [
    {
      id: "1",
      date: new Date(),
      meal: {
        id: "1",
        name: "qwert sadasdz zxczxc zc"
      }
    } as MealScheduleEntry,
    {
      id: "2",
      date: new Date(),
      meal: {
        id: "2",
        name: "jajecznica po sopocku"
      }
    } as MealScheduleEntry,
    {
      id: "3",
      date: new Date(),
      meal: {
        id: "3",
        name: "jajecznica po sopocku 1 "
      }
    } as MealScheduleEntry,
    {
      id: "4",
      date: new Date(),
      meal: {
        id: "4",
        name: "jajecznica po sopocku 2"
      }
    } as MealScheduleEntry,
    {
      id: "5",
      date: new Date(),
      meal: {
        id: "5",
        name: "jajecznica po sopocku 3"
      }
    } as MealScheduleEntry,
    {
      id: "6",
      date: new Date(),
      meal: {
        id: "6",
        name: "jajecznica po sopocku 4"
      }
    } as MealScheduleEntry
  ];

  @Prop({
    required: true
  })
  label!: string;

  get sortedMealScheduleEntries() {
    return _.sortBy(this.mealSchedule, entry => entry.date);
  }

  addMealScheduleEntry() {}

  deleteMealScheduleEntry(id: string) {}
}
</script>

<style lang="less" scoped>
#label {
  text-align: center;
}
.label {
  color: #4e4e4e;
  font-weight: bold;
  position: relative;
  display: flex;
  justify-content: space-between;
  border-bottom-style: groove;
  border-bottom-right-radius: 1px;
  border-bottom-left-radius: 1px;
}
.content {
  overflow-y: scroll;
  height: 95%;
}
#add-icon {
  cursor: pointer;
}
button {
  color: white;
}
</style>

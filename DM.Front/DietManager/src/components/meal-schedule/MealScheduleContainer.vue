<template>
  <div id="schedule-container">
    <div class="label">
      <span id="label">{{label}}</span>
      <span id="add-icon" @click="addMealScheduleEntry">
        <font-awesome-icon class="option-icon" icon="plus-circle" size="lg" />
      </span>
    </div>
    <div class="content">
      <div id="schedule-entry" v-for="scheduleEntry in sortedMealScheduleEntries" :key="scheduleEntry.id">
        <meal-schedule-item :mealScheduleEntry="scheduleEntry" @schedule-changed="onScheduleChanged" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Emit } from "vue-property-decorator";
import MealScheduleEntry from "@/ViewModels/meal-schedule/mealScheduleEntry";
import MealScheduleItem from "./MealScheduleItem.vue";
import { DaysOfWeek } from "@/ViewModels/enums/daysOfWeek";
import _ from "lodash";
import { Actions } from "@/ViewModels/enums/actions";

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

  onScheduleChanged(data: {
    action: string;
    id: string;
    newDate: Date | null;
  }) {
    if (data.action === Actions[Actions.Update] && data.newDate !== null) {
      this.updateEntry(data.id, data.newDate!);
    } else {
      if (data.action === Actions[Actions.Delete]) {
        this.deleteEntry(data.id);
      }
    }
  }

  updateEntry(id: string, newDate: Date) {
    const indexOfUpdatedItem = this.mealSchedule.findIndex(e => e.id == id);
    const oldValue = this.mealSchedule[indexOfUpdatedItem];
    const newEntry = {
      id: oldValue.id,
      meal: oldValue.meal,
      date: newDate
    } as MealScheduleEntry;

    this.$set(this.mealSchedule, indexOfUpdatedItem, newEntry);
  }

  deleteEntry(id: string) {
    const indexOfDeletedItem = this.mealSchedule.findIndex(e => e.id == id);

    this.mealSchedule.splice(indexOfDeletedItem, 1);
  }

  addMealScheduleEntry() {}
}
</script>

<style lang="less" scoped>
#schedule-container {
  overflow: hidden;
}
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
  margin-right: -17px;
  overflow-y: scroll;
  height: 95%;
}
#add-icon {
  cursor: pointer;
}
#schedule-entry {
  position: relative;
  left: -2px;
}
button {
  color: white;
}
</style>

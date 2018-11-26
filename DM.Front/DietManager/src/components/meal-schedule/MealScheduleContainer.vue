<template>
  <div id="schedule-container">
    <modal v-if="showAddNewEntryModal">

    </modal>
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
    <add-new-entry v-if="showAddNewEntryModal" />
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
import MealScheduleEntryCreation from "@/ViewModels/meal-schedule/mealScheduleEntryCreation";
import Modal from "@/components/common/Modal.vue";
import AddNewEntryModal from "@/components/meal-schedule/AddMealScheduleEntry.vue";

@Component({
  components: {
    "meal-schedule-item": MealScheduleItem,
    modal: Modal,
    "add-new-entry": AddNewEntryModal
  }
})
export default class MealScheduleContainer extends Vue {
  @Prop({
    required: true
  })
  private mealSchedule!: MealScheduleEntry[];

  @Prop({
    required: true
  })
  private label!: string;

  private showAddNewEntryModal = false;
  private entryCreation: MealScheduleEntryCreation = {} as MealScheduleEntryCreation;

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

  addMealScheduleEntry() {
    this.showAddNewEntryModal = true;
  }
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

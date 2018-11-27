<template>
  <div id="schedule-container">
    <div class="label">
      <span id="label">{{label}}</span>
      <span id="add-icon" @click="showAddNewEntryModal = true">
        <font-awesome-icon class="option-icon" icon="plus-circle" size="lg"/>
      </span>
    </div>
    <div class="content">
      <div
        id="schedule-entry"
        v-for="scheduleEntry in sortedMealScheduleEntries"
        :key="scheduleEntry.id"
      >
        <meal-schedule-item
          :mealScheduleEntry="scheduleEntry"
          @schedule-changed="onScheduleChanged"
        />
      </div>
    </div>
    <add-new-entry
      v-if="showAddNewEntryModal"
      @close-modal="showAddNewEntryModal = false"
      @add-schedule-entry="addMealScheduleEntry"
    />
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
import MealScheduleApi from "@/services/api-callers/mealScheduleApi";

@Component({
  components: {
    "meal-schedule-item": MealScheduleItem,
    modal: Modal,
    "add-new-entry": AddNewEntryModal
  }
})
export default class MealScheduleContainer extends Vue {
  @Prop({ required: true })
  private mealSchedule!: MealScheduleEntry[];
  @Prop({ required: true })
  private label!: string;
  @Prop({ required: true })
  private date!: Date;

  private showAddNewEntryModal = false;

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
    MealScheduleApi.update({ id: id, newDate: newDate }, () =>
      this.$router.go(0)
    );
  }

  deleteEntry(id: string) {
    MealScheduleApi.delete(id, () => this.$router.go(0));
  }

  addMealScheduleEntry(entryCreation: { id: string; time: string }) {
    if (
      entryCreation.time === "" ||
      entryCreation.time.length != 5 ||
      !entryCreation.id
    ) {
      return;
    }

    const completeDate = this.getDateWithTime(entryCreation.time);

    MealScheduleApi.add({ mealId: entryCreation.id, date: completeDate }, id =>
      this.$router.go(0)
    );
  }

  getDateWithTime(time: string) {
    const hourAndMinutes = time.split(":");

    const newDate = new Date(this.date.getTime());
    newDate.setHours(parseInt(hourAndMinutes[0]), parseInt(hourAndMinutes[1]));

    return newDate;
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
.container {
  width: 90% !important;
}
</style>

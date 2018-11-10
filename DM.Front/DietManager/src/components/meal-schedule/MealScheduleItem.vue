<template>
  <div id="schedule-item" class="content-background">
    <div id="date">
      {{date}}
    </div>
    <div id="image-and-buttons">
      <button id="edit-button" class="btn main-background-color" @click="showEditModal">
        <font-awesome-icon icon="pencil-alt" />
      </button>
      <div id="image-container">
        <font-awesome-icon class="main-color" icon="utensils" />
      </div>
      <button id="delete-button" class="btn main-background-color" @click="deleteMealScheduleEntry">
        <font-awesome-icon icon="trash-alt" />
      </button>
    </div>
    <div id="meal-name">
      {{mealScheduleEntry.meal.name}}
    </div>
    <button id="details-button" class="btn main-background-color" @click="showDetails">Details</button>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import MealScheduleEntry from "@/ViewModels/meal-schedule/mealScheduleEntry";
import { Prop, Emit } from "vue-property-decorator";

@Component
export default class MealScheduleItem extends Vue {
  @Prop({
    required: true
  })
  mealScheduleEntry!: MealScheduleEntry;

  get date() {
    let dateString: string = "";

    const date = this.mealScheduleEntry.date;

    const hours = date.getHours();
    const minutes = date.getMinutes();

    if (hours < 10) {
      dateString = "0" + hours + ":";
    } else {
      dateString = hours + ":";
    }

    if (minutes < 10) {
      dateString += "0" + minutes;
    } else {
      dateString += minutes;
    }

    return dateString;
  }

  showDetails() {}

  showDeleteModal() {}

  showEditModal() {}

  @Emit()
  deleteMealScheduleEntry() {
    return this.mealScheduleEntry.id;
  }
}
</script>

<style lang="less" scoped>
#date {
  background-color: white;
  border-radius: 10px;
  margin-bottom: 10px;
}
#schedule-item {
  margin: 5px;
  padding: 5px;
  border-radius: 10px;
}
#schedule-item:hover {
  button {
    visibility: initial;
  }
}
button {
  color: white;
  visibility: hidden;
}
#image-container {
  border-radius: 50%;
  width: 30px;
  height: 30px;
  align-content: center;
  > * {
    width: 100%;
    height: 100%;
  }
}
#meal-name {
  margin: 5px;
}
#delete-button {
}
#edit-button {
}
#image-and-buttons {
  align-content: center;
  width: 98%;
  display: inline-flex;
  justify-content: space-evenly;
  button {
    width: 28px;
    height: 28px;
    text-align: center;
    padding: 5px;
    > * {
      position: relative;
      top: -3px;
    }
  }
}
#details-button {
  padding-top: 5px;
}
</style>

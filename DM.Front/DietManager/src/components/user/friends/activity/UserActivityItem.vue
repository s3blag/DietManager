<template>
  <div id="user-activity">
    <div id="user">
      <router-link :to="'/users/' + userActivity.user.id" class="avatar-image-container">
        <image-wrapper :imageId="userActivity.user.imageId" id="image" :asWheel="true">
          <template slot="placeholder" class="placeholder">
            <font-awesome-icon class="user-avatar main-color" icon="user-circle" />
          </template>
        </image-wrapper>
      </router-link>
      <span id="activity-description">
        <router-link class="user-link" :to="'/users/' + userActivity.user.id">{{this.userActivity.user.name + " " + this.userActivity.user.surname}}</router-link>
        {{activityDescription}}
      </span>
    </div>
    <hr>
    <div id="achievement-content">
      <new-friend-activity v-if="isAboutNewFriend" :friend="userActivity.friend" />
      <meal-activity v-else-if="isAboutAddedMeal" :meal="userActivity.meal" />
      <new-favourite-activity v-else-if="isAboutNewFavouriteMeal" :meal="userActivity.favourite" />
      <meal-ingredient-activity v-else-if="isAboutAddedMealIngredient" :mealIngredient="userActivity.mealIngredient" />
      <new-achievement-activity v-else-if="isAboutNewAchievement" :achievement="userActivity.achievement" />
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import UserActivity from "@/ViewModels/user/userActivity";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import Component from "vue-class-component";
import { Prop } from "vue-property-decorator";
import FriendActivity from "./FriendActivityContent.vue";
import MealActivity from "./MealActivityContent.vue";
import MealIngredientActivity from "./MealIngredientActivityContent.vue";
import AchievementActivity from "@/components/achievement/AchievementItem.vue";
import ImageWrapper from "@/components/image/ImageWrapper.vue";

@Component({
  components: {
    "new-friend-activity": FriendActivity,
    "meal-activity": MealActivity,
    "new-favourite-activity": MealActivity,
    "meal-ingredient-activity": MealIngredientActivity,
    "new-achievement-activity": AchievementActivity,
    "image-wrapper": ImageWrapper
  }
})
export default class UserActivtyItem extends Vue {
  @Prop({ required: true })
  private userActivity!: UserActivity;

  get isAboutNewAchievement() {
    return this.userActivity.achievement !== null;
  }

  get isAboutNewFriend() {
    return this.userActivity.friend !== null;
  }

  get isAboutAddedMeal() {
    return this.userActivity.meal !== null;
  }

  get isAboutAddedMealIngredient() {
    return this.userActivity.mealIngredient !== null;
  }

  get isAboutNewFavouriteMeal() {
    return this.userActivity.favourite !== null;
  }

  get activityDescription() {
    if (this.isAboutNewAchievement) {
      return " got a new achievement.";
    } else if (this.isAboutNewFriend) {
      return " made a new friend.";
    } else if (this.isAboutAddedMeal) {
      return " added a new meal.";
    } else if (this.isAboutAddedMealIngredient) {
      return " added a new meal ingredient.";
    } else if (this.isAboutNewFavouriteMeal) {
      return " added a meal to his favourites.";
    } else {
      return "";
    }
  }
}
</script>

<style lang="less" scoped>
#placeholder {
  > * {
    padding: 2px !important;
  }
}
#user {
  display: flex;
}
.user-link {
  text-decoration: underline;
  color: inherit;
}
#image {
  margin-top: 10px;
  display: flex;
  width: 50px;
  height: 50px;
  margin-left: 10px;
}
#activity-description {
  text-align: left;
  margin: 10px 0px 0px 5px;
}
</style>


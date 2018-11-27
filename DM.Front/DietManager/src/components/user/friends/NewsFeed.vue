<template>
  <div class="list-container">
    <h3 class="main-color">News feed</h3>
    <div class="tile-container">
      <h4 v-if="activities.length === 0 && lastReturned">There are no news to show</h4>
      <user-activity-item
        v-else
        class="activity-item"
        v-for="(activity, index) in activities"
        :key="index"
        :userActivity="activity"
      />
    </div>

    <button
      @click="getFriendsActivities"
      class="load-more-button main-background-color"
      v-if="elementsRemainingToLoad"
    >Load more...</button>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import ImageApiCaller from "@/services/api-callers/imageApi";
import FriendsApiCaller from "@/services/api-callers/friendsApi";
import UserActivity from "@/ViewModels/user/userActivity";
import UserActivityItem from "@/components/user/friends/activity/UserActivityItem.vue";
Component.registerHooks(["beforeRouteEnter"]);

@Component({
  components: {
    "user-activity-item": UserActivityItem
  }
})
export default class NewsFeed extends Vue {
  private activities: UserActivity[] = [];
  private lastReturned: IndexedResult<UserActivity> | null = null;

  beforeRouteEnter(
    to: any,
    from: any,
    next: (onBeforeRouteEnter: (instance: NewsFeed) => void) => void
  ) {
    next(instance => {
      instance.getFriendsActivities();
    });
  }

  get elementsRemainingToLoad() {
    if (!this.lastReturned) {
      return true;
    } else {
      return !this.lastReturned.isLast;
    }
  }

  getFriendsActivities() {
    FriendsApiCaller.getNewsFeed(
      this.lastReturned,
      this.getFriendsActivitiesSuccessHandler
    );
  }

  getFriendsActivitiesSuccessHandler(
    indexedActivities: IndexedResult<UserActivity[]>
  ) {
    if (
      !indexedActivities.result !== null ||
      indexedActivities.result.length > 0
    ) {
      this.activities.push(...indexedActivities.result);

      this.lastReturned = {
        result: indexedActivities.result[indexedActivities.result.length - 1],
        index: indexedActivities.index,
        isLast: indexedActivities.isLast
      };
    }
  }
}
</script>

<style lang="less" scoped>
.activity-item {
  width: 300px;
  border-radius: 7px;
  border-width: 1px;
  border-style: solid;
  border-color: #aaa;
  background-color: white;
}
h1 {
  margin-bottom: 15px !important;
}
h4 {
  color: grey;
  margin-top: 50px !important;
}
.load-more-button {
  border-radius: 7px;
  padding: 5px 10px 28px 10px;
  height: 30px;
  text-align: center;
  color: white;
}
.tile-container {
  display: flex;
  flex-wrap: wrap;
  margin: 0 auto;
  justify-content: center;
  > * {
    margin: 10px;
  }
}
</style>

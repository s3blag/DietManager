<template>
  <div class="list-container">
    <h3 class="main-color">My Friends</h3>
    <user-preview-item class="user-preview-item" v-for="user in friends" :key="user.id" :userPreview="user" :showFriendPin="false" />
    <button @click="getFriends" class="load-more-button main-background-color" v-if="elementsRemainingToLoad">
      Load more...
    </button>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

import MealPreview from "@/ViewModels/meal/mealPreview";
import MealApiCaller from "@/services/api-callers/mealApi";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import ImageApiCaller from "@/services/api-callers/imageApi";
import UserPreviewItem from "./UserPreviewItem.vue";
import FriendsApiCaller from "@/services/api-callers/friendsApi";
import User from "@/ViewModels/user/user";

Component.registerHooks(["beforeRouteEnter"]);

@Component({
  components: {
    "user-preview-item": UserPreviewItem
  }
})
export default class MyFriends extends Vue {
  private friends: User[] = [];
  private lastReturned: IndexedResult<User> | null = null;

  beforeRouteEnter(
    to: any,
    from: any,
    next: (onBeforeRouteEnter: (instance: MyFriends) => void) => void
  ) {
    next(instance => {
      instance.getFriends();
    });
  }

  get elementsRemainingToLoad() {
    if (!this.lastReturned) {
      return true;
    } else {
      return !this.lastReturned.isLast;
    }
  }

  getFriends() {
    FriendsApiCaller.getFriends(
      this.lastReturned,
      this.getFriendsSuccessHandler
    );
  }

  getFriendsSuccessHandler(indexedFriends: IndexedResult<User[]>) {
    if (!indexedFriends.result !== null || indexedFriends.result.length > 0) {
      this.friends.push(...indexedFriends.result);

      this.lastReturned = {
        result: indexedFriends.result[indexedFriends.result.length - 1],
        index: indexedFriends.index,
        isLast: indexedFriends.isLast
      };
    }
  }
}
</script>

<style lang="less">
h1 {
  margin-bottom: 15px !important;
}
.load-more-button {
  border-radius: 7px;
  padding: 5px 10px 28px 10px;
  height: 30px;
  text-align: center;
  color: white;
}
</style>

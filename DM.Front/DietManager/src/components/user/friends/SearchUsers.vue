<template>
  <div id="search">
    <h3 class="main-color">User search</h3>
    <div id="search-input">
      <input
        @keyup.enter="search"
        type="text"
        class="form-control"
        id="search-query"
        placeholder="Search..."
        v-model="searchQuery"
      >
      <button id="search-button" class="button main-background-color" @click="search">
        <font-awesome-icon id="search-icon" icon="search"/>
      </button>
    </div>
    <div id="users-container">
      <h4 v-if="users.length === 0 && lastReturned">No users were found</h4>
      <user-preview-item
        v-else
        class="result-item"
        v-for="user in users"
        :key="user.id"
        :userPreview="user"
        :showFriendPin="true"
        @user-deleted="onUserDeleted"
      />
    </div>
    <button
      v-if="!isLast && lastReturned"
      @click="loadMore"
      class="load-more-button main-background-color"
    >Load more...</button>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Watch } from "vue-property-decorator";
import UserPreviewItem from "./UserPreviewItem.vue";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import User from "@/ViewModels/user/user";
import Search from "@/ViewModels/meal/mealSearch";
import UserApiCaller from "@/services/api-callers/userApi";
import FriendsApiCaller from "@/services/api-callers/favouritesApi";

@Component({
  components: {
    "user-preview-item": UserPreviewItem
  }
})
export default class SearchUsers extends Vue {
  private users: User[] = [];
  private lastReturned: IndexedResult<User> | null = null;
  private searchQuery: string = "";
  private previousSearchQuery: string = "";

  get isLast() {
    if (!this.lastReturned) {
      return false;
    } else {
      return this.lastReturned.isLast;
    }
  }

  get searchIndex() {
    if (!this.lastReturned) {
      return 0;
    } else {
      return this.lastReturned.index;
    }
  }

  get isQueryEmpty() {
    return this.searchQuery.length === 0 || !this.searchQuery.trim();
  }

  search() {
    if (
      this.isQueryEmpty ||
      this.searchQuery.length < 2 ||
      this.searchQuery === this.previousSearchQuery
    ) {
      return;
    }

    this.lastReturned = null;
    this.users = [];
    this.previousSearchQuery = this.searchQuery;

    const lastReturnedSearch: IndexedResult<Search> = {
      result: { query: this.searchQuery },
      isLast: this.isLast,
      index: this.searchIndex
    };

    this.callApi(lastReturnedSearch);
  }

  onUserDeleted(id: string) {
    const indexOfDeletedItem = this.users.findIndex(u => u.id == id);
    this.users.splice(indexOfDeletedItem, 1);
  }

  loadMore() {
    const lastReturnedSearch: IndexedResult<Search> = {
      result: { query: this.previousSearchQuery },
      isLast: this.isLast,
      index: this.searchIndex
    };

    this.callApi(lastReturnedSearch);
  }

  callApi(lastReturnedSearch: IndexedResult<Search>) {
    UserApiCaller.search(lastReturnedSearch, this.getUsersSuccessHandler);
  }

  getUsersSuccessHandler(indexedUsers: IndexedResult<User[]>) {
    if (!indexedUsers.result !== null || indexedUsers.result.length > 0) {
      this.users.push(...indexedUsers.result);

      this.lastReturned = {
        result: indexedUsers.result[indexedUsers.result.length - 1],
        index: indexedUsers.index,
        isLast: indexedUsers.isLast
      };
    }
  }
}
</script>

<style lang="less" scoped>
#search {
  padding: 10px;
  background-color: #e6e4e4;
  border-radius: 10px;
  min-height: 600px;
}
.load-more-button {
  border-radius: 7px;
  padding: 5px 10px 28px 10px;
  height: 30px;
  text-align: center;
  color: white;
}
#search-input {
  display: inline-flex;
  margin: 10px;
}
#search-button {
  border-radius: 0px 5px 5px 0px;
}
#search-query {
  border-radius: 5px 0px 0px 5px;
}
#search-icon {
  color: white;
}
#users-container {
  margin-top: 10px;
}
.result-item {
  margin-bottom: 5px !important;
}
h4 {
  margin-top: 80px;
  color: grey;
}
</style>

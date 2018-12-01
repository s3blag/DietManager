<template>
  <div class="list-container">
    <h3 class="main-color">Friend invitations</h3>
    <h4 v-if="friendInvitations.length === 0 && lastReturned">You haven't received any invitations</h4>
    <div id="invitations-container">
      <div class="friend-invitation" v-for="(invitation, i) in friendInvitations" :key="i">
        <div class="image-container">
          <image-wrapper :imageId="invitation.imageId">
            <template slot="placeholder">
              <font-awesome-icon class="user-avatar main-color" icon="user-circle"/>
            </template>
          </image-wrapper>
        </div>
        <div class="user-info-element user-name">
          <div class="label">Full name</div>
          <div class="value">{{invitation.name + ' ' + invitation.surname}}</div>
        </div>
        <div class="user-info-element">
          <div class="label">City</div>
          <div class="value">{{invitation.city}}</div>
        </div>
        <div class="invite-action-wrapper">
          <button class="button ignore" @click="showIgnoreModal = true">Ignore</button>
          <button class="button accept" @click="acceptInvitation(i)">Accept</button>
        </div>
        <modal v-if="showIgnoreModal">
          <span class="modal-text">Are you sure you want to ignore this invitation?</span>
          <div class="modal-buttons-container">
            <button class="button" @click="ignoreInvitation(i)">Ignore</button>
            <button class="button" @click="showIgnoreModal = false">Cancel</button>
          </div>
        </modal>
      </div>
    </div>
    <button
      @click="getFriendInvitations"
      class="load-more-button main-background-color"
      v-if="elementsRemainingToLoad"
    >Load more...</button>
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
import FriendInvitation from "@/ViewModels/user/friendInvitation";
import ImageWrapper from "@/components/image/ImageWrapper.vue";
import Modal from "@/components/common/Modal.vue";
Component.registerHooks(["beforeRouteEnter"]);

@Component({
  components: {
    "user-preview-item": UserPreviewItem,
    "image-wrapper": ImageWrapper,
    modal: Modal
  }
})
export default class FriendInvitations extends Vue {
  private friendInvitations: FriendInvitation[] = [];
  private lastReturned: IndexedResult<FriendInvitation> | null = null;
  private showIgnoreModal = false;
  beforeRouteEnter(
    to: any,
    from: any,
    next: (onBeforeRouteEnter: (instance: FriendInvitations) => void) => void
  ) {
    next(instance => {
      instance.getFriendInvitations();
    });
  }

  get elementsRemainingToLoad() {
    if (!this.lastReturned) {
      return true;
    } else {
      return !this.lastReturned.isLast;
    }
  }

  getFriendInvitations() {
    FriendsApiCaller.getInvitations(
      this.lastReturned,
      this.getFriendInvitationsSuccessHandler
    );
  }

  getFriendInvitationsSuccessHandler(
    indexedFriendInvitations: IndexedResult<FriendInvitation[]>
  ) {
    if (
      !indexedFriendInvitations.result !== null ||
      indexedFriendInvitations.result.length > 0
    ) {
      this.friendInvitations.push(...indexedFriendInvitations.result);

      this.lastReturned = {
        result:
          indexedFriendInvitations.result[
            indexedFriendInvitations.result.length - 1
          ],
        index: indexedFriendInvitations.index,
        isLast: indexedFriendInvitations.isLast
      };
    }
  }

  ignoreInvitation(index: number) {
    FriendsApiCaller.ignoreInvitation(
      this.friendInvitations[index].userId,
      () => {
        this.friendInvitations.splice(index, 1);
        this.showIgnoreModal = false;
      }
    );
  }

  acceptInvitation(index: number) {
    FriendsApiCaller.acceptInvitation(
      this.friendInvitations[index].userId,
      () => this.friendInvitations.splice(index, 1)
    );
  }
}
</script>

<style lang="less" scoped>
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
h4 {
  margin-top: 100px !important;
  color: grey !important;
}
.friend-invitation {
  border-radius: 10px;
  background-color: white;
  max-width: 700px;
  min-width: 305px;
  display: flex;
  margin: 0 auto;
  justify-content: space-between;
}
.invite-action-wrapper {
  margin: auto 10px auto 0px;
}
.button.ignore {
  margin-right: 10px;
}
#invitations-container {
  > * {
    margin-bottom: 10px;
  }
}
</style>

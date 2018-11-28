<template>
  <div id="friend">
    <div id="friend-data">
      <modal v-if="showDeleteFriendModal">
        <span class="modal-text">Are you sure you want to remove this user from your friends?</span>
        <div class="modal-buttons-container">
          <button class="button" @click="removeFromFriends">Delete</button>
          <button class="button" @click="showDeleteFriendModal = false">Cancel</button>
        </div>
      </modal>
      <div id="unfriend-button">
        <button class="button" @click="showDeleteFriendModal = true">Unfriend</button>
      </div>
      <div id="image">
        <div id="user-image">
          <image-wrapper :imageId="friendWithAchievements.user.imageId">
            <template slot="placeholder">
              <font-awesome-icon class="user-avatar main-color" icon="user-circle"/>
            </template>
          </image-wrapper>
        </div>
      </div>
      <div
        id="user-name"
      >{{friendWithAchievements.user.name + ' ' + friendWithAchievements.user.surname}}</div>
      <div id="user-city">{{friendWithAchievements.user.city}}</div>
    </div>
    <div id="achievements-container">
      <h4 class="main-color soft-border bottom">Achievements</h4>
      <user-achievements :friendsGroupedAchievements="friendWithAchievements.achievements"/>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import ImageWrapper from "@/components/image/ImageWrapper.vue";
import UserApiCaller from "@/services/api-callers/userApi";
import AuthService from "@/services/authService";
import User from "@/ViewModels/user/user";
import UserUpdate from "@/ViewModels/user/userUpdate";
import ImageApiCaller from "@/services/api-callers/imageApi";
import ImageUploader from "@/components/image/ImageUploader.vue";
import Modal from "@/components/common/Modal.vue";
import FriendWithAchievements from "@/ViewModels/user/friendWithAchievement";
import FriendsApiCaller from "@/services/api-callers/friendsApi";
import UserAchievements from "@/components/user/user-panel/UserAchievements.vue";
Component.registerHooks(["beforeRouteEnter"]);

@Component({
  components: {
    "image-wrapper": ImageWrapper,
    modal: Modal,
    "user-achievements": UserAchievements
  }
})
export default class Friend extends Vue {
  private friendWithAchievements: FriendWithAchievements = {
    user: { name: "", surname: "", imageId: null }
  } as any;
  private showDeleteFriendModal = false;
  beforeRouteEnter(
    to: any,
    from: any,
    next: (onBeforeRouteEnter: (instance: Friend) => void) => void
  ) {
    next(instance => {
      const pathArgs = instance.$route.path.split("/");
      const id = pathArgs[pathArgs.length - 1];
      instance.loadUser(id);
    });
  }

  loadUser(id: string) {
    FriendsApiCaller.get(id, this.onLoadUserSuccess);
  }

  onLoadUserSuccess(friendWithAchievements: FriendWithAchievements) {
    this.friendWithAchievements = friendWithAchievements;
  }

  removeFromFriends() {
    FriendsApiCaller.removeFromFriends(
      this.friendWithAchievements.user.id,
      () => this.$router.replace({ name: "Home" })
    );
  }
}
</script>

<style lang="less" scoped>
#friend {
  width: 80%;
  margin: 0 auto;
}
#user-city {
  margin-top: 5px;
}
#image {
  display: inline-flex;
}
#unfriend-button {
  text-align: right;
  margin-top: 15px;
  min-width: 100%;
  height: 60px;
  > * {
    margin-top: 10px;
    margin-right: 20px;
    height: 30px;
    font-size: 0.8em;
    letter-spacing: 1.5px;
  }
}

#image {
  display: inline-flex;
}
#friend-data {
  min-height: 220px;
  padding-bottom: 20px;
}
#placeholder {
  width: 350px;
  height: 350px;
  > * {
    width: 100%;
    height: 100%;
  }
}
.image-wrapper {
  width: 250px;
  height: 250px;
  min-width: 100px;
  min-height: 100px;
}
#achievements-container {
  margin: 50px auto 0px auto;
}
h4 {
  padding-bottom: 5px;
}
</style>

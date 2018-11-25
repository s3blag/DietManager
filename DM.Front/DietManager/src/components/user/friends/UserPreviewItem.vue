<template>
  <div id="preview-item">
    <div class="image-container">
      <image-wrapper :imageId="userPreview.imageId">
        <template slot="placeholder">
          <font-awesome-icon class="user-avatar main-color" icon="user-circle" />
        </template>
      </image-wrapper>
      <div v-if="showFriendPin && userPreview.isFriend" class="pin">
        <font-awesome-icon id="my-friends-icon" class="option-icon" icon="user-friends" />
      </div>
    </div>
    <div class="user-info-element user-name">
      <div class="label">Name</div>
      <div class="value">{{userPreview.name + ' ' + userPreview.surname}} </div>
    </div>
    <div class="user-info-element">
      <div class="label">City</div>
      <div class="value">{{userPreview.city}} </div>
    </div>
    <div id="invite-wrapper" v-if="!userPreview.isFriend">
      <div id="invite-button" @click="addToFriends">
        <font-awesome-icon id="invite-icon" class="option-icon" icon="user-plus" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import FriendsApiCaller from "@/services/api-callers/friendsApi";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import { Prop } from "vue-property-decorator";
import ImageWrapper from "@/components/image/ImageWrapper.vue";
import User from "@/ViewModels/user/user";

Component.registerHooks(["beforeRouteEnter"]);

@Component({
  components: {
    "image-wrapper": ImageWrapper
  }
})
export default class MyMeals extends Vue {
  @Prop({ required: true })
  private userPreview!: User;

  @Prop()
  private showFriendPin!: boolean;

  addToFriends() {
    FriendsApiCaller.sendInvitation(
      this.userPreview.id,
      this.invitationSentSuccessHandler
    );
  }

  invitationSentSuccessHandler() {
    this.userPreview.isFriend = true;
  }
}
</script>

<style lang="less" scoped>
#preview-item {
  border-radius: 10px;
  background-color: white;
  max-width: 450px;
  min-width: 305px;
  display: flex;
  margin: 0 auto;
}
.image-container {
  position: relative;
  margin-top: auto;
  margin-bottom: auto;
  text-align: center;
  vertical-align: center;
  width: 65px;
  height: 65px;
  max-width: 65px;
  max-height: 65px;
  min-width: 65px;
}
.pin {
  position: absolute;
  top: -10px;
  left: 53px;
}

.user-info-element {
  flex-grow: 1;
  margin-left: 10px;
  display: inline-block;
  margin-top: auto;
  margin-bottom: auto;
  width: 100px;
}
.user-name {
  overflow: hidden;
}
.label {
  color: #929191;
}
#invite-wrapper {
  position: relative;
  width: 60px;
}
#invite-button {
  padding: 20px;
}

@keyframes button-animation {
  0% {
    left: 0.6px;
  }
  10% {
    left: 1.2px;
  }
  20% {
    left: 1.8px;
  }
  30% {
    left: 2.4px;
  }
  40% {
    left: 3px;
  }
  50% {
    left: 3.6px;
  }
  60% {
    left: 4.2px;
  }
  70% {
    left: 4.8px;
  }
  80% {
    left: 5.4px;
  }
  90% {
    left: 6px;
  }
  100% {
    left: 6.6px;
  }
}
</style>

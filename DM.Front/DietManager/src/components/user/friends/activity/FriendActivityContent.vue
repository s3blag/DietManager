<template>
  <div>
    <layout>
      <template slot="picture">
        <image-wrapper :imageId="friend.imageId" :asWheel="false">
          <template slot="placeholder">
            <font-awesome-icon class="user-avatar main-color" icon="user-circle"/>
          </template>
        </image-wrapper>
      </template>
      <template slot="details">
        <div>{{friend.name + ' ' + friend.surname}}</div>
        <div id="city">{{friend.city}}</div>
      </template>
    </layout>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import Achievement from "@/ViewModels/achievement/achievement";
import { Prop } from "vue-property-decorator";
import ActivityContentLayout from "./ActivityContentLayout.vue";
import Meal from "@/ViewModels/meal/meal";
import ImageWrapper from "@/components/image/ImageWrapper.vue";
import User from "@/ViewModels/user/user";

@Component({
  components: {
    layout: ActivityContentLayout,
    "image-wrapper": ImageWrapper
  }
})
export default class FriendActivityContent extends Vue {
  @Prop({ required: true })
  private friend!: User;
}
</script>

<style lang="less" scoped>
#city {
  color: grey;
  font-size: 0.85em;
}
</style>

<template>
  <div id="image-wrapper">
    <img v-if="imageId" :src="'/api/image/' + imageId" alt="image">
    <slot v-else name="placeholder"></slot>
  </div>
</template>
<script lang="ts">
import Vue from "vue";
import { Prop, Component } from "vue-property-decorator";

@Component
export default class ImageWrapper extends Vue {
  @Prop({ required: true })
  imageId!: string;
}
</script>
<style lang="less" scoped>
#image-wrapper {
  width: 65px;
  height: 65px;
  img {
    width: 100%;
    height: 100%;
    border-radius: 50%;
  }
}
</style>

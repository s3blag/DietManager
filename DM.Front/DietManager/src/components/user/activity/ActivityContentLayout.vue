<template>
  <div id="activity-content">
    <div id="activity-content-picture">
      <slot name="picture"></slot>
    </div>
    <hr>
    <div id="activity-content-details">
      <slot name="details"></slot>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop } from "vue-property-decorator";

@Component
export default class ActivityContentLayout extends Vue {}
</script>

<style lang="less" scoped>
#activity-content {
  width: 225px;
  display: block;
  // border-width: 1px;
  // border-radius: 8px;
  // border-color: rgb(195, 195, 195);
  // border-style: solid;
  > * {
    width: 100%;
  }
}
#activity-content-picture {
  padding: 10px;
  height: 150px;
  > * {
    width: 100%;
    height: 100%;
  }
}
#activity-content-details {
  margin-bottom: 5px;
  > * {
    width: 100%;
  }
}
img {
  border-radius: 10px !important;
}
</style>


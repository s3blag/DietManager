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
  width: 100%;
  display: block;
  text-decoration: underline;
  > * {
    width: 100%;
  }
}
#activity-content-picture {
  padding: 10px;
  min-height: 150px;
  > * {
    max-width: 100%;
    max-height: 100%;
    min-width: 80%;
    min-height: 150px;
    margin: 0 auto;
  }
}
#activity-content-details {
  margin: 0px 5px 16px 5px;
  > * {
    width: 100%;
  }
}
img {
  border-radius: 10px !important;
}
</style>


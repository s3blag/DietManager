<template>
  <div id="achievement">
    <div id="achievement-icon">
      <font-awesome-icon class="user-avatar main-color" icon="trophy" size="lg" />
    </div>
    <hr>
    <div id="achievement-details">
      <div>
        Category: {{achievement.category}}
      </div>
      <div>
        Type: {{achievement.type}}
      </div>
      <div>
        Value: {{achievement.value}}
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import Achievement from "@/ViewModels/achievement/achievement";
import { Prop } from "vue-property-decorator";

@Component
export default class AchievementItem extends Vue {
  @Prop({ required: true })
  private achievement!: Achievement;
}
</script>

<style lang="less" scoped>
#achievement {
  width: 225px;
  display: block;
  border-width: 1px;
  border-radius: 8px;
  border-color: rgb(195, 195, 195);
  border-style: solid;
  > * {
    margin: 5px;
    width: 100%;
  }
}
#achievement-icon {
  padding: 5px;
  height: 150px;
  > * {
    width: 100%;
    height: 100%;
  }
}
#achievement-details {
  margin-bottom: 5px;
  > * {
    width: 100%;
  }
}
</style>

<template>
  <div>
    <div class="achievement-type soft-border bottom">{{translate[type]}}</div>
    <div class="values">
      <div
        v-if="values && values.length > 0"
        class="value-circle main-background-color"
        v-for="v in values"
        :key="v"
      >{{v}}</div>
      <div v-else>
        <h5>No achievements in this category.</h5>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop } from "vue-property-decorator";
import Translate from "@/services/translationDictionary";

@Component({
  components: {}
})
export default class UserAchievements extends Vue {
  @Prop({ required: true })
  private values!: number[];
  @Prop({ required: true })
  private type!: string;
  private translate = Translate;
  created() {}
}
</script>

<style lang="less" scoped>
.value-circle {
  display: flex;
  width: 35px;
  height: 35px;
  border-radius: 50%;
  color: white;
  margin: 5px;
  justify-content: center;
  align-content: center;
  flex-direction: column;
  position: relative;
  top: 10px;
}
.values {
  display: flex;
}
</style>

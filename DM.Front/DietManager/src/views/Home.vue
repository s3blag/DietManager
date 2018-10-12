<template>
  <div class="welcome">
    Welcome to DietManager
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component({
  components: {}
})
export default class Home extends Vue {}
</script>

<style>
.welcome {
  position: absolute;
  top: 50%;
  left: 48%;
}
</style>


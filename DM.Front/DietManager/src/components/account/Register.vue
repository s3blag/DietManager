<template>
  <div></div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import Modal from "@/components/common/Modal.vue";
import UserRegistrationForm from "@/ViewModels/user/userRegistrationForm";
Component.registerHooks(["beforeRouteEnter"]);

@Component({
  components: {
    modal: Modal
  }
})
export default class Register extends Vue {
  private registrationForm: UserRegistrationForm = {} as any;
}
</script>

<style lang="less" scoped>
</style>

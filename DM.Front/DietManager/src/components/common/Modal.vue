<template>
  <transition name="modal">
    <div class="mask">
      <div class="wrapper">
        <div class="container">
          <slot>
            default header
          </slot>
        </div>
      </div>
    </div>

  </transition>
</template>

<script lang="ts">
import Vue from "vue";
export default Vue.extend({});
</script>

<style lang="less" scoped>
.mask {
  position: fixed;
  z-index: 100000;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: table;
  transition: opacity 0.3s ease;
}

.wrapper {
  display: table-cell;
  vertical-align: middle;
}

.container {
  display: flex;
  flex-wrap: wrap;
  max-height: 90%;
  max-width: 95%;
  width: fit-content;
  margin: 0px auto;
  padding: 20px 30px;
  background-color: #fff;
  border-radius: 10px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
  transition: all 0.3s ease;
  font-family: Helvetica, Arial, sans-serif;
}

.modal-enter {
  opacity: 0;
}

.modal-leave-active {
  opacity: 0;
}

.modal-enter .container,
.modal-leave-active .container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}

@media screen and (max-width: 860px) {
  .container {
    overflow-y: auto;
  }
}
</style>

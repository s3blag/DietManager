<template>
  <div class="row">
    <div class="column options">
      <div>
        <ul>
          <slot></slot>
        </ul>
      </div>
    </div>
    <div class="column content">
      <router-view></router-view>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

@Component
export default class SideMenu extends Vue {}
</script>

<style scoped>
.column {
  margin: 10px;
  float: left;
}
.column.options {
  border: gray;
  border-radius: 10px;
  background-color: #e6e4e4;
  max-width: 8%;
  min-width: 180px;
  height: fit-content;
  padding: 2px;
  flex-grow: 1;
}
.column.content {
  width: 95%;
  min-height: 750px;
  height: fit-content;
  border-radius: 10px;
}
.row {
  margin: 20px;
  height: 75%;
  display: flex;
  flex-wrap: nowrap;
}
.row:after {
  content: "";
  display: table;
  clear: both;
}
ul {
  list-style: none;
  padding: 0px;
  margin: 10px 0px;
  font-size: 18px;
}
li:hover .option-icon {
  color: white;
}
li {
  padding: 5px 0px;
  border-radius: 8px;
  margin: 10px;
}
li:hover {
  background-color: #4495d8;
  color: white;
}
@media screen and (max-width: 1000px) {
  .column {
    margin-left: auto;
    margin-right: auto;
    float: none;
    display: block;
  }
  .column.options {
    width: 100%;
  }
  .column.content {
    width: 100%;
  }
}
</style>

<style lang="less">
.option-icon {
  color: #3c88c7;
}
</style>


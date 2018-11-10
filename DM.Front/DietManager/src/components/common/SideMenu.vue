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
  width: 10%;
  min-width: 250px;
  height: fit-content;
  padding: 2px;
}
.column.content {
  width: 84.3%;
  min-height: 750px;
  height: fit-content;
  /* background-color: #e6e4e4; */
  border-radius: 10px;
}
.row {
  margin: 20px;
  height: 75%;
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
.link {
  cursor: pointer;
  text-decoration: none;
  color: inherit;
  border-radius: 5px;
}
@media screen and (max-width: 1300px) {
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


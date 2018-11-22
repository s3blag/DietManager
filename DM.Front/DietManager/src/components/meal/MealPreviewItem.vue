<template>
  <div id="preview-item">
    <div class="image-container">
      <image-wrapper :imageId="mealPreview.imageId">
        <template slot="placeholder">
          <font-awesome-icon class="main-color" icon="utensils" size="2x" />
        </template>
      </image-wrapper>
      <div v-if="mealPreview.isFavourite" class="pin">
        <font-awesome-icon class="main-color" icon="star" />
      </div>
      <div v-else-if="createdByUser && enableFavouriteMarkToggling" class="pin">
        <font-awesome-icon class="main-color" icon="user-circle" />
      </div>
    </div>
    <div class="meal-info-element meal-name">
      <div class="label">Name</div>
      <div class="value">{{mealPreview.name}} </div>
    </div>
    <div class="meal-info-element">
      <div class="label">Calories</div>
      <div class="value">{{mealPreview.calories}} </div>
    </div>
    <span v-if="!isMobile" class="details">
      <div class="meal-info-element">
        <div class="label">Used by</div>
        <div class="value">{{mealPreview.numberOfUses}} people</div>
      </div>
      <div class="meal-info-element">
        <div class="label">Favourite by</div>
        <div class="value">{{mealPreview.numberOfFavouriteMarks}} people</div>
      </div>
    </span>
    <div id="add-to-favourites-wrapper" v-if="enableFavouriteMarkToggling">
      <div v-if="enableFavouriteMarkToggling && mealPreview.isFavourite !== null" id="add-to-favourites-button" @click="toggleFavouriteMark">
        <font-awesome-icon v-if="mealPreview.isFavourite === false" id="add-icon" class="option-icon" icon="plus-circle" />
        <font-awesome-icon v-else id="delete-icon" class="option-icon" icon="minus-circle" />
      </div>
    </div>

    <router-link class="go-to-meal" :to="'/meal/' + mealPreview.id">
      <font-awesome-icon class="main-color" icon="arrow-alt-circle-right" size="2x" />
    </router-link>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import MealPreview from "@/ViewModels/meal/mealPreview";
import MealApiCaller from "@/services/api-callers/mealApi";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import ImageApiCaller from "@/services/api-callers/imageApi";
import FavouritesApiCaller from "@/services/api-callers/favouritesApi";
import { Prop } from "vue-property-decorator";
import ImageWrapper from "@/components/image/ImageWrapper.vue";

Component.registerHooks(["beforeRouteEnter"]);

@Component({
  components: {
    "image-wrapper": ImageWrapper
  }
})
export default class MyMeals extends Vue {
  @Prop({
    required: true
  })
  private mealPreview!: MealPreview;

  @Prop({
    required: true
  })
  private enableFavouriteMarkToggling!: boolean;

  get isMobile() {
    if (window.innerWidth < 860) {
      return true;
    } else {
      return false;
    }
  }

  get createdByUser() {
    return this.mealPreview.isFavourite === null;
  }

  toggleFavouriteMark() {
    if (this.mealPreview.isFavourite) {
      FavouritesApiCaller.delete(
        this.mealPreview.id,
        this.deletedFromFavouritesSuccessHandler
      );
    } else {
      FavouritesApiCaller.add(
        this.mealPreview.id,
        this.addedToFavouritesSuccessHandler
      );
    }
  }

  deletedFromFavouritesSuccessHandler() {
    this.mealPreview.isFavourite = false;
    this.mealPreview.numberOfFavouriteMarks--;
    this.$emit("deleted-from-favourites", this.mealPreview.id);
  }

  addedToFavouritesSuccessHandler() {
    this.mealPreview.isFavourite = true;
    this.mealPreview.numberOfFavouriteMarks++;
  }
}
</script>

<style>
.image-container {
  position: relative;
  margin-top: auto;
  margin-bottom: auto;
  text-align: center;
  vertical-align: center;
  width: 65px;
  height: 65px;
  max-width: 65px;
  max-height: 65px;
  min-width: 65px;
}

.meal-info-element {
  flex-grow: 1;
  margin-left: 10px;
  display: inline-block;
  margin-top: auto;
  margin-bottom: auto;
  width: 100px;
}
.meal-name {
  overflow: hidden;
}
.label {
  color: #929191;
}
.go-to-meal {
  margin-top: auto;
  margin-bottom: auto;
  text-align: center;
  width: 65px;
}
.go-to-meal:hover {
  position: relative;
  animation-name: button-animation;
  animation-duration: 0.1s;
  animation-timing-function: ease-in-out;
  animation-fill-mode: both;
}
.details {
  flex-grow: 2;
  display: flex;
  justify-content: space-between;
}
.details > * {
  width: 100px;
}
#add-to-favourites-wrapper {
  position: relative;
  width: 60px;
}
#add-to-favourites-button {
  padding: 20px;
}

@keyframes button-animation {
  0% {
    left: 0.6px;
  }
  10% {
    left: 1.2px;
  }
  20% {
    left: 1.8px;
  }
  30% {
    left: 2.4px;
  }
  40% {
    left: 3px;
  }
  50% {
    left: 3.6px;
  }
  60% {
    left: 4.2px;
  }
  70% {
    left: 4.8px;
  }
  80% {
    left: 5.4px;
  }
  90% {
    left: 6px;
  }
  100% {
    left: 6.6px;
  }
}
</style>

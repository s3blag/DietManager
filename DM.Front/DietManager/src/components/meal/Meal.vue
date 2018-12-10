<template>
  <div id="meal" class="content-background">
    <div id="meal-name" class="main-color">{{meal.name}}</div>
    <button v-if="isAdmin" class="button" @click="showDeleteModal = true">Delete meal</button>
    <div id="meal-summary">
      <div class="column-left content-background">
        <div id="meal-image">
          <image-wrapper :imageId="meal.imageId" :asWheel="false">
            <template slot="placeholder">
              <font-awesome-icon class="main-color" icon="utensils" size="2x"/>
            </template>
          </image-wrapper>
        </div>
        <div id="brief-description">
          <div id="add-to-favourites-button" v-if="meal.isFavourite === false">
            <button class="button" @click="addToFavourites">Add to favourites</button>
          </div>
          <div id="creator">
            <div id="user-image">
              <image-wrapper :imageId="meal.creator.imageId">
                <template slot="placeholder">
                  <font-awesome-icon class="user-avatar main-color" icon="user-circle"/>
                </template>
              </image-wrapper>
            </div>
            <span>{{meal.creator.name + ' ' + meal.creator.surname}}</span>
          </div>
          <div class="stats">Used {{meal.numberOfUses}} times</div>
          <div class="stats">Favourite by {{meal.numberOfFavouriteMarks}} people</div>
        </div>
      </div>

      <div id="cntr">
        <div class="meal-ingredients-container">
          <div>
            <h6>Ingredients</h6>
          </div>
          <ul class="added-meal-ingredients">
            <li
              class="meal-ingredient"
              v-for="mealIngredientWithQuantity in meal.ingredients"
              :key="mealIngredientWithQuantity.mealIngredient.id"
            >
              <div class="image"></div>
              <span class="name">{{mealIngredientWithQuantity.mealIngredient.name}}</span>
              <span class="quantity">{{mealIngredientWithQuantity.quantity + 'g'}}</span>
            </li>
          </ul>
        </div>
        <div>
          <h6>Summary</h6>
          <meal-summary class="summary" :mealIngredients="meal.ingredients"/>
        </div>
      </div>

      <div id="recipe-container">
        <h6>Recipe</h6>
        <div id="recipe">{{meal.description}}</div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import ImageWrapper from "@/components/image/ImageWrapper.vue";
import MealApiCaller from "@/services/api-callers/mealApi";
import MealVM from "@/ViewModels/meal/meal";
import FavouritesApiCaller from "@/services/api-callers/favouritesApi";
import MealSummary from "./MealSummary.vue";
Component.registerHooks(["beforeRouteEnter"]);

@Component({
  components: {
    "image-wrapper": ImageWrapper,
    "meal-summary": MealSummary
  }
})
export default class Meal extends Vue {
  private meal: MealVM = {
    creator: { imageId: null },
    imageId: null,
    ingredients: []
  } as any;

  beforeRouteEnter(
    to: any,
    from: any,
    next: (onBeforeRouteEnter: (instance: Meal) => void) => void
  ) {
    next(instance => {
      const pathArgs = instance.$route.path.split("/");
      const id = pathArgs[pathArgs.length - 1];
      instance.loadMeal(id);
    });
  }

  loadMeal(id: string) {
    MealApiCaller.get(id, this.onLoadMealSuccess);
  }

  onLoadMealSuccess(meal: MealVM) {
    this.meal = meal;
  }

  addToFavourites() {
    FavouritesApiCaller.add(this.meal.id, () => (this.meal.isFavourite = true));
  }

  get isAdmin() {
    return this.$route.params && this.$route.params.asAdmin;
  }
}
</script>

<style lang="less" scoped>
#cntr {
  display: inline-flex;
  flex-grow: 4;
  justify-content: space-evenly;
  flex-wrap: wrap;
}
#meal {
  width: 75%;
  margin: 25px auto;
  border-radius: 10px;
  max-width: 1500px;
}
#meal-name {
  font-weight: bold;
  font-size: 1.5em;
}
.column-left {
  display: inline-flex;
  flex-grow: 1;
  min-width: 300px;
  height: fit-content;
  padding: 10px;
  border-radius: 10px;
  flex-wrap: wrap;
}
#brief-description {
  display: block;
  padding: 0px;
  margin-left: 15px;
  > * {
    margin-bottom: 10px;
  }
}
.meal-ingredients-container {
  .added-meal-ingredients {
    border-color: rgb(190, 190, 190);
    border-style: solid;
    border-radius: 10px;
    text-overflow: ellipsis;
  }
  height: fit-content;
  max-width: 250px;
  min-width: 200px;
  > * {
    padding: 5px;
  }
}
.meal-ingredient {
  width: 100%;
  display: inline-flex;
  border-bottom-style: solid;
  border-color: rgb(190, 190, 190);
  .quantity {
    text-align: right;
  }
  .name {
    width: 100%;
    text-align: left;
  }
}

#meal-image {
  > * {
    max-width: 400px;
    min-width: 267px;
    min-height: 150px;
    max-height: 225px;
    border-radius: 10px;
  }
}
#add-to-favourites-button {
  padding: 0px;
  text-align: left;
  button {
    width: 140px;
  }
}
#user-image {
  padding: 0px;
  width: fit-content;
  height: fit-content;

  .image-wrapper {
    width: 40px;
    height: 40px;
  }
}
#creator {
  display: inline-flex;
  word-wrap: break-word;
  span {
    padding-left: 5px;
    margin: auto auto;
  }
}
#meal-summary {
  width: 100%;
  display: inline-flex;
  justify-content: space-between;
  flex-wrap: wrap;
}

.stats {
  text-align: left;
}
.summary {
  border-style: solid;
  border-color: rgb(190, 190, 190);
  padding: 10px;
}
#recipe-container {
  max-width: 100%;
  margin: 0 auto;
  padding: 10px;
}
#recipe {
  border-style: solid;
  border-color: rgb(190, 190, 190);
  width: 800px;
  word-wrap: break-word;
  white-space: pre-wrap;
  min-height: 300px;
  border-radius: 10px;
  text-align: left;
  padding: 10px;
  max-width: 100%;
}
</style>

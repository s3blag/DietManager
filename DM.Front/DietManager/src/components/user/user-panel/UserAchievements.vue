<template>
  <div id="user-achievements">
    <div class="achievements">
      <h4 v-if="!groupedAchievements.any">
        <span v-if="!isSelf">This user hasn't earned any achievements yet</span>
        <span v-else>You haven't earned any achievements yet</span>
      </h4>
      <div
        v-else
        class="achievement-wrapper"
        v-for="(achievements, categoryName) in groupedAchievements"
        :key="categoryName"
      >
        <div
          v-if="!isEmpty(achievements)"
          class="category soft-border bottom"
        >{{translate[categoryName]}}</div>
        <div class="achievement" v-for="(values, name) in achievements" :key="name">
          <user-achievement :values="values" :type="name"/>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import GroupedAchievements from "@/ViewModels/achievement/groupedAchievements";
import AchievementsApi from "@/services/api-callers/achievementsApi";
import Achievement from "@/ViewModels/achievement/achievement";
import UserAchievement from "@/components/achievement/UserAchievement.vue";
import Translate from "@/services/translationDictionary";
import { Prop } from "vue-property-decorator";

@Component({
  components: {
    "user-achievement": UserAchievement
  }
})
export default class UserAchievements extends Vue {
  @Prop({ required: false, default: null })
  private friendsGroupedAchievements!: GroupedAchievements | null;

  private groupedAchievements: GroupedAchievements = {
    mealAchievement: {},
    mealIngredientAchievement: {},
    userAchievement: {},
    mealScheduleAchievement: {},
    friendAchievement: {},
    any: false
  };
  private translate = Translate;

  beforeRouteEnter(
    to: any,
    from: any,
    next: (onBeforeRouteEnter: (instance: UserAchievements) => void) => void
  ) {
    next(instance => {
      //TODO:
      //instance.getLoggedInUser();

      if (instance.isSelf) {
        instance.fetchAchievements();
      } else {
        instance.groupedAchievements = instance.friendsGroupedAchievements!;
      }
    });
  }

  get isSelf() {
    return (
      typeof this.friendsGroupedAchievements === "undefined" ||
      !this.friendsGroupedAchievements
    );
  }

  fetchAchievements() {
    AchievementsApi.get(achievements => {
      if (achievements.any) {
        this.groupedAchievements = Object.assign(
          {},
          this.groupedAchievements,
          achievements.groupedAchievements
        );
      }
    });
  }

  isEmpty(obj: object) {
    return Object.keys(obj).length === 0;
  }
}
</script>

<style lang="less" scoped>
.category {
  font-size: 1.05em;
  font-weight: bold;
}
#user-achievements {
  width: 95%;
  margin: 15px auto;
}
.achievement {
  width: fit-content;
  margin: 25px 10px 10px 10px;
}
h4 {
  margin-top: 100px !important;
  color: grey !important;
}
</style>

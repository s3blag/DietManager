<template>
  <div>
    <modal v-if="showChangeImageWarning">
      <span class="modal-text">Changing the photo will remove the previous one. <br>Do you still want to proceed?</span>
      <div class="modal-buttons-container">
        <button class="button" @click="showFileInput">Continue</button>
        <button class="button" @click="showChangeImageWarning = false">Cancel</button>
      </div>
    </modal>
    <modal v-if="showDeleteImageWarning">
      <span class="modal-text">Are you sure you want to remove this photo?</span>
      <div class="modal-buttons-container">
        <button class="button" @click="deleteImage">Delete</button>
        <button class="button" @click="showDeleteImageWarning = false">Cancel</button>
      </div>
    </modal>
    <div>
      <img v-if="predefinedImageId" class="preview" :src="'/api/image/' + predefinedImageId">
      <slot v-else name="placeholder"></slot>
    </div>
    <input hidden="hidden" type="file" @change="showImage" ref="file-input" accept="image/*">
    <div id="image-buttons" class="input-buttons-container">
      <div>
        <button v-if="predefinedImageId" class="button" @click="showChangeImageWarning = true">
          <span>Change</span>
        </button>
        <button v-else class="button" @click="showFileInput">
          <span>Add</span>
        </button>
      </div>
      <div v-if="predefinedImageId">
        <button class="button" @click="showDeleteImageWarning = true">
          Remove
        </button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { Prop, Component } from "vue-property-decorator";
import ImageApiCaller from "@/services/api-callers/imageApi";
import Modal from "@/components/common/Modal.vue";

@Component({
  components: {
    modal: Modal
  }
})
export default class ImageUploader extends Vue {
  @Prop({ required: true, default: null })
  private predefinedImageId!: string | null;
  private base64Image: string | null = null;

  private showChangeImageWarning: boolean = false;
  private showDeleteImageWarning: boolean = false;

  showImage(event: any) {
    const fileInput = event.target;

    if (fileInput.files && fileInput.files[0]) {
      var reader = new FileReader();
      reader.onload = (evnt: any) => {
        this.base64Image = evnt.target.result;
        if (this.base64Image!.length > 0) {
          ImageApiCaller.add(this.base64Image!, id => {
            this.$emit("addImage", id);
          });
        }
      };
      reader.readAsDataURL(fileInput.files[0]);
    }
  }

  deleteImage() {
    this.showDeleteImageWarning = false;
    this.base64Image = null;
    this.$emit("deleteImage");
  }

  showFileInput() {
    this.showChangeImageWarning = false;
    (this.$refs["file-input"] as any).click();
  }
}
</script>

<style lang="less" scoped>
.input-buttons-container {
  display: block;
  min-width: 100px;
  justify-content: space-evenly;
  margin: auto auto;
}
.button {
  width: 75px;
  height: 35px;
}
</style>

<style lang="less">
.preview {
  max-width: 300px;
  max-height: 169px;
  border-radius: 10px;
}
#image-buttons {
  margin: 10px;
  > * {
    margin: 10px;
  }
}

@media screen and (max-width: 860px) {
  .preview {
    max-width: 100%;
    max-height: 100%;
  }
}
</style>

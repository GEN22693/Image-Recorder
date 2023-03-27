<template>
  <section :style="styling" v-if="isValid" class="photo-capture">
    <div class="imgrec-content">
      <div class="imgrec-video">
        <video
          v-show="showVideo"
          ref="player"
          class="camera"
          autoplay
          playsinline
        />
      </div>
      <div class="imgrec-preview">
        <canvas
          v-show="!showVideo"
          id="canvas"
          class="preview"
          ref="canvas"
          @mousedown="onMouseDown"
          @mouseup="onMouseUp"
          @mouseleave="onMouseLeave"
          @mousemove="onMouseMove"
        />
      </div>
      <div v-if="!hideBtns" class="imgrec-capture">
        <button
          :class="'imgrec-btn' + buttonsClasses"
          @click.prevent="capture"
          v-if="showVideo"
        >
          {{ captureBtnContent }}
        </button>
        <div class="controls" v-else>
          <button
            :class="'imgrec-btn ' + buttonsClasses"
            @click.prevent="cancel"
          >
            {{ cancelBtnContent }}
          </button>
          <button :class="'imgrec-btn' + buttonsClasses" @click.prevent="done">
            {{ doneBtnContent }}
          </button>
          <button
            :class="'imgrec-btn ' + buttonsClasses"
            @click.prevent="drawRectangle"
            id="drawRectBtn"
          >
            {{ drawRectBtnContent }}
          </button>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
let isActive = false;
let isRectangleDraw = false;
export default {
  name: "PhotoCapture",
  props: {
    hideBtns: {
      type: Boolean,
      isRequired: false,
      default: false,
    },
    styling: {
      type: Object,
      isRequired: false,
    },
    value: {
      default: null,
    },
    hideButtons: {
      type: Boolean,
      default: false,
    },
    buttonsClasses: {
      type: String,
      default: "",
    },
    captureBtnContent: {
      default: "Capture",
    },
    cancelBtnContent: {
      default: "Cancel",
    },
    doneBtnContent: {
      default: "Save",
    },
    drawRectangleBtnContent: {
      default: "Rechteck",
    },
  },
  data() {
    return {
      showVideo: true,
      picture: null,
      isValid: true,
      canvasImage: null,
    };
  },
  mounted() {
    this.videoPlayer = this.$refs.player;
    this.canvasElement = this.$refs.canvas;
    this.streamUserMediaVideo();
  },
  methods: {
    streamUserMediaVideo() {
      if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
        return;
      }
      navigator.mediaDevices
        .getUserMedia({ video: true })
        .then((stream) => (this.videoPlayer.srcObject = stream))
        .catch(() => {
          this.isValid = false;
        });
    },
    capture() {
      this.showVideo = false;
      this.canvasElement.width = this.videoPlayer.videoWidth;
      this.canvasElement.height = this.videoPlayer.videoHeight;
      const context = this.canvasElement.getContext("2d");
      context.drawImage(this.$refs.player, 0, 0);
      this.stopVideoStream();
      this.picture = this.canvasElement.toDataURL();
      // Set the captured image as the source of an image element
      const img = new Image();
      img.onload = function () {
        context.drawImage(img, 0, 0);
      };
      img.src = this.picture;
    },
    stopVideoStream() {
      if (!(this.$refs.player && this.$refs.player.srcObject)) return;
      this.$refs.player.srcObject.getVideoTracks().forEach((track) => {
        track.stop();
      });
    },
    drawRectangle() {
      isRectangleDraw = false;
      isActive = false;
      let startX, startY, endX, endY;
      const canvas = this.$refs.canvas;
      const ctx = canvas.getContext("2d");
      // Load the captured image
      const img = new Image();
      img.onload = function () {
        ctx.drawImage(img, 0, 0);
      };
      img.src = this.picture;
      // Set up the drawing settings
      ctx.strokeStyle = "red";
      ctx.lineWidth = 4;
      // Set up the event listeners
      canvas.addEventListener("mousedown", startRectangleDraw);
      canvas.addEventListener("mouseup", stopRectangleDraw);
      canvas.addEventListener("mouseleave", stopRectangleDraw);
      canvas.addEventListener("mousemove", drawRectangle);

      function startRectangleDraw(e) {
        isActive = true;
        startX = e.clientX - canvas.offsetLeft;
        startY = e.clientY - canvas.offsetTop;
        isRectangleDraw = true;
      }
      function drawRectangle(e) {
        if (!isActive) return;
        if (isRectangleDraw) {
          // Clear the canvas
          ctx.clearRect(0, 0, canvas.width, canvas.height);
          // Draw the captured image
          ctx.drawImage(img, 0, 0);
          // Draw the rectangle
          ctx.beginPath();
          ctx.rect(
            startX,
            startY,
            e.pageX - canvas.offsetLeft - startX,
            e.pageY - canvas.offsetTop - startY
          );
          ctx.stroke();
        }
      }
      function stopRectangleDraw() {
        isActive = false;
        isRectangleDraw = false;
        canvas.removeEventListener("mousedown", startRectangleDraw);
        canvas.removeEventListener("mouseup", stopRectangleDraw);
        canvas.removeEventListener("mouseleave", stopRectangleDraw);
        canvas.removeEventListener("mousemove", drawRectangle);
      }
    },
    done() {
      this.$emit("input", this.picture);
      this.showVideo = true;
      this.streamUserMediaVideo();
      isActive = false;
      isRectangleDraw = false;
    },
    cancel() {
      this.showVideo = true;
      this.streamUserMediaVideo();
      isRectangleDraw = false;
      isActive = false;
    },
  },
};
</script>
<style>
.imgrec-content {
  margin-left: auto;
  margin-right: auto;
  height: 100vh;
  width: 100vw;
  background-color: transparent;
}

.imgrec-video {
  margin-left: auto;
  margin-right: auto;
  width: 80%;
}

.camera {
  width: 100%;
  height: 600px;
  object-fit: cover;
  filter: contrast(1.5);
  transition: filter 0.2s ease-in;
}

.imgrec-capture {
  margin-left: auto;
  margin-right: auto;
  width: 80%;
  height: 50px;
}

.imgrec-btn {
  margin-top: 15px;
}

.imgrec-preview {
  margin-left: auto;
  margin-right: auto;
  width: 80%;
}



@media screen and (max-width: 1000px) {
  .imgrec-content {
    height: 100vh;
    width: 100%;
    background-color: green;
  }

  .imgrec-video {
    width: 100%;
    height: 100vh;
    position: absolute;
    background-color: transparent;
  }

  .camera {
    width: 100%;
    height: 99vh;
    object-fit: cover;
    filter: contrast(1.5);
    transition: filter 0.2s ease-in;
  }

  .imgrec-capture {
    position: absolute;
    width: 100%;
    height: 80px;
    background-color: blue;
    bottom: 0px;
  }

  .imgrec-preview {
    position: absolute;
    width: 100%;
    height: 100vh;
  }
}
</style>

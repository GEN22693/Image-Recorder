<template>
  <div>
    <video ref="video" v-show="showVideo" playsinline="true"></video>
    <div class="div-canvas" v-show="!showVideo">
      <canvas
        ref="canvas"
        v-show="!showVideo"
        @mousedown="onMouseDown"
        @mouseup="onMouseUp"
        @mouseleave="onMouseLeave"
        @mousemove="onMouseMove"
        @touchstart="onTouchStart"
        @touchend="onTouchEnd"
        @touchmove="onTouchMove"
      >
      </canvas>
    </div>
    <div class="button-container-imgrec">
      <Button
        icon="pi pi-camera"
        class="p-button-success p-button-rounded button-resize-imgrec"
        v-if="showButton"
        @click.prevent="capture()"
      >
      </Button>
      <Button
        icon="pi pi-check-circle"
        class="p-button-success p-button-rounded button-resize-imgrec"
        v-if="!showButton"
        @click.prevent="done()"
      >
      </Button>
      <Button
        v-if="!showButton"
        icon="pi pi-pencil"
        class="p-button-warning p-button-rounded button-abstand-imgrec"
        @click.prevent="drawRectangle()"
      />
      <Button
        v-if="!showButton"
        icon="pi pi-times"
        class="p-button-danger p-button-rounded button-abstand-imgrec2"
        @click.prevent="cancel()"
      />
      <Button
      v-if="showVideo"
      label="Start Camera"
      class="p-button-success p-button-rounded"
      @click.prevent="startVideoPlayback"
    />

    </div>
  </div>
</template>

<script>
import Button from "primevue/button";

let isActive = false;
let isRectangleDraw = false;

export default {
  data() {
    return {
      showVideo: true,
      showButton: true,
      capturedImage: null,
      touchIdentifier: null,
    };
  },
  mounted() {
    this.initCamera();
  },
  methods: {
    initCamera() {
      const constraints = {
        video: true,
      };

      navigator.mediaDevices
        .getUserMedia(constraints)
        .then((stream) => {
          this.$refs.video.srcObject = stream;
          this.$refs.video.play();
        })
        .catch((error) => {
          console.error("Kamerazugriff fehlgeschlagen: ", error);
        });
    },
    capture() {
      this.showVideo = false;
      this.showButton = false;
      const video = this.$refs.video;
      const canvas = this.$refs.canvas;
      const context = canvas.getContext("2d");
      canvas.width = video.videoWidth;
      canvas.height = video.videoHeight;
      context.drawImage(video, 0, 0, canvas.width, canvas.height);
      this.capturedImage = canvas.toDataURL();
      const img = new Image();
      img.onload = function () {
        context.drawImage(img, 0, 0);
      };
      img.src = this.capturedImage;
    },
    drawRectangle() {
      isRectangleDraw = false;
      isActive = false;
      let startX, startY;
      const canvas = this.$refs.canvas;
      const ctx = canvas.getContext("2d");
      const img = new Image();
      img.onload = function () {
        ctx.drawImage(img, 0, 0);
      };
      img.src = this.capturedImage;
      ctx.strokeStyle = "red";
      ctx.lineWidth = 4;

      canvas.addEventListener("mousedown", startRectangleDraw);
      canvas.addEventListener("mouseup", stopRectangleDraw);
      canvas.addEventListener("mouseleave", stopRectangleDraw);
      canvas.addEventListener("mousemove", drawRectangle);

      canvas.addEventListener("touchstart", startRectangleDraw);
      canvas.addEventListener("touchend", stopRectangleDraw);
      canvas.addEventListener("touchmove", drawRectangle);

      function startRectangleDraw(e) {
        isActive = true;
        isRectangleDraw = true;
        if (e.touches) {
          const touch = e.touches[0];
          startX = touch.clientX - canvas.offsetLeft;
          startY = touch.clientY - canvas.offsetTop;
          this.touchIdentifier = touch.identifier;
        } else {
          startX = e.clientX - canvas.offsetLeft;
          startY = e.clientY - canvas.offsetTop;
        }
      }

      function drawRectangle(e) {
        if (!isActive) return;
        if (isRectangleDraw) {
          ctx.clearRect(0, 0, canvas.width, canvas.height);
          ctx.drawImage(img, 0, 0);
          ctx.beginPath();
          if (e.touches) {
            const touch = Array.from(e.touches).find(
              (t) => t.identifier === this.touchIdentifier
            );
            if (touch) {
              ctx.rect(
                startX,
                startY,
                touch.pageX - canvas.offsetLeft - startX,
                touch.pageY - canvas.offsetTop - startY
              );
            }
          } else {
            ctx.rect(
              startX,
              startY,
              e.pageX - canvas.offsetLeft - startX,
              e.pageY - canvas.offsetTop - startY
            );
          }
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

        canvas.removeEventListener("touchstart", startRectangleDraw);
        canvas.removeEventListener("touchend", stopRectangleDraw);
        canvas.removeEventListener("touchmove", drawRectangle);
      }
    },
    done() {
      const canvas = this.$refs.canvas;
      const context = canvas.getContext("2d");
      this.capturedImage = canvas.toDataURL();
      const img = new Image();
      img.onload = function () {
        context.drawImage(img, 0, 0);
      };
      img.src = this.capturedImage;
      this.$emit("recorded", this.capturedImage);
      this.showVideo = true;
      this.showButton = true;
    },
    cancel() {
      this.showVideo = true;
      this.showButton = true;
    },
    startVideoPlayback() {
    const video = this.$refs.video;
    video.play()
      .catch((error) => {
        console.error("Failed to start video playback: ", error);
      });
  }
  },
  components: { Button, Button },
};
</script>

<style>
video {
  margin-top: 25px;
  width: 100%;
  height: 80vh;
  filter: contrast(1);
}

.div-canvas {
  margin-top: 25px;
  margin-left: auto;
  margin-right: auto;
  width: fit-content;
}

.button-container-imgrec {
  margin-left: auto;
  margin-right: auto;
  margin-top: 15px;
  width: fit-content;
  height: auto;
  background-color: transparent;
}

.button-abstand-imgrec {
  left: 15px;
}

.button-abstand-imgrec2 {
  left: 30px;
}

.button-resize-imgrec {
  margin-left: -15px;
}

@media screen and (max-width: 1000px) {
  video {
    width: 100%;
    height: 99vh;
    object-fit: cover;
    filter: contrast(1);
    transition: filter 0.2s ease-in;
  }
  .div-canvas {
    margin-top: 25px;
    margin-left: auto;
    margin-right: auto;
    background-color: green;
  }
  canvas {
    width: 100%;
    height: auto;
    display: block;
  }
}
</style>

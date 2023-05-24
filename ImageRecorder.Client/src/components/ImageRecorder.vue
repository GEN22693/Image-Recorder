<template>
  <div>
    <video ref="video" v-show="showVideo"></video>
    <div class="div-canvas" v-show="!showVideo">
      <canvas
        ref="canvas"
        v-show="!showVideo"
        @mousedown="onMouseDown"
        @mouseup="onMouseUp"
        @mouseleave="onMouseLeave"
        @mousemove="onMouseMove"
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
    </div>
  </div>
</template>

<script>
import Button from 'primevue/button';


let isActive = false;
let isRectangleDraw = false;
export default {
    /*props: {
        hideBtns: {
          type: Boolean,
          isRequired: false,
          default: false,
        },
     }, */
    data() {
        return {
            showVideo: true,
            showButton: true,
            capturedImage: null,
        };
    },
    mounted() {
        this.initCamera();
    },
    methods: {
        initCamera() {
            navigator.mediaDevices
                .getUserMedia({ video: true })
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
            // Load the captured image
            const img = new Image();
            img.onload = function () {
                ctx.drawImage(img, 0, 0);
            };
            img.src = this.capturedImage;
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
                if (!isActive)
                    return;
                if (isRectangleDraw) {
                    // Clear the canvas
                    ctx.clearRect(0, 0, canvas.width, canvas.height);
                    // Draw the captured image
                    ctx.drawImage(img, 0, 0);
                    // Draw the rectangle
                    ctx.beginPath();
                    ctx.rect(startX, startY, e.pageX - canvas.offsetLeft - startX, e.pageY - canvas.offsetTop - startY);
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
        },
        cancel() {
            this.showVideo = true;
            this.showButton = true;
        },
    },
    components: { Button, Button }
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

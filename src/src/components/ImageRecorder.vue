<template>
  <section :style="styling" v-if="isValid" class="photo-capture">
    <video
      v-show="showVideo"
      ref="player"
      class="camera"
      autoplay
      playsinline
    />
    <canvas
      v-show="!showVideo"
      id="canvas"
      class="preview"
      ref="canvas"
      width="800"
      height="600"
    />
    <div v-if="!hideBtns" class="center photo-capture-actions">
      <button
        :class="'btn flex-center ' + buttonsClasses"
        @click.prevent="capture"
        v-if="showVideo"
      >
        {{ captureBtnContent }}
      </button>
      <div class="controls" v-else>
        <button :class="'btn ' + buttonsClasses" @click.prevent="cancel">
          {{ cancelBtnContent }}
        </button>
        <button :class="'btn ' + buttonsClasses" @click.prevent="done">
          {{ doneBtnContent }}
        </button>
        <button :class="'btn ' + buttonsClasses" @click.prevent="drawPen">
          {{ drawPenBtnContent }}
        </button>

        <button
          :class="'btn ' + buttonsClasses"
          @click.prevent="drawCircle"
          id="drawCircleBtn"
        >
          {{ drawCircleBtnContent }}
        </button>

        <button :class="'btn ' + buttonsClasses" @click.prevent="drawRectangle">
          {{ drawRectangleBtnContent }}
        </button>
      </div>
    </div>
  </section>
</template>

<script>
let markierenButtonPressed = false;
let isActive = false;
let isCircleDraw = false;
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
    drawPenBtnContent: {
      default: "Markieren",
    },

    drawCircleBtnContent: {
      default: "Kreis",
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
      var context = this.canvasElement.getContext("2d");
      context.drawImage(this.$refs.player, 0, 0);
      this.stopVideoStream();
      this.picture = this.$refs.canvas.toDataURL();
    },
    stopVideoStream() {
      if (!(this.$refs.player && this.$refs.player.srcObject)) return;
      this.$refs.player.srcObject.getVideoTracks().forEach((track) => {
        track.stop();
      });
    },
    drawPen() {
      let canvasImage;
      isCircleDraw = false;
      markierenButtonPressed = true;
      const canvas = this.$refs.canvas;
      const ctx = canvas.getContext("2d");
      ctx.strokeStyle = "red";
      ctx.lineWidth = 4;
      let startX, startY;

    

      function startDraw(e) {
        isActive = true;
        startX = e.clientX - canvas.offsetLeft;
        startY = e.clientY - canvas.offsetTop;
      }

      function draw(e) {
        if (!markierenButtonPressed || !isActive) return;

        if (canvasImage) {
          ctx.drawImage(canvasImage, 0, 0);
        }
        ctx.beginPath();
        ctx.moveTo(startX, startY);
        ctx.lineTo(e.clientX - canvas.offsetLeft, e.clientY - canvas.offsetTop);
        ctx.stroke();
        startX = e.clientX - canvas.offsetLeft;
        startY = e.clientY - canvas.offsetTop;
      }

      canvas.addEventListener("mousedown", startDraw);
      canvas.addEventListener("mousemove", draw);
      canvas.addEventListener("mouseup", stopDraw);
      canvas.addEventListener("mouseleave", stopDraw);
      canvas.addEventListener("touchmove", draw);
      canvas.addEventListener("touchend", stopDraw);

      function stopDraw() {
        isActive = false;
        markierenButtonPressed = false;
        canvasImage = new Image();
        canvasImage.src = canvas.toDataURL();
      }
    },

    drawRectangle() {
      let canvasImage;
      let isRectangleDraw = false;
      let isActive = false;
      let startX, startY, endX, endY;
      const canvas = this.$refs.canvas;
      const ctx = canvas.getContext("2d");
      ctx.strokeStyle = "red";
      ctx.lineWidth = 4;

      canvas.addEventListener("mousedown", startRectangleDraw);
      canvas.addEventListener("mouseup", stopRectangleDraw);
      canvas.addEventListener("mouseleave", stopRectangleDraw);
      canvas.addEventListener("mousemove", drawRectangle);

      function startRectangleDraw(e) {
        isActive = true;
        startX = e.clientX - canvas.offsetLeft;
        startY = e.clientY - canvas.offsetTop;
      }

      function drawRectangle(e) {
        if (!isRectangleDraw || !isActive) return;

        endX = e.clientX - canvas.offsetLeft;
        endY = e.clientY - canvas.offsetTop;

        ctx.clearRect(0, 0, canvas.width, canvas.height);
        if (canvasImage) {
          ctx.drawImage(canvasImage, 0, 0);
        }

        ctx.beginPath();
        ctx.rect(startX, startY, endX - startX, endY - startY);
        ctx.stroke();
      }

      function stopRectangleDraw() {
        isActive = false;
        isRectangleDraw = true;
        canvasImage = new Image();
        canvasImage.src = canvas.toDataURL();
      }
    },
    drawCircle() {
      let canvasImage;
      let isCircleDraw = false;
      let isActive = false;
      let startX, startY, endX, endY;
      const canvas = this.$refs.canvas;
      const ctx = canvas.getContext("2d");
      ctx.strokeStyle = "red";
      ctx.lineWidth = 4;

      canvas.addEventListener("mousedown", startCircleDraw);
      canvas.addEventListener("mouseup", stopCircleDraw);
      canvas.addEventListener("mouseleave", stopCircleDraw);
      canvas.addEventListener("mousemove", drawCircle);

      function getRadius() {
        return Math.sqrt(
          Math.pow(endX - startX, 2) + Math.pow(endY - startY, 2)
        );
      }

      function startCircleDraw(e) {
        isActive = true;
        startX = e.clientX - canvas.offsetLeft;
        startY = e.clientY - canvas.offsetTop;
      }

      function drawCircle(e) {
        if (!isCircleDraw || !isActive) return;

        endX = e.clientX - canvas.offsetLeft;
        endY = e.clientY - canvas.offsetTop;

        ctx.clearRect(0, 0, canvas.width, canvas.height);
        if (canvasImage) {
          ctx.drawImage(canvasImage, 0, 0);
        }

        ctx.beginPath();
        ctx.arc(startX, startY, getRadius(), 0, 2 * Math.PI);
        ctx.stroke();
      }

      function stopCircleDraw() {
        isActive = false;
        isCircleDraw = true;
        canvasImage = new Image();
        canvasImage.src = canvas.toDataURL();
      }
    },
    done() {
      this.$emit("input", this.picture);
      this.showVideo = true;
      this.streamUserMediaVideo();
      markierenButtonPressed = false;
      isCircleDraw = false;
    },
    cancel() {
      this.showVideo = true;
      this.streamUserMediaVideo();
      markierenButtonPressed = false;
      isCircleDraw = false;
    },
  },
};

/* window.addEventListener("load", () => {
  const canvas = document.querySelector("#canvas")
  const ctx = canvas.getContext("2D")

  let painting = false;

  function startPosition() {
    painting = true
  }

  function finishedPosition() {
    painting = false
  }

  function draw(e) {
    if(!painting) return;
    ctx.lineWidth = 10;
    ctx.lineCap = "round";

    ctx.lineTo(e.clientX, e.clientY);
    ctx.stroke();
  }

  canvas.addEventListener("mousedown", startPosition);
  canvas.addEventListener("mouseup", finishedPosition);
  canvas.addEventListener("mousemove", draw);

}); */
</script>
<style>
.camera {
  width: 100%;
  height: 100%;
  object-fit: cover;
  filter: contrast(1.5);
  transition: filter 0.2s ease-in;
}
</style>

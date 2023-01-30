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
      </div>
    </div>
  </section>
</template>


<script>
let markierenButtonPressed = false;
let isActive = false;

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
    drawPen(){
      markierenButtonPressed = true;
      const canvas = this.$refs.canvas;
      const ctx = canvas.getContext("2d");
      ctx.strokeStyle = "red";
      ctx.lineWidth = 4;
      let startX, startY;

      canvas.addEventListener("mousedown", startDraw);
      canvas.addEventListener("mousemove", draw);
      canvas.addEventListener("mouseup", stopDraw);
      canvas.addEventListener("mouseleave", stopDraw);
      canvas.addEventListener("touchmove", draw);
      canvas.addEventListener("touchend", stopDraw);

      function startDraw(e) {
        isActive = true;
        startX = e.clientX - canvas.offsetLeft;
        startY = e.clientY - canvas.offsetTop;
      }

      function draw(e) {
        if (!markierenButtonPressed || !isActive) return;
        ctx.beginPath();
        ctx.moveTo(startX, startY);
        ctx.lineTo(e.clientX - canvas.offsetLeft, e.clientY - canvas.offsetTop);
        ctx.stroke();
        startX = e.clientX - canvas.offsetLeft;
        startY = e.clientY - canvas.offsetTop;
      }

      function stopDraw() {
        isActive = false;
      }
    },
    done() {
      this.$emit("input", this.picture);
      this.showVideo = true;
      this.streamUserMediaVideo();
      markierenButtonPressed = false;
    },
    cancel() {
      this.showVideo = true;
      this.streamUserMediaVideo();
      markierenButtonPressed = false;
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

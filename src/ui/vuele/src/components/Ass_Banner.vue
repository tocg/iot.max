<template>
    <div class="lunbo-box">
    <el-carousel indicator-position="none">
      <el-carousel-item
        class="lunbo-box-item"
        v-for="(item,index) in abc_img"
        :key="index"
      >
        <img :src="item.src" class="lunbotu" alt="轮播图">
      </el-carousel-item>
    </el-carousel>
  </div>
</template>



<script>
export default {
  name: "Banner",

  data() {
    return {
      // 图片地址数组
abc_img: [
    { src: require("@/assets/banner/banner1.jpg") },
    { src: require("@/assets/banner/banner2.jpg") }
  ],
      img_list: [require('@/assets/banner/banner1.jpg'), require('@/assets/banner/banner2.jpg'), ], // 图片父容器高度

      bannerHeight: 1000, // 浏览器宽度

      screenWidth: 0,
    };
  },

  methods: {
    setSize: function () {
      // 通过浏览器宽度(图片宽度)计算高度
      this.bannerHeight = (400 / 1920) * this.screenWidth;
    },
  },

  mounted() {
    // 首次加载时,需要调用一次

    this.screenWidth = window.innerWidth;

    this.setSize(); // 窗口大小发生改变时,调用一次

    window.onresize = () => {
      this.screenWidth = window.innerWidth;

      this.setSize();
    };
  },
};
</script>



<style scoped>
.el-carousel__item h3  {
   color: #475669;
   font-size: 14px;
   opacity: 0.75;
   line-height: 300px;
   margin: 0;
}

.el-carousel__item:nth-child(2n)  {
    background-color: #99a9bf;
}

.el-carousel__item:nth-child(2n  +  1)  {
    background-color: #d3dce6;
}

img  {
    width: 100%;
    height: inherit;
}
</style>
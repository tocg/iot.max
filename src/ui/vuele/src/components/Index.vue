<template>
  <div class="hello">
      <HeaderNav />
  <div class="banner-box">
      <Banner />
    </div>
    <h1>{{ msg }}</h1>
  </div>
</template>


<script>

import HeaderNav from '@/components/Ass_HeaderNav'
import Banner from '@/components/Ass_Banner'

export default {
  name: "Index",
  data() {
    return {
      msg: "Index",
    };
  },
  components:{
      HeaderNav,Banner
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.banner-box{
    margin: 20px 0;
}
</style>

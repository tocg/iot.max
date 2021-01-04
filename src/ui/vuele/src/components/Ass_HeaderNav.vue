<template>
  <el-header>
    <div class="container">
      <el-row :gutter="20">
        <el-col :span="4">
          <a>
            <img
              src="@/assets/logo.png"
              style="height: 50px; margin-top: 5px"
            />
          </a>
        </el-col>
        <el-col :span="18">
          <el-menu
            router
            :default-active="$route.path"
            class="el-menu-demo"
            mode="horizontal"
            @select="handleSelect"
            v-for="(item, i) in items"
            :key="i"
          >
            <el-menu-item :index="item.navurl">{{ item.navname }}</el-menu-item>
          </el-menu>
        </el-col>
        <el-col :span="2" class="n-login">
          <div v-if="loginShow">
            <el-button type="text" @click="open">登录</el-button>
          </div>
          <div v-else>
            <label>
              <router-link :to="{ name: 'own', params: { id: loginInfo } }">{{
                loginInfo
              }}</router-link>
            </label>
            <label>退出</label>
          </div>
        </el-col>
      </el-row>
    </div>

    <div>
      <AssLogin :dialogVisiblea="a" @listenToChildEvent="showUser"></AssLogin>
    </div>
  </el-header>
</template>

<script>
import AssLogin from "@/components/Ass_Login";

export default {
  name: "AssHeaderNav",
  data() {
    return {
      activeIndex: "1",
      activeIndex2: "1",
      dialogTableVisible: false,
      testurl: "tf",
      a: 1,
      loginShow: true,
      loginInfo: "",
      items: [
        { navname: "后台管理", navurl: "/admin", navsort: "", navstate: "0" },
        { navname: "技能详解", navurl: "5", navsort: "", navstate: "0" },
        { navname: "面试录音", navurl: "4", navsort: "", navstate: "0" },
        { navname: "面试技巧", navurl: "3", navsort: "", navstate: "0" },
        { navname: "面试问题", navurl: "2", navsort: "", navstate: "0" },
        { navname: "首页", navurl: "/", navsort: "", navstate: "0" },
      ],
    };
  },
  components: {
    AssLogin,
  },
  created() {
    this.onLoad();
  },
  methods: {
    onLoad() {
      var s = sessionStorage.getItem("user");
      if (s != null) {
        this.loginShow = false;
        this.loginInfo = s;
      }
    },
    handleSelect(key, keyPath) {
      console.log(key, keyPath);
    },
    open() {
      // this.$alert('<C_Login></C_Login>aa<h1>bbb</h1>','登录', {
      //   dangerouslyUseHTMLString: true
      // });
      this.dialogTableVisible = true;
      console.log(this.dialogTableVisible);
      this.a = this.a + 1;
    },
    showUser(data) {
      console.log(data);
      this.loginShow = false;
      this.loginInfo = data;
    },
  },
};
</script>

<style scoped>
.el-header {
  display: flex !important;
  line-height: 60px;
}
.el-menu {
  float: right;
}
.el-menu.el-menu--horizontal {
  border-bottom: none;
}

.n-login {
  text-align: right;
}
</style>
<template>
  <el-dialog :visible.sync="childrenSay" class="opusuploadDialog" custom-class="customWidth">
    <el-form ref="form" :model="form" :rules="rules" label-width="80px" @submit.native.prevent>
      <el-form-item label="账号">
          <el-input v-model="form.username" placeholder="账号"></el-input>
      </el-form-item>
      <el-form-item label="密码">
          <el-input v-model="form.password" type="password" placeholder="密码"></el-input>
      </el-form-item>
      
      <el-form-item>
        <el-checkbox v-model="form.checked" disabled>记住密码</el-checkbox>
      </el-form-item>
      <el-form-item>
        说明：Demo v1.0关闭浏览器或页面需要重新登录
      </el-form-item>
      <el-form-item>
          <el-button type="primary" @click="onSubmit">立即登录</el-button>
          <el-button @click="onClose">取消</el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script>
 export default {
   props:["dialogVisiblea"],
    data(){
        return {
            form: {
                username: 'admin',
                password: '',
                checked:false
            },
            rules: {
                username: [{required: true, message: '请输入你的账号', trigger: 'blur'}],
                password: [{required: true, message: '请输入你的密码', trigger: 'blur'}]
            },
            childrenSay: false
        }
    },
    watch: {
      dialogVisiblea(){
          this.childrenSay = true;       
      }
    },
    methods: {
      onClose(){
        
            this.childrenSay  = false
            this.form.username=''
            this.form.checked=false
            this.form.password=''
      },
      onSubmit(event){
          this.$refs.form.validate((valid) => {
              if(valid){
                console.log(this.form.username + ',' + this.form.password)
                  if(this.form.username === 'admin' && 
                      this.form.password === '123456'){
                          this.childrenSay = false;
                          sessionStorage.setItem('user', this.form.username);
                          //this.$router.push({path: '/'});
                          

                          this.$emit('listenToChildEvent', this.form.username)  
                          //this.OnReset()
                  }else{
                      this.childrenSay = true;
                      this.$alert('账号及密码不正确!', 'info', {
                          confirmButtonText: 'ok'
                      })
                  }
              }else{
                  console.log('error submit!');
                  return false;
              }
          })
      }
    }
};
</script>

<style scoped>
.customWidth{
  width:40%;
}
</style>
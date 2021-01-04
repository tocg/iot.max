<template>
  <div id="app">
    <router-view/>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style>
html,body{
  margin: 0;
  padding: 0;
}
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
}


body, div, dl, dt, dd, ul, ol, li, h1, h2, h3, h4, h5, h6, pre, form, fieldset, input, p, blockquote, th, td { margin:0; padding:0; }
body { font-size:14px; color:#2c3e50; font-family:Verdana, Microsoft YaHei, Simsun;  line-height:24px; }
fieldset, img { border:0; }
ol, ul { list-style:none; }
h1, h2, h3, h4, h5, h6{ font-size:100%; }
em { font-style:normal; }
input, button, select, textarea { outline:none; }
textarea { resize:none; }
p{ text-align:justify; text-justify:distribute;}

a { color: #2c3e50; text-decoration:none; }


.container{
    width: 1340px;
    margin: 0 auto;
}

.el-main{
    background: #f7f7f7;
}


.card-title{
    padding-bottom: 10px;
    margin-bottom: 20px;
    border-bottom: 1px solid #f7f7f7;
}

.card-title > span{
    border-bottom: 1px solid red;
    padding: 0 15px 13px 15px;
}

footer{
    line-height: 60px;
    text-align: center;
}
</style>

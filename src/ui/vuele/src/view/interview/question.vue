<template>
    <div class="container" >
        <HeaderNav />
        <div class="contentbox">
            <el-tag>Asp.Net</el-tag>
            <el-tag type="success">SQL</el-tag>
            <el-tag type="info">NoSQL</el-tag>
            <el-tag type="warning">业务逻辑</el-tag>
            <el-tag type="danger">数据算法</el-tag>
        </div>
        <div>
            <el-collapse v-model="activeName" accordion>
                <div v-for="(item,i)  in list" :key="i" class="item-box">
                    <el-collapse-item :title="item.title" :name="i"  >
                        <div v-html="item.answer"></div>
                    </el-collapse-item>
                </div>
            </el-collapse>
            <div class="block">
            <el-pagination
                layout="prev, pager, next" 
                @current-change="handleCurrentChange"
                :page-size="limit"
                :total="rowTotal">
            </el-pagination>
            </div>
        </div>
    </div>
</template>

<script>

import HeaderNav from '@/components/Ass_HeaderNav'

export default {
    name:'question',
    components:{
        HeaderNav
    }
    ,data(){
        return{
            list:[],
            limit:10,
            rowTotal:0,
            activeName: '1'
        }
    }
    ,created:function(){
        
            var that =this
            that.init('1')
    }
    ,methods:{
        init(page){
            var that =this
            this.axios.get('/inter/question/query/?page='+page+'&limit='+that.limit).then(res=>{
                console.log(res.data)
                this.list = res.data.data
                this.rowTotal = res.data.count
            })
        }
        ,handleCurrentChange(val) {
            console.log(`当前页: ${val}`);
            var that =this
            that.init(val)
        }
    }
}
</script>

<style scoped>
.container{
    width: auto;
}
.el-tag{
    padding:0 30px;
    margin-left: 20px;
    margin-right: 20px;
}
.el-collapse{
    margin-left: 20px;
}
.contentbox{
     margin-top: 20px;
     margin-bottom: 20px;
}

.item-box{
    border-bottom:1px solid #dcdcdc;
}
.el-pagination{
    text-align: center;
    margin-top: 30px;
}
</style>
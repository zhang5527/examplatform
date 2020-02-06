var strh,strm,strs;
var data=new Array();
var hour = 1,
minutes = 30,
seconds = 0;
var strh,strm,strs;
var initata;
$(document).ready(function(){
    initata=localStorage.getItem("data");
    if(seconds<10) strs="0"+seconds
    if(minutes<10) strm="0"+minutes
    if(hour<10) strh="0"+hour;
    $('#timer_s').html(strs);
    $('#timer_h').html(strh);
    $('#timer_m').html(strm);    
    var int=self.setInterval("clock()",1000);
    // self.setInterval("getData()",3000);
})

function clock()
{
 if(seconds>=1){
        seconds--;
        strs=seconds;
        if(seconds<10) strs="0"+seconds
        $('#timer_s').html(strs);        
 }     
 else if(minutes>=1){
    seconds=60;
    minutes--;
    strm=minutes;
    if(minutes<10) strm="0"+minutes
    $('#timer_m').html(strm);
 }
 else if(hour>=1){
     minutes=60;
     hour--;
     strh=hour;
     if(hour<10) strh="0"+hour;
     $('#timer_h').html(strh);
 }
 else {
      alert('结束');
 }

}
function getData(){
  var params = $('input').serializeArray();
  console.log('123')
  console.log(params);
  localStorage.setItem("data",JSON.stringify(params));
  console.log(localStorage.getItem("data"))
}
function test(){
    initata=$.parseJSON(localStorage.getItem("data"));
    for(name in initata){
        var v=initata[name];
        console.log('我在运行',initata[name]);        
        var name=v.name;
        var value=v.value;
        $(":radio[name="+name+"]").each(function(){
            var that = $(this);
            if(value == that.val()){
                console.log(value)
                console.log('123')
                that.prop("checked",true);        
            }
        });
    }
    layui.use('form', function(){
        var form = layui.form;
        form.render('radio')
        //监听提交
      });
    // $("#no1").removeAttr("checked");
    // $("#inc0").attr("checked","checked");     
}
// function init(){


// }
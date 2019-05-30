$(document).ready(function(){
   
   var pos = $(".loginbox .head a").eq(1).position();
   var width = $(".loginbox .head a").eq(1).width();
    
   $(".loginbox .bottomline").css({'left':pos.left+'px','width':width+'px'});
   $(".loginbox .ct .item").eq(1).show();

  
   $(".loginbox .head a").click(function(){
   	   var index = $(this).index();
   	  
   	   $(".loginbox .ct .item").hide();
   
   	   $(".loginbox .ct .item").eq(index).show();
   	
   	   pos = $(this).position();
       width = $(this).width();       
       $(".loginbox .bottomline").animate({'left':pos.left+'px','width':width+'px'},300);
       
       $(this).addClass('sel').siblings().removeClass("sel");
   });
 
   $(".delusername").click(function(){
   		$("input[name='username']").val('');
   });

 
   $(".outbox").click(function(){
   	   $(this).find(".checkbox").toggleClass('sel');
   });

   $(".submitBtn").click(function(){
   	   var username = $("input[name='username']").val().trim();
   	   var password = $("input[name='password']").val().trim();
      /* var patrn_0 = /^[1-9][0-9]{5,12}$/;//验证手机号或qq号*/
	   //var patrn_0 = /^1[34578]\d{9}$/;//验证手机号
     /*  var patrn_1= /^[1-9][0-9]{4,}@qq\.com$/;//验证邮箱  */ 
   

   	   if(username.length<1)
   	   {
   	   	   $(".logintip").html('请输入账号');
		   //alert("请输入手机号");
   	   	   return;
   	   }
   	   //if(!patrn_0.test(username)/* && !patrn_1.test(username)*/)
   	   //{
       //   $(".logintip").html('手机号格式不正确');
	   //    alert("手机号格式不正确");
   	   //	   return;
   	   //} 
        if(password.length<3)
        {
        	$(".logintip").html('请正确输入密码');
			 //alert("请输入正确密码");
   	   	   return;
        }

        $.ajax({
            //几个参数需要注意一下
            type: "POST",//方法类型
            dataType: "json",//预期服务器返回的数据类型
            url: "/UserInfo/ProcessLogin",//url
            data: { username: username, password: password },
            success: function (data) {
                if (data == "ok") {
                    window.location = "/Home/Index";
                }
                else {
                    alert(data);
                    
                }
            },

        });

		if(event.keyCode==13){
			
			return;
			
			}
		//else{
			
		//	alert("登录成功");
		//	window.location="index.html";<!--js页面跳转-->
			
		//	}

   });

   //清空
   $(".loginbox .inputbox input").focus(function(){
   	   $(".logintip").html('');
   });

});





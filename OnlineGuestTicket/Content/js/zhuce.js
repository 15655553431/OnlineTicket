  
$(".submit").click(function(){
    $(".t1").html('');
    $(".t2").html('');
    $(".t3").html('');
    $(".t4").html('');
    $(".t5").html('');
    $(".t6").html('');

    var login = $("input[name='Login']").val().trim();
    var pwd = $("input[name='pwd']").val().trim();  
    var pwd1 = $("input[name='pwd1']").val().trim();  
    var uname = $("input[name='uname']").val().trim();  
    var idcard = $("input[name='idcard']").val().trim();  
    var phone = $("input[name='phone']").val().trim();  
    var phonetest=/^1[34578]\d{9}$/;/*验证手机号*/


    if(login.length<1)
    {
        $(".t1").html('请输入登录名').css("color","#F00").css("font-size","16px");
        return;
    }
    if(pwd.length<6)
    {
        $(".t2").html('请正确输入密码').css("color","#F00").css("font-size","16px");
        return;
    }
    if(pwd!=pwd1)
    {
        $(".t3").html('请再次确认密码').css("color","#F00").css("font-size","16px");
        return;
    }
    if(uname.length<1)
    {
        $(".t4").html('请输入真实姓名').css("color","#F00").css("font-size","16px");
        return;
    }

    if(idcard.length!=18)
    {
        $(".t5").html('请输入正确身份证号').css("color","#F00").css("font-size","16px");
        return;
    }

    if(phone.length<1)
    {
        $(".t6").html('请输入手机号').css("color","#F00").css("font-size","16px");
        return;
    }
    if(!phonetest.test(phone))
    {
        $(".t6").html('手机号格式不正确').css("color","#F00").css("font-size","16px");
        return;
    } 
    else{


        document.getElementById("submit").value="请稍后... ...";
        //<!--window.location="";js页面跳转-->
			
        $(".validate").show(200)
        $(".user").hide(200);
        document.getElementById("tell").innerHTML=name;
        $(".user_y").css("box-shadow","0px 0px 5px 4px #fff");

    }
   	  
			
	 

   });
 
$(document).ready(function(e) {
	$(".enter").on("click",function(){
		$(".validate").hide(200);
		$(".user").show(200);
		document.getElementById("submit").value="下一步";
		 $(".user_y").css("box-shadow","none");
		});
});

//<!--完成-->
 function y_z_m(){
        var arr = ['A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7','8','9'];
        var str = '';
        for(var i = 0 ; i < 4 ; i ++ )
            str += ''+arr[Math.floor(Math.random() * arr.length)];
        return str;
    }
$("#wanc").click(function(){
	   
	var name = $("input[name='phone']").val().trim();
	    var yz= $("input[name='yz']").val().trim();
	    var yz1=/^['A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7','8','9']{4}$/;
			if(yz.length==0){
				$(".yzm").html('请输入验证码').css("font-size","16px").css("color","red");
				return;		
				}
				if(document.getElementById("y_z_m").innerHTML!=yz){
					$(".yzm").html('请输正确入验证码').css("font-size","16px").css("color","red");
					return;	
					
					}
			if(!yz1.test(yz)){
				
				$(".yzm").html('验证码格式不正确').css("font-size","16px").css("color","red");
				return;		
				
				}
			else {

			    //向后台发起请求
			    $.ajax({
			        //几个参数需要注意一下
			        type: "POST",//方法类型
			        dataType: "json",//预期服务器返回的数据类型
			        url: "/UserInfo/ZhuCe",//url
			        data: $('#zhuceform').serialize(),
			        success: function (data) {
			            if (data == "ok") {
			                $(".complete").show(200);
			                $(".validate").hide(200);
			                document.getElementById("mob").innerHTML=name;
			                $(".user_z").css("box-shadow","0px 0px 5px 4px #fff");		
			            }
			            else {
			                $(".completeerr").show(200);
			                $(".validate").hide(200);
			                $(".user_z").css("box-shadow", "0px 0px 5px 4px #fff");
			            }
			        },
			        error: function (data) {
			            $(".completeerr").show(200);
			            $(".validate").hide(200);
			            $(".user_z").css("box-shadow", "0px 0px 5px 4px #fff");
			        }
                    

			    });


					//$(".complete").show(200);
					//$(".validate").hide(200);
					//document.getElementById("mob").innerHTML=name;
					//$(".user_z").css("box-shadow","0px 0px 5px 4px #fff");		

			}
	
	});
$(".dj").click(function(){
	
	window.location="Login";
	});
//<!--完成 end-->
  
   //清空
   $("#name,#pw,#pw1,#yz").focus(function(){
   	   $(".t,.t1,.t2,.yzm").html('');
   });

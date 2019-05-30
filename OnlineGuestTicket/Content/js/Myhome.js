
//home界面的所有的 ajax数据交互操作

//*****************汽车时刻表*****************//
layui.use('laydate', function () {
    var laydate = layui.laydate;
    var now = new Date();
    var def = new Date(now.getTime() + 3700000);
    //日期时间选择器
    laydate.render({
        elem: '#dtstart'
      , type: 'date'
        , min: 0 //0天前
        , max: 30 //30天后
        , value: def

    });

});

var GG = {
    "kk": function (pageindex) {
        var indexkey = $('#PageIndex');
        var nowindex = indexkey.val();
        if (nowindex != pageindex) {
            indexkey.val(pageindex);
            pageajax();
        }
    }
}

function initMypage(count, current, pagecount) {
    //总数量，当前显示页，每页显示数，输出当前页函数
    $("#MyPage").initPage(count, current, pagecount, GG.kk);
}


//每页数量，当前页
function pageajax() {
    var tbody = window.document.getElementById("pagetbody");
    var sumdata = window.document.getElementById("listcount");
    var nowindex = window.document.getElementById("PageIndex");
    $.ajax({
        //几个参数需要注意一下
        type: "POST",//方法类型
        dataType: "json",//预期服务器返回的数据类型
        url: "/Home/ShowTicket",//url
        data: $('#searchform').serialize(),
        success: function (data) {
            var str = "";
            var flightinfolist = data.Item1;

            for (var i = 0; i < flightinfolist.length; i++) {

                var gotime = Num2Date(flightinfolist[i].GoTime, 'yyyy-MM-dd hh:mm:ss');
                str += "<tr class='text-c'>";
                str += "<td>" + gotime + "</td>";
                str += "<td>" + flightinfolist[i].OverTime + "分钟</td>";
                str += "<td>" + flightinfolist[i].Number + "</td>";

                str += "<td>" + flightinfolist[i].PathInfo.Origin + "</td>";
                str += "<td>" + flightinfolist[i].PathInfo.Destination + "</td>";
                str += "<td>" + flightinfolist[i].Price + "元</td>";
                str += "<td>" + flightinfolist[i].Passengers + "</td>";

                str += "<td class='td-manage'>";
                if (flightinfolist[i].Passengers > 0) {
                    str += "<a class='layui-btn layui-btn-xs' onClick=\"sellTicket('" + flightinfolist[i].ID + "','" + gotime + "')\">立即购票</a>";
                }
                else {
                    str += "<a class='layui-btn layui-btn-xs layui-btn-disabled'>暂无余票</a>";
                }

                str += "</td>";
                str += "</tr>";
            }
            tbody.innerHTML = str;
            sumdata.innerText = data.Item2.TotalItems;
            nowindex.value = data.Item2.PageIndex;
            initMypage(data.Item2.TotalItems, data.Item2.PageIndex, data.Item2.PageSize);

        },
        error: function (data) {

        }

    });

}

function sellTicket(id, gotime) {
    var url = "/Home/SellTicket?id=" + id + "&dtstart=" + gotime;
    layer_show("请输入乘客信息...", url, 600, 300);

}

//*****************汽车时刻表*****************//







//*****************乘车记录*****************//
layui.use('laydate', function () {
    var laydate = layui.laydate;
    
    //日期时间选择器
    laydate.render({
        elem: '#gotimecc'
      , type: 'date'
        , max: 0 //0天后
        

    });

});

var GGcc = {
    "kk": function (pageindex) {
        var indexkey = $('#PageIndexcc');
        var nowindex = indexkey.val();
        if (nowindex != pageindex) {
            indexkey.val(pageindex);
            pageajaxcc();
        }
    }
}


function initMypagecc(count, current, pagecount) {
    //总数量，当前显示页，每页显示数，输出当前页函数
    $("#MyPagecc").initPage(count, current, pagecount, GGcc.kk);
}

//每页数量，当前页
function pageajaxcc() {
    var tbody = window.document.getElementById("pagetbodycc");
    var sumdata = window.document.getElementById("listcountcc");
    var nowindex = window.document.getElementById("PageIndexcc");
    $.ajax({
        //几个参数需要注意一下
        type: "POST",//方法类型
        dataType: "json",//预期服务器返回的数据类型
        url: "/Home/MyselfTicket",//url
        data: $('#searchChengChe').serialize(),
        success: function (data) {
            var str = "";
            var ticketlist = data.Item1;

            for (var i = 0; i < ticketlist.length; i++) {

                str += "<tr class='text-c'>";
                str += "<td>" + Num2Date(ticketlist[i].Gotime, 'yyyy-MM-dd hh:mm:ss') + "</td>";
                str += "<td>" + Num2Date(ticketlist[i].Arrtime, 'yyyy-MM-dd hh:mm:ss') + "</td>";
                str += "<td>" + ticketlist[i].Number + "</td>";

                str += "<td>" + ticketlist[i].Origin + "</td>";
                str += "<td>" + ticketlist[i].Destination + "</td>";
                str += "<td>" + ticketlist[i].Money + "元</td>";

                str += "<td class='td-manage' align='center'>";
                str += "<a class='layui-btn layui-btn-xs' onClick=\"eticket('" + ticketlist[i].GUID + "')\">电子票</a>";

                str += "</td>";
                str += "</tr>";
            }
            tbody.innerHTML = str;
            sumdata.innerText = data.Item2.TotalItems;
            nowindex.value = data.Item2.PageIndex;
            initMypagecc(data.Item2.TotalItems, data.Item2.PageIndex, data.Item2.PageSize);

        },
        error: function (data) {

        }

    });

}

function eticket(guid) {
    layer_show("电子车票", "/Home/TicketDetails?guid=" + guid, 600, 380);
}


//*****************乘车记录*****************//




//*****************购票记录*****************//
layui.use('laydate', function () {
    var laydate = layui.laydate;

    //日期时间选择器
    laydate.render({
        elem: '#gotimegp'
      , type: 'date'
        , max: 0 //0天后


    });

});



var GGgp = {
    "kk": function (pageindex) {
        var indexkey = $('#PageIndexgp');
        var nowindex = indexkey.val();
        if (nowindex != pageindex) {
            indexkey.val(pageindex);
            pageajaxgp();
        }
    }
}

function initMypagegp(count, current, pagecount) {
    //总数量，当前显示页，每页显示数，输出当前页函数
    $("#MyPagegp").initPage(count, current, pagecount, GGgp.kk);
}



//每页数量，当前页
function pageajaxgp() {
    var tbody = window.document.getElementById("pagetbodygp");
    var sumdata = window.document.getElementById("listcountgp");
    var nowindex = window.document.getElementById("PageIndexgp");

    var nowdate = new Date();

    $.ajax({
        //几个参数需要注意一下
        type: "POST",//方法类型
        dataType: "json",//预期服务器返回的数据类型
        url: "/Home/BuyTicket",//url
        data: $('#searchGouPiao').serialize(),
        success: function (data) {
            var str = "";
            var ticketlist = data.Item1;

            for (var i = 0; i < ticketlist.length; i++) {

                str += "<tr class='text-c'>";
                str += "<td>" + Num2Date(ticketlist[i].Gotime, 'yyyy-MM-dd hh:mm:ss') + "</td>";
                str += "<td>" + Num2Date(ticketlist[i].BuyDate, 'yyyy-MM-dd hh:mm:ss') + "</td>";
                str += "<td>" + ticketlist[i].Name + "</td>";

                str += "<td>" + ticketlist[i].Origin + "</td>";
                str += "<td>" + ticketlist[i].Destination + "</td>";
                str += "<td>" + ticketlist[i].Money + "元</td>";

                str += "<td class='td-manage' align='center'>";
                str += "<a class='layui-btn layui-btn-xs' onClick=\"eticket('" + ticketlist[i].GUID + "')\">电子票</a>";

                if (ticketlist[i].Status == 1) {
                    str += "<a class='layui-btn layui-btn-primary layui-btn-xs' onClick=\"QuitTicket('" + ticketlist[i].GUID + "','" + ticketlist[i].Money + "')\">退票</a>";
                    str += "<a class='layui-btn layui-btn-primary layui-btn-xs' onClick=\"ChangeTicket('" + ticketlist[i].GUID + "','" + ticketlist[i].Money + "')\">改签</a>";

                }

                str += "</td>";
                str += "</tr>";
            }
            tbody.innerHTML = str;
            sumdata.innerText = data.Item2.TotalItems;
            nowindex.value = data.Item2.PageIndex;
            initMypagegp(data.Item2.TotalItems, data.Item2.PageIndex, data.Item2.PageSize);

        },
        error: function (data) {

        }

    });

}

//改签操作
function ChangeTicket(guid, money) {
    var msg = "确认改签吗？售价：" + money + "元。改签费用：" + money * 0.1 + "元";
    layer.confirm(msg, function (index) {

        layer_show("车票改签", "/Home/ChangeTicket?guid=" + guid, 600, 580);
        layer.close(index);
    });

}

//退票操作
function QuitTicket(guid,money) {
    var msg = "确认退票吗？本次退费：" + money * 0.8 + "元";
    layer.confirm(msg, function (index) {
        $.ajax({
            type: 'POST',
            url: '/Home/QuitTicket',
            data: { guid: guid },
            dataType: 'json',
            success: function (data) {
                if (data == "ok") {
                    layer.msg('退票成功!', { icon: 1, time: 1500 });
                }
                else if (data == "no") {
                    layer.msg('退票失败!发车前30分钟后无法退票!', { icon: 2, time: 1500 });
                }
                else if (data == "yet") {
                    layer.msg('该车票无法退款!', { icon: 2, time: 1500 });
                }
                else {
                    layer.msg('退票失败!请稍后重试!', { icon: 2, time: 1500 });
                }
            },
            error: function (data) {
                console.log(data.msg);
            },
        });
    });

}



//*****************购票记录*****************//








//*****************修改密码*****************//



///获取当前登陆信息，赋值给修改密码的表单
function GetNowUser() {

    $.ajax({
        //几个参数需要注意一下
        type: "POST",//方法类型
        dataType: "json",//预期服务器返回的数据类型
        url: "/UserInfo/GetNow",//url
        data: {data:"1"},
        success: function (data) {
            var options = { jsonValue: data, isDebug: false };
            $("#userinfoform").initForm(options);
            
            //解决日期赋值错误的问题
            $("#mySignDate").val(Num2Date(data.SignDate, 'yyyy-MM-dd hh:mm:ss'))
        },
        error: function (date) {

        }
    });
}


function Setnewpwd() {
    if ($("#NewPwd").val().length < 5 || $("#NewPwd").val() != $("#NewPwd2").val()) {
        layer.msg("密码长度不足或密码不一致！", { icon: 2, time: 1500 });
        return;
    }

    $.ajax({
        //几个参数需要注意一下
        type: "POST",//方法类型
        dataType: "json",//预期服务器返回的数据类型
        url: "/UserInfo/SetNewPwd",//url
        data: { newpwd: $("#NewPwd").val() },
        success: function (data) {
            if (data == "ok") {
                layer.msg("密码修改成功！", { icon: 1, time: 2000 });
            }
            else {
                layer.msg("密码修改失败！请稍后重试！", { icon: 2, time: 2000 });
            }
            $("#NewPwd").val("");
            $("#NewPwd2").val("");
        },
        error: function (date) {
            layer.msg("密码修改失败！请稍后重试！", { icon: 2, time: 2000 });
        }
    });

}



//*****************修改密码*****************//
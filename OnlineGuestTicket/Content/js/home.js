// JavaScript Document
$(document).ready(function(e) {
    $(".middle>.cont>.class>ul>a").on("click",function(){
		$(this).addClass("net");
		$(this).siblings("a").removeClass("net");
		});
});
$(document).ready(function(e) {
    $(".middle>.cont>.class>ul>.net").on("click",function(){
		$(".all1").fadeIn();
		$(".all2,.all3,.all4,.all5").hide();
		});
});
$(document).ready(function(e) {
    $(".middle>.cont>.class>ul>._all2").on("click",function(){
		$(".all2").fadeIn();
		$(".all1,.all3,.all4,.all5").hide();
		});
});
$(document).ready(function(e) {
    $(".middle>.cont>.class>ul>._all3").on("click",function(){
		$(".all3").fadeIn();
		$(".all1,.all2,.all4,.all5").hide();
		});
});
$(document).ready(function(e) {
    $(".middle>.cont>.class>ul>._all4").on("click",function(){
		$(".all4").fadeIn();
		$(".all1,.all2,.all3,.all5").hide();
		});
});
$(document).ready(function(e) {
    $(".middle>.cont>.class>ul>._all5").on("click",function(){
		$(".all5").fadeIn();
		$(".all2,.all3,.all4,.all1").hide();
		});
});


/*弹出层*/
/*
	参数解释：
	title	标题
	url		请求的url
	id		需要操作的数据id
	w		弹出层宽度（缺省调默认值）
	h		弹出层高度（缺省调默认值）
*/
function layer_show(title, url, w, h) {
    if (title == null || title == '') {
        title = false;
    };
    if (url == null || url == '') {
        url = "404.html";
    };
    if (w == null || w == '') {
        w = 800;
    };
    if (h == null || h == '') {
        h = ($(window).height() - 50);
    };
    layer.open({
        type: 2,
        area: [w + 'px', h + 'px'],
        fix: false, //不固定
        maxmin: true,
        shade: 0.4,
        title: title,
        content: url
    });
}
/*关闭弹出框口*/
function layer_close() {
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.close(index);
}
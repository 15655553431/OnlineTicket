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


/*������*/
/*
	�������ͣ�
	title	����
	url		�����url
	id		��Ҫ����������id
	w		�������ȣ�ȱʡ��Ĭ��ֵ��
	h		������߶ȣ�ȱʡ��Ĭ��ֵ��
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
        fix: false, //���̶�
        maxmin: true,
        shade: 0.4,
        title: title,
        content: url
    });
}
/*�رյ������*/
function layer_close() {
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.close(index);
}
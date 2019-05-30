//时间日期转换成string
function Num2Date(str, value) {
    if (value == "yyyy-MM-dd hh:mm:ss") {
        var d = eval('new ' + str.substr(1, str.length - 2));
        var ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate(), d.getHours(), d.getMinutes(), d.getSeconds()];
        for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
        return ar_date.slice(0, 3).join('-') + ' ' + ar_date.slice(3).join(':');
        function dFormat(i) { return i < 10 ? "0" + i.toString() : i; }
    }
    else if (value == "yyyy-MM-dd") {
        var d = eval('new ' + str.substr(1, str.length - 2));
        var ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate()];
        for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
        return ar_date.join('-');
        function dFormat(i) { return i < 10 ? "0" + i.toString() : i; }
    }
}

/*
 * jquery 初始化form插件，传入一个json对象，为form赋值 
 * version: 1.0.0-2013.06.24
 * @requires jQuery v1.5 or later
 * Copyright (c) 2013
 * note:  1、此方法能赋值一般所有表单，但考虑到checkbox的赋值难度，以及表单中很少用checkbox，这里不对checkbox赋值
 *		  2、此插件现在只接收json赋值，不考虑到其他的来源数据
 *		  3、对于特殊的textarea，比如CKEditor,kindeditor...，他们的赋值有提供不同的自带方法，这里不做统一，如果项目中有用到，不能正确赋值，请单独赋值
 */
(function ($) {
    $.fn.extend({
        initForm: function (options) {
            //默认参数
            var defaults = {
                jsonValue: "",
                isDebug: false	//是否需要调试，这个用于开发阶段，发布阶段请将设置为false，默认为false,true将会把name value打印出来
            }
            //设置参数
            var setting = $.extend({}, defaults, options);
            var form = this;
            jsonValue = setting.jsonValue;
            //如果传入的json字符串，将转为json对象
            if ($.type(setting.jsonValue) === "string") {
                jsonValue = $.parseJSON(jsonValue);
            }
            //如果传入的json对象为空，则不做任何操作
            if (!$.isEmptyObject(jsonValue)) {
                var debugInfo = "";
                $.each(jsonValue, function (key, value) {
                    //是否开启调试，开启将会把name value打印出来
                    if (setting.isDebug) {
                        alert("name:" + key + "; value:" + value);
                        debugInfo += "name:" + key + "; value:" + value + " || ";
                    }
                    var formField = form.find("[name='" + key + "']");
                    if ($.type(formField[0]) === "undefined") {
                        if (setting.isDebug) {
                            alert("can not find name:[" + key + "] in form!!!");	//没找到指定name的表单
                        }
                    } else {
                        var fieldTagName = formField[0].tagName.toLowerCase();
                        if (fieldTagName == "input") {
                            if (formField.attr("type") == "radio") {
                                $("input:radio[name='" + key + "'][value='" + value + "']").attr("checked", "checked");
                            } else {
                                formField.val(value);
                            }
                        } else if (fieldTagName == "select") {
                            //do something special
                            formField.val(value);
                        } else if (fieldTagName == "textarea") {
                            //do something special
                            formField.val(value);
                        } else {
                            formField.val(value);
                        }
                    }
                })
                if (setting.isDebug) {
                    alert(debugInfo);
                }
            }
            return form;	//返回对象，提供链式操作
        }
    });
})(jQuery)



/**
 * Created by zxm on 2017/3/31.
 */
$.fn.extend({
    "initPage": function (listCount, currentPage, pagelistcount, fun) {
        var maxshowpageitem = $(this).attr("maxshowpageitem");
        if (maxshowpageitem != null && maxshowpageitem > 0 && maxshowpageitem != "") {
            page.maxshowpageitem = maxshowpageitem;
        }
        //var pagelistcount = $(this).attr("pagelistcount");
        if (pagelistcount != null && pagelistcount > 0 && pagelistcount != "") {
            page.pagelistcount = pagelistcount;
        }

        var pageId = $(this).attr("id");
        page.pageId = pageId;
        if (listCount < 0) {
            listCount = 0;
        }
        if (currentPage <= 0) {
            currentPage = 1;
        }
        page.setPageListCount(listCount, currentPage, fun);

    }
});


var page = {
    "pageId": "",
    "maxshowpageitem": 5,//最多显示的页码个数
    //"pagelistcount":10,//每一页显示的内容条数
    /**
     * 初始化分页界面
     * @param listCount 列表总量
     */
    "initWithUl": function (listCount, currentPage) {
        var pageCount = 1;
        if (listCount >= 0) {
            var pageCount = listCount % page.pagelistcount > 0 ? parseInt(listCount / page.pagelistcount) + 1 : parseInt(listCount / page.pagelistcount);
        }
        var appendStr = page.getPageListModel(pageCount, currentPage);
        $("#" + page.pageId).html(appendStr);
    },
    /**
     * 设置列表总量和当前页码
     * @param listCount 列表总量
     * @param currentPage 当前页码
     */
    "setPageListCount": function (listCount, currentPage, fun) {
        listCount = parseInt(listCount);
        currentPage = parseInt(currentPage);
        page.initWithUl(listCount, currentPage);
        page.initPageEvent(listCount, fun);
        fun(currentPage);
    },
    "initPageEvent": function (listCount, fun) {
        $("#" + page.pageId + ">li[class='pageItem']").on("click", function () {
            page.setPageListCount(listCount, $(this).attr("page-data"), fun);
        });
    },
    "getPageListModel": function (pageCount, currentPage) {
        var prePage = currentPage - 1;
        var nextPage = currentPage + 1;
        var prePageClass = "pageItem";
        var nextPageClass = "pageItem";
        if (prePage <= 0) {
            prePageClass = "pageItemDisable";
        }
        if (nextPage > pageCount) {
            nextPageClass = "pageItemDisable";
        }
        var appendStr = "";
        appendStr += "<li class='" + prePageClass + "' page-data='1' page-rel='firstpage' style='width:30px;'>首页</li>";
        appendStr += "<li class='" + prePageClass + "' page-data='" + prePage + "' page-rel='prepage' style='width:55px;'>&lt;上一页</li>";
        var miniPageNumber = 1;
        if (currentPage - parseInt(page.maxshowpageitem / 2) > 0 && currentPage + parseInt(page.maxshowpageitem / 2) <= pageCount) {
            miniPageNumber = currentPage - parseInt(page.maxshowpageitem / 2);
        } else if (currentPage - parseInt(page.maxshowpageitem / 2) > 0 && currentPage + parseInt(page.maxshowpageitem / 2) > pageCount) {
            miniPageNumber = pageCount - page.maxshowpageitem + 1;
            if (miniPageNumber <= 0) {
                miniPageNumber = 1;
            }
        }
        var showPageNum = parseInt(page.maxshowpageitem);
        if (pageCount < showPageNum) {
            showPageNum = pageCount
        }
        for (var i = 0; i < showPageNum; i++) {
            var pageNumber = miniPageNumber++;
            var itemPageClass = "pageItem";
            if (pageNumber == currentPage) {
                itemPageClass = "pageItemActive";
            }

            appendStr += "<li class='" + itemPageClass + "' page-data='" + pageNumber + "' page-rel='itempage'>" + pageNumber + "</li>";
        }
        appendStr += "<li class='" + nextPageClass + "' page-data='" + nextPage + "' page-rel='nextpage' style='width:55px;'>下一页&gt;</li>";
        appendStr += "<li class='" + nextPageClass + "' page-data='" + pageCount + "' page-rel='lastpage' style='width:30px;'>尾页</li>";
        return appendStr;

    }
}



//


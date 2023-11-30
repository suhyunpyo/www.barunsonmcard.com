
$(document).ready(function () {

    $("#accordionSidebar").find("li").hide();

    var url = location.href.toLocaleUpperCase();

    var LeftNaviName = "";

    if (url.indexOf("BANNER") > 0 || url.indexOf("BANNER/CATEGORY") > 0 || url.indexOf("BANNER/ADD") > 0 || url.indexOf("POPUP") > 0 || url.indexOf("NOTICE") > 0 || url.indexOf("FAQ") > 0) {

        LeftNaviName = "ManageNevi";
    }
    else if (url.indexOf("STATISTICS") > 0) {

        LeftNaviName = "StatisticsNevi";
    }
    else if (url.indexOf("PRODUCTLIST") > 0 || url.indexOf("MAINDISPLAY") > 0 || url.indexOf("CATEGORYDISPLAY") > 0 || url.indexOf("DISPLAYORDER") > 0 ||
        url.indexOf("MAINCATEGORY") > 0 || url.indexOf("CATEGORY") > 0 || url.indexOf("PRODUCTREGIST") > 0 || url.indexOf("TEMPLATE") > 0)
    {
        LeftNaviName = "ProductNevi";
    }
    else if (url.indexOf("ORDER") > 0) {

        LeftNaviName = "OrderNevi";
    }
    else if (url.indexOf("MEMBER") > 0 || url.indexOf("AUTHORITY") > 0)
    {
        LeftNaviName = "MemberNevi";

        //if (url.indexOf("MEMBER") > 0) {
        //    $(".top-rocation div").html("<a href=\"#none\" onclick=\"location.href='/Admin/Member'\">회원관리</a>");
        //}
        //else {
        //    $(".top-rocation div").html("<a href=\"#none\" onclick=\"location.href='/Admin/Authority'\">권한관리</a>");
        //}
    }
    else if (url.indexOf("COUPON") > 0) {

        LeftNaviName = "CouponNavi";
    }
    //else if (url.indexOf("ORDER") > 0) {

    //    LeftNaviName = "OrderNevi";
    //}
    else if (url.indexOf("MENU") > 0) {

        LeftNaviName = "ManageNevi";
    }
    
    //alert(url);
    LeftViewMenu(LeftNaviName);
  
    $(".top-rocation div").children().remove();
   
    if (LeftNaviName == "ProductNevi") { //상품관리

        $(".navbar-nav_top li:eq(2)").css({
            "background": "#84a0ff", "width": "105px", "height": "88px", "display:": "block"
        });

        if (url.indexOf("PRODUCTLIST") > 0) {

            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"/Admin/Productlist\">상품관리 ></a>");
        }
        else if (url.indexOf("MAINDISPLAY") > 0) {

            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"/Admin/Productlist\">진열관리 ></a><a href=\"/Admin/MainDisplay\"> 메인진열</a>");
        }
        else if (url.indexOf("CATEGORYDISPLAY") > 0) {

            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"/Admin/Productlist\">진열관리 ></a><a href=\"/Admin/CategoryDisplay\"> 카테고리진열</a>");
        }
        else if (url.indexOf("DISPLAYORDER") > 0) {

            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"/Admin/Productlist\">진열관리 ></a><a href=\"/Admin/DisplayOrder\"> 진열순서</a>");
        }
        else if (url.indexOf("MAINCATEGORY") > 0) {

            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"/Admin/Productlist\">분류관리 ></a><a href=\"/Admin/MainCategory\"> 메인분류</a>");
        }
        else if (url.indexOf("CATEGORY") > 0) {

            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"/Admin/Productlist\">분류관리 ></a><a href=\"/Admin/Category\"> 카테고리분류</a>");
        }
        else if (url.indexOf("PRODUCTREGIST") > 0) {

            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"/Admin/ProductRegist\"> 상품등록</a>");
        }


    }
    else if (LeftNaviName == "OrderNevi") {

        $(".navbar-nav_top li:eq(3)").css({
            "background": "#84a0ff", "width": "105px", "height": "88px", "display:": "block"
        });

        if (url.indexOf("CANCEL") > 0) {

            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"/Admin/Order\">주문관리 ></a><a href=\"/Admin/Order/Cancel\"> 취소/환불</a>");
        }
        else if (url.indexOf("ORDER") > 0) {

            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"/Admin/Order\">주문관리 ></a><a href=\"/Admin/Order\"> 주문목록</a>");
        }

        /*LeftNaviName = "OrderNevi";*/
    }
    else if (LeftNaviName == "MemberNevi") {

        $(".navbar-nav_top li:eq(1)").css({
            "background": "#84a0ff", "width": "105px", "height": "88px", "display:": "block"
        });


        if (url.indexOf("MEMBER") > 0) {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Member'\">회원관리 ></a><a href=\"/Admin/Member\"> 회원관리</a>");
        }
        else {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Member'\">회원관리 ><a href=\"#none\" onclick=\"location.href='/Admin/Authority'\"> 권한관리</a>");
        }

    }
    else if (LeftNaviName == "CouponNavi") {


        $(".navbar-nav_top li:eq(4)").css({
            "background": "#84a0ff", "width": "105px", "height": "88px", "display:": "block"
        });

        if (url.indexOf("COUPON/ADD") > 0) {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Coupon'\">쿠폰관리 ></a><a href=\"/Admin/Coupon/Add\"> 쿠폰등록</a>");
        }
        else {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Coupon'\">쿠폰관리 ><a href=\"#none\" onclick=\"location.href='/Admin/Coupon'\"> 쿠폰목록</a>");
        }

    }
    else if (LeftNaviName == "ManageNevi") {

        $(".navbar-nav_top li:eq(5)").css({
            "background": "#84a0ff", "width": "105px", "height": "88px", "display:": "block"

        });
        if (url.indexOf("BANNER/CATEGORY") > 0) {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Banner'\">배너관리 ></a><a href=\"/Admin/Banner/Category\"> 배너분류</a>");
        }
        else if (url.indexOf("BANNER") > 0) {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Banner'\">배너관리 ><a href=\"#none\" onclick=\"location.href='/Admin/Banner/'\"> 배너목록</a>");
        }
        else if (url.indexOf("POPUP") > 0) {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Popup'\">팝업관리 >");
        }
        else if (url.indexOf("NOTICE") > 0) {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Notice'\">공지사항 >");
        }
        else if (url.indexOf("FAQ") > 0) {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Faq'\">FAQ >");
        }
        else if (url.indexOf("ADMIN/MENU/MTC01") > 0) {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Menu/MTC01'\">메뉴관리 ></a><a href=\"/Admin/Menu/MTC01\"> 상단GNB</a>");
        }
        else if (url.indexOf("ADMIN/MENU/MTC02") > 0) {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Menu/MTC02'\">메뉴관리 ></a><a href=\"/Admin/Menu/MTC02\"> 풋터</a>");
        }

    }
    else if (LeftNaviName == "StatisticsNevi") {
        $(".navbar-nav_top li:eq(0)").css({
            "background": "#84a0ff", "width": "105px", "height": "88px", "display:": "block"
        });

        if (url.indexOf("/STATISTICS/TOTALDAILY") > 0) {

            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Statistics/TotalDaily'\">통계관리 ></a><a href=\"/Admin/Statistics/TotalDaily\">전체현황>일별</a>");
        }
        else if (url.indexOf("/STATISTICS/TOTALMONTHLY") > 0) {

            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Statistics/TotalDaily'\">통계관리 ></a><a href=\"/Admin/Statistics/TotalMonthly\">전체현황>월별</a>");
        }
        else if (url.indexOf("/STATISTICS/SALESDAILY") > 0) {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Statistics/TotalDaily'\">통계관리 ></a><a href=\"/Admin/Statistics/SalesDaily\">매출통계>일별</a>");

        }
        else if (url.indexOf("/STATISTICS/SALESMONTHLY") > 0) {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Statistics/TotalDaily'\">통계관리 ></a><a href=\"/Admin/Statistics/SalesMonthly\">매출통계>월별</a>");

        }
        else if (url.indexOf("/STATISTICS/PURCHASEPATTERN") > 0) {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Statistics/TotalDaily'\">통계관리 ></a><a href=\"/Admin/Statistics/PurchasePattern\">구매패턴</a>");

        }
        else if (url.indexOf("/STATISTICS/PAYTYPEDAILY") > 0) {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Statistics/TotalDaily'\">통계관리 ></a><a href=\"/Admin/Statistics/PayTypeDaily\">결제수단>일별</a>");

        }
        else if (url.indexOf("/STATISTICS/PAYTYPEMONTHLY") > 0) {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Statistics/TotalDaily'\">통계관리 ></a><a href=\"/Admin/Statistics/PayTypeMonthly\">결제수단>월별</a>");

        }
        else if (url.indexOf("/STATISTICS/PRODUCTS") > 0) {
            $(".top-rocation div").append("<a href=\"/admin\"> HOME > <a><a href=\"#none\" onclick=\"location.href='/Admin/Statistics/TotalDaily'\">통계관리 ></a><a href=\"/Admin/Statistics/Products\">제품통계</a>");

        }
    }

      //<a href="#">통계></a>
      //      <a href="#">전체현황 ></a>
      //      <a href="#">일별현황</a>
});
function LeftViewMenu(objname) {
   
    $("li[name =\"" + objname + "\"]").css("display", "block");
  }
    
//왼쪽 메뉴 활성화 
function LeftViewSubMenu(gubun, objname, idx, idx2) {

    var obj = $("li[name = \"" + objname + "\"]:eq(" + idx + ")");
    if (gubun == "A") { //하위로 떨어지는 메뉴가 존재시 
        $(obj).find("a").removeClass("collapsed").parent().find("div").addClass("show").find("div a:eq(" + idx2 + ")").css("background-color", "#eaecf4");
    }
    else {
        $(obj).find("a").removeClass("collapsed");
        //$(".sidebar-dark .nav-item .nav-link i").attr("color", "");
    }

    $(obj).find("a").find("i").css("color", "rgba(255, 255, 255, 0.8)");
   
}

//페이징 네비 커스터마이징
function PagingNavigator() {
    var obj = $(".pagination-container");
    $(obj).attr("class", "col-sm-12 col-md-7 text-center").css({/* "width": "380px", */"margin": "0 auto" });
    var naviObj = $(obj).find(".pagination").html();
    // alert(naviObj);
    $(obj).find(".pagination").replaceWith("<div class=\"dataTables_paginate paging_simple_numbers\" id=\"dataTable_paginate\"></div>");
    $("#dataTable_paginate").append("<ul class=\pagination\>" + naviObj + "</ul>");
    $("#dataTable_paginate ul li").each(function () {
        if ($(this).attr("class") == "PagedList-skipToPrevious") {
            $(this).attr("class", "paginate_button page-item previous");
            $(this).find("a").attr("class", "page-link").text("Previous");
        }
        else if ($(this).attr("class") == "PagedList-skipToNext") {
            $(this).attr("class", "paginate_button page-item next");
            $(this).find("a").attr("class", "page-link").text("Next");

        }
        else if ($(this).attr("class") == "active") {
            $(this).attr("class", "paginate_button page-item active");
            var actNum = $(this).find("span").text();

            $(this).find("span").replaceWith(function () {
                return $('<a>', { class: "page-link", text: actNum })
            });

        } else {
            $(this).attr("class", "paginate_button page-item").find("a").attr("class", "page-link");
        }

    });
}


function TimeChk() {
    var html = [];
    var value = "";

    for (var i = 0; i < 24; i++) {
        if (i < 10) {
            value = "0" + i;
        }
        else {
            value = i;
        }
        html[i] = "<option value=" + value + ">" + value + "</option>"

        //if (vu != "" && vu == value) {
        //    html[i] = "<option value=" + value + " seleted>" + value + "</option>"
        //}
        //else html[i] = "<option value=" + value + ">" + value + "</option>"
     
    }
    return html;
}


var isEmpty = function (val) {
    if (val === "" || val === null || val === undefined
        || (val !== null && typeof val === "object" && !Object.keys(val).length)
    ) {
        return true
    } else {
        return false
    }

};

var ratio = 1; 
var _Width = 0;
var areas = {};
var repeatData = {};
var objects = {};
$(document).ready(function () {

    if ($(document).width() < 500) {
        if (typeof parent.$(".product_img .img_con").val() != 'undefined') {
            _Width = parent.$(".product_img .img_con").width();
        } else {
            _Width = $("#wrap").width();
        }

    } else {
        _Width = $("#wrap").width();
    }
    ratio = _Width / 800;

    var prodCateCode = $("#Product_Category_Code").val();

    if ($("#RepeatData").text() != "")
        repeatData = JSON.parse($("#RepeatData").text());
    if ($("#TB_Area").text() != "")
        var areas = JSON.parse($("#TB_Area").text());

    if ($("#Objects").text() != "") 
        objects = JSON.parse($("#Objects").text());

    
    var idx = 0;
    //아기정보 본문은 반복으로 제외
    var areaItems = $.grep(objects, function (item, index) { return (item.pid != "area19") });

    areaItems.forEach(function (elem) {

        var w = elem.width * ratio
        var h = elem.height * ratio
        var x = elem.left * ratio
        var y = elem.top * ratio
        idx++;
        if (elem.type == 'img') {
            var div = "<div id='" + elem.id + "'  class='item' style='top: " + y + "px; left: " + x + "px;  position:absolute;'><img class='img' src='" + $("#CDN").val() + elem.resource_url + "' width='" + w + "px' height='" + h + "px'  /></div>";
            $('#' + elem.pid).append(div);
        } else if (elem.type == 'photo') {
            var div = "<div id='" + elem.id + "'  class='item photo-image' style='top: " + y + "px; left: " + x + "px;  position:absolute;'><img class='img' src='" + $("#CDN").val() + elem.resource_url + "' width='" + w + "px' height='" + h + "px'  /></div>";
            $('#' + elem.pid).append(div);
        } else if (elem.type == 'profile') {
            var div = "<div id='" + elem.id + "'  class='item profile-image' style='top: " + y + "px; left: " + x + "px;  position:absolute;'><img class='img' src='" + $("#CDN").val() + elem.resource_url + "' width='" + w + "px' height='" + h + "px'  /></div>";
            $('#' + elem.pid).append(div);
        } else {
            var text = matchText(elem.chracterset);
            var matchinfo = "<input type='text' id='" + idx + "' idx='" + idx + "' class='matchinfo' value='" + elem.chracterset + "'>"
            $('#divMatch').append(matchinfo);

            var txtfontsize = elem.fontsize * ratio;
            
            var div = "<div id='" + elem.id + "' class='item'  style='top: " + y + "px; left: " + x + "px;    position:absolute; width:" + w + "px;  height:" + h + "px;'><div class='text'>" + text + "</div></div>";
            $('#' + elem.pid).append(div);
            $('#' + elem.id).css('background-color', elem.bgcolor);
            $('#' + elem.id).children(".ui-resizable-handle").removeClass('resizabled');
            $('#' + elem.id).children(".ui-resizable-handle").css('display', 'none');
            $('#' + elem.id + ">.text").css('font-family', elem.font);
            $('#' + elem.id + ">.text").css('font-size', txtfontsize + "px");
            $('#' + elem.id + ">.text").css('color', elem.fontcolor);
            $('#' + elem.id + ">.text").css('font-weight', elem.bold_yn ? "bold" : "");
            $('#' + elem.id + ">.text").css('font-style', elem.italic_yn ? "italic" : "");
            $('#' + elem.id + ">.text").css('text-decoration', elem.underline_yn ? "underline" : "");
            if (elem.horizontal_align == "C") {
                $('#' + elem.id + ">.text").css('text-align', "center")
            } else if (elem.horizontal_align == "R") {
                $('#' + elem.id + ">.text").css('text-align', "right")
            } else if (elem.horizontal_align == "L") {
                $('#' + elem.id + ">.text").css('text-align', "left");
            } else {
                $('#' + elem.id + ">.text").css('text-align', "");
            }
            if (elem.vertical_align == "T") {
                $('#' + elem.id).css('align-items', "flex-start")
            } else if (elem.vertical_align == "M") {
                $('#' + elem.id).css('align-items', "center")
            } else if (elem.vertical_align == "B") {
                $('#' + elem.id).css('align-items', "flex-end");
            } else {
                $('#' + elem.id).css('align-items', "");
            }
            $('#' + elem.id + ">.text").css('letter-spacing', elem.between_text / 100 + "em");
            $('#' + elem.id + ">.text").css('line-height', elem.between_line + "em");
            $('.item').each(function () {
                $(this).data("height", $(this).outerHeight());
                $(this).data("width", $(this).outerWidth());
            });
            $('.text', '.item').each(function () {
                $(this).data("height", $(this).outerHeight());
                //$(this).data("width", $(this).outerWidth());
                $(this).data("fontSize", parseInt($(this).css("font-size")));
            });
        }
    });

    //신랑 신부 연락정보
    $("#area4").prepend("<img src=\"https://static.barunsoncard.com/barunsonmcard/invitation/preview/img_01.png\"/>");
    $("#area4 img").css("width", _Width + "px");
    $("#area4 img").css("display", "block");
    //엄마 아빠연락처
    $("#area21").prepend("<img src=\"https://static.barunsoncard.com/barunsonmcard/invitation/preview/tell_PCC03.png\"/>");
    $("#area21 img").css("width", _Width + "px");
    $("#area21 img").css("display", "block");
    //화환선물
    $("div.flowergift").hide();

    if (prodCateCode == "PCC01") {
        //혼주 연락 정보
        $("#area4 .list_wrap").append("<img src=\"https://static.barunsoncard.com/barunsonmcard/invitation/preview/img_02.png\"/>");
        $("#area4 .list_wrap").css('margin-top', '0');
        $("#area4 .list_wrap img").css("width", _Width + "px");
        $("#area4 .list_wrap img").css("display", "block");

        //갤러리
        $("#area6").append("<img src=\"https://static.barunsoncard.com/barunsonmcard/invitation/preview/img_03.png\"/>");
        $("#area6 img").css("width", _Width + "px");
        $("#area6 img").css("display", "block");

        //동영상
        $("#area8").append("<img src=\"https://static.barunsoncard.com/barunsonmcard/invitation/preview/img_04.png\"/>");
        $("#area8 img").css("width", _Width + "px");
        $("#area8 img").css("display", "block");


        $("#area15").append("<img src=\"https://static.barunsoncard.com/barunsonmcard/invitation/preview/message.png\"/>");
        $("#area15 img").css("width", _Width + "px");
        $("#area15 img").css("display", "block");

        //송금하기
        $(".remittance").append("<img src=\"https://static.barunsoncard.com/barunsonmcard/invitation/preview/img_06.png\"/>");
        $(".remittance img").css("width", _Width + "px");
        $(".remittance img").css("display", "block");

        //오시는길
        $("#area10").append("<img src=\"https://static.barunsoncard.com/barunsonmcard/invitation/preview/img_05.png\"/>");
        $("#area10 img").css("width", _Width + "px");
        $("#area10 img").css("display", "block");

        //화환선물
        $("div.flowergift").show();
        $("div.flowergift img").css("width", _Width + "px");
    }

    if (prodCateCode == "PCC03") { //돌잔치
        //갤러리
        $("#area6").append("<img src=\"https://static.barunsoncard.com/barunsonmcard/invitation/preview/gallery_PCC03.png\"/>");
        $("#area6 img").css("width", _Width + "px");
        $("#area6 img").css("display", "block");

        //동영상
        $("#area8").append("<img src=\"https://static.barunsoncard.com/barunsonmcard/invitation/preview/video_PCC03.png\"/>");
        $("#area8 img").css("width", _Width + "px");
        $("#area8 img").css("display", "block");

        //방명록
        $("#area15").append("<img src=\"https://static.barunsoncard.com/barunsonmcard/invitation/preview/guest_book_PCC03.png\"/>");
        $("#area15 img").css("width", _Width + "px");
        $("#area15 img").css("display", "block");
        //방명록

        //송금하기
        $(".remittance").append("<img src=\"https://static.barunsoncard.com/barunsonmcard/invitation/preview/btn_remittance_PCC03.png\"/>");
        $(".remittance img").css("width", _Width + "px");
        $(".remittance img").css("display", "block");

        //오시는길
        $("#area10").append("<img src=\"https://static.barunsoncard.com/barunsonmcard/invitation/preview/img_05.png\"/>");
        $("#area10 img").css("width", _Width + "px");
        $("#area10 img").css("display", "block");

    } 

    //공유하기
    $("#area16").append("<img src=\"https://static.barunsoncard.com/barunsonmcard/invitation/preview/share.png\"/>");
    $("#area16").append("<img src=\"https://static.barunsoncard.com/barunsonmcard/invitation/preview/barunson.png\"/>");
    $("#area16 img").css("width", _Width + "px");
    $("#area16 img").css("display", "block");

    //area init
    $(".templatearea").hide();
    areas.forEach(function (elem) {

        $(".templatearea").each(function () {
            if ($(this).attr('idx') == elem.Area_ID && elem.Product_Category_Codes.includes(prodCateCode)) {

                if ($('#area' + elem.Area_ID).find('.item').length > 0) {
                    $('#area' + elem.Area_ID).css('height', elem.Size_Height * ratio + "px");
                    $('#area' + elem.Area_ID).css('background-color', elem.Color);
                }

                $(this).show();
            }
        });
    });


    //아기 정보 반복
    if (repeatData.Baby_Info_List != null) {
        var babyinfoItems = $.grep(objects, function (item, index) { return (item.pid == "area19") });
        var areaH = 0; //area height
        areas.forEach(function (elem) {
            if (elem.Area_ID == "19") {
                areaH = elem.Size_Height * ratio
            }
        });
        idx = 0;
        for (var babyProp in repeatData.Baby_Info_List) {
            let layerDiv = $("<div>")
                .css("position", "absolute")
                .css("top", areaH * idx + "px")
                .css("height", areaH + "px");

            $("#area19").append(layerDiv);

            babyinfoItems.forEach(function (elem) {
                var w = elem.width * ratio
                var h = elem.height * ratio
                var x = elem.left * ratio
                var y = elem.top * ratio

                if (elem.type == 'img') {
                    let divitem = $("<div>")
                        .addClass("item")
                        .css("top", y + "px")
                        .css("left", x + "px")
                        .css("position", "absolute")
                        .append($("<img>")
                            .addClass("img")
                            .attr("src", $("#CDN").val() + elem.resource_url)
                            .css("width", w + "px")
                            .css("height", h + "px")
                    );
                    layerDiv.append(divitem);
                } else {
                    var txtfontsize = elem.fontsize * ratio;
                    var text = "";
                    if (elem.chracterset == "#아기정보키#")
                        text = babyProp;
                    else if (elem.chracterset == "#아기정보값#")
                        text = repeatData.Baby_Info_List[babyProp];

                    let divitem = $("<div>")
                        .addClass("item")
                        .css("top", y + "px")
                        .css("left", x + "px")
                        .css("width", w + "px")
                        .css("height", h + "px")
                        .css("position", "absolute")
                        .css("background-color", elem.bgcolor);

                    if (elem.vertical_align == "T") {
                        divitem.css('align-items', "flex-start")
                    } else if (elem.vertical_align == "M") {
                        divitem.css('align-items', "center")
                    } else if (elem.vertical_align == "B") {
                        divitem.css('align-items', "flex-end");
                    } else {
                        divitem.css('align-items', "");
                    }

                    let divtext = $("<div>")
                        .addClass("text")
                        .css("font-family", elem.font)
                        .css("font-size", txtfontsize + "px")
                        .css("color", elem.fontcolor)
                        .css("font-weight", elem.bold_yn ? "bold" : "")
                        .css("font-style", elem.italic_yn ? "italic" : "")
                        .css("text-decoration'", elem.underline_yn ? "underline" : "")
                        .css('letter-spacing', elem.between_text / 100 + "em")
                        .css('line-height', elem.between_line + "em")
                        .text(text);
                    if (elem.horizontal_align == "C") {
                        divtext.css('text-align', "center")
                    } else if (elem.horizontal_align == "R") {
                        divtext.css('text-align', "right")
                    } else if (elem.horizontal_align == "L") {
                        divtext.css('text-align', "left");
                    } else {
                        divtext.css('text-align', "");
                    }

                    divitem.append(divtext);
                    layerDiv.append(divitem);

                }
            });

            idx++;

        }
        $("#area19").css('height', areaH * idx + "px");

    }

    $("dt").each(function () {
        $(this).css("font-size", ($(this).css("font-size").replace("px", "") * ratio) + "px");
    });
});
function matchText(text) {

    if (text != null) {
        var _matches = text.match(/#[^#]+#/g);

        if (_matches != null) {
            for (var i = 0; i < _matches.length; i++) {
                var target = _matches[i].replace(/#/g, '');
                var split = target.split(/\|/);
                var _append = $('[match="' + split[0] + '"]').val();
                if (split.length > 1) {
                    _append = '<span style="' + split[1] + '">' + _append + '</span>'
                }
                text = text.replace(_matches[i], _append);
            }
        }
        text = text.replace(/\r|\n|\r\n/g, "<br>");
    }
    return text;
}
$(window).load(function () {
    if (typeof $(".d_day").val() != 'undefined') {
        $(".d_day").css("font-size", (parseInt($(".d_day").css("font-size").replace("px", "")) * ratio) + "px");
    }
    $(".frame_wrap").show();
    $(".loader_main_preview").hide();
});
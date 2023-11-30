var objects = [];
var areas = [];
var etcs = [];
var babyInfos = [];
var lat = $("#lat").val();
var lng = $("#lot").val();
var loc = $("#loc").val();
var ratio = 1;
var _Width = 0;
var _min_height = 0;

$(document).ready(function () {

    $('.account_pop').hide();

    if ($(document).width() < 500) {
        if (typeof parent.$(".product_img .img_con").val() != 'undefined') {
            _Width = parent.$(".product_img .img_con").width();
        } else {
            _Width = $("#wrap").width();
        }

    } else {
        _Width = $("#wrap").width();
    }

    if ($("#Objects").text() != "") {
        objects = JSON.parse($("#Objects").text());
        var idx = 0;
        objects.forEach(function (elem) {
            ratio = _Width / 800;
            var w = elem.width * ratio
            var h = elem.height * ratio
            var x = elem.left * ratio
            var y = elem.top * ratio
            idx++;

            if (elem.type == 'img') {
                var div = "<div id='" + elem.id + "'  class='item' style='top: " + y + "px; left: " + x + "px;  position:absolute;'><img class='img' src='" + $("#CDN").val() + elem.resource_url + "' width='" + w + "px' height='" + h + "px'  /></div>";
                $('#' + elem.pid).append(div);
            }
            else if (elem.type == 'photo') {
                var div = "<div id='" + elem.id + "'  class='item photo' style='top: " + y + "px; left: " + x + "px;  width:" + w + "px; height:" + h + "px;  position:absolute;'><img class='img'  style='max-width:" + w + "px;max-height:" + h + "px;'></div>";
                $('#' + elem.pid).append(div);

                parent.$("#photo_width").val(w);
                parent.$("#photo_height").val(h);

                if ($("#Delegate_Image_URL").val() != "") {
                    $(".item.photo img").attr("src", $("#CDN").val() + $("#Delegate_Image_URL").val()).css("width", "100%");
                }
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
                    $(this).data("fontSize", parseInt($(this).css("font-size")));
                });
            }
        });

    }
    if ($("#ETCs").text() != "") {
        etcs = JSON.parse($("#ETCs").text());
        etcs.forEach(function (elem) {

            $("#etc_title_" + elem.Sort).html(elem.Etc_Title);
            $("#etc_contents_" + elem.Sort).html(elem.Information_Content.replace(/\n/g, '<br/>'));
            $("#etc_title_" + elem.Sort).parents('li').css("padding-bottom", "10px");
        });
    }



    //축의금 계좌정보
    var accounts = [];
    if ($("#Accounts").text() != "") {
        accounts = JSON.parse($("#Accounts").text());
        accounts.forEach(function (elm) {
            if (elm.Sort > 2) {
                var elem = "<dl id=\"dl_" + elm.Sort + "\">";
                elem = elem + "<dt>";
                elem = elem + "<span id=\"inputSender_" + elm.Sort + "\" class=\"input_sender\"></span>&nbsp;&nbsp;계좌";
                elem = elem + "<a href=\"javascript:;\" class=\"copy_btn\">복사하기</a>";
                elem = elem + "</dt>";
                elem = elem + "<dd><span id=\"inputBank_" + elm.Sort + "\" class=\"input_bank\"></span>&nbsp;&nbsp;(예금주 : <span id=\"inputAccountHolder_" + elm.Sort + "\" class=\"input_accountholder\" ></span>)</dd>";
                elem = elem + "<dd><span id=\"inputAccountNumber_" + elm.Sort + "\" class=\"input_accountnumber\"></span></dd>";
                elem = elem + "</dl>";
                $(".account_list.account").append(elem);
            }

            $("#inputSender_" + elm.Sort).text(elm.Send_Name);
            $("#inputBank_" + elm.Sort).text(elm.Bank_Name);
            $("#inputAccountHolder_" + elm.Sort).text(elm.Account_Holder);
            $("#inputAccountNumber_" + elm.Sort).text(elm.Account_Number);
        });

        if (accounts.length < 2) {
            $("#dl_2").hide();
        }
    }

    var groom_accounts = [];
    if ($("#GroomAccounts").text() != "") {
        groom_accounts = JSON.parse($("#GroomAccounts").text());

        groom_accounts.forEach(function (elm) {
            if (elm.Sort > 1) {
                var elem = "<dl id=\"dl_groom_" + elm.Sort + "\">";
                elem = elem + "<dt>";
                elem = elem + "<span id=\"inputGroomSender_" + elm.Sort + "\" class=\"input_groom_sender\"></span>&nbsp;&nbsp;계좌";
                elem = elem + "<a href=\"javascript:;\" class=\"copy_btn\">복사하기</a>";
                elem = elem + "</dt>";
                elem = elem + "<dd><span id=\"inputGroomBank_" + elm.Sort + "\" class=\"input_groom_bank\"></span>&nbsp;&nbsp;(예금주 : <span id=\"inputGroomAccountHolder_" + elm.Sort + "\" class=\"input_groom_accountholder\" ></span>)</dd>";
                elem = elem + "<dd><span id=\"inputGroomAccountNumber_" + elm.Sort + "\" class=\"input_groom_accountnumber\"></span></dd>";
                elem = elem + "</dl>";
                $(".account_list.groom").append(elem);
            }

            $("#inputGroomSender_" + elm.Sort).text(elm.Send_Name);
            $("#inputGroomBank_" + elm.Sort).text(elm.Bank_Name);
            $("#inputGroomAccountHolder_" + elm.Sort).text(elm.Account_Holder);
            $("#inputGroomAccountNumber_" + elm.Sort).text(elm.Account_Number);
        });
    }

    var bride_accounts = [];
    if ($("#BrideAccounts").text() != "") {
        bride_accounts = JSON.parse($("#BrideAccounts").text());
        bride_accounts.forEach(function (elm) {
            if (elm.Sort > 1) {
                var elem = "<dl id=\"dl_bride_" + elm.Sort + "\">";
                elem = elem + "<dt>";
                elem = elem + "<span id=\"inputBrideSender_" + elm.Sort + "\" class=\"input_bride_sender\"></span>&nbsp;&nbsp;계좌";
                elem = elem + "<a href=\"javascript:;\" class=\"copy_btn\">복사하기</a>";
                elem = elem + "</dt>";
                elem = elem + "<dd><span id=\"inputBrideBank_" + elm.Sort + "\" class=\"input_bride_bank\"></span>&nbsp;&nbsp;(예금주 : <span id=\"inputBrideAccountHolder_" + elm.Sort + "\" class=\"input_bride_accountholder\" ></span>)</dd>";
                elem = elem + "<dd><span id=\"inputBrideAccountNumber_" + elm.Sort + "\" class=\"input_bride_accountnumber\"></span></dd>";
                elem = elem + "</dl>";
                $(".account_list.bride").append(elem);
            }

            $("#inputBrideSender_" + elm.Sort).text(elm.Send_Name);
            $("#inputBrideBank_" + elm.Sort).text(elm.Bank_Name);
            $("#inputBrideAccountHolder_" + elm.Sort).text(elm.Account_Holder);
            $("#inputBrideAccountNumber_" + elm.Sort).text(elm.Account_Number);
        });
    }


    if ($("#TB_Area").text() != "") {
        areas = JSON.parse($("#TB_Area").text());
        areas.forEach(function (elem) {
            $(".templatearea").each(function () {
                if ($(this).attr('idx') == elem.Area_ID) {
                    //$("div[idx='" + elem.Area_ID + "']").css("display", "block");

                    if ($(this).hasClass('templatearea')) {

                        var ratio = _Width / 800;

                        if ($('#area' + elem.Area_ID).find('.item').length > 0) {
                            if (elem.Area_ID != 2) {
                                $('#area' + elem.Area_ID).css('height', elem.Size_Height * ratio + "px");
                            } else {
                                //$('#area' + elem.Area_ID).css('height', elem.Size_Height * ratio + "px");
                                var divarea = $('#area2');
                                var areaHeight = $(divarea).height();
                                var ratio = _Width / 800;
                                var totheight = 30.0;
                                var imgheight = $(divarea).find('.img').outerHeight(true) + totheight;
                                var txtheight = $(divarea).find('.text').outerHeight(true);

                                $(divarea).css('height', (imgheight + txtheight) + "px");
                                $(divarea).find('.text').parent().css('height', (txtheight) + "px");
                            }
                            $('#area' + elem.Area_ID).css('background-color', elem.Color);
                        }

                    }
                }
            });
        });
    }
    //화환 영역
    $("div.flowergift .templatearea").css("height", 224 * _Width / 800 + "px");
    $("div.flowergift .templatearea img").css("height", 224 * _Width / 800 + "px");
    $("div.flowergift .templatearea img").css("width", 800 * _Width / 800 + "px");
    if ($("#Step").val() == "2") {
        $(document).scrollTop($('.onoff_5').offset().top + 1);
    }

    //신랑신부정보
    if ($("#Groom_Phone").val() != "") {
        var rt = $("#Groom_Global_Phone_YN").val() == "Y" ? true : false;
        $(".at_Groom_Phone").attr("href", "tel:" + (rt ? $("#Groom_Global_Phone_Number").val() : "") + $("#Groom_Phone").val());
        $(".as_Groom_Phone").attr("href", "sms:" + (rt ? $("#Groom_Global_Phone_Number").val() : "") + $("#Groom_Phone").val());
    }
    if ($("#Groom_Phone").val() != "") {
        var rt = $("#Bride_Global_Phone_YN").val() == "Y" ? true : false;
        $(".at_Bride_Phone").attr("href", "tel:" + (rt ? $("#Bride_Global_Phone_Number").val() : "") + $("#Bride_Phone").val());
        $(".as_Bride_Phone").attr("href", "sms:" + (rt ? $("#Bride_Global_Phone_Number").val() : "") + $("#Bride_Phone").val());
    }

    //혼주정보
    if ($("#Parents_Information_Use_YN").val() == "N") {
        $(".onoff_2").hide();
    } else {
        $(".onoff_2").show();
    }

    if ($("#Groom_Parents1_Phone").val() != "") {
        $(".at_Groom_Parents1_Phone").show();
        $(".as_Groom_Parents1_Phone").show();
        var rt = $("#Groom_Parents1_Global_Phone_Number_YN").val() == "Y" ? true : false;
        $(".at_Groom_Parents1_Phone").attr("href", "tel:" + (rt ? $("#Groom_Parents1_Global_Phone_Number").val() : "") + $("#Groom_Parents1_Phone").val());
        $(".as_Groom_Parents1_Phone").attr("href", "sms:" + (rt ? $("#Groom_Parents1_Global_Phone_Number").val() : "") + $("#Groom_Parents1_Phone").val());
    }
    if ($("#Groom_Parents2_Phone").val() != "") {
        $(".at_Groom_Parents2_Phone").show();
        $(".as_Groom_Parents2_Phone").show();
        var rt = $("#Groom_Parents2_Global_Phone_Number_YN").val() == "Y" ? true : false;
        $(".at_Groom_Parents2_Phone").attr("href", "tel:" + (rt ? $("#Groom_Parents2_Global_Phone_Number").val() : "") + $("#Groom_Parents2_Phone").val());
        $(".as_Groom_Parents2_Phone").attr("href", "sms:" + (rt ? $("#Groom_Parents2_Global_Phone_Number").val() : "") + $("#Groom_Parents2_Phone").val());
    }
    if ($("#Bride_Parents1_Phone").val() != "") {
        $(".at_Bride_Parents1_Phone").show();
        $(".as_Bride_Parents1_Phone").show();
        var rt = $("#Bride_Parents1_Global_Phone_Number_YN").val() == "Y" ? true : false;
        $(".at_Bride_Parents1_Phone").attr("href", "tel:" + (rt ? $("#Bride_Parents1_Global_Phone_Number").val() : "") + $("#Bride_Parents1_Phone").val());
        $(".as_Bride_Parents1_Phone").attr("href", "sms:" + (rt ? $("#Bride_Parents1_Global_Phone_Number").val() : "") + $("#Bride_Parents1_Phone").val());
    }
    if ($("#Bride_Parents2_Phone").val() != "") {
        $(".at_Bride_Parents2_Phone").show();
        $(".as_Bride_Parents2_Phone").show();
        var rt = $("#Bride_Parents2_Global_Phone_Number_YN").val() == "Y" ? true : false;
        $(".at_Bride_Parents2_Phone").attr("href", "tel:" + (rt ? $("#Bride_Parents2_Global_Phone_Number").val() : "") + $("#Bride_Parents2_Phone").val());
        $(".as_Bride_Parents2_Phone").attr("href", "sms:" + (rt ? $("#Bride_Parents2_Global_Phone_Number").val() : "") + $("#Bride_Parents2_Phone").val());
    }
    //오시는길
    if ($("#Outline_Type_Code").val() == "OTC02") {

        if ($("#Outline_Image_URL").val() != "") {
            $("#Outline_Image").attr('src', $("#CDN").val() + $("#Outline_Image_URL").val()).css("max-width", "360px");
        }
        $(".map_wrap").hide();
        $(".input_imgmap").show();
        $(".area .btn_wrap").hide()

    }
    //갤러리
    if ($("#Gallery_Use_YN").val() == "N") {
        $(".onoff_5").hide();
    } else {
        $(".onoff_5").show();

        setImageType($("#Gallery_Type_Code").val());
    }
    //동영상
    if ($("#Invitation_Video_Use_YN").val() == "N") {
        $(".onoff_6").hide();
    } else {
        $(".onoff_6").show();

        if ($("#Step").val() == "0") {
            $(".iframe_wrap.preview").show();
            $(".iframe_wrap.embed").hide();
        } else {
            $(".iframe_wrap.preview").hide();
            $(".iframe_wrap.embed").show();
        }

        setVideo($("#Invitation_Video_Type_Code").val());
    }

    //기타정보
    if ($("#Etc_Information_Use_YN").val() == "N") {
        $(".onoff_3").hide();
    } else {
        $(".onoff_3").show();
    }

    //방명록
    if ($("#GuestBook_Use_YN").val() == "N") {
        $(".onoff_1").hide();
    } else {
        $(".onoff_1").show();
    }
    //축의금
    if ($("#MoneyGift_Remit_Use_YN").val() == "N" && $("#MoneyAccount_Remit_Use_YN").val() == "N" &&
        $("#MoneyAccount_Div_Use_YN").val() == "N") {
        $(".onoff_4").hide();
    } else {
        //비회원
        if ($("#User_ID").val() == "") {
            $(".remittance_btn").hide();

            if ($("#MoneyAccount_Remit_Use_YN").val() == "Y") {
                $(".an_btn.account").show();
            } else {
                $(".an_btn.account").hide();
            }

            if ($("#MoneyAccount_Div_Use_YN").val() == "Y" && groom_accounts.length > 0) {
                $(".an_btn.groom").show();
            } else {
                $(".an_btn.groom").hide();
            }

            if ($("#MoneyAccount_Div_Use_YN").val() == "Y" && bride_accounts.length > 0) {
                $(".an_btn.bride").show();
            } else {
                $(".an_btn.bride").hide();
            }

        } else {
            $(".onoff_4").show();

            if ($("#MoneyGift_Remit_Use_YN").val() == "Y") {
                $(".remittance_btn").show();
            } else {
                $(".remittance_btn").hide();
            }

            if ($("#MoneyAccount_Remit_Use_YN").val() == "Y") {
                $(".an_btn.account").show();
            } else {
                $(".an_btn.account").hide();
            }

            if ($("#MoneyAccount_Div_Use_YN").val() == "Y" && groom_accounts.length > 0) {
                $(".an_btn.groom").show();
            } else {
                $(".an_btn.groom").hide();
            }

            if ($("#MoneyAccount_Div_Use_YN").val() == "Y" && bride_accounts.length > 0) {
                $(".an_btn.bride").show();
            } else {
                $(".an_btn.bride").hide();
            }
        }
    }
    //화환 선물
    if ($("#Flower_gift_YN").val() == "Y") {
        $("div.flowergift").show();
    } else {
        $("div.flowergift").hide();
    }

    var w = parseInt($("#area1").css('width')) - 20
    $("#area11").css("width", w + "px");

    //아기정보 표시
    if ($("#BabyInfos").text() != null && $("#BabyInfos").text() != "" ) {
        var b = JSON.parse($("#BabyInfos").text());
        fn_RenderBabyInfo(b);
    }

    if ($("#Product_Category_Code").val() == "PCC01") {
        //초대장
        $(".onoff_7").hide();
    } else if ($("#Product_Category_Code").val() == "PCC02") {
        //감사장
        $(".onoff_2").hide();
        $(".onoff_5").hide();
        $(".onoff_6").hide();
        $("#area9").hide();
        $("#area10").hide();
        $(".onoff_3").hide();
        $(".onoff_4").hide();
        $(".onoff_1").hide();
        $(".onoff_7").hide();
    } else if ($("#Product_Category_Code").val() == "PCC03") {
        //돌잔치
        $("#area4").hide();
    }

    $('body').on('keyup', '.matchinfo', function () {
        changeText(this);
    });
    //신랑 & 신부측 혼주 보기
    $('.list_con').slideDown();
    $('.info_detail').on('click', function () {
        $(this).parent('.list_wrap').toggleClass('on');
        $('.list_con').slideToggle();
    });

    // 내비게이션 (0:네이버/1:카카오)
    $(".map_btn").click(function () {
        var navurl = "";

        // 웹브라우저 내에서 네이버지도로 이동
        navurl = 'http://map.naver.com/index.nhn?enc=utf8&level=2&lng=' + lng + '&lat=' + lat + '&pinTitle=' + encodeURIComponent(loc) + '&pinType=SITE';
        var f = window.open(navurl);

        return false;
    });


    $("span").each(function () {
        $(this).css("font-size", $(this).css("font-size").replace("px", "") * 0.88 + "px");


        if ($(this).attr('class') == "i_WeddingHallDetail" && $("#DetailNewLineYN").val() == "Y") {
            $(this).prepend("</br>");
        }

        if ($(this).attr('class') == "i_Weddinghall_Name" || $(this).attr('class') == "i_WeddingHallDetail") {
            $(this).css("font-size", "14.2px");
        }
    });
    $("strong").each(function () {
        $(this).css("font-size", $(this).css("font-size").replace("px", "") * 0.88 + "px");
    });
    $("p").each(function () {
        $(this).css("font-size", $(this).css("font-size").replace("px", "") * 0.88 + "px");
    });
    $("a").each(function () {
        $(this).css("font-size", $(this).css("font-size").replace("px", "") * 0.88 + "px");
    });
    $("dt").each(function () {
        $(this).css("font-size", $(this).css("font-size").replace("px", "") * 0.88 + "px");
    });
    $("h3").each(function () {
        $(this).css("font-size", $(this).css("font-size").replace("px", "") * 0.88 + "px");
    });
    $("#area15 input").each(function () {
        $(this).css("font-size", $(this).css("font-size").replace("px", "") * 0.88 + "px");
    });
    $(".remittance_btn").css("background-size", "52.8px");
    $("img").each(function () {
        if ($(this).attr("alt") == "전화 아이콘" || $(this).attr("alt") == "SMS 아이콘" || $(this).attr("alt") == "바른손Mcard" || $(this).attr("alt") == "페이스북" || $(this).attr("alt") == "카카오톡" || $(this).attr("alt") == "링크복사" || $(this).attr("alt") == "예식장 전화하기") {
            $(this).css("width", $(this).width() * 0.88 + "px");
        }
    });
    $(".sns_list").css("height", $(".sns_list").height() * 0.88 + "px");


    $('.an_btn.groom').on('click', function () {
        fn_AccountClose();
        $('.account_pop.groom').show();
    });

    $('.btn.an_btn.bride').on('click', function () {
        fn_AccountClose();
        $('.account_pop.bride').show();
    });

    $('.btn.an_btn.account').on('click', function () {
        fn_AccountClose();
        $('.account_pop.account').show();
    });

    $('.btn.close').on('click', function () {
        fn_AccountClose();
    });
});
function setImageType(id) {
    $(".gallery_type01").hide();
    $(".gallery_type02").hide();
    $(".gallery_type03").hide();

    switch (id) {
        case "GTC01"://슬라이드
            $(".gallery_type03").show();
            fn_type03();
            break;
        case "GTC02": //바둑판
            $(".gallery_type01").show();
            break;
        case "GTC03"://리스트
            $(".gallery_type02").show();
            break;
        default:
            $(".gallery_type01").show();
            break;
    }
}
function changeText(target) {

    var val = $(target).attr('value');
    var n = $(target).attr('idx');
    var text = matchText($(target).val());

    $('#item_' + n + ' .text').html(text);

  
    if (val == "#인사말#") {
        var divarea = $('#area2');
        var areaHeight = $(divarea).height();
        var ratio = _Width / 800;
        var totheight = 30.0;
        var imgheight = $(divarea).find('.img').outerHeight(true) + totheight;
        var txtheight = $(divarea).find('.text').outerHeight(true);

        $(divarea).css('height', (imgheight + txtheight) + "px");
        $(divarea).find('.text').parent().css('height', (txtheight) + "px");
    } 
    if (val == "#D-DAY#") {
        var eventdate = $("#WeddingDate", parent.document).val();
        var dday = getDday(eventdate);
        $('#item_' + n + ' .text').html(dday);
    }
};
function fn_match(target, txt) {
    $("#" + target).val(txt);

    if (txt == "입력안함") {
        txt = "";
    }

    if ($("span.i_" + target).length > 0) {
        $("span.i_" + target).text(txt);
    }

    $('.matchinfo').trigger('keyup');
}

function fn_match_extra2(target, txt) {
    $("#" + target).text(txt);
    var id = target.split('_')[1];
    var str = $("#inputSender_" + id).text() + $("#inputBank_" + id).text() + $("#inputAccountNumber_" + id).text() + $("#inputAccountHolder_" + id).text();
    if (str == "") {
        $("#dl_" + id).hide();
    } else {
        $("#dl_" + id).show();
    }
}

function fn_match_groom(target, txt) {
    $("#" + target).text(txt);
    var id = target.split('_')[1];
    var str = $("#inputGroomSender_" + id).text() + $("#inputGroomBank_" + id).text() + $("#inputGroomAccountNumber_" + id).text() + $("#inputGroomAccountHolder_" + id).text();
    if (str == "") {
        $("#dl_groom_" + id).hide();
    } else {
        $("#dl_groom_" + id).show();
    }
}

function fn_match_bride(target, txt) {
    $("#" + target).text(txt);
    var id = target.split('_')[1];
    var str = $("#inputBrideSender_" + id).text() + $("#inputBrideBank_" + id).text() + $("#inputBrideAccountNumber_" + id).text() + $("#inputBrideAccountHolder_" + id).text();
    if (str == "") {
        $("#dl_bride_" + id).hide();
    } else {
        $("#dl_bride_" + id).show();
    }
}

function fn_newline(chk) {
    if (chk) {
        $(".i_WeddingHallDetail").before("</br>");
    } else {
        $(".i_WeddingHallDetail").parent().find('br').remove();
    }
}

function fn_match_extra1(target1, txt1, target2, txt2, flag) {
    if (flag) {
        if (txt1 + txt2 != "") {
            $("a.at_" + target2).show();
            $("a.as_" + target2).show();
        } else {
            $("a.at_" + target2).hide();
            $("a.as_" + target2).hide();
        }
        $("a.at_" + target2).prop("href", "tel:" + txt1 + txt2)
        $("a.as_" + target2).prop("href", "sms:" + txt1 + txt2)
    } else {
        if (txt2 != "") {
            $("a.at_" + target2).show();
            $("a.as_" + target2).show();
        } else {
            $("a.at_" + target2).hide();
            $("a.as_" + target2).hide();
        }
        $("a.at_" + target2).prop("href", "tel:" + txt2)
        $("a.as_" + target2).prop("href", "sms:" + txt2)
    }
}
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
function fn_type03() {
    //갤러리 타입03 - 썸네일 슬라이드
    var galleryThumbs = new Swiper('.gallery-thumbs', {
        spaceBetween: 10,
        slidesPerView: 3,
        loop: true,
        freeMode: true,
        loopedSlides: 5, //looped slides should be the same
        watchSlidesVisibility: true,
        watchSlidesProgress: true,
    });

    var galleryTop = new Swiper('.gallery-top', {
        spaceBetween: 10,
        loop: true,
        loopedSlides: 5, //looped slides should be the same
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        },
        thumbs: {
            swiper: galleryThumbs,
        },
    });
}
function setVideo(id) {
    switch (id) {
        case "VTC01"://Youtube
            var htmlInput = $("#Invitation_Video_URL").val();
            var pattern = /(?:http?s?:\/\/)?(?:www\.)?(?:youtube\.com|youtu\.be)\/(?:watch\?v=)?(.+)/g;

            if (pattern.test(htmlInput)) {
                var replacement = '<iframe src="//www.youtube.com/embed/$1" frameborder="0" class="embed-container" allowfullscreen></iframe>';


                htmlInput = htmlInput.replace(pattern, replacement);
            }

            $(".iframe_wrap.embed").html(htmlInput);
            $(".iframe_wrap.preview").html(htmlInput);

            break;
        case "VTC02": //Vimeo
            $(".iframe_wrap.embed").html($("#Invitation_Video_URL").val());
            $(".iframe_wrap.preview").html($("#Invitation_Video_URL").val());
            break;
        case "VTC03"://FEELMAKER
            $(".iframe_wrap.embed").html($("#Invitation_Video_URL").val());
            $(".iframe_wrap.preview").html($("#Invitation_Video_URL").val());
            break;

    }

}
function uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
function getDday(wedding_date) {
    var returnval = "";
    var today = new Date();
    var wedd_day = new Date(wedding_date);
    var distance = wedd_day - today
    var dday = Math.ceil(distance / (1000 * 60 * 60 * 24));
    if (parseInt(dday) > 0) {
        //예식일이전
        returnval = "D-" + parseInt(dday);
    } else if (parseInt(dday) == 0) {
        returnval = "D-Day";
        //예식일당일
    } 
    return returnval;
}
function setDday(wedding_date) {
    if (typeof $(".d_day").val() != 'undefined') {
        var today = new Date();
        var wedd_day = new Date(wedding_date);
        var distance = wedd_day - today
        var dday = Math.ceil(distance / (1000 * 60 * 60 * 24));
        if (parseInt(dday) > 0) {
            //예식일이전
            $(".d_day").text("D-" + parseInt(dday));
            $(".d_day").show();
        } else if (parseInt(dday) == 0) {
            $(".d_day").text("D-Day");
            $(".d_day").show();
            //예식일당일
        } else {
            //예식일이후
            $(".d_day").hide();
        }
    }
}

function fn_showBtn(btn1, btn2) {
    if (btn1) {
        $(".remittance_btn").show();
    } else {
        $(".remittance_btn").hide();
    }
    if (btn2) {
        $(".an_btn").show();
    } else {
        $(".an_btn").hide();
    }
}


function fn_InitETC() {
    var id =1;
    $(".etc_info").find('li').each(function () {
        $(this).find("strong").html("");
        $(this).find("span").html("");
        id++;
    });
}

function fn_AccountBtn(isGroom, isBride, isAccount, isGift) {

    if (isGroom) {
        $('.an_btn.groom').show();
    } else {
        $('.an_btn.groom').hide();
    }
    if (isBride) {
        $('.an_btn.bride').show();
    } else {
        $('.an_btn.bride').hide();
    }
    if (isAccount) {
        $(".an_btn.account").show();
    } else {
        $(".an_btn.account").hide();
    }
    if (isGift) {
        $(".remittance_btn").show();
    } else {
        $(".remittance_btn").hide();
    }
    
}
function fn_FlowerGiftBtn(isFlower) {
    if (isFlower) {
        $("div.flowergift").show();
    } else {
        $("div.flowergift").hide();
    }
}

function fn_AccountClose() {
    $('.account_pop.groom').hide();
    $('.account_pop.bride').hide();
    $('.account_pop.account').hide();
}


function fn_AccountPop_ScrollBottom() {
    $('.account_list').scrollTop($('.layer_pop').height());
}

function fn_AccountPop_Scroll(id) {
    var ids = id.split('_');
    var sort = ids[1];
    var idx = ids[1] - 1;
    var ht = $('#dl_1').height() * idx;

    var str = $("#inputSender_" + sort).text() + $("#inputBank_" + sort).text() + $("#inputAccountNumber_" + sort).text() + $("#inputAccountHolder_" + sort).text();
    if (str == "") {
        $("#dl_" + sort).hide();
    } else {
        $("#dl_" + sort).show();
    }

    $('.account_list').scrollTop(ht);
}



function fn_AccountAdd() {
    var sort = $(".account_list").find('dl').length + 1;
    var elem = "<dl id=\"dl_" + sort + "\">";
    elem = elem + "<dt>";
    elem = elem + "<span id=\"inputSender_" + sort + "\" class=\"input_sender\"></span>&nbsp;&nbsp;계좌";
    elem = elem + "<a href=\"javascript:;\" class=\"copy_btn\">복사하기</a>";
    elem = elem + "</dt>";
    elem = elem + "<dd><span id=\"inputBank_" + sort + "\" class=\"input_bank\"></span>&nbsp;&nbsp;(예금주 : <span id=\"inputAccountHolder_" + sort + "\" class=\"input_accountholder\" ></span>)</dd>";
    elem = elem + "<dd><span id=\"inputAccountNumber_" + sort + "\" class=\"input_accountnumber\"></span></dd>";
    elem = elem + "</dl>";
    $(".account_list").append(elem);
}

function fn_GroomAccountAdd() {
    var sort = $(".account_list.groom").find('dl').length + 1;
    var elem = "<dl id=\"dl_groom_" + sort + "\">";
    elem = elem + "<dt>";
    elem = elem + "<span id=\"inputGroomSender_" + sort + "\" class=\"input_groom_sender\"></span>&nbsp;&nbsp;계좌";
    elem = elem + "<a href=\"javascript:;\" class=\"copy_btn\">복사하기</a>";
    elem = elem + "</dt>";
    elem = elem + "<dd><span id=\"inputGroomBank_" + sort + "\" class=\"input_groom_bank\"></span>&nbsp;&nbsp;(예금주 : <span id=\"inputGroomAccountHolder_" + sort + "\" class=\"input_groom_accountholder\" ></span>)</dd>";
    elem = elem + "<dd><span id=\"inputGroomAccountNumber_" + sort + "\" class=\"input_groom_accountnumber\"></span></dd>";
    elem = elem + "</dl>";
    $(".account_list.groom").append(elem);
}

function fn_BrideAccountAdd() {
    var sort = $(".account_list.bride").find('dl').length + 1;
    var elem = "<dl id=\"dl_bride_" + sort + "\">";
    elem = elem + "<dt>";
    elem = elem + "<span id=\"inputBrideSender_" + sort + "\" class=\"input_bride_sender\"></span>&nbsp;&nbsp;계좌";
    elem = elem + "<a href=\"javascript:;\" class=\"copy_btn\">복사하기</a>";
    elem = elem + "</dt>";
    elem = elem + "<dd><span id=\"inputBrideBank_" + sort + "\" class=\"input_bride_bank\"></span>&nbsp;&nbsp;(예금주 : <span id=\"inputBrideAccountHolder_" + sort + "\" class=\"input_bride_accountholder\" ></span>)</dd>";
    elem = elem + "<dd><span id=\"inputBrideAccountNumber_" + sort + "\" class=\"input_bride_accountnumber\"></span></dd>";
    elem = elem + "</dl>";
    $(".account_list.bride").append(elem);
}

function fn_AccountDel(sort) {

    $("#dl_" + sort).remove();

    var idx = 1;
    $(".account_list").find('dl').each(function () {
        $(this).attr("id", "dl_" + idx);
        $(this).find(".input_sender").attr("id", "inputSender_" + idx);
        $(this).find(".input_bank").attr("id", "inputBank_" + idx);
        $(this).find(".input_accountnumber").attr("id", "inputAccountNumber_" + idx);
        $(this).find(".input_accountholder").attr("id", "inputAccountHolder_" + idx);
        idx++;
    });
}

function fn_GroomAccountDel(sort) {

    $("#dl_groom_" + sort).remove();

    var idx = 1;
    $(".account_list.groom").find('dl').each(function () {
        $(this).attr("id", "dl_groom_" + idx);
        $(this).find(".input_groom_sender").attr("id", "inputGroomSender_" + idx);
        $(this).find(".input_groom_bank").attr("id", "inputGroomBank_" + idx);
        $(this).find(".input_groom_accountnumber").attr("id", "inputGroomAccountNumber_" + idx);
        $(this).find(".input_groom_accountholder").attr("id", "inputGroomAccountHolder_" + idx);
        idx++;
    });
}

function fn_BrideAccountDel(sort) {
    $("#dl_bride_" + sort).remove();

    var idx = 1;
    $(".account_list.bride").find('dl').each(function () {
        $(this).attr("id", "dl_bride_" + idx);
        $(this).find(".input_bride_sender").attr("id", "inputBrideSender_" + idx);
        $(this).find(".input_bride_bank").attr("id", "inputBrideBank_" + idx);
        $(this).find(".input_bride_accountnumber").attr("id", "inputBrideAccountNumber_" + idx);
        $(this).find(".input_bride_accountholder").attr("id", "inputBrideAccountHolder_" + idx);
        idx++;
    });
}
function accOpen() {
    $('.account_pop').show();
}
function accClose() {
    $('.account_pop').hide();
}
//아기정보 변경
function fn_RenderBabyInfo(binfo) {
    babyInfos = binfo;
    var divRoot = $("div.baby_area");
    divRoot.empty();
    babyInfos.sort((a, b) => a.idx - b.idx);

    var babyAreas = [17, 18, 19, 20];

    babyInfos.forEach(function (elem) {
        var divInfo = $("<div />");
        
        babyAreas.forEach(function (areaId) {
            var areaItem = $.grep(areas, function (item, index) { return (item.Area_ID == areaId) })[0];

            var atemplate = $.grep(objects, function (item, index) { return (item.pid == "area" + areaId) });
            atemplate.sort((a, b) => a.zindex - b.zindex);

            //아기정보 본문
            if (areaId == 19) {
                $.each(elem.ExtraInfos, function (index, extentItem) {
                    var areadiv = $("<div />").attr("idx", areaId).addClass("templatearea");
                    fn_BindBabyInfo(elem, atemplate, areadiv, extentItem);;
                    
                    divInfo.append(areadiv);
                    
                });

            } else {
                var areadiv = $("<div />").attr("idx", areaId).addClass("templatearea");
                fn_BindBabyInfo(elem, atemplate, areadiv, null);
                areadiv.css('height', areaItem.Size_Height * _Width / 800 + "px");
                divInfo.append(areadiv);
            }

        });

        divRoot.append(divInfo);

        $.each(divRoot.find("div.templatearea"), function (idx, areadiv) {
            if ($(areadiv).attr("idx") == "19") {
                var totheight = 15.0;
                $.each($(areadiv).find("div.text"), function (idx, txtItem) {
                    var curHeight = $(txtItem).outerHeight(true);
                    totheight += curHeight;
                    $(txtItem).parent().css("height", curHeight + "px")
                });

                $(areadiv).find("img.img").height(totheight + "px");
                $(areadiv).css('height', totheight + "px")

            } else {
                $(areadiv).find('.text', '.item').each(function () {
                    $(this).data("height", $(this).outerHeight());
                    $(this).data("fontSize", parseInt($(this).css("font-size")));
                });
            }
        });

    });
}
function fn_BindBabyInfo(item, template, tareget, extentItem) {

    template.forEach(function (elem) {
        ratio = _Width / 800;
        var w = elem.width * ratio
        var h = elem.height * ratio
        var x = elem.left * ratio
        var y = elem.top * ratio
        
        if (elem.type == 'img') {
            var div = "<div class='item' style='top: " + y + "px; left: " + x + "px;  position:absolute;'><img class='img' src='" + $("#CDN").val() + elem.resource_url + "' width='" + w + "px' height='" + h + "px'  /></div>";
            tareget.append(div);
        } else if (elem.type == 'profile') {
            if (item.Image_Width > 0)
                w = item.Image_Width * ratio;
            if (item.Image_Height > 0)
                h = item.Image_Height * ratio;

            var div = "<div class='item profile-image' style='top: " + y + "px; left: " + x + "px;  position:absolute;'><img class='img' src='" + $("#CDN").val() + item.Image_URL + "' width='" + w + "px' height='" + h + "px'  /></div>";
            tareget.append(div);

        } else {
            var birthDay = new Date(item.Birthday);
            var text = elem.chracterset;
            //아기정보 match
            var _matches = text.match(/#[^#]+#/g);
            if (_matches != null) {
                for (var i = 0; i < _matches.length; i++) {
                    switch (_matches[i]) {
                        case "#아기이름#":
                            text = text.replace(_matches[i], item.Name);
                            break;
                        case "#탄생일자#":
                            var d = birthDay.getDate();
                            var m = birthDay.getMonth() + 1; //Month from 0 to 11
                            var y = birthDay.getFullYear().toString().substr(-2);
                            var datestr = '' + y + '.' + (m <= 9 ? '0' + m : m) + '.' + (d <= 9 ? '0' + d : d);
                            text = text.replace(_matches[i], datestr);
                            break;
                        case "#탄생년#":
                            text = text.replace(_matches[i], birthDay.getFullYear());
                            break;
                        case "#탄생월#":
                            var mm = birthDay.getMonth() + 1; // getMonth() is zero-based
                            text = text.replace(_matches[i], (mm > 9 ? '' : '0') + mm);
                            break;
                        case "#탄생일#":
                            var dd = birthDay.getDate();
                            text = text.replace(_matches[i], (dd > 9 ? '' : '0') + dd);
                            break;
                        case "#아기정보키#":
                            text = text.replace(_matches[i], extentItem.Title);
                            break;
                        case "#아기정보값#":
                            text = text.replace(_matches[i], extentItem.Value);
                            break;

                    }
                }
                //기본 match
                text = matchText(text);
                var div = $("<div />").addClass("item").css("top", y + "px").css("left", x + "px").css("position", "absolute").css("width", w + "px").css("height", h + "px").css('background-color', elem.bgcolor);

                var txtfontsize = elem.fontsize * ratio;
                var textdiv = $("<div />").addClass("text").css('font-family', elem.font).css('font-size', txtfontsize + "px").css('color', elem.fontcolor)
                    .css('font-weight', elem.bold_yn ? "bold" : "").css('font-style', elem.italic_yn ? "italic" : "").css('text-decoration', elem.underline_yn ? "underline" : "")
                    .css('letter-spacing', elem.between_text / 100 + "em").css('line-height', elem.between_line + "em")
                    .html(text);

                if (elem.horizontal_align == "C") {
                    textdiv.css('text-align', "center")
                } else if (elem.horizontal_align == "R") {
                    textdiv.css('text-align', "right")
                } else if (elem.horizontal_align == "L") {
                    textdiv.css('text-align', "left");
                }

                if (elem.vertical_align == "T") {
                    div.css('align-items', "flex-start");
                } else if (elem.vertical_align == "M") {
                    div.css('align-items', "center");
                } else if (elem.vertical_align == "B") {
                    div.css('align-items', "flex-end");
                } 

                div.append(textdiv);
                tareget.append(div);

            }
                       
        }

    });
   
}

$(window).load(function () {
    if (typeof $(".d_day").val() != 'undefined') {
        $(".d_day").css("font-size", (parseInt($(".d_day").css("font-size").replace("px", "")) * ratio) + "px");
        $(".d_day").show();
    }

    setDday($("#WeddingDate").val());
    $("#area1").focus();
    $(".loader_preview").hide();

});




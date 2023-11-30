var image_select = "";
$(document).ready(function () {

    //비회원
    if ($("#User_ID").val() == "") {
        $('.kakao_text').text("비회원은 '축의금 송금 서비스'를 이용하실 수 없습니다.");
        $('.money_gift').find('.btn_wrap').hide();
    } 

    //이메일주소
    $("#selEmail option").each(function () {
        if ($(this).val() == $("#Email_Address").val()) {
            $(this).prop('selected', true);
            $("#Email_Address").hide();
        }
    });
    $("#selEmail").on('change', function () {
        if ($("#selEmail option:selected").index() > 0) {
            $("#Email_Address").val($("#selEmail option:selected").val());
            $("#Email_Address").hide();
        } else {
    $("#Email_Address").val("");
            $("#Email_Address").show();
        }
    });
    //공유이미지
    if ($(".sns_img").attr('src') != "" && $(".sns_img").attr('src') != null) {
    $(".sns_img").parents(".img_preview").show();
        $("#sns_img").find('label').hide();
    }
    //대표 이미지
    if ($("#Photo_YN").val() == "Y") {
    $('.isPhotoYn').show();
        if ($("#Delegate_Image_URL").val() != "") {
    $("#main_img .img_preview").show();
        }
    } else {
    $('.isPhotoYn').hide();
    }
    //약도입력
    if ($("#Outline_Type_Code").val() == "OTC02") {
    $("#Outline_Image").attr('src', $("#Outline_Image_URL").val()).css("max-width", "100%");
        $(".mark").focus();
        $(".map_box").show();
        $(".input_imgmap").show();
        $(".input_navermap").hide();
        $("#map_tip").hide();
        $("#outlinemap").prop("checked", true)

    }
    //기타정보
    var etcs = [];
    if ($("#ETCs").text() != "") {
        etcs = JSON.parse($("#ETCs").text());
        etcs.forEach(function (elem) {
            if (elem.Sort > 2) {
                var tr_elem = "<tr><th><div style=\"position:relative;\"><input type=\"text\" id=\"etc_title_" + elem.Sort + "\" class=\"inp wp100 etc_title\" placeholder=\"직접 입력\"></div></th>";
                tr_elem += "<td><div class=\"fl_wrap btn_wrap\">";
                tr_elem += "<textarea cols=\"17\" rows=\"1\" class=\"etc_contents inp wp75 \" id=\"etc_contents_" + elem.Sort + "\" maxlength=\"500\"></textarea>";
                tr_elem += "<a href=\"javascript:;\" class=\"line_del_btn wp23 fr \" idx=\"" + elem.Sort + "\">삭제</a>";
                tr_elem += "</div></td></tr>";

                $(".info_reg_list").append(tr_elem);

                $("#etc_title_" + elem.Sort).val(elem.Etc_Title);
            }
            $("#etc_contents_" + elem.Sort).val(elem.Information_Content);
        });
        $(".line_del_btn").on('click', function () {

            var id = $(this).attr('idx');

            var title_id = "etc_title_" + id;
            var contents_id = "etc_contents_" + id;


            $(this).parents('tr').remove();

            if ($(".info_reg_list").find('tr').length < 6) {
                $(".add_inp_btn.etc_info").show();
            }
        });
    }
   
    //아기정보 테이블 추가
    var $babyContents = $('.baby_content.content02');

    $(".baby_btn").on('click', function () {

        //쌍둥이추가 버튼 추가 클릭 시
        if ($(this).hasClass("baby_table_reg")) {
            if (!$babyContents.eq(0).hasClass('on')) {
                $babyContents.eq(0).addClass('on');
            }
            else {
                $babyContents.eq(1).addClass('on');
            }
        }
        //쌍둥이추가 버튼 삭제 클릭 시
        else {
            $(this).parents($babyContents).removeClass('on');

            //초기화
            $(this).parents('.baby_content.content02').find('.baby_title').parents('tr').remove();
        }

    });


    //아기정보 항목추가
    $('.baby_content').each(function (i) {
        var $this = $(this);
        var i = i + 1;

        $(this).find(".add_inp_btn.baby").on('click', function () {
            var id = $this.find('tr').length + 1;

            var elem = "<tr><th><input type=\"text\" id=\"baby_title_" + i + "_" + id + "\" class=\"inp w120 baby_title\" placeholder=\"직접 입력\"></th><td><textarea class=\"inp baby_contents wp75\" id=\"baby_contents_" + i + "_" + id + "\"></textarea><a href=\"javascript:;\" class=\"line_del_btn baby wp23 fr\" id=\"btn_del_baby_" + i + "_" + id + "\" idx=\"" + i + "_" + id + "\">삭제</a></td></tr>";

            $this.find('.baby_list').append(elem);

            $("#btn_del_baby_" + i + "_" + id + " ").on('click', function () {

                var id = $(this).attr('idx');
                var title_id = "baby_title_" + id;
                var contents_id = "baby_contents_" + id;

                //$('#preview_frame').contents().find("#" + title_id).html("");
                //$('#preview_frame').contents().find("#" + contents_id).html("");

                $(this).parents('tr').remove();


                var idx = 1;

                /*var preview_frame = document.getElementById("preview_frame");
            
                preview_frame.contentWindow.fn_InitETC();*/

                $this.find('.baby_list').find('tr').each(function () {
                    $(this).find(".baby_title").attr("id", "baby_title_" + i + "_" + idx);
                    $(this).find(".baby_contents").attr("id", "baby_contents_" + i + "_" + idx);
                    $(this).attr("idx", idx);

                    /*if (idx < 3) {
                        $('#preview_frame').contents().find("#etc_title_" + idx).html($(this).find(".etc_title").text());
                    } else {
                        $('#preview_frame').contents().find("#etc_title_" + idx).html($(this).find(".etc_title").val());
                    }
                    $('#preview_frame').contents().find("#etc_contents_" + idx).html($(this).find(".etc_contents").val());*/

                    idx++;
                });


                if ($this.find('tr').length <= 5) {
                    $this.find(".add_inp_btn.baby").show();
                }

                //var position = $('#preview_frame').contents().find("#area11").offset();
                //$('#preview_frame').contents().scrollTop(position.top);
            });

            if ($this.find('tr').length > 5) {
                $this.find(".add_inp_btn.baby").hide();
            }
        });
    });

    //입금대상선택 리스트 바인딩
    var sender = [];

    if ($("#Sender").text() != "") {
        sender = JSON.parse($("#Sender").text());
    }


    $(".select.sel_sender").each(function (obj) {
        var obj = $(this);
        obj.append($("<option>", { value: "", text: "입급대상선택" }));
        $.each(sender, function (key, value) {
            obj.append($('<option>',
                {
                    value: value.Code,
                    text: value.Code_Name
                }));
        });

    });

    var groom_sender = [];
    if ($("#GroomSender").text() != "") {
        groom_sender = JSON.parse($("#GroomSender").text());
    }
    $(".select.sel_groom_sender").each(function (obj) {
        var obj = $(this);
        obj.append($("<option>", { value: "", text: "입급대상선택" }));
        $.each(groom_sender, function (key, value) {
            obj.append($('<option>',
                {
                    value: value.Code,
                    text: value.Code_Name
                }));
        });

    });

    var bride_sender = [];
    if ($("#BrideSender").text() != "") {
        bride_sender = JSON.parse($("#BrideSender").text());
    }
    $(".select.sel_bride_sender").each(function (obj) {
        var obj = $(this);
        obj.append($("<option>", { value: "", text: "입급대상선택" }));
        $.each(bride_sender, function (key, value) {
            obj.append($('<option>',
                {
                    value: value.Code,
                    text: value.Code_Name
                }));
        });
    });


    //은행선택 리스트 바인딩
    var banks = [];

    if ($("#Banks").text() != "") {
        banks = JSON.parse($("#Banks").text());
    }

    $(".sel_bank").each(function (obj) {
        var obj = $(this);
        obj.append($("<option>", { value: "", text: "은행선택" }));
        $.each(banks, function (key, value) {
            obj.append($('<option>',
                {
                    value: value.Bank_Code,
                    text: value.Bank_Name
                }));
        });

    });

    $(".sel_groom_bank").each(function (obj) {
        var obj = $(this);
        obj.append($("<option>", { value: "", text: "은행선택" }));
        $.each(banks, function (key, value) {
            obj.append($('<option>',
                {
                    value: value.Bank_Code,
                    text: value.Bank_Name
                }));
        });

    });
    $(".sel_bride_bank").each(function (obj) {
        var obj = $(this);
        obj.append($("<option>", { value: "", text: "은행선택" }));
        $.each(banks, function (key, value) {
            obj.append($('<option>',
                {
                    value: value.Bank_Code,
                    text: value.Bank_Name
                }));
        });

    });



    //축의금 계좌정보
    var accounts = [];
    if ($("#Accounts").text() != "") {
        accounts = JSON.parse($("#Accounts").text());
        accounts.forEach(function (elm) {
            if (elm.Sort > 2) {
                var elem = "<tr>";
                elem = elem + "<th style=\"position: relative;\">";
                elem = elem + "<span class=\"select_wrap type01 wp60\">";
                elem = elem + "<select class=\"select sel_sender\" id=\"selSender_" + elm.Sort + "\">";
                elem = elem + "</select>";
                elem = elem + "</span>";
                elem = elem + "<input type=\"text\" class=\"inp wp25 ml5 input_sender\" placeholder=\"관계\" style=\"display: none;\" id=\"inputSender_" + elm.Sort + "\" value=\"\" />";
                elem = elem + "<span class=\"select_wrap type01 wp35 fr\">";
                elem = elem + "<select class=\"select sel_bank\" id=\"selBank_" + elm.Sort + "\">";
                elem = elem + "</select>";
                elem = elem + "</span>";
                elem = elem + "</th>";
                elem = elem + "<td style=\"position: relative;\">";
                elem = elem + "<input type=\"text\" class=\"inp num_inp wp100 input_accountnumber\" inputmode=\"numeric\" placeholder=\"계좌번호입력(- 없이 숫자만 입력)\" maxlength=\"100\" onkeyup=\"chkNumber(this);\" id=\"inputAccountNumber_" + elm.Sort + "\"/> <br />";
                elem = elem + "</td>";
                elem = elem + "<td style=\"position: relative;\">";
                elem = elem + "<input type=\"text\" class=\"inp num_inp wp75 input_accountholder\" placeholder=\"예금주 입력\" maxlength=\"50\" id=\"inputAccountHolder_" + elm.Sort + "\" />";
                elem = elem + "<a href=\"javascript: ;\" class=\"line_del_btn account wp23 fr\"  sort=\"" + elm.Sort + "\">삭제</a>";
                elem = elem + "</td>";
                elem = elem + "</tr>";
                $(".account_list").append(elem);

                $("#selSender_" + elm.Sort).append($("<option>", { value: "", text: "입급대상선택" }));
                $.each(sender, function (key, value) {
                    $("#selSender_" + elm.Sort).append($('<option>',
                        {
                            value: value.Code,
                            text: value.Code_Name
                        }));
                });

                $("#selBank_" + elm.Sort).append($("<option>", { value: "", text: "은행선택" }));
                $.each(banks, function (key, value) {
                    $("#selBank_" + elm.Sort).append($('<option>',
                        {
                            value: value.Bank_Code,
                            text: value.Bank_Name
                        }));
                });
            }

            if (elm.Send_Target_Code == "ATC005") {
                $("#selSender_" + elm.Sort).parent('span').removeClass("wp60");
                $("#selSender_" + elm.Sort).parent('span').addClass("wp40");

                $("#selBank_" + elm.Sort).parent('span').removeClass("wp35");
                $("#selBank_" + elm.Sort).parent('span').addClass("wp30");

                $("#inputSender_" + elm.Sort).show();
            }

            $("#selSender_" + elm.Sort).val(elm.Send_Target_Code);
            $("#inputSender_" + elm.Sort).val(elm.Send_Name);
            $("#selBank_" + elm.Sort).val(elm.Bank_Code);
            $("#inputAccountNumber_" + elm.Sort).val(elm.Account_Number);
            $("#inputAccountHolder_" + elm.Sort).val(elm.Account_Holder);

        });

        $(".line_del_btn.account").on('click', function () {

            $(this).parents('tr').remove();

            var idx = 1;
            $(".account_list").find('tr').each(function () {
                $(this).find(".sel_sender").attr("id", "selSender_" + idx);
                $(this).find(".input_sender").attr("id", "inputSender_" + idx);
                $(this).find(".sel_bank").attr("id", "selBank_" + idx);
                $(this).find(".input_accountnumber").attr("id", "inputAccountNumber_" + idx);
                $(this).find(".input_accountholder").attr("id", "inputAccountHolder_" + idx);
                $(this).find(".line_del_btn.account").attr("sort", idx);
                idx++;
            });
            if ($(".account_list").find('tr').length < 6) {
                $(".add_inp_btn.account").show();
            }
        });
    }

    var groom_accounts = [];
    if ($("#GroomAccounts").text() != "") {
        groom_accounts = JSON.parse($("#GroomAccounts").text());
        groom_accounts.forEach(function (elm) {
            if (elm.Sort > 1) {
                var elem = "<tr>";
                elem = elem + "<th style=\"position: relative;\">";
                elem = elem + "<span class=\"select_wrap type01 wp60\">";
                elem = elem + "<select class=\"select sel_groom_sender\" id=\"selGroomSender_" + elm.Sort + "\">";
                elem = elem + "</select>";
                elem = elem + "</span>";
                elem = elem + "<input type=\"text\" class=\"inp wp25 ml5 input_groom_sender\" placeholder=\"관계\" style=\"display: none;\" id=\"inputGroomSender_" + elm.Sort + "\" value=\"\" />";
                elem = elem + "<span class=\"select_wrap type01 wp35 fr\">";
                elem = elem + "<select class=\"select sel_groom_bank\" id=\"selGroomBank_" + elm.Sort + "\">";
                elem = elem + "</select>";
                elem = elem + "</span>";
                elem = elem + "</th>";
                elem = elem + "<td style=\"position: relative;\">";
                elem = elem + "<input type=\"text\" class=\"inp num_inp wp100 input_groom_accountnumber\" inputmode=\"numeric\" placeholder=\"계좌번호입력(- 없이 숫자만 입력)\" maxlength=\"100\" onkeyup=\"chkNumber(this);\" id=\"inputGroomAccountNumber_" + elm.Sort + "\"/> <br />";
                elem = elem + "</td>";
                elem = elem + "<td style=\"position: relative;\">";
                elem = elem + "<input type=\"text\" class=\"inp num_inp wp75 input_groom_accountholder\" placeholder=\"예금주 입력\" maxlength=\"50\" id=\"inputGroomAccountHolder_" + elm.Sort + "\" />";
                elem = elem + "<a href=\"javascript: ;\" class=\"line_del_btn account_groom wp23 fr\"  sort=\"" + elm.Sort + "\">삭제</a>";
                elem = elem + "</td>";
                elem = elem + "</tr>";
                $(".account_groom_list").append(elem);

                $("#selGroomSender_" + elm.Sort).append($("<option>", { value: "", text: "입급대상선택" }));
                $.each(groom_sender, function (key, value) {
                    $("#selGroomSender_" + elm.Sort).append($('<option>',
                        {
                            value: value.Code,
                            text: value.Code_Name
                        }));
                });

                $("#selGroomBank_" + elm.Sort).append($("<option>", { value: "", text: "은행선택" }));
                $.each(banks, function (key, value) {
                    $("#selGroomBank_" + elm.Sort).append($('<option>',
                        {
                            value: value.Bank_Code,
                            text: value.Bank_Name
                        }));
                });
            }

            if (elm.Send_Target_Code == "ATC005") {
                $("#selGroomSender_" + elm.Sort).parent('span').removeClass("wp60");
                $("#selGroomSender_" + elm.Sort).parent('span').addClass("wp40");

                $("#selGroomBank_" + elm.Sort).parent('span').removeClass("wp35");
                $("#selGroomBank_" + elm.Sort).parent('span').addClass("wp30");

                $("#inputGroomSender_" + elm.Sort).show();
            }

            $("#selGroomSender_" + elm.Sort).val(elm.Send_Target_Code);
            $("#inputGroomSender_" + elm.Sort).val(elm.Send_Name);
            $("#selGroomBank_" + elm.Sort).val(elm.Bank_Code);
            $("#inputGroomAccountNumber_" + elm.Sort).val(elm.Account_Number);
            $("#inputGroomAccountHolder_" + elm.Sort).val(elm.Account_Holder);

        });

        $(".line_del_btn.account_groom").on('click', function () {

            $(this).parents('tr').remove();

            var idx = 1;
            $(".account_list").find('tr').each(function () {
                $(this).find(".sel_groom_sender").attr("id", "selGroomSender_" + idx);
                $(this).find(".input_groom_sender").attr("id", "inputGroomSender_" + idx);
                $(this).find(".sel_groom_bank").attr("id", "selGroomBank_" + idx);
                $(this).find(".input_groom_accountnumber").attr("id", "inputGroomAccountNumber_" + idx);
                $(this).find(".input_groom_accountholder").attr("id", "inputGroomAccountHolder_" + idx);
                $(this).find(".line_del_btn.account_groom").attr("sort", idx);
                idx++;
            });
            if ($(".account_groom_list").find('tr').length < 3) {
                $(".add_inp_btn.account_groom").show();
            }
        });
    }

    var bride_accounts = [];
    if ($("#BrideAccounts").text() != "") {
        bride_accounts = JSON.parse($("#BrideAccounts").text());
        bride_accounts.forEach(function (elm) {
            if (elm.Sort > 1) {
                var elem = "<tr>";
                elem = elem + "<th style=\"position: relative;\">";
                elem = elem + "<span class=\"select_wrap type01 wp60\">";
                elem = elem + "<select class=\"select sel_bride_sender\" id=\"selBrideSender_" + elm.Sort + "\">";
                elem = elem + "</select>";
                elem = elem + "</span>";
                elem = elem + "<input type=\"text\" class=\"inp wp25 ml5 input_bride_sender\" placeholder=\"관계\" style=\"display: none;\" id=\"inputBrideSender_" + elm.Sort + "\" value=\"\" />";
                elem = elem + "<span class=\"select_wrap type01 wp35 fr\">";
                elem = elem + "<select class=\"select sel_bride_bank\" id=\"selBrideBank_" + elm.Sort + "\">";
                elem = elem + "</select>";
                elem = elem + "</span>";
                elem = elem + "</th>";
                elem = elem + "<td style=\"position: relative;\">";
                elem = elem + "<input type=\"text\" class=\"inp num_inp wp100 input_bride_accountnumber\" inputmode=\"numeric\" placeholder=\"계좌번호입력(- 없이 숫자만 입력)\" maxlength=\"100\" onkeyup=\"chkNumber(this);\" id=\"inputBrideAccountNumber_" + elm.Sort + "\"/> <br />";
                elem = elem + "</td>";
                elem = elem + "<td style=\"position: relative;\">";
                elem = elem + "<input type=\"text\" class=\"inp num_inp wp75 input_bride_accountholder\" placeholder=\"예금주 입력\" maxlength=\"50\" id=\"inputBrideAccountHolder_" + elm.Sort + "\" />";
                elem = elem + "<a href=\"javascript: ;\" class=\"line_del_btn account_bride wp23 fr\"  sort=\"" + elm.Sort + "\">삭제</a>";
                elem = elem + "</td>";
                elem = elem + "</tr>";
                $(".account_bride_list").append(elem);

                $("#selBrideSender_" + elm.Sort).append($("<option>", { value: "", text: "입급대상선택" }));
                $.each(bride_sender, function (key, value) {
                    $("#selBrideSender_" + elm.Sort).append($('<option>',
                        {
                            value: value.Code,
                            text: value.Code_Name
                        }));
                });

                $("#selBrideBank_" + elm.Sort).append($("<option>", { value: "", text: "은행선택" }));
                $.each(banks, function (key, value) {
                    $("#selBrideBank_" + elm.Sort).append($('<option>',
                        {
                            value: value.Bank_Code,
                            text: value.Bank_Name
                        }));
                });
            }

            if (elm.Send_Target_Code == "ATC005") {
                $("#selBrideSender_" + elm.Sort).parent('span').removeClass("wp60");
                $("#selBrideSender_" + elm.Sort).parent('span').addClass("wp40");

                $("#selBrideBank_" + elm.Sort).parent('span').removeClass("wp35");
                $("#selBrideBank_" + elm.Sort).parent('span').addClass("wp30");

                $("#inputBrideSender_" + elm.Sort).show();
            }

            $("#selBrideSender_" + elm.Sort).val(elm.Send_Target_Code);
            $("#inputBrideSender_" + elm.Sort).val(elm.Send_Name);
            $("#selBrideBank_" + elm.Sort).val(elm.Bank_Code);
            $("#inputBrideAccountNumber_" + elm.Sort).val(elm.Account_Number);
            $("#inputBrideAccountHolder_" + elm.Sort).val(elm.Account_Holder);

        });

        $(".line_del_btn.account_bride").on('click', function () {

            $(this).parents('tr').remove();

            var idx = 1;
            $(".account_bride_list").find('tr').each(function () {
                $(this).find(".sel_bride_sender").attr("id", "selBrideSender_" + idx);
                $(this).find(".input_bride_sender").attr("id", "inputBrideSender_" + idx);
                $(this).find(".sel_bride_bank").attr("id", "selBrideBank_" + idx);
                $(this).find(".input_bride_accountnumber").attr("id", "inputBrideAccountNumber_" + idx);
                $(this).find(".input_bride_accountholder").attr("id", "inputBrideAccountHolder_" + idx);
                $(this).find(".line_del_btn.account_bride").attr("sort", idx);
                idx++;
            });
            if ($(".account_bride_list").find('tr').length < 3) {
                $(".add_inp_btn.account_bride").show();
            }
        });
    }


    //포토사이즈
    if ($("#Objects").text() != "") {
        objects = JSON.parse($("#Objects").text());
        objects.forEach(function (elem) {
            if (elem.type == 'photo') {
                $("#photo_width").val(elem.width);
                $("#photo_height").val(elem.height);
            }
        });
    }

    if ($("#Etc_Information_Use_YN").val() == "Y") {
    $('#Etc_Information_Use_Y').prop('checked', true);
    } else {
    $('#Etc_Information_Use_N').prop('checked', true);
    $('.etc_info .order_detail').hide();
    }

    //축의금송금(카카오페이)
    if ($("#MoneyGift_Remit_Use_YN").val() == "Y") {
        $('#MoneyGift_Remit_Use_Y').prop('checked', true);
        $('.money_gift .order_detail').show();
    } else {
        $('#MoneyGift_Remit_Use_N').prop('checked', true);
        $('.money_gift .order_detail').hide();
    }

    //축의금송금통합(계좌이체)
    if ($("#MoneyAccount_Remit_Use_YN").val() == "Y") {
        $('#MoneyAccount_Remit_Use_Y').prop('checked', true);
        $('.money_account.all .order_detail').show();
    } else {
        $('#MoneyAccount_Remit_Use_N').prop('checked', true);
        $('.money_account.all .order_detail').hide();
    }

    //축의금송금분리(계좌이체)

    if ($("#MoneyAccount_Div_Use_YN").val() == "Y") {
        $('#MoneyAccount_Div_Use_Y').prop('checked', true);
        $('.money_account.div .order_detail').show();
    } else {
        $('#MoneyAccount_Div_Use_N').prop('checked', true);
        $('.money_account.div .order_detail').hide();
    }

    //화환 선물
    if ($("#Flower_gift_YN").val() == "Y") {
        $('#Flower_gift_Y').prop('checked', true);
        $('.c_flowers .order_detail').show();
    } else {
        $('#Flower_gift_N').prop('checked', true);
        $('.c_flowers .order_detail').hide();
    }

    $(".sel_sender").on('change', function () {
        //직접입력
        var id = $(this).attr('id').replace("selSender", "inputSender")

        var sort = $(this).attr('id').split('_')[1];

        if ($(this).find("option:selected").val() == "ATC005") {
            $("#" + id).val("");
            $("#" + id).show();

            $("#selSender_" + sort).parent('span').removeClass("wp60");
            $("#selSender_" + sort).parent('span').addClass("wp40");

            $("#selBank_" + sort).parent('span').removeClass("wp35");
            $("#selBank_" + sort).parent('span').addClass("wp30");

        } else {
            $("#" + id).hide();
            $("#" + id).val($(this).find("option:selected").val() != "" ? $(this).find("option:selected").text() : "");

            $("#selSender_" + sort).parent('span').removeClass("wp40");
            $("#selSender_" + sort).parent('span').addClass("wp60");

            $("#selBank_" + sort).parent('span').removeClass("wp30");
            $("#selBank_" + sort).parent('span').addClass("wp35");
        }
    });

    $(".sel_groom_sender").on('change', function () {
        //직접입력
        var id = $(this).attr('id').replace("selGroomSender", "inputGroomSender")

        var sort = $(this).attr('id').split('_')[1];

        if ($(this).find("option:selected").val() == "ATC005") {
            $("#" + id).val("");
            $("#" + id).show();

            $("#selGroomSender_" + sort).parent('span').removeClass("wp60");
            $("#selGroomSender_" + sort).parent('span').addClass("wp40");

            $("#selGroomBank_" + sort).parent('span').removeClass("wp35");
            $("#selGroomBank_" + sort).parent('span').addClass("wp30");

        } else {
            $("#" + id).hide();
            $("#" + id).val($(this).find("option:selected").val() != "" ? $(this).find("option:selected").text() : "");

            $("#selGroomSender_" + sort).parent('span').removeClass("wp40");
            $("#selGroomSender_" + sort).parent('span').addClass("wp60");

            $("#selGroomBank_" + sort).parent('span').removeClass("wp30");
            $("#selGroomBank_" + sort).parent('span').addClass("wp35");
        }
    });


    $(".sel_bride_sender").on('change', function () {
        //직접입력
        var id = $(this).attr('id').replace("selBrideSender", "inputBrideSender")

        var sort = $(this).attr('id').split('_')[1];

        if ($(this).find("option:selected").val() == "ATC005") {
            $("#" + id).val("");
            $("#" + id).show();

            $("#selBrideSender_" + sort).parent('span').removeClass("wp60");
            $("#selBrideSender_" + sort).parent('span').addClass("wp40");

            $("#selBrideBank_" + sort).parent('span').removeClass("wp35");
            $("#selBrideBank_" + sort).parent('span').addClass("wp30");

        } else {
            $("#" + id).hide();
            $("#" + id).val($(this).find("option:selected").val() != "" ? $(this).find("option:selected").text() : "");
            

            $("#selBrideSender_" + sort).parent('span').removeClass("wp40");
            $("#selBrideSender_" + sort).parent('span').addClass("wp60");

            $("#selBrideBank_" + sort).parent('span').removeClass("wp30");
            $("#selBrideBank_" + sort).parent('span').addClass("wp35");
        }
    });

    $("input[name=Time_Type_Code]").on('click', function () {
        $("#Time_Type_Code").val($(this).attr('kname'));
    });

    //예식일시
    $(".date_inp").val($("#WeddingDate").val() + " " + $("#WeddingWeek").val());
    $(".add_inp_btn.etc_info").on('click', function () {
        var id = $(".info_reg_list").find('tr').length + 1;
        var elem = "<tr><th><div style=\"position:relative;\"><input type=\"text\" id=\"etc_title_" + id + "\" class=\"inp wp100 etc_title\" placeholder=\"직접 입력\"></div></th>";
        elem += "<td><div class=\"fl_wrap btn_wrap\">";
        elem += "<textarea cols =\"17\" rows=\"1\" class=\"etc_contents inp wp75 \" id=\"etc_contents_" + id + "\" maxlength=\"500\"></textarea>";
        elem += "<a href=\"javascript:;\" class=\"line_del_btn wp23 fr \" idx=\"" + id + "\">삭제</a>";
        elem += "</div></td></tr>";
        $(".info_reg_list").append(elem);
        $(".line_del_btn").on('click', function () {
            $(this).parents('tr').remove();
            if ($(".info_reg_list").find('tr').length < 6) {
                $(".add_inp_btn.etc_info").show();
            }
        });
        if ($(".info_reg_list").find('tr').length > 5) {
            $(".add_inp_btn.etc_info").hide();
        }
    });

    $(".add_inp_btn.account").on('click', function () {

        var id = $(".account_list").find('tr').length + 1;

        var elem = "<tr>";
        elem = elem + "<th style=\"position: relative;\">";
        elem = elem + "<span class=\"select_wrap type01 wp60\">";
        elem = elem + "<select class=\"select sel_sender\" id=\"selSender_" + id + "\">";
        elem = elem + "</select>";
        elem = elem + "</span>";
        elem = elem + "<input type=\"text\" class=\"inp wp25 ml5 input_sender\" placeholder=\"관계\" style=\"display: none;\" id=\"inputSender_"  + id +"\" value=\"\" />";
        elem = elem + "<span class=\"select_wrap type01 wp35 fr\">";
        elem = elem + "<select class=\"select sel_bank\" id=\"selBank_" + id + "\">";
        elem = elem + "</select>";
        elem = elem + "</span>"; 
        elem = elem + "</th>";
        elem = elem + "<td style=\"position: relative;\">";
        elem = elem + "<input type=\"text\" class=\"inp num_inp wp100 input_accountnumber\" inputmode=\"numeric\" placeholder=\"계좌번호입력(- 없이 숫자만 입력)\" maxlength=\"100\" onkeyup=\"chkNumber(this);\" id=\"inputAccountNumber_" + id + "\"/> <br />";
        elem = elem + "</td>";
        elem = elem + "<td style=\"position: relative;\">";
        elem = elem + "<input type=\"text\" class=\"inp num_inp wp75 input_accountholder\" placeholder=\"예금주 입력\" maxlength=\"50\" id=\"inputAccountHolder_" + id + "\" />";
        elem = elem + "<a href=\"javascript: ;\" class=\"line_del_btn account wp23 fr\"  sort=\"" + id + "\">삭제</a>";
        elem = elem + "</td>";
        elem = elem + "</tr>";

        $(".account_list").append(elem);


        $("#selSender_" + id).append($("<option>", { value: "", text: "입급대상선택" }));
        $.each(sender, function (key, value) {
            $("#selSender_" + id).append($('<option>',
                {
                    value: value.Code,
                    text: value.Code_Name
                }));
        });
        $("#selBank_" + id).append($("<option>", { value: "", text: "은행선택" }));
        $.each(banks, function (key, value) {
            $("#selBank_" + id).append($('<option>',
                {
                    value: value.Bank_Code,
                    text: value.Bank_Name
                }));
        });


        $(".sel_sender").on('change', function () {
            //직접입력
            var id = $(this).attr('id').replace("selSender", "inputSender")

            var sort = $(this).attr('id').split('_')[1];

            if ($(this).find("option:selected").val() == "ATC005") {
                $("#" + id).val("");
                $("#" + id).show();

                $("#selSender_" + sort).parent('span').removeClass("wp60");
                $("#selSender_" + sort).parent('span').addClass("wp40");

                $("#selBank_" + sort).parent('span').removeClass("wp35");
                $("#selBank_" + sort).parent('span').addClass("wp30");

            } else {
                $("#" + id).hide();
                $("#" + id).val($(this).find("option:selected").val() != "" ? $(this).find("option:selected").text() : "");

                $("#selSender_" + sort).parent('span').removeClass("wp40");
                $("#selSender_" + sort).parent('span').addClass("wp60");

                $("#selBank_" + sort).parent('span').removeClass("wp30");
                $("#selBank_" + sort).parent('span').addClass("wp35");
            }
        });

        $(".line_del_btn.account").on('click', function () {
          
            $(this).parents('tr').remove();

            var idx = 1;
            $(".account_list").find('tr').each(function () {
                $(this).find(".sel_sender").attr("id", "selSender_" + idx);
                $(this).find(".input_sender").attr("id", "inputSender_" + idx);
                $(this).find(".sel_bank").attr("id", "selBank_" + idx);
                $(this).find(".input_accountnumber").attr("id", "inputAccountNumber_" + idx);
                $(this).find(".input_accountholder").attr("id", "inputAccountHolder_" + idx);
                $(this).find(".line_del_btn.account").attr("sort", idx);
                idx++;
            });


            if ($(".account_list").find('tr').length < 6) {
                $(".add_inp_btn.account").show();
            }

        });

        if ($(".account_list").find('tr').length > 5) {
            $(".add_inp_btn.account").hide();
        }

    });

    $(".add_inp_btn.account_groom").on('click', function () {

        var id = $(".account_groom_list").find('tr').length + 1;

        var elem = "<tr>";
        elem = elem + "<th style=\"position: relative;\">";
        elem = elem + "<span class=\"select_wrap type01 wp60\">";
        elem = elem + "<select class=\"select sel_groom_sender\" id=\"selGroomSender_" + id + "\">";
        elem = elem + "</select>";
        elem = elem + "</span>";
        elem = elem + "<input type=\"text\" class=\"inp wp25 ml5 input_groom_sender\" placeholder=\"관계\" style=\"display: none;\" id=\"inputGroomSender_" + id + "\" value=\"\" />";
        elem = elem + "<span class=\"select_wrap type01 wp35 fr\">";
        elem = elem + "<select class=\"select sel_groom_bank\" id=\"selGroomBank_" + id + "\">";
        elem = elem + "</select>";
        elem = elem + "</span>";
        elem = elem + "</th>";
        elem = elem + "<td style=\"position: relative;\">";
        elem = elem + "<input type=\"text\" class=\"inp num_inp wp100 input_groom_accountnumber\" inputmode=\"numeric\" placeholder=\"계좌번호입력(- 없이 숫자만 입력)\" maxlength=\"100\" onkeyup=\"chkNumber(this);\" id=\"inputGroomAccountNumber_" + id + "\"/> <br />";
        elem = elem + "</td>";
        elem = elem + "<td style=\"position: relative;\">";
        elem = elem + "<input type=\"text\" class=\"inp num_inp wp75 input_groom_accountholder\" placeholder=\"예금주 입력\" maxlength=\"50\" id=\"inputGroomAccountHolder_" + id + "\" />";
        elem = elem + "<a href=\"javascript: ;\" class=\"line_del_btn account_groom wp23 fr\"  sort=\"" + id + "\">삭제</a>";
        elem = elem + "</td>";
        elem = elem + "</tr>";

        $(".account_groom_list").append(elem);


        $("#selGroomSender_" + id).append($("<option>", { value: "", text: "입급대상선택" }));
        $.each(groom_sender, function (key, value) {
            $("#selGroomSender_" + id).append($('<option>',
                {
                    value: value.Code,
                    text: value.Code_Name
                }));
        });
        $("#selGroomBank_" + id).append($("<option>", { value: "", text: "은행선택" }));
        $.each(banks, function (key, value) {
            $("#selGroomBank_" + id).append($('<option>',
                {
                    value: value.Bank_Code,
                    text: value.Bank_Name
                }));
        });


        $(".sel_groom_sender").on('change', function () {
            //직접입력
            var id = $(this).attr('id').replace("selGroomSender", "inputGroomSender")

            var sort = $(this).attr('id').split('_')[1];

            if ($(this).find("option:selected").val() == "ATC005") {
                $("#" + id).val("");
                $("#" + id).show();

                $("#selGroomSender_" + sort).parent('span').removeClass("wp60");
                $("#selGroomSender_" + sort).parent('span').addClass("wp40");

                $("#selGroomBank_" + sort).parent('span').removeClass("wp35");
                $("#selGroomBank_" + sort).parent('span').addClass("wp30");

            } else {
                $("#" + id).hide();
                $("#" + id).val($(this).find("option:selected").val() != "" ? $(this).find("option:selected").text() : "");

                $("#selGroomSender_" + sort).parent('span').removeClass("wp40");
                $("#selGroomSender_" + sort).parent('span').addClass("wp60");

                $("#selGroomBank_" + sort).parent('span').removeClass("wp30");
                $("#selGroomBank_" + sort).parent('span').addClass("wp35");
            }
        });

        $(".line_del_btn.account_groom").on('click', function () {

            $(this).parents('tr').remove();

            var idx = 1;
            $(".account_groom_list").find('tr').each(function () {
                $(this).find(".sel_groom_sender").attr("id", "selGroomSender_" + idx);
                $(this).find(".input_groom_sender").attr("id", "inputGroomSender_" + idx);
                $(this).find(".sel_groom_bank").attr("id", "selBank_" + idx);
                $(this).find(".input_groom_accountnumber").attr("id", "inputGroomAccountNumber_" + idx);
                $(this).find(".input_groom_accountholder").attr("id", "inputGroomAccountHolder_" + idx);
                $(this).find(".line_del_btn.account_groom").attr("sort", idx);
                idx++;
            });


            if ($(".account_groom_list").find('tr').length < 3) {
                $(".add_inp_btn.account_groom").show();
            }

        });

        if ($(".account_groom_list").find('tr').length > 2) {
            $(".add_inp_btn.account_groom").hide();
        }

    });

    $(".add_inp_btn.account_bride").on('click', function () {

        var id = $(".account_bride_list").find('tr').length + 1;

        var elem = "<tr>";
        elem = elem + "<th style=\"position: relative;\">";
        elem = elem + "<span class=\"select_wrap type01 wp60\">";
        elem = elem + "<select class=\"select sel_bride_sender\" id=\"selBrideSender_" + id + "\">";
        elem = elem + "</select>";
        elem = elem + "</span>";
        elem = elem + "<input type=\"text\" class=\"inp wp25 ml5 input_bride_sender\" placeholder=\"관계\" style=\"display: none;\" id=\"inputBrideSender_" + id + "\" value=\"\" />";
        elem = elem + "<span class=\"select_wrap type01 wp35 fr\">";
        elem = elem + "<select class=\"select sel_bride_bank\" id=\"selBrideBank_" + id + "\">";
        elem = elem + "</select>";
        elem = elem + "</span>";
        elem = elem + "</th>";
        elem = elem + "<td style=\"position: relative;\">";
        elem = elem + "<input type=\"text\" class=\"inp num_inp wp100 input_bride_accountnumber\" inputmode=\"numeric\" placeholder=\"계좌번호입력(- 없이 숫자만 입력)\" maxlength=\"100\" onkeyup=\"chkNumber(this);\" id=\"inputBrideAccountNumber_" + id + "\"/> <br />";
        elem = elem + "</td>";
        elem = elem + "<td style=\"position: relative;\">";
        elem = elem + "<input type=\"text\" class=\"inp num_inp wp75 input_bride_accountholder\" placeholder=\"예금주 입력\" maxlength=\"50\" id=\"inputBrideAccountHolder_" + id + "\" />";
        elem = elem + "<a href=\"javascript: ;\" class=\"line_del_btn account_bride wp23 fr\"  sort=\"" + id + "\">삭제</a>";
        elem = elem + "</td>";
        elem = elem + "</tr>";

        $(".account_bride_list").append(elem);

        $("#selBrideSender_" + id).append($("<option>", { value: "", text: "입급대상선택" }));
        $.each(bride_sender, function (key, value) {
            $("#selBrideSender_" + id).append($('<option>',
                {
                    value: value.Code,
                    text: value.Code_Name
                }));
        });
        $("#selBrideBank_" + id).append($("<option>", { value: "", text: "은행선택" }));
        $.each(banks, function (key, value) {
            $("#selBrideBank_" + id).append($('<option>',
                {
                    value: value.Bank_Code,
                    text: value.Bank_Name
                }));
        });


        $(".sel_bride_sender").on('change', function () {
            //직접입력
            var id = $(this).attr('id').replace("selBrideSender", "inputBrideSender")

            var sort = $(this).attr('id').split('_')[1];

            if ($(this).find("option:selected").val() == "ATC005") {
                $("#" + id).val("");
                $("#" + id).show();

                $("#selBrideSender_" + sort).parent('span').removeClass("wp60");
                $("#selBrideSender_" + sort).parent('span').addClass("wp40");

                $("#selBrideBank_" + sort).parent('span').removeClass("wp35");
                $("#selBrideBank_" + sort).parent('span').addClass("wp30");

            } else {
                $("#" + id).hide();
                $("#" + id).val($(this).find("option:selected").val() != "" ? $(this).find("option:selected").text() : "");

                $("#selBrideSender_" + sort).parent('span').removeClass("wp40");
                $("#selBrideSender_" + sort).parent('span').addClass("wp60");

                $("#selBrideBank_" + sort).parent('span').removeClass("wp30");
                $("#selBrideBank_" + sort).parent('span').addClass("wp35");
            }
        });

        $(".line_del_btn.account_bride").on('click', function () {

            $(this).parents('tr').remove();

            var idx = 1;
            $(".account_bride_list").find('tr').each(function () {
                $(this).find(".sel_bride_sender").attr("id", "selBrideSender_" + idx);
                $(this).find(".input_bride_sender").attr("id", "inputBrideSender_" + idx);
                $(this).find(".sel_bride_bank").attr("id", "selBank_" + idx);
                $(this).find(".input_bride_accountnumber").attr("id", "inputBrideAccountNumber_" + idx);
                $(this).find(".input_bride_accountholder").attr("id", "inputBrideAccountHolder_" + idx);
                $(this).find(".line_del_btn.account_bride").attr("sort", idx);
                idx++;
            });


            if ($(".account_bride_list").find('tr').length < 3) {
                $(".add_inp_btn.account_bride").show();
            }

        });

        if ($(".account_bride_list").find('tr').length > 2) {
            $(".add_inp_btn.account_bride").hide();
        }

    });


    $('.btn_use').on('click', function () {
        $("#" + $(this).attr("name")).val("Y");
        $(this).parents(".order_con").find('.order_detail').slideDown();
        if ($(".info_reg_list").find('tr').length > 5) {
            $(".add_inp_btn.etc_info").hide();
        }
    });
    $('.btn_unuse').on('click', function () {
        $("#" + $(this).attr("name")).val("N");
        $(this).parents(".order_con").find('.order_detail').slideUp();
    });
    $('.crop.ico').on('click', function () {
        var id = $(this).parents('li').attr('id');
        var url = $("." + id).attr('src')
        if (typeof (url) != "undefined") {
            $('.pop_wrap.' + id.replace("img", "pop")).show();
            cropImage(id, url);
        }
    });
    $('.delete.ico').on('click', function () {
    var id = $(this).parents('li').attr('id');
    var filepath = $("." + id).attr('orgimg')

    if (filepath != null && filepath != "") {
        var mydata = {
            "filepath": filepath,
            "Invitation_Id": $("#Invitation_Id").val(),
            "type": id
        };

        $.ajax({
            type: "POST",
            url: "/Invitation/RemoveImage",
            async: false,
            data: mydata,
            dataType: "json",
            success: function (result) {
                if (result.success == "Y") {
                    $("." + id).attr("src", null);
                    $("#" + id).find('label').show();
                    $("#" + id + " .img_preview").hide();
                } else {
                    if (!result.auth) {
                        location.reload();
                    } else {
                        $(".error").css("display", "flex").delay(1000).fadeOut();
                    }
                }
            }
        });
    }

    });
    //$('.call_val').attr('readonly', true);
    $('.tip_box label').on('click', function () {
    var $checkBox = $(this).prev('input[type="checkbox"]');
    var $tip = $(this).next('.bn_tip');
    var $callVal = $(this).parents('.number_box').find('.call_val');
    var delayTime = 5000;

    if ($checkBox.is(":checked") == false) {
        $tip.show().delay(delayTime).fadeOut();
        $callVal.attr('readonly', false);
    } else {
        $tip.hide();
        $callVal.attr('readonly', true);
    }

    });

    var mcdays = ['일', '월', '화', '수', '목', '금', '토'],
    mcmonths = ['1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월'],
    opts = {
        dateFormat: 'yy-mm-dd DD',
        constrainInput: true,
        dayNames: mcdays,
        dayNamesShort: mcdays,
        dayNamesMin: mcdays,
        monthNames: mcmonths,
        monthNamesShort: mcmonths,
        monthNamesMin: mcmonths,
        constrainInput: true,
        showMonthAfterYear: true,
        yearSuffix: "년",
        changeYear: false,
        changeMonth: false,
        onSelect: function (dateText) {

            $("#WeddingDate").val(dateText.split(' ')[0]);


            $("#WeddingYY").val(parseInt(dateText.split('-')[0]));
            $('#WeddingYY').trigger('keyup');

            $("#WeddingMM").val(parseInt(dateText.split('-')[1]));
            $('#WeddingMM').trigger('keyup');

            var sp = dateText.split('-')[2];
            $('#WeddingDD').val(parseInt(sp.split(' ')[0]));
            $('#WeddingDD').trigger('keyup');

            $('#WeddingWeek').val(sp.split(' ')[1]);
            $('#WeddingWeek').trigger('keyup');
        }
        };

    //예식 일시
    $('.ico.date').on('click', function () {
        if ($("#ui-datepicker-div").css("display") == "none") {
            $('.date_inp').datepicker().datepicker("show");
        }
    });

    $('.date_inp').datepicker(opts);
    $('.radio_use').on('click', function () {
    $(this).parents(".order_con").find('.order_detail').show()();
    });
    $('.radio_nuse').on('click', function () {
    $(this).parents(".order_con").find('.order_detail').hide()();
    });
    $("#btn_next").click(function () {
        if (validation_check()) {
            fn_save();
        }
    });

    $('#Invitation_URL').on('keyup', function (event) {
        var regType1 = /^[A-Za-z0-9_-]*$/;

        if ($(this).val() != "") {
            if (!regType1.test($(this).val())) {
                $(".url_check").removeClass("true");
                $(".url_check").addClass("false");
                $(".url_text").text("사용할 수 없는 주소입니다.");
                return false;
            }
            checkDuplicateURL($(this).val());
        } else {
            $(".url_check").removeClass("true");
            $(".url_check").removeClass("false");
            $(".url_text").text("");
        }
    });

    //혼주정보
    if ($("#Parents_Information_Use_YN").val() == "Y") {
        $('#Parents_Information_Use_Y').prop('checked', true);

        if ($("#Groom_Parents1_Title").val() == "아버지") {
            $("#sel_g1").val($("#Groom_Parents1_Title").val()).prop("selected", true);
        }
        else if ($("#Groom_Parents1_Title").val() == "어머니") {
            $("#sel_g1").val($("#Groom_Parents1_Title").val()).prop("selected", true);
        }
        else if ($("#Groom_Parents1_Title").val() == "입력안함") {

            $("#sel_g1").val($("#Groom_Parents1_Title").val()).prop("selected", true);
            $("#Groom_Parents1_Global_Phone_Number_YN").prop('checked', false);
            $("#Groom_Parents1_Name").attr("disabled", true);
            $("#Groom_Parents1_Global_Phone_Number").attr("disabled", true);
            $("#Groom_Parents1_Phone").attr("disabled", true);
            $("#Groom_Parents1_Global_Phone_Number_YN").attr("disabled", true);
        } else {
            if ($("#Groom_Parents1_Title").val() != "") {
                $("#sel_g1").val("직접입력").prop("selected", true);
                $("#Groom_Parents1_Title").show();
            }
        }

        if ($("#Groom_Parents2_Title").val() == "아버지") {
            $("#sel_g2").val($("#Groom_Parents2_Title").val()).prop("selected", true);
        }
        else if ($("#Groom_Parents2_Title").val() == "어머니") {
            $("#sel_g2").val($("#Groom_Parents2_Title").val()).prop("selected", true);
        } else if ($("#Groom_Parents2_Title").val() == "입력안함") {
            $("#sel_g2").val($("#Groom_Parents2_Title").val()).prop("selected", true);
            $("#Groom_Parents2_Global_Phone_Number_YN").prop('checked', false);
            $("#Groom_Parents2_Name").attr("disabled", true);
            $("#Groom_Parents2_Global_Phone_Number").attr("disabled", true);
            $("#Groom_Parents2_Phone").attr("disabled", true);
            $("#Groom_Parents2_Global_Phone_Number_YN").attr("disabled", true);
        } else {
            if ($("#Groom_Parents2_Title").val() != "") {
                $("#sel_g2").val("직접입력").prop("selected", true);
                $("#Groom_Parents2_Title").show();
            }
        }

        if ($("#Bride_Parents1_Title").val() == "아버지") {
            $("#sel_b1").val($("#Bride_Parents1_Title").val()).prop("selected", true);
        }
        else if ($("#Bride_Parents1_Title").val() == "어머니") {
            $("#sel_b1").val($("#Bride_Parents1_Title").val()).prop("selected", true);
        } else if ($("#Bride_Parents1_Title").val() == "입력안함") {
            $("#sel_b1").val($("#Bride_Parents1_Title").val()).prop("selected", true);
            $("#Bride_Parents1_Global_Phone_Number_YN").prop('checked', false);
            $("#Bride_Parents1_Name").attr("disabled", true);
            $("#Bride_Parents1_Global_Phone_Number").attr("disabled", true);
            $("#Bride_Parents1_Phone").attr("disabled", true);
            $("#Bride_Parents1_Global_Phone_Number_YN").attr("disabled", true);
        } else {
            if ($("#Bride_Parents1_Title").val() != "") {
                $("#sel_b1").val("직접입력").prop("selected", true);
                $("#Bride_Parents1_Title").show();
            }
        }

        if ($("#Bride_Parents2_Title").val() == "아버지") {
            $("#sel_b2").val($("#Bride_Parents2_Title").val()).prop("selected", true);
        }
        else if ($("#Bride_Parents2_Title").val() == "어머니") {
            $("#sel_b2").val($("#Bride_Parents2_Title").val()).prop("selected", true);
        } else if ($("#Bride_Parents2_Title").val() == "입력안함") {
            $("#sel_b2").val($("#Bride_Parents2_Title").val()).prop("selected", true);
            $("#Bride_Parents2_Global_Phone_Number_YN").prop('checked', false);
            $("#Bride_Parents2_Name").attr("disabled", true);
            $("#Bride_Parents2_Global_Phone_Number").attr("disabled", true);
            $("#Bride_Parents2_Phone").attr("disabled", true);
            $("#Bride_Parents2_Global_Phone_Number_YN").attr("disabled", true);
        } else {
            if ($("#Bride_Parents2_Title").val() != "") {
                $("#sel_b2").val("직접입력").prop("selected", true);
                $("#Bride_Parents2_Title").show();
                $("#Bride_Parents2_Title").next().removeClass("wp70");
                $("#Bride_Parents2_Title").next().addClass("wp43");
            }
        }

    } else {
        $('#Parents_Information_Use_N').prop('checked', true);
        $('.honju_info .order_detail').hide();
    }



    $(".seltitle").on('change', function () {
        if ($(this).find("option:selected").index() == 2) {
             //직접입력
            $("." + $(this).attr('id')).val("");
            $("." + $(this).attr('id')).show();
            $("." + $(this).attr('id')).next().removeClass("wp70");
            $("." + $(this).attr('id')).next().addClass("wp43");

            $("." + $(this).attr('id')).next('.wp43').attr("disabled", false);
            $("." + $(this).attr('id')).parent().nextAll('.number_box').find('.inp_box').children('.w50').attr("disabled", false);
            $("." + $(this).attr('id')).parent().nextAll('.number_box').find('.inp_box').children('.wp70').attr("disabled", false);
            $("." + $(this).attr('id')).parent().nextAll('.number_box').find('.tip_box').children('input').attr("disabled", false);

        } else if ($(this).find("option:selected").index() == 3) {
            //입력안함
            $("." + $(this).attr('id')).val($(this).find("option:selected").text());
            $("." + $(this).attr('id')).hide();
            $("." + $(this).attr('id')).next().removeClass("wp43");
            $("." + $(this).attr('id')).next().addClass("wp70");

            $("." + $(this).attr('id')).next('.wp70').val("");
            $("." + $(this).attr('id')).next('.wp70').attr("disabled", true);
            $("." + $(this).attr('id')).parent().nextAll('.number_box').find('.inp_box').children('.w50').attr("disabled", true);
            $("." + $(this).attr('id')).parent().nextAll('.number_box').find('.inp_box').children('.wp70').val("");
            $("." + $(this).attr('id')).parent().nextAll('.number_box').find('.inp_box').children('.wp70').attr("disabled", true);
            $("." + $(this).attr('id')).parent().nextAll('.number_box').find('.tip_box').children('input').prop('checked', false);
            $("." + $(this).attr('id')).parent().nextAll('.number_box').find('.tip_box').children('input').attr("disabled", true);
        } else {
            $("." + $(this).attr('id')).val($(this).find("option:selected").text());
            $("." + $(this).attr('id')).hide();
            $("." + $(this).attr('id')).next().removeClass("wp43");
            $("." + $(this).attr('id')).next().addClass("wp70");

            $("." + $(this).attr('id')).next('.wp70').attr("disabled", false);
            $("." + $(this).attr('id')).parent().nextAll('.number_box').find('.inp_box').children('.w50').attr("disabled", false);
            $("." + $(this).attr('id')).parent().nextAll('.number_box').find('.inp_box').children('.wp70').attr("disabled", false);
            $("." + $(this).attr('id')).parent().nextAll('.number_box').find('.tip_box').children('input').attr("disabled", false);
        }
    });


    $("#Weddinghall_Name").on("keyup", function () {
        $('#incNaverMap')[0].contentWindow.changeMarker();
    });
});
function popGreetingSample() {
    window.open("/popup/pop_GreetingSearch01.html", "_greeting1");
}
function execDaumPostcode() {

    new daum.Postcode({
        oncomplete: function (data) {
            var fullAddr = data.address;
            document.getElementById('Weddinghall_Address').value = fullAddr;
            $('.Weddinghall_Address').html("");
            $('#incNaverMap')[0].contentWindow.codeAddress();
        },
        width: '100%',
        height: '100%'
    }).open();
}
function fn_add_SNS_Image() {
    $('#uploadFrm').get(0).reset();
    $("#snsfile").click();
    $("#snsfile").unbind();
    $('#snsfile').change(function (e) {
        var form = $('#uploadFrm')[0];
        var formData = new FormData(form);

        var name = document.getElementById("snsfile").files[0].name;
        if (!(/png|jpe?g/i).test(name)) {
            alert('jpg, png 파일만 등록이 가능합니다.');
            return false;
        }
        var size = document.getElementById("snsfile").files[0].size;
        if (size > 13631489) {
            alert('파일 용량은 13MB를 초과할 수 없습니다.');
            return false;
        }

        formData.append("upload_path", $('#upload_path').val());
        formData.append("Invitation_Id", $("#Invitation_Id").val());

        $(".loader").show();

        $.ajax({
            url: "/Order/McardStep1_Upload_SNS_Image",
            data: formData,
            type: 'POST',
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            dataType: 'json',
            cache: false,
            success: function (result) {
                if (result.success == "Y") {
                    $(".sns_img").attr('src', result.resource_url).css("max-height", "159px");
                    $(".sns_img").attr('orgimg', result.resource_url);
                    $("#sns_img .img_preview").show();
                    $("#sns_img").find('label').hide();
                    $(".loader").hide();
                } else {
                    $(".loader").hide();
                    if (!result.auth) {
                        location.reload();
                    } else {
                        $(".error").css("display", "flex").delay(1000).fadeOut();
                    }
                }
            }
        });

    });
}
function fn_add_Main_Image() {
    $('#uploadFrm').get(0).reset();
    $("#mainfile").click();
    $("#mainfile").unbind();
    $('#mainfile').change(function (e) {
        var form = $('#uploadFrm')[0];
        var formData = new FormData(form);

        var name = document.getElementById("mainfile").files[0].name;
        if (!(/png|jpe?g/i).test(name)) {
            alert('jpg, png 파일만 등록이 가능합니다.');
            return false;
        }
        var size = document.getElementById("mainfile").files[0].size;
        if (size > 13631489) {
            alert('파일 용량은 13MB를 초과할 수 없습니다.');
            return false;
        }

        formData.append("upload_path", $('#upload_path').val());
        formData.append("Invitation_Id", $("#Invitation_Id").val());
        $(".loader").show();
        $.ajax({
            url: "/Order/McardStep1_Upload_Main_Image",
            data: formData,
            type: 'POST',
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            dataType: 'json',
            cache: false,
            success: function (result) {
                if (result.success == "Y") {
                    $(".main_img").attr('src', result.resource_url).css("max-height", "159px");
                    $(".main_img").attr('orgimg', result.resource_url);
                    $("#main_img .img_preview").show();
                    $("#main_img").find('label').hide();
                    $(".loader").hide();
                } else {
                    $(".loader").hide();
                    if (!result.auth) {
                        location.reload();
                    } else {
                        $(".error").css("display", "flex").delay(1000).fadeOut();
                    }
                }
            }
        });

    });
}
function cropImage(id, url) {
    image_select = id;
    var modal_con = '';

    modal_con += "<div class=\"crop_container\" >";
    modal_con += "<img id=\"crop_" + id + "\" src=\"" + url + "\" style=\" max-height: 300px;\">";
    modal_con += "</div>";

    var scale_x = 1;
    var scale_y = 1;

    if (id == "main_img") {
        var w = $("#photo_width").val();
        var h = $("#photo_height").val();

        if (w < h) {
            scale_x = w / h
            scale_y = 1
        } else {
            scale_x = 1
            scale_y = h / w
        }
    }

    $("." + id.replace("img", "pop") + " .img_box").html(modal_con);

    var image = document.getElementById("crop_" + id);

    var minWidth = $('.layer_pop .img_box').width() - 100;
    var minHeight = minWidth * scale_y;
    if (id == "main_img") {
        cropper = new Cropper(image, {
            aspectRatio: scale_x / scale_y,
            minContainerWidth: minWidth,
            minContainerHeight: minHeight,
        });
    } else {
        cropper = new Cropper(image, {
            minContainerWidth: minWidth,
            minContainerHeight: minHeight,
        });
    }
}
function CompleteCrop(type) {
    var img_url = cropper.getCroppedCanvas().toDataURL().replace(/^data[:]image\/(png|jpg|jpeg)[;]base64,/i, "");
    fn_CropImageUpload(img_url, type);
}
function fn_CropImageUpload(img_url, type) {

    $.ajax({
        url: '/Invitation/CropImageUpload',
        data: {
            "imageData": img_url,
            "type": type,
            "id": $("#Invitation_Id").val(),
            "url": $("#" + image_select + " ." + image_select).attr("orgimg")
        },
        type: 'POST',
        success: function (result) {
            if (result.success == "Y") {
                //$("#" + image_select + " ." + image_select).attr("src", $("#" + image_select + " ." + image_select).attr("orgimg") + "?" + uuidv4());

                $("#" + image_select + " ." + image_select).attr("src", result.path);
                

                popClose();
            } else {
                if (!result.auth) {
                    location.reload();
                } else {
                    $(".error").css("display", "flex").delay(1000).fadeOut();
                }
            }
        }
    });
}
function uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
function hiddenFile() {
    var naverMap = $(".input_navermap").html();
    var naverMap2 = $("#map_canvas").html();
    var viewfileSH = $("#viewfile").is(":visible");
    if (viewfileSH == true) $("#incNaverMap").attr("src", $("#incNaverMap").attr("src"));
    if (viewfileSH == true) $("#map_canvas").html(naverMap2);
    $(".map_box").hide();
    $(".input_imgmap").hide();
    $(".input_navermap").show();
    $("#map_tip").show();
    $("#Outline_Type_Code").val("OTC01");
}
function viewFile() {
    var imgMap = $(".input_imgmap").html();
    $(".mark").focus();
    $(".map_box").show();
    $(".input_imgmap").show();
    $(".input_navermap").hide();
    $("#map_tip").hide();
    $("#Outline_Type_Code").val("OTC02");
}
function fn_add_Outline_Image() {
    $('#uploadFrm').get(0).reset();
    $("#mapfile").click();
    $("#mapfile").unbind();
    $('#mapfile').change(function (e) {
        var form = $('#uploadFrm')[0];
        var formData = new FormData(form);

        var name = document.getElementById("mapfile").files[0].name;
        if (!(/png|jpe?g/i).test(name)) {
            alert('jpg, png 파일만 등록이 가능합니다.');
            return false;
        }
        var size = document.getElementById("mapfile").files[0].size;
        if (size > 13631489) {
            alert('파일 용량은 13MB를 초과할 수 없습니다.');
            return false;
        }

        formData.append("upload_path", $('#upload_path').val());
        $(".loader").show();
        $.ajax({
            url: "/Order/McardStep1_Upload_Outline_Image",
            data: formData,
            type: 'POST',
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            dataType: 'json',
            cache: false,
            success: function (result) {
                if (result.success == "Y") {
                    $("#Outline_Image").attr('src', result.resource_url).css("max-width", "100%");
                    $("#Outline_Image_URL").val(result.resource_url);
                    viewFile();
                    $(".loader").hide();
                } else {
                    $(".loader").hide();

                    if (!result.auth) {
                        location.reload();
                    } else {
                        $(".error").css("display", "flex").delay(1000).fadeOut();
                    }
                }
            }
        });

    });
}
function validation_check() {

    if (fn_trim($("#Name").val()) == "") {
        alert("주문자명을 입력하세요.");
        $("#Name").focus();
        return false;
    }

    if (fn_trim($("#CellPhone_Number").val()) == "") {
        alert("휴대폰 번호를 입력하세요.");
        $("#CellPhone_Number").focus();
        return false;
    }

    if (fn_trim($("#Email_Account").val()) == "") {
        alert("이메일 주소를 입력하세요.");
        $("#Email_Account").focus();
        return false;
    }

    if ($("#selEmail option:selected").index() == 0) {
        if (fn_trim($("#Email_Address").val()) == "") {
            alert("이메일 주소를 입력하세요.");
            $("#Email_Address").focus();
            return false;
        }
    } 

    if (fn_trim($("#Invitation_URL").val()) == "") {
        alert("초대장 url을 입력하세요.");
        $("#Invitation_URL").focus();
        return false;
    } else {
        var regType1 = /^[A-Za-z0-9_-]*$/;

        if (!regType1.test(fn_trim($("#Invitation_URL").val()))) {
            $(".url_check").removeClass("true");
            $(".url_check").addClass("false");
            $(".url_text").text("사용할 수 없는 주소입니다.");

            $("#Invitation_URL").focus();
            return false;
        }
    }

    if (fn_trim($("#Invitation_Title").val()) == "") {
        alert("초대장 제목을 입력하세요.");
        $("#Invitation_Title").focus();
        return false;
    }

    if (fn_trim($("#Groom_Name").val()) == "") {
        alert("신랑 이름을 입력하세요.");
        $("#Groom_Name").focus();
        return false;
    }

    if (fn_trim($("#Bride_Name").val()) == "") {
        alert("신부 이름을 입력하세요.");
        $("#Bride_Name").focus();
        return false;
    }

    if (fn_trim($("#WeddingHour").val()) == "") {
        alert("예식 시간을 입력하세요.");
        $("#WeddingHour").focus();
        return false;
    }

    if (fn_trim($("#WeddingMin").val()) == "") {
        alert("예식 시간을 입력하세요.");
        $("#WeddingMin").focus();
        return false;
    }

    if ($("#Parents_Information_Use_YN").val() == "Y") {

        if ($("#sel_g1 option:selected").index() == 2 && fn_trim($("#Groom_Parents1_Title").val()) == "") {
            alert("혼주정보를 입력하세요.");
            $("#Groom_Parents1_Title").focus();
            return false;
        }
        if ($("#sel_g1 option:selected").index() != 3 && fn_trim($("#Groom_Parents1_Name").val()) == "") {
            alert("혼주정보를 입력하세요.");
            $("#Groom_Parents1_Name").focus();
            return false;
        }

        if ($("#sel_g2 option:selected").index() == 2 && fn_trim($("#Groom_Parents2_Title").val()) == "") {
            alert("혼주정보를 입력하세요.");
            $("#Groom_Parents2_Title").focus();
            return false;
        }
        if ($("#sel_g2 option:selected").index() != 3 && fn_trim($("#Groom_Parents2_Name").val()) == "") {
            alert("혼주정보를 입력하세요.");
            $("#Groom_Parents2_Name").focus();
            return false;
        }

        if ($("#sel_b1 option:selected").index() == 2 && fn_trim($("#Bride_Parents1_Title").val()) == "") {
            alert("혼주정보를 입력하세요.");
            $("#Bride_Parents1_Title").focus();
            return false;
        }
        if ($("#sel_b1 option:selected").index() != 3 && fn_trim($("#Bride_Parents1_Name").val()) == "") {
            alert("혼주정보를 입력하세요.");
            $("#Bride_Parents1_Name").focus();
            return false;
        }

        if ($("#sel_b2 option:selected").index() == 2 && fn_trim($("#Bride_Parents2_Title").val()) == "") {
            alert("혼주정보를 입력하세요.");
            $("#Bride_Parents2_Title").focus();
            return false;
        }
        if ($("#sel_b2 option:selected").index() != 3 && fn_trim($("#Bride_Parents2_Name").val()) == "") {
            alert("혼주정보를 입력하세요.");
            $("#Bride_Parents2_Name").focus();
            return false;
        } 
    }

    return true;
}
function fn_save() {
    $(".loader").show();
    var order = new Object();
    var invitation_detail = new Object();

    var invitation_detail_etcs = [];

    var idx = 0;
    $(".info_reg_list tr").each(function () {
        idx++;
        var invitation_detail_etc = new Object();
        invitation_detail_etc.Invitation_ID = $("#Invitation_Id").val();
        invitation_detail_etc.Sort = idx;
        if (idx < 3) {
            invitation_detail_etc.Etc_Title = $(this).find(".etc_title").html();
        } else {
            invitation_detail_etc.Etc_Title = $(this).find(".etc_title").val();
        }
        invitation_detail_etc.Information_Content = $(this).find(".etc_contents").val();

        //if (invitation_detail_etc.Etc_Title != "" && invitation_detail_etc.Information_Content != "") {
        if (invitation_detail_etc.Information_Content != "" && idx < 3) {
            invitation_detail_etcs.push(invitation_detail_etc);
        }
        else if (invitation_detail_etc.Etc_Title != "" && idx > 2) {
            invitation_detail_etcs.push(invitation_detail_etc);
        }
    });

    var account_extras = [];
    idx = 0;
    $(".account_list tr").each(function () {
        idx++;
        var account_extra = new Object();
        account_extra.Invitation_ID = $("#Invitation_Id").val();
        account_extra.Sort = idx;
        account_extra.Send_Target_Code = $(this).find(".sel_sender").val();
        account_extra.Send_Name = $(this).find(".input_sender").val();
        account_extra.Bank_Code = $(this).find(".sel_bank").val();
        account_extra.Account_Number = $(this).find(".input_accountnumber").val();
        account_extra.Account_Holder = $(this).find(".input_accountholder").val();
        if ($(this).find(".sel_sender").val() != "" || $(this).find(".input_sender").val() != "" || $(this).find(".sel_bank").val() != "" || $(this).find(".input_accountnumber").val() != "" || $(this).find(".input_accountholder").val() != "") {
            account_extras.push(account_extra);
        }
    });

    var invitation_accounts = [];
    idx = 0;
    $(".account_groom_list tr").each(function () {
        idx++;
        var invitation_account = new Object();
        invitation_account.Invitation_ID = $("#Invitation_Id").val();
        invitation_account.Sort = idx;
        invitation_account.Send_Target_Code = $(this).find(".sel_groom_sender").val();
        invitation_account.Send_Name = $(this).find(".input_groom_sender").val();
        invitation_account.Bank_Code = $(this).find(".sel_groom_bank").val();
        invitation_account.Account_Number = $(this).find(".input_groom_accountnumber").val();
        invitation_account.Account_Holder = $(this).find(".input_groom_accountholder").val();
        invitation_account.Category = 1;

        if ($(this).find(".sel_groom_sender").val() != "" || $(this).find(".input_groom_sender").val() != "" || $(this).find(".sel_groom_bank").val() != "" || $(this).find(".input_groom_accountnumber").val() != "" || $(this).find(".input_groom_accountholder").val() != "") {
            invitation_accounts.push(invitation_account);
        }
    });

    idx = 0;
    $(".account_bride_list tr").each(function () {
        idx++;
        var invitation_account = new Object();
        invitation_account.Invitation_ID = $("#Invitation_Id").val();
        invitation_account.Sort = idx;
        invitation_account.Send_Target_Code = $(this).find(".sel_bride_sender").val();
        invitation_account.Send_Name = $(this).find(".input_bride_sender").val();
        invitation_account.Bank_Code = $(this).find(".sel_bride_bank").val();
        invitation_account.Account_Number = $(this).find(".input_bride_accountnumber").val();
        invitation_account.Account_Holder = $(this).find(".input_bride_accountholder").val();
        invitation_account.Category = 2;
        if ($(this).find(".sel_bride_sender").val() != "" || $(this).find(".input_bride_sender").val() != "" || $(this).find(".sel_bride_bank").val() != "" || $(this).find(".input_bride_accountnumber").val() != "" || $(this).find(".input_bride_accountholder").val() != "") {
            invitation_accounts.push(invitation_account);
        }
    });

    order.order_id = $("#Order_Id").val();
    order.Name = $("#Name").val();
    order.CellPhone_Number = $("#CellPhone_Number").val();
    order.Email = $("#Email_Account").val() + "%" + $("#Email_Address").val();

    invitation_detail.invitation_id = $("#Invitation_Id").val();
    invitation_detail.Invitation_Title = $("#Invitation_Title").val();
    invitation_detail.Invitation_URL = $("#Invitation_URL").val();
    invitation_detail.greetings = $("#Greetings").val();
    invitation_detail.groom_name = $("#Groom_Name").val();
    invitation_detail.Groom_Global_Phone_YN = $("#Groom_Global_Phone_YN").is(":checked") ? "Y" : "N";
    invitation_detail.Groom_Global_Phone_Number = $("#Groom_Global_Phone_Number").val();
    invitation_detail.groom_phone = $("#Groom_Phone").val();
    invitation_detail.bride_name = $("#Bride_Name").val();
    invitation_detail.Bride_Global_Phone_YN = $("#Bride_Global_Phone_YN").is(":checked") ? "Y" : "N";
    invitation_detail.Bride_Global_Phone_Number = $("#Bride_Global_Phone_Number").val();
    invitation_detail.bride_phone = $("#Bride_Phone").val();
    invitation_detail.groom_parents1_name = $("#Groom_Parents1_Name").val();
    invitation_detail.Groom_Parents1_Global_Phone_Number_YN = $("#Groom_Parents1_Global_Phone_Number_YN").is(":checked") ? "Y" : "N";
    invitation_detail.Groom_Parents1_Global_Phone_Number = $("#Groom_Parents1_Global_Phone_Number").val();
    invitation_detail.groom_parents1_phone = $("#Groom_Parents1_Phone").val();
    invitation_detail.groom_parents2_name = $("#Groom_Parents2_Name").val();
    invitation_detail.Groom_Parents2_Global_Phone_Number_YN = $("#Groom_Parents2_Global_Phone_Number_YN").is(":checked") ? "Y" : "N";
    invitation_detail.Groom_Parents2_Global_Phone_Number = $("#Groom_Parents2_Global_Phone_Number").val();
    invitation_detail.groom_parents2_phone = $("#Groom_Parents2_Phone").val();
    invitation_detail.bride_parents1_name = $("#Bride_Parents1_Name").val();
    invitation_detail.Bride_Parents1_Global_Phone_Number_YN = $("#Bride_Parents1_Global_Phone_Number_YN").is(":checked") ? "Y" : "N";
    invitation_detail.Bride_Parents1_Global_Phone_Number = $("#Bride_Parents1_Global_Phone_Number").val();
    invitation_detail.bride_parents1_phone = $("#Bride_Parents1_Phone").val();
    invitation_detail.bride_parents2_name = $("#Bride_Parents2_Name").val();
    invitation_detail.Bride_Parents2_Global_Phone_Number_YN = $("#Bride_Parents2_Global_Phone_Number_YN").is(":checked") ? "Y" : "N";
    invitation_detail.Bride_Parents2_Global_Phone_Number = $("#Bride_Parents2_Global_Phone_Number").val();
    invitation_detail.bride_parents2_phone = $("#Bride_Parents2_Phone").val();

    invitation_detail.Groom_Parents1_Title = $("#Groom_Parents1_Title").val();
    invitation_detail.Groom_Parents2_Title = $("#Groom_Parents2_Title").val();
    invitation_detail.Bride_Parents1_Title = $("#Bride_Parents1_Title").val();
    invitation_detail.Bride_Parents2_Title = $("#Bride_Parents2_Title").val();


    invitation_detail.Location_LAT = $("#lat").val();
    invitation_detail.Location_LOT = $("#lot").val();
    invitation_detail.Outline_Type_Code = $("#Outline_Type_Code").val();
    invitation_detail.Outline_Image_URL = $("#Outline_Image_URL").val();

    invitation_detail.GuestBook_Use_YN = $("#GuestBook_Use_YN").val();
    invitation_detail.Etc_Information_Use_YN = $("#Etc_Information_Use_YN").val();
    invitation_detail.Parents_Information_Use_YN = $("#Parents_Information_Use_YN").val();
    invitation_detail.MoneyGift_Remit_Use_YN = $("#MoneyGift_Remit_Use_YN").val();
    invitation_detail.MoneyAccount_Remit_Use_YN = $("#MoneyAccount_Remit_Use_YN").val();
    invitation_detail.MoneyAccount_Div_Use_YN = $("#MoneyAccount_Div_Use_YN").val();
    invitation_detail.Invitation_Video_Use_YN = $("#Invitation_Video_Use_YN").val();
    invitation_detail.Gallery_Use_YN = $("#Gallery_Use_YN").val();

    invitation_detail.Gallery_Use_YN = $("#Gallery_Use_YN").val();
    invitation_detail.Gallery_Type_Code = $("#Gallery_Type_Code").val();
    invitation_detail.Invitation_Video_Type_Code = $("#Invitation_Video_Type_Code").val();



    invitation_detail.weddingdate = $("#WeddingDate").val();
    invitation_detail.weddinghhmm = $("#WeddingHour").val() + $("#WeddingMin").val();
    invitation_detail.time_type_code = $("#Time_Type_Code").val();
    invitation_detail.weddingyy = $('#WeddingYY').val();
    invitation_detail.weddingmm = $('#WeddingMM').val();
    invitation_detail.weddingdd = $('#WeddingDD').val();
    invitation_detail.weddingweek = $('#WeddingWeek').val();
    invitation_detail.weddinghour = $("#WeddingHour").val();
    invitation_detail.weddingmin = $("#WeddingMin").val();

    invitation_detail.weddinghall_name = $("#Weddinghall_Name").val();
    invitation_detail.weddinghalldetail = $("#WeddingHallDetail").val();
    invitation_detail.weddinghall_address = $("#Weddinghall_Address").val();
    invitation_detail.weddinghall_phonenumber = $("#Weddinghall_PhoneNumber").val();
    invitation_detail.DetailNewLineYN = $("#DetailNewLineYN").is(":checked") ? "Y" : "N";

    invitation_detail.Conf_KaKaoPay_YN = $("#MoneyGift_Remit_Use_YN").val();
    invitation_detail.Conf_Remit_YN = $("#MoneyAccount_Remit_Use_YN").val() == "Y" || $("#MoneyAccount_Div_Use_YN").val() == "Y" ? "Y" : "N";
    invitation_detail.Flower_gift_YN = $("#Flower_gift_YN").val();

    var myData = {
        "order": order,
        "invitation_detail": invitation_detail,
        "invitation_detail_etcs": invitation_detail_etcs,
        "account_extras": account_extras,
        "invitation_accounts": invitation_accounts 
    };



    $.ajax({
        url: "/Order/McardStep1_Save",
        data: myData,
        type: 'POST',
        dataType: 'json',
        success: function (result) {
            if (result.success == "Y") {
                setTimeout(function () {
                    location.href = "/Order/m_McardStep2/" + $("#Order_Id").val();
                }, 1000);
            } else {
                if (result.auth) {
                    //중복시
                    $(".loader").hide();
                    $(".url_check").removeClass("true");
                    $(".url_check").addClass("false");
                    $(".url_text").text("사용할 수 없는 주소입니다.");
                    $("#Invitation_URL").focus();
                } else {
                    location.reload();
                }
            }
        }
    });
}
function checkDuplicateURL(val) {
    var mydata = {
        "invitation_url": val,
        "invitation_id": $("#Invitation_Id").val()
    };
    $.ajax({
        type: "POST",
        url: "/Invitation/CheckDuplicateURL",
        data: mydata,
        dataType: "json",
        success: function (result) {
            if (result.success == "Y") {
                //사용가능 시
                $(".url_check").removeClass("false");
                $(".url_check").addClass("true");
                $(".url_text").text("사용할 수 있는 주소입니다.");
            } else {
                //중복시
                $(".url_check").removeClass("true");
                $(".url_check").addClass("false");
                $(".url_text").text("사용할 수 없는 주소입니다.");
                $("#Invitation_URL").focus();
            }
        }
    });
}

function fn_replace(str, searchStr, replaceStr) {
    return str.split(searchStr).join(replaceStr);
}
function fn_trim(str) {
    str = str.replace(/^\s*/, '').replace(/\s*$/, '');
    return str;
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

//연락처 문자열 제거
var chkNumber = function (obj) {
    $(obj).val($(obj).val().replace(/[^0-9]/gi, ""));
}
//var
var image_select = "";
var realW = 0;
var realH = 0;
var scaleW = 0;
var scaleH = 0;

$(document).ready(function () {
    $('.preview_box .img_con').on('scroll touchmove mousewheel', function (event) {
        var prevScroll = $(this).scrollTop();
        var innerHeight = $(this).innerHeight();
        var scrollHeight = $(this).prop('scrollHeight');

        if (prevScroll + innerHeight >= scrollHeight) {
            event.preventDefault();
            event.stopPropagation();
            return false;
        } else {
            $('.preview_box .img_con').unbind();
        }

    });
    $(".input_match").on("keyup focus", function () {
        var keyInput = $(this).attr("id");
        var idx = $(this).attr("idx");
        var inputValue = $(this).val();
        var preview_frame = document.getElementById("preview_frame");

        preview_frame.contentWindow.fn_match(keyInput, inputValue);

        var position = $('#preview_frame').contents().find('#area' + idx).offset();

        if (typeof (position) != "undefined") {
            $('#preview_frame').contents().scrollTop(position.top);
        }
    });
    $(".input_match_ex1").on("keyup focus", function () {
        var id1 = $(this).parents(".number_box").find('input').eq(0).attr('id');
        var val1 = $(this).parents(".number_box").find('input').eq(0).val();
        var id2 = $(this).parents(".number_box").find('input').eq(1).attr('id');
        var val2 = $(this).parents(".number_box").find('input').eq(1).val();


        var chk = $(this).attr("type") == "checkbox" ? !$(this).parents(".number_box").find('input').eq(2).is(":checked") : $(this).parents(".number_box").find('input').eq(2).is(":checked");

        var preview_frame = document.getElementById("preview_frame");

        preview_frame.contentWindow.fn_match_extra1(id1, val1, id2, val2, chk);

        var position = $('#preview_frame').contents().find('#area4').offset();
        $('#preview_frame').contents().scrollTop(position.top);
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
    $("#selEmail option").each(function () {
        if ($(this).val() == $("#Email_Address").val()) {
            $(this).prop('selected', true);
            $("#Email_Address").hide();
        }
    });
    $("#btn_next").click(function () {
        if (validation_check()) {
            fn_save();
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
                        if (id == "main_img") {
                            $('#preview_frame').contents().find(".item.photo img").attr("src", null);
                            var position = $('#preview_frame').contents().find('#area1').offset();
                            $('#preview_frame').contents().scrollTop(position.top);
                        }
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
    $('.crop.ico').on('click', function () {

        var id = $(this).parents('li').attr('id');
        var url = $("." + id).attr('src')
        if (typeof (url) != "undefined") {
            $('.pop_wrap.' + id.replace("img","pop")).show();
            cropImage(id, url);
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

    $("#selEmail").on('change', function () {
        if ($("#selEmail option:selected").index() > 0) {
            $("#Email_Address").val($("#selEmail option:selected").val());
            $("#Email_Address").hide();
        } else {
            $("#Email_Address").val("");
            $("#Email_Address").show();
        }
    });
    $('.sample_save').on('click', function () {
        $("#Greetings").val(fn_trim(fn_replace($(this).prev("p").html(), "<br>", "\n")));
        $("#Greetings").focus();
        popClose();
    });
});
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

        if (invitation_detail_etc.Etc_Title != "" && invitation_detail_etc.Information_Content != "") {
            invitation_detail_etcs.push(invitation_detail_etc);
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


    var myData = {
        "order": order,
        "invitation_detail": invitation_detail
    };

    $.ajax({
        url: "/Order/MthanksStep1_Save",
        data: myData,
        type: 'POST',
        dataType: 'json',
        success: function (result) {
            if (result.success == "Y") {
                //$(".loader").hide();
               // $(".saved").css("display", "flex");

                setTimeout(function () {
                    location.href = "/Order/StepLast/" + $("#Order_Id").val();
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
function fn_add_Main_Image() {
    $('#uploadFrm').get(0).reset();
    $("#mainfile").click();
    $("#mainfile").unbind();
    $('#mainfile').change(function (e) {
        var form = $('#uploadFrm')[0];
        var formData = new FormData(form);

        formData.append("upload_path", $('#upload_path').val());
        formData.append("Invitation_Id", $("#Invitation_Id").val());

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

                    var h = $('#preview_frame').contents().find(".item.photo").css('height');
                    var w = $('#preview_frame').contents().find(".item.photo").css('width');
                    $('#preview_frame').contents().find(".item.photo img").attr("src", result.resource_url).css("max-width", w + "px").css("max-height", h + "px")
                    var position = $('#preview_frame').contents().find('#area1').offset();
                    $('#preview_frame').contents().scrollTop(position.top);
                    $(".loader").hide();
                }  else {
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
function validation_check() {

    if (fn_trim($("#Name").val()) == "") {
        alert("주문자명을 입력하세요.");
        $("#Name").focus();
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

    if (fn_trim($("#CellPhone_Number").val()) == "") {
        alert("휴대폰 번호를 입력하세요.");
        $("#CellPhone_Number").focus();
        return false;
    }

    if (fn_trim($("#Invitation_URL").val()) == "") {
        alert("감사장 url을 입력하세요.");
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
        alert("감사장 제목을 입력하세요.");
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


    return true;

}
function cropImage(id, url) {
    image_select = id;
    var modal_con = '';

    modal_con += "<div class=\"crop_container\" >";
    modal_con += "<img id=\"crop_" + id + "\" src=\"" + url + "\" style=\" max-height: 500px;\">";
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
                

                if (type == "main") {
                    var h = $('#preview_frame').contents().find(".item.photo").css('height');
                    var w = $('#preview_frame').contents().find(".item.photo").css('width');
                    $('#preview_frame').contents().find(".item.photo img").attr("src", $("#" + image_select + " ." + image_select).attr("src")).css("max-width", w + "px").css("max-height", h + "px");
                    var position = $('#preview_frame').contents().find('#area1').offset();
                    $('#preview_frame').contents().scrollTop(position.top);
                }
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

//국제전화 툴팁, 국제코드 공통
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

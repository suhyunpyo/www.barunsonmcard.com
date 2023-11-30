//const
var maxWidth = $("#crop_img").css("max-width").replace("px", "");
var maxHeight = $("#crop_img").css("max-height").replace("px", "");
//var
var image_select = "";
var realW = 0;
var realH = 0;
var scaleW = 0;
var scaleH = 0;
var upload_img_cnt = 0;
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
    //이미지 관리 사용여부
    if ($("#Gallery_Use_YN").val() == "Y") {
        $('#Gallery_Use_Y').prop('checked', true);
    } else {
        $('#Gallery_Use_N').prop('checked', true);
        $('.img_type_list').parents('.order_detail').hide();
    }
    //이미지 타입
    $(".img_type_list a").each(function () {
        var id = $("#Gallery_Type_Code").val();

        if ($(this).attr('id') == id) {
            $(this).addClass('active');
        } else {
            $(this).removeClass('active');
        }
    });
    //초대영상 사용여부
    if ($("#Invitation_Video_Use_YN").val() == "Y") {
        $('#Invitation_Video_Use_Y').prop('checked', true);
    } else {
        $('#Invitation_Video_Use_N').prop('checked', true);
        $('.video_list').parents('.order_detail').hide();
    }
    //초대영상 타입
    $("input[name=Invitation_Video_Type_Code]").each(function () {

        var id = $("#Invitation_Video_Type_Code").val();

        if ($(this).attr('id') == id) {

            $(this).prop('checked', true);

            switch (id) {
                case "VTC01"://Youtube
                    $("#url_" + id).val($("#Invitation_Video_URL").val());
                    $("#tr_VTC01").show();
                    break;
                case "VTC02": //Vimeo
                    $("#url_" + id).val($("#Invitation_Video_URL").val());
                    $("#tr_VTC02").show();
                    break;
                case "VTC03"://FEELMAKER
                    $("#url_" + id).val($("#Invitation_Video_URL").val());
                    $("#tr_VTC03").show();
                    break;
            }

        } else {
            $(this).prop('checked', false);
        }
    });

    $(".sort_wrap.sort_drag").disableSelection();
    if ($(".gallery_list").length >= 18) { $(".no_img").hide(); }
    denyEnter();
    $('.img_preview').on('click', function () {
        var $prevImg = $(this).find('img');
        if ($prevImg.length > 0) {

            var item = $(this).parents("li");

            item.toggleClass('on');
        }

        initSelectAll();
    });
    $(".check_all").on("click", function () {
        if ($(this).is(":checked")) {
            //전체선택
            $(".gallery_list").addClass("on");
        } else {
            //전체해제
            $(".gallery_list").removeClass("on");
        }
    });
    $("#gallery_file").fileupload({
        autoUpload: true,
        dataType: "json",
        dropZone: $(".img_drag"),
        url: "/Invitation/GalleryFileUpload",
        sequentialUploads: true,
        limitMultiFileUploads: 18 - $(".gallery_list").length,
        formData: {
            Invitation_Id: $("#Invitation_Id").val(),
            upload_path: $("#upload_path").val()
        },
        add: function (e, data) {
            upload_img_cnt++;

            if ($('.pop_wrap.upload').css("display") == "none") {
                prog = 0;
                cntErrorCount = 0;
                cntErrorSize = 0;
                cntErrorType = 0;
                cntErrorEtc = 0;
                $("#popErrors p").not($("#popErrors p").last()).remove()
                $("#popErrors").hide();
                $('.pop_wrap.upload').show();
            }
            var uploadFile = data.files[0];
            var gallery_img_cnt = $(".gallery_list").length + upload_img_cnt;
            var isValid = true;
            if (!(/png|jpe?g/i).test(uploadFile.name)) {
                cntErrorType++;
                addUploadError();
                isValid = false;
            } else if (uploadFile.size > 13631489) {
                cntErrorSize++;
                addUploadError();
                isValid = false;
            } else if (gallery_img_cnt > 18) {
                cntErrorCount++;
                addUploadError();
                isValid = false;
            }

            if (isValid) {
                data.submit();
            }
        },
        progressall: function (e, data) {
            prog = parseInt(data.loaded / data.total * 100, 10);
            $("#popProgress").css("width", prog + "%");

        },
        done: function (e, data) {

            if (data.result.success == "Y") {
                $("li.no_img").before(gallery_elem);

                $(".gallery_list").last().attr("id", "gl_" + data.result.gallery_id);
                $(".gallery_list").last().find(".gallery_img").attr("src", data.result.resource_url).css("max-width", "159px").css("max-height", "159px");
                $(".gallery_list").last().find(".gallery_img").attr("orgimg", data.result.resource_url);
                $(".gallery_list").last().find(".crop.ico").on("click", function () {
                    var idx = $(this).parents('li').index();
                    $(this).parents('.sort_wrap').find($('.pop_wrap.crop')).show();

                    var id = $(this).parents('li').attr('id');
                    var url = $('.gallery_img').eq(idx).attr('src');
                    cropImage(id, url);

                });
                $(".gallery_list").last().find(".delete.ico").on("click", function () {
                    var id = $(this).parents('li').attr('id').replace("gl_", "");
                    removeGalleryItem(id);
                });

                $(".gallery_list").last().find('.img_preview').on("click", function () {
                    var item = $(this).parents("li");
                    item.toggleClass("on");

                    if ($('.gallery_list.on').length != $('.gallery_list').length) {
                        $(".check_all").prop("checked", false);
                    } else {
                        $(".check_all").prop("checked", true);
                    }

                });

                syncPreviewGallery();

                if ($(".gallery_list").length >= 18) {
                    $(".no_img").hide();
                }

                initSelectAll();
                upload_img_cnt = 0;
            } else {
                if (data.result.auth) {
                    alert('에러 : ' + data.result.message);
                    upload_img_cnt = 0;
                } else {
                    location.reload();
                }
            }
            if (prog == 100) {
                if ($("#popErrors p").length < 1) {
                    popClose_p();
                    prog = 0;
                }
            }
        }
    });
    $('#sort_list').sortable({
        opacity: 0.8,
        items: "li:not(.no_img)",
        handle: ".move",
        update: function (event, ui) {
            changeSort($(".gallery_list").eq(ui.item.index()).attr("id").replace("gl_", ""), ui.item.index() + 1);
        }
    });
    $(".delete.ico").on("click", function () {
        var id = $(this).parents('li').attr('id').replace("gl_", "");
        removeGalleryItem(id);
    });
    $(".select_del_btn").on("click", function () {
        $(".gallery_list.on").each(function () {
            var id = $(this).attr("id").replace("gl_", "");
            removeGalleryItem(id);
        });
    });
    $("#btn_next").click(function () {
        if (validation_check()) {
            fn_save("next");
        }
    });
    $("#btn_prev").click(function () {
        if (validation_check()) {
            fn_save("prev");
        }
    });
    $(".crop.ico").on("click", function (event) {
        var idx = $(this).parents('li').index();
        $(this).parents('.sort_wrap').find($('.pop_wrap.crop')).show();
        var id = $(this).parents('li').attr('id');
        var url = $('.gallery_img').eq(idx).attr('src');
        cropImage(id, url);
    });
    $("input[name=Invitation_Video_Type_Code]").on("click", function (event) {
        var id = $(this).attr('id');
        $("#tr_VTC01").hide();
        $("#tr_VTC02").hide();
        $("#tr_VTC03").hide();

        switch (id) {
            case "VTC01"://Youtube
                if ($("#url_" + id).val().indexOf("youtu") < 0) {
                    $("#url_" + id).val("");
                }
                $("#tr_" + id).show();
                var pattern = /(?:http?s?:\/\/)?(?:www\.)?(?:youtube\.com|youtu\.be)\/(?:watch\?v=)?(.+)/g;

                var htmlInput = $("#url_" + id).val();

                $("#Invitation_Video_URL").val($("#url_" + id).val());

                if (pattern.test(htmlInput)) {
                    var replacement = '<iframe src="//www.youtube.com/embed/$1" frameborder="0" class="embed-container" allowfullscreen></iframe>';
                    htmlInput = htmlInput.replace(pattern, replacement);
                }

                $('#preview_frame').contents().find(".iframe_wrap").html(htmlInput);
                $("#Invitation_Video_Type_Code").val(id);

                break;
            case "VTC02": //Vimeo
                if ($("#url_" + id).val().indexOf("vimeo") < 0) {
                    $("#url_" + id).val("");
                }
                $("#tr_" + id).show();
                $('#preview_frame').contents().find(".iframe_wrap").html($("#url_" + id).val());
                $("#Invitation_Video_URL").val($("#url_" + id).val());
                $("#Invitation_Video_Type_Code").val(id);
                break;
            case "VTC03"://FEELMAKER
                if ($("#url_" + id).val().indexOf("feelmaker") < 0) {
                    $("#url_" + id).val("");
                }
                $("#tr_" + id).show();
                $('#preview_frame').contents().find(".iframe_wrap").html($("#url_" + id).val());
                $("#Invitation_Video_URL").val($("#url_" + id).val());
                $("#Invitation_Video_Type_Code").val(id);
                break;
        }

        var position = $('#preview_frame').contents().find('.onoff_6').offset();
        $('#preview_frame').contents().scrollTop(position.top - 20);
    });
    $(".img_type").on("click", function (event) {
        var id = $(this).attr('id');

        $(".img_type_list a").each(function () {
            if ($(this).attr('id') == id) {
                $(this).addClass('active');
                $("#Gallery_Type_Code").val(id);
                var preview_frame = document.getElementById("preview_frame");
                preview_frame.contentWindow.setImageType(id);
            } else {
                $(this).removeClass('active');
            }
        });
        var position = $('#preview_frame').contents().find('.onoff_5').offset();
        $('#preview_frame').contents().scrollTop(position.top - 20);
    });
    //사용함 공통
    $('.btn_use').on('click', function () {
        $("#" + $(this).attr("name")).val("Y");
        $(this).parents(".order_con").find('.order_detail').show();

        var idx = $(this).attr("idx");

        $('#preview_frame').contents().find('.onoff_' + idx).show();

        var position = $('#preview_frame').contents().find('.onoff_' + idx).offset();
        $('#preview_frame').contents().scrollTop(position.top - 20);

    });
    //사용안함 공통
    $('.btn_unuse').on('click', function () {
        $("#" + $(this).attr("name")).val("N");
        $(this).parents(".order_con").find('.order_detail').hide();

        var idx = $(this).attr("idx");
        var position = $('#preview_frame').contents().find('.onoff_' + idx).offset();
        $('#preview_frame').contents().scrollTop(position.top - 20);

        $('#preview_frame').contents().find('.onoff_' + idx).hide();
    });

    $('.url_input').on('keyup', function () {
        $("#Invitation_Video_URL").val("");
    });

    //사진확대 방지
    if ($("#GalleryPreventPhoto_YN").val() == "Y") {
        $('#PreventPhoto').prop('checked', true);
    } else {
        $('#PreventPhoto').prop('checked', false);
    }
    $("#PreventPhoto").on("click", function () {
        if ($(this).is(":checked")) {
            //확대금지
            $("#GalleryPreventPhoto_YN").val("Y");
        } else {
            //확대가능
            $("#GalleryPreventPhoto_YN").val("N");
        }
    });

    $(".loader").hide();
});
function validation_check() {

    if ($("#Invitation_Video_Use_YN").val() == "Y") {

        id = $("input[name=Invitation_Video_Type_Code]:checked").attr('id');

        if ($("#url_" + id).val() == "") {
            alert("초대영상 URL 혹은 소스정보를 입력해주세요.");
            $("#url_" + id).focus();
            return false;
        }

        if ($("#Invitation_Video_URL").val() == "") {
            alert("적용하기 버튼을 눌러주세요.")
            return false;
        }
    }


    if ($("#Gallery_Use_YN").val() == "Y" && $(".gallery_list").length < 1) {
        alert("이미지 관리 사용시 최소 1컷 이상의 파일을 업로드 해주셔야 합니다");
        return false;
    }
    return true;
};

//dropzone
$(document).bind('dragover', function (e) {
    var dropZone = $('.img_drag'),
        timeout = window.dropZoneTimeout;
    if (!timeout) {
        dropZone.addClass('in');
    } else {
        clearTimeout(timeout);
    }
    var found = false,
        node = e.target;
    do {
        if (node === dropZone[0]) {
            found = true;
            break;
        }
        node = node.parentNode;
    } while (node != null);
    if (found) {
        dropZone.addClass('hover');
    } else {
        dropZone.removeClass('hover');
    }
    window.dropZoneTimeout = setTimeout(function () {
        window.dropZoneTimeout = null;
        dropZone.removeClass('in hover');
    }, 100);
});
function denyEnter() {
    $(window).keydown(function (event) {
        if (event.keyCode == 13 && !$(event.target).is("textarea")) {
            return false;
        }
    });
}
var prog = 0;
var cntErrorCount = 0;
var cntErrorSize = 0;
var cntErrorType = 0;
var cntErrorEtc = 0;
var gallery_elem = "<li class=\"gallery_list\" >";
gallery_elem += "<div class=\"file_box\">";
gallery_elem += "<div class=\"img_preview\">";
gallery_elem += "<img class=\"gallery_img\">";
gallery_elem += "</div ></div >";
gallery_elem += "<div class=\"btn_wrap edit\">";
gallery_elem += "<a href=\"javascript:;\" class=\"crop ico\">자르기</a>";
gallery_elem += "<a href=\"javascript:;\" class=\"move ico ui-sortable-handle\">움직이기</a>";
gallery_elem += "<a href=\"javascript:;\" class=\"delete ico\">삭제</a>";
gallery_elem += "</div ></li > ";
function removeGalleryItem(id) {
    var mydata = {
        "Gallery_Id": id
    };

    $.ajax({
        type: "POST",
        url: "/Invitation/RemoveGalleryItem",
        async: false,
        data: mydata,
        dataType: "json",
        success: function (result) {
            if (result.success == "Y") {
                $("#gl_" + id).remove();

                if ($(".gallery_list").length < 18) {
                    $(".no_img").show();
                }
                syncPreviewGallery();
                initSelectAll();
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
function changeSort(id, idx) {

    var mydata = {
        "Gallery_Id": id,
        "newSort": idx
    };

    $.ajax({
        type: "POST",
        url: "/Invitation/ChangeGallerySort",
        data: mydata,
        dataType: "json",
        success: function (result) {
            if (result.success == "Y") {
                syncPreviewGallery();
            } else {
                if (!result.auth) {
                    location.reload();
                } else {
                    $(".error").css("display", "flex").delay(1000).fadeOut();
                }
            }
        }
    });
};
function initSelectAll() {
    if ($('.gallery_list.on').length != $('.gallery_list').length) {
        $(".check_all").prop("checked", false);
    } else {
        $(".check_all").prop("checked", true);
    }
}
function cropImage(id, url) {
    image_select = id;
    var modal_con = '';

    modal_con += "<div class=\"crop_container\" >";
    modal_con += "<img id=\"crop_img\" src=\"" + url + "\" style=\" max-height: 500px;\">";
    modal_con += "</div>";

    $(".crop .img_box").html(modal_con);

    var image = document.getElementById("crop_img");

    var minWidth = $('.layer_pop .img_box').width() - 100;
    var minHeight = minWidth
    cropper = new Cropper(image, {
        minContainerWidth: minWidth,
        minContainerHeight: minHeight,
    });

}
function previewSize(img, selection) {
}
function initCropLayer() {
    $("#black, .layerpop_a, .imgareaselect-outer").fadeOut(200);
    var ias = $('#crop_img').imgAreaSelect({ instance: true });
    $('#x1').val("");
    $('#y1').val("");
    $('#x2').val("");
    $('#y2').val("");

    ias.cancelSelection();

    initCropVal();
}
function initCropVal() {
    image_select = "";
    realW = 0;
    realH = 0;
    scaleW = 0;
    scaleH = 0;
}
function getScaleVal() {
    if (realW != scaleW) {
        if (realW >= realH) {
            return (realW / scaleW);
        } else {
            return (realH / scaleH);
        }
    } else {
        return 1;
    }
}
function CompleteCrop() {
    var img_url = cropper.getCroppedCanvas().toDataURL().replace(/^data[:]image\/(png|jpg|jpeg)[;]base64,/i, "");
    fn_CropImageUpload(img_url);
}
function fn_CropImageUpload(img_url) {

    $.ajax({
        url: '/Invitation/CropImageUpload',
        data: {
            "imageData": img_url,
            "type": "gallery",
            "id": image_select.replace("gl_", ""),
            "url": $("#" + image_select + " .gallery_img").attr("orgimg")
        },
        type: 'POST',
        success: function (result) {
            if (result.success == "Y") {
                //$("#" + image_select + " .gallery_img").attr("src", $("#" + image_select + " .gallery_img").attr("orgimg") + "?" + uuidv4());
                $("#" + image_select + " .gallery_img").attr("src", result.path);
                syncPreviewGallery();
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
function syncPreviewGallery() {
    var gallery_type01 = ""; //바둑판
    var gallery_type02 = ""; //원형
    var gallery_type03 = ""; //슬라이드

    $(".gallery_list .gallery_img").each(function (i) {
        gallery_type01 += "<figure><a href=\"#\" style=\"background: url(" + $(this).attr('src') + ") no-repeat center center\" itemprop = \"contentUrl\" > <img src=\"/img/skin/img_frame.png\" class=\"img_frame\" itemprop=\"thumbnail\" alt=\"\"></a></figure > ";
        gallery_type02 += "<li><img src='" + $(this).attr('src') + "' alt=\"\"></li>";
        gallery_type03 += "<div class=\"swiper-slide\"><img src='" + $(this).attr('src') + "' alt=\"\"></div>";


        //  $(this).attr("src", $(this).attr('src') + "?" + uuidv4());
    });
    var position = $('#preview_frame').contents().find('.onoff_5').offset();
    $('#preview_frame').contents().scrollTop(position.top - 20);
    $('#preview_frame').contents().find(".gallery_type01 > div").empty().html(gallery_type01);
    $('#preview_frame').contents().find(".gallery_type02 ul").empty().html(gallery_type02);


    type3_elem = "<div class=\"swiper-container gallery-top\">";
    type3_elem += "<div class=\"swiper-wrapper\">";
    type3_elem += gallery_type03;
    type3_elem += "</div>";
    type3_elem += "<div class=\"swiper-button-next swiper-button-white\"></div>";
    type3_elem += "<div class=\"swiper-button-prev swiper-button-white\"></div>";
    type3_elem += "</div >";
    type3_elem += "<div class=\"swiper-container gallery-thumbs\">";
    type3_elem += "<div class=\"swiper-wrapper\">";
    type3_elem += gallery_type03;
    type3_elem += "</div>";
    type3_elem += "</div >";




    $('#preview_frame').contents().find(".gallery_type03").empty().html(type3_elem);



}
function addUploadError() {
    var msg = "";
    if (cntErrorCount > 0) {
        msg = "<p>사진 업로드수가 초과되어 등록되지 않았습니다 (" + cntErrorCount + "건)</p>";
    }
    if (cntErrorSize > 0) {
        msg += "<p>사진 용량이 초과되어 등록되지 않았습니다 (" + cntErrorSize + "건)</p>";
    }
    if (cntErrorType > 0) {
        msg += "<p>파일 형식이 지원되지 않아서 등록되지 않았습니다 (" + cntErrorType + "건)</p>";
    }
    if (cntErrorEtc > 0) {
        msg += "<p>기타 오류로 인해 등록되지 않았습니다 (" + cntErrorEtc + "건)</p>";
    }
    $(".popErrors").html(msg);
    if ($("#popErrors").css("display") == "none") $("#popErrors").show();
}
function popClose_p() {
    $('.pop_wrap').hide();
    $("#popErrors p").remove();
    upload_img_cnt = 0;
}

function fn_apply(id) {

    switch (id) {
        case "VTC01"://Youtube
            if ($("#url_" + id).val().indexOf("youtu") < 0) {
                alert("입력값이 올바른지 확인해주세요.");
                $("#url_" + id).focus();
                return false;
            }
            var pattern = /(?:http?s?:\/\/)?(?:www\.)?(?:youtube\.com|youtu\.be)\/(?:watch\?v=)?(.+)/g;
            var htmlInput = $("#url_" + id).val();
            $("#Invitation_Video_URL").val($("#url_" + id).val());
            if (pattern.test(htmlInput)) {
                var replacement = '<iframe src="//www.youtube.com/embed/$1" frameborder="0" class="embed-container" allowfullscreen></iframe>';
                htmlInput = htmlInput.replace(pattern, replacement);
            }
            $('#preview_frame').contents().find(".iframe_wrap").html(htmlInput);
            $("#Invitation_Video_Type_Code").val(id);
            break;
        case "VTC02": //Vimeo
            if ($("#url_" + id).val().indexOf("vimeo") < 0) {
                alert("입력값이 올바른지 확인해주세요.");
                $("#url_" + id).focus();
                return false;
            }
            $('#preview_frame').contents().find(".iframe_wrap").html($("#url_" + id).val());
            $("#Invitation_Video_URL").val($("#url_" + id).val());
            $("#Invitation_Video_Type_Code").val(id);
            break;
        case "VTC03"://FEELMAKER
            if ($("#url_" + id).val().indexOf("feelmaker") < 0) {
                alert("입력값이 올바른지 확인해주세요.");
                $("#url_" + id).focus();
                return false;
            }
            $('#preview_frame').contents().find(".iframe_wrap").html($("#url_" + id).val());
            $("#Invitation_Video_URL").val($("#url_" + id).val());
            $("#Invitation_Video_Type_Code").val(id);
            break;
    }

    var position = $('#preview_frame').contents().find('.onoff_6').offset();
    $('#preview_frame').contents().scrollTop(position.top - 20);
}
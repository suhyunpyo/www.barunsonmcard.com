var photo_yn = false;
var invitation_areas = [];
var area = {};
var objects = [];
var mappingfield = {};
var tb_area = [];
var item = function () {
    return {
        item_id: '',
        resource_id: '',
        pid: '',
        id: '',
        type: '',
        top: 0,
        left: 0,
        height: 0,
        width: 0,
        chracterset: '',
        fontsize: 0,
        fontcolor: '#000000',
        bgcolor: '',
        bold_yn: false,
        italic_yn: false,
        underline_yn: false,
        between_text: 0,
        between_line: 1.4,
        vertical_align: '',
        horizontal_align: '',
        zindex: 0,
        font: "'Noto Sans KR', sans-serif",
        bring_front: false,
        bring_forward: false,
        send_back: false,
        send_backward: false,
        resource_url: '',
        org_height: 0,
        org_width: 0
    };
}
var invitation = {
    idx: 0,
    id: null,
    width: 0,
    height: 0,
    x: 0,
    y: 0,

    addImage: function (e) {

        $('#uploadFrm').get(0).reset();
        $("#file").click();
        $("#file").unbind();
        $('#file').change(function () {
            var form = $('#uploadFrm')[0];
            var formData = new FormData(form);

            formData.append("upload_path", $("#upload_path").val());

            $.ajax({
                url: '/Admin/Invitation/InvitationImageUpload',
                data: formData,
                type: 'POST',
                enctype: 'multipart/form-data',
                processData: false,
                contentType: false,
                dataType: 'json',
                cache: false,
                success: function (result) {
                    document.getElementById("file").value = "";

                    invitation.org_width = result.width;
                    invitation.org_height = result.height

                    if (result.width >= area.width) {
                        invitation.x = 0;
                        invitation.y = 0;

                        result.height = parseFloat(result.height * area.width / result.width)

                        result.width = parseFloat(area.width)

                        if (area.height < result.height) {
                            $('#' + area.id).height(result.height);
                            area.height = result.height;
                        }
                    } else {
                        if (result.height >= area.height) {
                            $('#' + area.id).height(result.height);
                            area.height = result.height;
                            invitation.y = 0;
                        }
                    }

                    invitation.resource_url = result.resource_url;
                    invitation.width = result.width;
                    invitation.height = result.height

                    invitation.setInvitationIdx();

                    invitation.id = 'item_' + invitation.idx;
                    var div = "<div id='" + invitation.id + "' idx='" + invitation.idx + "' class='item ui-widget-content selected resizable' style='top: " + invitation.y + "px; left: " + invitation.x + "px;  position:absolute;'><span class='topline'></span><span class='rightline'></span><span class='botline'></span><span class='leftline'></span><img class='img' src='" + invitation.resource_url + "' width='" + invitation.width + "px' height='" + invitation.height + "px'  /><span class='img-info'></span></div>";
                    $('#' + area.id).append(div);
                    invitation.addObject($(this), 'img');
                    invitation.imageMode();
                    invitation.addImageEvent();
                    invitation.hidePlaceholder($('#' + area.id));
                }
            });
        });

    },
    addImageEvent: function (e) {



        $('#' + invitation.id).resizable({
            handles: 'n, e, s, w, sw, nw, ne, se',
            containment: '#' + area.id,
            start: function (event, ui) {
                var handle = $(this).data('ui-resizable').axis;
                var maintain_aspect_ratio = (["n", "e", "s", "w"].indexOf(handle) == -1);
                $(this).resizable("option", "aspectRatio", maintain_aspect_ratio)
                    .data('ui-resizable')
                    ._aspectRatio = maintain_aspect_ratio;
            },
            resize: function (event, ui) {
                var hr = $(this).outerHeight();
                var wr = $(this).outerWidth();
                $(this).find(".img").css({ "width": wr, "height": hr });
                $(this).children(".ui-resizable-handle").removeClass('resizabled');
                invitation.pid = $("#" + invitation.id).parent('div').attr('id');
                invitation.showInfo($(this), 'img');
            },
            stop: function (event, ui) {
                $(this).children(".ui-resizable-handle").addClass('resizabled')
                $(this).find($('.img-info')).css('display', 'none');
                invitation.changeObject($(this), 'img');
            }
        });

        $('#' + invitation.id).draggable({
            containment: "#" + area.id,
            cursor: "move",
            snap: ".invitationarea",
            snapTolerance: 1,
            drag: function (event, ui) {
                $(this).children(".ui-resizable-handle").removeClass('resizabled');
                invitation.showInfo($(this), 'img');
                $(".guideline").show();
            },
            stop: function (event, ui) {
                $(this).children(".ui-resizable-handle").addClass('resizabled');
                $(this).find($('.img-info')).css('display', 'none');
                invitation.changeObject($(this), 'img');
                $(".guideline").hide();
            }
        });

        $('#' + invitation.id).mousedown(function (e) {

            $(".ui-resizable-handle").removeClass('resizabled');
            $(".resizable").removeClass("selected");
            $(this).addClass("selected");
            $(this).children(".ui-resizable-handle").addClass('resizabled');
            $(".ui-resizable-handle").hide();
            $(this).children(".ui-resizable-handle").show();

            area.id = $(this).parent('div').attr('id')
            invitation.id = $(this).attr('id');
            invitation.x = e.pageX - $(this).parent('div').offset().left;
            invitation.y = e.pageY - $(this).parent('div').offset().top;
            $(".matchinfo").removeClass("selected");
            invitation.imageMode();
            invitation.showInfo($(this), 'img');
            e.stopPropagation();

        });

        $('#' + invitation.id).mouseup(function (e) {
            $(this).find($('.img-info')).css('display', 'none');
        });

        $(".ui-resizable-handle").removeClass('ui-icon ui-icon-gripsmall-diagonal-se');

        $('#' + invitation.id).children(".ui-resizable-handle").addClass('resizabled');

    },
    setInvitationIdx: function () {
        if ($('.item').length > 0) {
            var tmp_idx = 1;
            $('.item').each(function (index, obj) {
                tmp_idx = parseInt($("#" + obj.id).attr('idx')) + 1;
                if (invitation.idx < tmp_idx) {
                    invitation.idx = tmp_idx;
                }
            });
        } else {
            invitation.idx = 1;
        }
    },
    changeText: function (target) {
        objects.forEach(function (elem) {
            if (elem.id == $(".item.selected").attr('id') && $(".matchinfo.selected").attr('idx') == $(target).attr('idx')) {
                elem.chracterset = $(target).val();
            }
        });
        var n = $(target).attr('idx');
        var text = invitation.matchText($(target).val());
        $('#item_' + n + ' .text').html(text);
    },
    changeTextCss: function (target) {

        objects.forEach(function (elem) {
            if (elem.id == $(".item.selected").attr('id')) {
                switch ($(target).attr('id')) {
                    case 'FontSize':
                        elem.fontsize = parseInt($("#FontSize").val());
                        $('.item.selected>.text').css('font-size', parseInt($("#FontSize").val()));
                        break;
                    case 'Bold':
                        elem.bold_yn = $("#Bold").hasClass("selected") ? true : false;
                        $('.item.selected>.text').css('font-weight', $("#Bold").hasClass("selected") ? "bold" : "");
                        break;
                    case 'Italic':
                        elem.italic_yn = $("#Italic").hasClass("selected") ? true : false;
                        $('.item.selected>.text').css('font-style', $("#Italic").hasClass("selected") ? "italic" : "");
                        break;
                    case 'Underline':
                        elem.underline_yn = $("#Underline").hasClass("selected") ? true : false;
                        $('.item.selected>.text').css('text-decoration', $("#Underline").hasClass("selected") ? "underline" : "");
                        break;
                    case 'Left':
                        elem.horizontal_align = $("#Left").hasClass("selected") ? "L" : elem.horizontal_align;
                        $('.item.selected>.text').css('text-align', $("#Left").hasClass("selected") ? "left" : "");
                        break;
                    case 'Center':
                        elem.horizontal_align = $("#Center").hasClass("selected") ? "C" : elem.horizontal_align;
                        $('.item.selected>.text').css('text-align', $("#Center").hasClass("selected") ? "center" : "");
                        break;
                    case 'Right':
                        elem.horizontal_align = $("#Right").hasClass("selected") ? "R" : elem.horizontal_align;
                        $('.item.selected>.text').css('text-align', $("#Right").hasClass("selected") ? "right" : "");
                        break;
                    case 'Top':
                        elem.vertical_align = $("#Top").hasClass("selected") ? "T" : elem.vertical_align;
                        $('.item.selected').css('align-items', $("#Top").hasClass("selected") ? "flex-start" : "");
                        break;
                    case 'Middle':
                        elem.vertical_align = $("#Middle").hasClass("selected") ? "M" : elem.vertical_align;
                        $('.item.selected').css('align-items', $("#Middle").hasClass("selected") ? "center" : "");
                        break;
                    case 'Bottom':
                        elem.vertical_align = $("#Bottom").hasClass("selected") ? "B" : elem.vertical_align;
                        $('.item.selected').css('align-items', $("#Bottom").hasClass("selected") ? "flex-end" : "");
                        break;
                    case 'Between_Text_Calc':
                        elem.between_text = parseFloat($("#Between_Text").val());
                        $('.item.selected>.text').css('letter-spacing', parseFloat($("#Between_Text").val()) / 100 + "em");
                        break;
                    case 'Between_Line_Calc':
                        elem.between_line = parseFloat($("#Between_Line").val());
                        $('.item.selected>.text').css('line-height', parseFloat($("#Between_Line").val()) + "em");
                        break;
                    case 'Between_Text':
                        elem.between_text = parseFloat($("#Between_Text_Calc").val());
                        $('.item.selected>.text').css('letter-spacing', parseFloat($("#Between_Text").val()) / 100 + "em");
                        break;
                    case 'selFont':
                        elem.font = $("#selFont").val();
                        $('.item.selected>.text').css('font-family', $("#selFont").val());
                        break;
                }

                elem.width = $(".item.selected>.text").width();
                elem.height = $(".item.selected>.text").height();

                invitation.pid = $(".item.selected").parent('div').attr('id');

            }
        });

        $('.item').each(function () {
            $(this).data("height", $(this).outerHeight());
            $(this).data("width", $(this).outerWidth());
        });
        $('.text', '.item').each(function () {
            $(this).data("height", $(this).outerHeight());
            $(this).data("width", $(this).outerWidth());
            $(this).data("fontSize", parseInt($(this).css("font-size")));
        });

    },
    matchText: function (text) {

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
    },
    addText: function (key, e) {

        invitation.setInvitationIdx();

        var text = invitation.matchText(key);

        invitation.id = 'item_' + invitation.idx;
        invitation.txtid = 'addtext_' + invitation.idx;
        var div = "<div id='" + invitation.id + "' idx='" + invitation.idx + "' class='item ui-widget-content selected resizable' style='top: " + invitation.y + "px; left: " + invitation.x + "px; position:absolute;'><span class='topline'></span><span class='rightline'></span><span class='botline'></span><span class='leftline'></span><div class='text'  >" + text + "</div><span class='txt-info'></span></div>";
        var matchinfo = "<input type='text' id='" + invitation.txtid + "' idx='" + invitation.idx + "' class='form-control form-control-sm matchinfo selected' value='" + key + "'>"


        $('#' + area.id).append(div);
        $('#divMatch').append(matchinfo);

        $("#" + invitation.txtid).on("focus", function (e) {
            $(document).unbind('keydown');
        });
        $("#" + invitation.txtid).on("blur", function (e) {
            $(document).on("keydown", function (event) {
                if (event.keyCode == 46) {

                    if ($(".item.selected").hasClass('photo-image')) {
                        $('#btnphotoupload').addClass('btn-primary');
                        $('#btnphotoupload').removeClass('btn-secondary');
                        $('#btnphotoupload').addClass('empty');
                        $('#btnphotoupload').text('추가');
                    }

                    invitation.pid = $(".item.selected").parent('div').attr('id');
                    $(".item.selected").remove();
                    $(".matchinfo.selected").remove();
                    invitation.delObject();
                    invitation.initMode();
                    if ($("#" + invitation.pid + " .item").length < 1) {
                        invitation.resizeArea('delete', invitation.pid);
                    }
                    invitation.showPlaceholder($('#' + invitation.pid));
                }
            });
        });

        $('.item.selected>.text').css('font-family', "'Noto Sans KR', sans-serif");

        invitation.org_height = parseFloat($('#' + invitation.id).outerHeight());
        invitation.org_width = parseFloat($('#' + invitation.id).outerWidth());
        invitation.height = parseFloat($('#' + invitation.id).outerHeight());
        invitation.width = parseFloat($('#' + invitation.id).outerWidth());
        invitation.characterset = key;

        invitation.addObject($(this), 'txt');

        $('.item').each(function () {
            $(this).data("height", $(this).outerHeight());
            $(this).data("width", $(this).outerWidth());
        });
        $('.text', '.item').each(function () {
            $(this).data("height", $(this).outerHeight());
            $(this).data("width", $(this).outerWidth());
            $(this).data("fontSize", parseInt($(this).css("font-size")));
        });

        if (key == "#인사말#") {
            $("#Center").trigger('click');
            $("#FontSize").val(34);
            $('#FontSize').trigger('change');
            $("#Between_Line_Calc").val(2.3);
            $("#Between_Line_Calc").trigger('change');
        } else {
            $("#Left").trigger('click');
        }
        $("#Middle").trigger('click');

        invitation.textMode();
        invitation.addTextEvent();

        invitation.hidePlaceholder($('#' + area.id));
    },
    addTextEvent: function (e) {
        $("#" + invitation.id).resizable({
            handles: 'n, e, s, w, sw, nw, ne, se',
            containment: '#' + area.id,
            start: function (event, ui) {
                var handle = $(this).data('ui-resizable').axis;
                var maintain_aspect_ratio = (["n", "e", "s", "w"].indexOf(handle) == -1);
                $(this).resizable("option", "aspectRatio", maintain_aspect_ratio)
                    .data('ui-resizable')
                    ._aspectRatio = maintain_aspect_ratio;
            },
            resize: function (event, ui) {
                invitation.pid = $("#" + invitation.id).parent('div').attr('id');
                $("#FontSize").val(parseInt($(this).children('.text').css("font-size")));
                invitation.showInfo($(this), 'txt');
            },
            stop: function (event, ui) {
                $(this).find($('.txt-info')).css('display', 'none');
                invitation.changeObject($(this), 'txt');
            }

        });

        $('#' + invitation.id).draggable({
            containment: "#" + area.id,
            cursor: "move",
            snap: ".invitationarea",
            snapTolerance: 1,
            start: function (event, ui) {

                $(".ui-resizable-handle").removeClass('resizabled');
            },
            drag: function (event, ui) {
                invitation.showInfo($(this), 'txt');

                $(".guideline").show();
            },
            stop: function (event, ui) {
                $(this).children(".ui-resizable-handle").addClass('resizabled');
                $(this).find($('.txt-info')).css('display', 'none');
                invitation.changeObject($(this), 'txt');
                $(".guideline").hide();
            }
        });

        $('#' + invitation.id).mousedown(function (e) {
            $(".ui-resizable-handle").removeClass('resizabled');
            $(".resizable").removeClass("selected");
            $(this).addClass("selected");
            $(this).children(".ui-resizable-handle").addClass('resizabled');
            $(".ui-resizable-handle").hide();
            $(this).children(".ui-resizable-handle").show();

            area.id = $(this).parent('div').attr('id')
            invitation.id = $(this).attr('id');
            invitation.x = e.pageX - $(this).parent('div').offset().left;
            invitation.y = e.pageY - $(this).parent('div').offset().top;

            $(".matchinfo").removeClass("selected");
            $(".matchinfo[idx=" + $(this).attr('idx') + "]").addClass("selected");

            invitation.textMode();

            invitation.showInfo($(this), 'txt')

            e.stopPropagation();
        });

        $('#' + invitation.id).mouseup(function (e) {
            $(this).find($('.txt-info')).css('display', 'none');
        });

        $(".ui-resizable-handle").removeClass('ui-icon ui-icon-gripsmall-diagonal-se');

        $('#' + invitation.id).children(".ui-resizable-handle").addClass('resizabled');
    },
    bring_front: function (e) {
      
        var pid = $(".item.selected").parent('div').attr('id');

     

        $("#" + pid + " > .item").last().after($(".item.selected"));
        invitation.initObject();
    },
    bring_forward: function (e) {
        var pid = $(".item.selected").parent('div').attr('id');
       
        $("#" + pid + " > .item.selected").next().after($(".item.selected"));
        invitation.initObject();
    },
    send_back: function (e) {
        var pid = $(".item.selected").parent('div').attr('id');
        
        $("#" + pid + " > .item").first().before($(".item.selected"));
        invitation.initObject();
    },
    send_backward: function (e) {
        var pid = $(".item.selected").parent('div').attr('id');
     
        $("#" + pid + " > .item.selected").prev().before($(".item.selected"));
        invitation.initObject();
    },
    changeObject: function (ui, type) {
        invitation.x = parseFloat(ui.position().left);
        invitation.y = parseFloat(ui.position().top);
        invitation.width = parseFloat(ui.outerWidth());
        invitation.height = parseFloat(ui.outerHeight());

        if (type == 'txt') {
            invitation.fontsize = parseInt(ui.children('.text').css("font-size"));
        } else {
            invitation.fontsize = 0;
        }

        objects.forEach(function (elem) {
            if (elem.id == invitation.id) {
                elem.left = invitation.x
                elem.top = invitation.y
                elem.width = invitation.width
                elem.height = invitation.height
                elem.fontsize = invitation.fontsize
            }
        });

    },
    addObject: function (ui, type) {


        var obj = new item();

        obj.id = $(this).attr('id');

        var idx = 0;
        objects.forEach(function (elem) {
            if (elem.pid == $("#" + obj.id).parent('div').attr('id')) {
                idx++;
            }
        });

        obj.type = type
        obj.zindex = idx + 1;
        obj.pid = $("#" + obj.id).parent('div').attr('id');
        obj.top = invitation.y;
        obj.left = invitation.x;
        obj.org_width = invitation.org_width;
        obj.org_height = invitation.org_height;
        obj.width = invitation.width;
        obj.height = invitation.height;


        if (type == 'img' || type == 'photo') {

            obj.resource_url = invitation.resource_url;
            obj.fontsize = 0;
        }

        if (type == 'txt') {
            obj.fontsize = parseInt($('html').css("font-size"));
            obj.chracterset = invitation.characterset;
        }

        objects.push(obj);
        invitation.initObject();
    },
    initObject: function (e) {

        var idx = 1;
        $('.item').each(function (e) {
            var obj = new item();
            obj.id = $(this).attr('id');

            objects.forEach(function (elem) {

                if (elem.pid == $("#" + obj.id).parent('div').attr('id') && elem.id == obj.id) {
                    elem.zindex = idx;
                    invitation.resetLayerButton(elem);
                }
            });
            invitation.setLayerStatus();
            idx++;
        });
    },
    delObject: function (e) {

        var tmp_objects = [];

        var idx = 1;

        $('.item').each(function (e) {
            var obj = new item();
            obj.id = $(this).attr('id');
            objects.forEach(function (elem) {
                if (elem.pid == $("#" + obj.id).parent('div').attr('id') && elem.id == obj.id) {
                    elem.zindex = idx;
                    invitation.resetLayerButton(elem);
                    tmp_objects.push(elem);
                }
            });
            idx++;

        });
        objects = tmp_objects;
    },
    resetLayerButton: function (elem) {
        elem.bring_front = $("#" + elem.id).attr('id') == $("#" + elem.pid + " > .item").last().attr('id') ? false : true;
        elem.bring_forward = $("#" + elem.id).attr('id') == $("#" + elem.pid + " > .item").last().attr('id') ? false : true;
        elem.send_back = $("#" + elem.id).attr('id') == $("#" + elem.pid + " > .item").first().attr('id') ? false : true;
        elem.send_backward = $("#" + elem.id).attr('id') == $("#" + elem.pid + " > .item").first().attr('id') ? false : true;
    },
    showInfo: function (ui, type) {


        if (parseInt(ui.position().left) + parseInt(ui.outerWidth()) > 665) {
            $('.img-info').css({ "right": 0 });
            $('.txt-info').css({ "right": 0 });
        } else {
            $('.img-info').css({ "right": -210 });
            $('.txt-info').css({ "right": -210 });
        }

        var z_index = 0;
        for (var a = 0; a < objects.length; a++) {
            if ((objects[a].id) == ui.attr('id')) {
                z_index = objects[a].zindex;
                break;
            }
        }
        if (type == 'txt') {
            ui.find($('.txt-info')).html('ㅤLEFT : ' + parseInt(ui.position().left) + 'ㅤ/ㅤTOP : ' + parseInt(ui.position().top) + '<br/>ㅤWIDTH : ' + parseInt(ui.outerWidth()) + 'ㅤ/ㅤHIEGHT : ' + parseInt(ui.outerHeight()) + '<br/>ㅤFONT-SIZE : ' + parseInt(ui.children('.text').css("font-size")) + '<br/>ㅤZ-INDEX : ' + z_index);
            ui.find($('.txt-info')).css('display', 'block');
        }
        else {
            ui.find($('.img-info')).html('ㅤLEFT : ' + parseInt(ui.position().left) + 'ㅤ/ㅤTOP : ' + parseInt(ui.position().top) + '<br/>ㅤWIDTH : ' + parseInt(ui.outerWidth()) + 'ㅤ/ㅤHIEGHT : ' + parseInt(ui.outerHeight()) + '<br/>ㅤZ-INDEX : ' + z_index);
            ui.find($('.img-info')).css('display', 'block');
        }
    },
    initClass: function () {
        $(".ui-resizable-handle").hide();
        $(".invitationarea").removeClass("selected");
        $(".ui-resizable-handle").removeClass('resizabled');
        $(".resizable").removeClass("selected");
        $(".resizable").removeClass("ui-widget-content");
    },
    textMode: function () {
        $("#divDisabled1").css("display", "none");
        $("#divDisabled2").css("display", "none");
        $("#divDisabled3").css("display", "none");
        $("#divDisabled4").css("display", "none");
        $("#divDisabled5").css("display", "none");

        $('.dvTool a').removeClass('disabled');
        $('.dvTool select').removeAttr("disabled");
        $('.dvTool input ').removeAttr("disabled");
        invitation.setTextStatus();
    },
    imageMode: function () {
        invitation.initMode();

        $("#divDisabled4").css("display", "none");

        invitation.setImageStatus();
    },
    initMode: function () {
        $("#divDisabled1").css("display", "block");
        $("#divDisabled2").css("display", "block");
        $("#divDisabled3").css("display", "block");
        $("#divDisabled4").css("display", "block");
        $("#divDisabled5").css("display", "block");

        $("#FontSize").val(16);
        $("#Bold").removeClass("selected");
        $("#Italic").removeClass("selected");
        $("#Underline").removeClass("selected");
        $("#Left").removeClass("selected");
        $("#Center").removeClass("selected");
        $("#Right").removeClass("selected");
        $("#Top").removeClass("selected");
        $("#Middle").removeClass("selected");
        $("#Bottom").removeClass("selected");
        $("#object_txtcolor").val("#000000");
        $("#Between_Text_Calc").val(0);
        $("#Between_Text").val(0);
        $("#Between_Line_Calc").val(1.4);
        $("#Between_Line").val(1.4);
    },
    setTextStatus: function () {

        var id = $('.item.selected').attr('id');

        objects.forEach(function (elem) {


            if (elem.id == id) {

                $("#selFont").val(elem.font);

                $("#FontSize").val(elem.fontsize);
                $("#Between_Text_Calc").val(elem.between_text);
                $("#Between_Text").val(elem.between_text);
                $("#Between_Line_Calc").val(elem.between_line);
                $("#Between_Line").val(elem.between_line);



                elem.bold_yn ? $("#Bold").addClass("selected") : $("#Bold").removeClass("selected");
                elem.italic_yn ? $("#Italic").addClass("selected") : $("#Italic").removeClass("selected");
                elem.underline_yn ? $("#Underline").addClass("selected") : $("#Underline").removeClass("selected");


                $("#Left").removeClass("selected");
                $("#Center").removeClass("selected");
                $("#Right").removeClass("selected");
                switch (elem.horizontal_align) {
                    case "L":
                        $("#Left").addClass("selected");
                        break;
                    case "C":
                        $("#Center").addClass("selected");
                        break;
                    case "R":
                        $("#Right").addClass("selected");
                        break;
                }
                switch (elem.vertical_align) {
                    case "T":
                        $("#Top").addClass("selected");
                        break;
                    case "M":
                        $("#Middle").addClass("selected");
                        break;
                    case "B":
                        $("#Bottom").addClass("selected");
                        break;
                }

                $("#object_txtcolor").val(elem.fontcolor);
                $("#object_bgcolor").val(elem.bgcolor);


                invitation.resetLayerButton(elem);

                elem.bring_front ? $("#Bring_Front").removeClass('disabled') : $("#Bring_Front").addClass('disabled');
                elem.bring_forward ? $("#Bring_Forward").removeClass('disabled') : $("#Bring_Forward").addClass('disabled');
                elem.send_back ? $("#Send_Back").removeClass('disabled') : $("#Send_Back").addClass('disabled');
                elem.send_backward ? $("#Send_Backward").removeClass('disabled') : $("#Send_Backward").addClass('disabled');
            }
        });
    },
    copyTitle: function () {
        area.id = "area12";
        invitation.resource_url = $("#MoneyGift_Remit_Title_URL").val();
        invitation.width = 800;
        invitation.height = 223;
        invitation.y = 0;
        invitation.x = 0;
        invitation.idx = 1;

        $('#' + area.id).css('height', invitation.height + "px");

        invitation.id = 'item_' + invitation.idx;
        var div = "<div id='" + invitation.id + "' idx='" + invitation.idx + "' class='item ui-widget-content selected resizable' style='top: " + invitation.y + "px; left: " + invitation.x + "px;  position:absolute;'><span class='topline'></span><span class='rightline'></span><span class='botline'></span><span class='leftline'></span><img class='img' src='" + invitation.resource_url + "' width='" + invitation.width + "px' height='" + invitation.height + "px'  /><span class='img-info'></span></div>";

        $('#' + area.id).append(div);
        invitation.addObject($(this), 'img');
        invitation.imageMode();
        invitation.addImageEvent();
        invitation.hidePlaceholder($('#' + area.id));

        $(".ui-resizable-handle").removeClass('resizabled');
        $(".resizable").removeClass("selected");
        $(".ui-resizable-handle").hide();
    },
    setImageStatus: function () {
        var id = $('.item.selected').attr('id');

        objects.forEach(function (elem) {
            if (elem.id == id) {

                invitation.resetLayerButton(elem);


                elem.bring_front ? $("#Bring_Front").removeClass('disabled') : $("#Bring_Front").addClass('disabled');
                elem.bring_forward ? $("#Bring_Forward").removeClass('disabled') : $("#Bring_Forward").addClass('disabled');
                elem.send_back ? $("#Send_Back").removeClass('disabled') : $("#Send_Back").addClass('disabled');
                elem.send_backward ? $("#Send_Backward").removeClass('disabled') : $("#Send_Backward").addClass('disabled');
            }
        });
    },
    setLayerStatus: function () {

        var id = $('.item.selected').attr('id');

        objects.forEach(function (elem) {

            if (elem.id == id) {
                elem.bring_front ? $("#Bring_Front").removeClass('disabled') : $("#Bring_Front").addClass('disabled');
                elem.bring_forward ? $("#Bring_Forward").removeClass('disabled') : $("#Bring_Forward").addClass('disabled');
                elem.send_back ? $("#Send_Back").removeClass('disabled') : $("#Send_Back").addClass('disabled');
                elem.send_backward ? $("#Send_Backward").removeClass('disabled') : $("#Send_Backward").addClass('disabled');
            }
        });
    },
    resizeArea: function (type, area_id) {

        var maxObjectSize = 100;

        $("#" + area_id).children('.item').each(function () {
            var a = parseInt($(this).position().top) + parseInt($(this).outerHeight());

            if (maxObjectSize < a) {
                maxObjectSize = a;
            }
        });

        if (type == 'resize') {
            if (parseFloat($('.item.selected').position().top) + parseFloat($('.item.selected').outerHeight()) > maxObjectSize) {
                $('.item.selected').parent('div').css('height', parseFloat($('.item.selected').position().top) + parseFloat($('.item.selected').outerHeight()))
            } else {
                $('.item.selected').parent('div').css('height', maxObjectSize)
            }
        } else {
            $("#" + area_id).css('height', maxObjectSize)
            // area.height = maxObjectSize;
        }
    },
    setItem: function () {
        objects.forEach(function (elem) {
            invitation.idx++;
            invitation.id = elem.id;
            invitation.pid = elem.pid;
            invitation.x = elem.left;
            invitation.y = elem.top;
            invitation.resource_url = elem.resource_url;
            invitation.width = elem.width;
            invitation.height = elem.height;
            if (elem.type == 'img') {
                var div = "<div id='" + invitation.id + "' idx='" + invitation.idx + "' class='item ui-widget-content resizable' style='top: " + invitation.y + "px; left: " + invitation.x + "px;  position:absolute;'><span class='topline'></span><span class='rightline'></span><span class='botline'></span><span class='leftline'></span><img class='img' src='" + invitation.resource_url + "' width='" + invitation.width + "px' height='" + invitation.height + "px'  /><span class='img-info'></span></div>";
                $('#' + invitation.pid).append(div);
                area.id = invitation.pid;
                invitation.addImageEvent();
                $('#' + invitation.id).children(".ui-resizable-handle").removeClass('resizabled');
                $('#' + invitation.id).children(".ui-resizable-handle").css('display', 'none');
            }
            else if (elem.type == 'photo') {
                //var div = "<div id='" + invitation.id + "' idx='" + invitation.idx + "' class='item ui-widget-content resizable photo-image ' style='top: " + invitation.y + "px; left: " + invitation.x + "px;  position:absolute;'><span class='topline'></span><span class='rightline'></span><span class='botline'></span><span class='leftline'></span><img class='img' src='" + invitation.resource_url + "' width='" + invitation.width + "px' height='" + invitation.height + "px'  /><span class='img-info'></span></div>";

                var div = "<div id='" + invitation.id + "' idx='" + invitation.idx + "' class='item ui-widget-content resizable photo-image ' style='top: " + invitation.y + "px; left: " + invitation.x + "px; width:" + invitation.width + "px; height:" + invitation.height + "px; position:absolute;'><span class='topline'></span><span class='rightline'></span><span class='botline'></span><span class='leftline'></span><img class='img'  style='max-width:" + invitation.width + "px;max-height:" + invitation.height + "px;'><span class='img-info'></span></div>";
                $('#' + invitation.pid).append(div);
                invitation.addImageEvent();
                $('#' + invitation.id).children(".ui-resizable-handle").removeClass('resizabled');
                $('#' + invitation.id).children(".ui-resizable-handle").css('display', 'none');

                $('#btnphotoupload').removeClass('btn-primary');
                $('#btnphotoupload').addClass('btn-secondary');
                $('#btnphotoupload').removeClass('empty');
                $('#btnphotoupload').text('삭제');


                if ($("#Delegate_Image_URL").val() != "") {
                    $(".item.photo-image img").attr("src", $("#Delegate_Image_URL").val() + "?" + uuidv4()).css("width", "100%");
                }

            } else {
                var text = invitation.matchText(elem.chracterset);


                invitation.id = 'item_' + invitation.idx;
                invitation.txtid = 'addtext_' + invitation.idx;
                var div = "<div id='" + invitation.id + "' idx='" + invitation.idx + "' class='item ui-widget-content resizable' style='top: " + invitation.y + "px; left: " + invitation.x + "px;    position:absolute; width:" + invitation.width + "px;  height:" + invitation.height + "px;'><span class='topline'></span><span class='rightline'></span><span class='botline'></span><span class='leftline'></span><div class='text'>" + text + "</div><span class='txt-info'></span></div>";
                var matchinfo = "<input type='text' id='" + invitation.txtid + "' idx='" + invitation.idx + "' class='form-control form-control-sm matchinfo ' value='" + elem.chracterset + "'>"
                $('#' + invitation.pid).append(div);
                $('#divMatch').append(matchinfo);
                area.id = invitation.pid;
                invitation.addTextEvent();
                $('#' + invitation.id).css('background-color', elem.bgcolor);
                $('#' + invitation.id).children(".ui-resizable-handle").removeClass('resizabled');
                $('#' + invitation.id).children(".ui-resizable-handle").css('display', 'none');
                $('#' + invitation.id + ">.text").css('font-family', elem.font);
                $('#' + invitation.id + ">.text").css('font-size', elem.fontsize);
                $('#' + invitation.id + ">.text").css('color', elem.fontcolor);
                $('#' + invitation.id + ">.text").css('font-weight', elem.bold_yn ? "bold" : "");
                $('#' + invitation.id + ">.text").css('font-style', elem.italic_yn ? "italic" : "");
                $('#' + invitation.id + ">.text").css('text-decoration', elem.underline_yn ? "underline" : "");
                if (elem.horizontal_align == "C") {
                    $('#' + invitation.id + ">.text").css('text-align', "center")
                } else if (elem.horizontal_align == "R") {
                    $('#' + invitation.id + ">.text").css('text-align', "right")
                } else if (elem.horizontal_align == "L") {
                    $('#' + invitation.id + ">.text").css('text-align', "left");
                } else {
                    $('#' + invitation.id + ">.text").css('text-align', "");
                }
                if (elem.vertical_align == "T") {
                    $('#' + invitation.id).css('align-items', "flex-start")
                } else if (elem.vertical_align == "M") {
                    $('#' + invitation.id).css('align-items', "center")
                } else if (elem.vertical_align == "B") {
                    $('#' + invitation.id).css('align-items', "flex-end");
                } else {
                    $('#' + invitation.id).css('align-items', "");
                }
                $('#' + invitation.id + ">.text").css('letter-spacing', elem.between_text / 100 + "em");
                $('#' + invitation.id + ">.text").css('line-height', elem.between_line + "em");
                $('.item').each(function () {
                    $(this).data("height", $(this).outerHeight());
                    $(this).data("width", $(this).outerWidth());
                });
                $('.text', '.item').each(function () {
                    $(this).data("height", $(this).outerHeight());
                    $(this).data("width", $(this).outerWidth());
                    $(this).data("fontSize", parseInt($(this).css("font-size")));
                });
            }
        });
    },
    initItem: function () {
        $("#wrap").css('background-color', '');
        $(".invitationarea").css('height', '100px');
        $(".invitationarea").css('background-color', '');
        $(".invitationarea").removeAttr("color_val");
        $(".invitationarea").removeClass("selected");
        $(".invitationarea div").remove();
        $(".invitationarea").find('.placeholder').show();

        $("#Invitation_URL").val("");
        $("#Invitation_Title").val("");

        $("#divMatch input").remove();
        $(".w25 li p").removeClass("selected");
        $(".w25 li img").removeClass("selected");
        objects = [];


        $('.invitationarea').mousedown(function (e) {
            $(".ui-resizable-handle").removeClass('resizabled');
            $(".resizable").removeClass("selected");
            $(this).addClass("selected");
            $(this).children(".ui-resizable-handle").addClass('resizabled');
            $(".ui-resizable-handle").hide();
            $(this).children(".ui-resizable-handle").show();
            $('.ui-resizable-handle.ui-resizable-s.resizabled').css("display", "block");
            $('.ui-resizable-handle.ui-resizable-s.resizabled').css('height', 0);
            $('.ui-resizable-handle.ui-resizable-s.resizabled').css('bottom', -2);

            e.stopPropagation();
        });

        $('.invitationarea').resizable({
            handles: 's'
        });

        $('.ui-resizable-handle.ui-resizable-s').css("display", "none");


    },
    setArea: function (obj) {

        tb_area = obj;


        obj.forEach(function (elem) {
            $("#wrap").find(">div").each(function () {
                if ($(this).attr('idx') == elem.Area_ID) {
                    $("#wrap>div[idx='" + elem.Area_ID + "']").css("display", "block");

                    if ($(this).hasClass('invitationarea')) {
                        if (elem.Area_ID != 2) {
                            $('#area' + elem.Area_ID).css('height', elem.Size_Height + "px");
                        } else {

                            var space = 0
                            if (typeof ($('#area' + elem.Area_ID).closest('div').find('img')[0]) != "undefined") {
                                space = parseInt($('#area' + elem.Area_ID + ' .text').parent('div').css('top')) - $('#area' + elem.Area_ID).closest('div').find('img')[0].height + 30;
                            } else {
                                space = parseInt($('#area' + elem.Area_ID + ' .text').parent('div').css('top')) + 30;
                            }
                           
                            var bottom = $('#area' + elem.Area_ID + ' img').eq(1).height() == null ? 0 : $('#area' + elem.Area_ID + ' img').eq(1).height();

                            var height = $('#area' + elem.Area_ID + ' img').height() + $('#area' + elem.Area_ID + ' .text').height() + bottom + space

                            $('#area' + elem.Area_ID).css('height', height);

                            if (typeof ($('#area' + elem.Area_ID).closest('div').find('img')[1]) != "undefined") {
                                var id = $('#area' + elem.Area_ID).closest('div').find('img').parent('div')[1].id;

                                var top = parseInt($('#area' + elem.Area_ID + ' img').height() + $('#area' + elem.Area_ID + ' .text').height() + space);

                                $("#" + id).css('top', top);
                            }
                        }
                        $('#area' + elem.Area_ID).css('background-color', elem.Color);
                        $('#area' + elem.Area_ID).attr('color_val', elem.Color);

                        invitation.hidePlaceholder($('#area' + elem.Area_ID));
                    }
                } 
            });
        });
    },
    showPlaceholder: function (area) {
        if (area.find('.item').length == 0) {
            area.find('.placeholder').show();
        }
    },
    hidePlaceholder: function (area) {
        if (area.find('.item').length > 0) {
            area.find('.placeholder').hide();
        }
    }
}
var fonts = [
    { name: 'Noto Sans', key: "'Noto Sans KR', sans-serif" },
    { name: '나눔 고딕', key: "'Nanum Gothic', sans-serif" },
    { name: '나눔 고딕 코딩', key: "'Nanum Gothic Coding', monospace" },
    { name: '나눔 명조', key: "'Nanum Myeongjo', serif", },
    { name: '나눔 펜 스크립트', key: "'Nanum Pen Script', cursive" },
    { name: '나눔 브러쉬 스크립트', key: "'Nanum Brush Script', cursive" },
    { name: '제주 고딕', key: "'Jeju Gothic', sans-serif" },
    { name: '제주 명조', key: "'Jeju Myeongjo', serif" },
    { name: '제주 한라산', key: "'Jeju Hallasan', cursive" },
    { name: '코펍 바탕', key: "'KoPub Batang', serif" },
    { name: '한나', key: "'Hanna', sans-serif" },
    { name: '아리따부리', key: "'Arita-buri-SemiBold'" },
    { name: '앵무부리', key: "'116angmuburi'" },
    { name: '에스코어드림Lt', key: "'S-CoreDream-3Light'" },
    { name: '에스코어드림Reg', key: "'S-CoreDream-4Regular'" },
    { name: 'Cinzel', key: "'Cinzel', serif" }]
var font_cnt = 0;
$(document).ready(function () {

    var strMapMedia = "";
    var container = [];
    var cropper;

    if ($("#Order_Code").val() == "") {
        alert("유효하지 않은 주문입니다.")
        location.replace("/");
    }

    invitation.initMode();
    if ($("#Photo_YN").val() == "Y") {
        photo_yn = true;
    }
    else {
        photo_yn = false;
    }
    if (photo_yn) {
        $('.tdphoto').css('display', '')
    } else {
        $('.tdphoto').css('display', 'none')
    }
    $('#wrap').css('background-color', $("#Background_Color").val());
    if ($("#Objects").text() != "") {
        objects = JSON.parse($("#Objects").text());
        invitation.setItem();
    }


    invitation.setArea(JSON.parse($("#TB_Area").text()));

    if ($("#ETCs").text() != "") {
        etcs = JSON.parse($("#ETCs").text());
        etcs.forEach(function (elem) {
            $("#etc_title_" + elem.Sort).html(elem.Etc_Title);
            $("#etc_contents_" + elem.Sort).html(elem.Information_Content.replace(/\n/g, '<br/>'));
            $("#etc_title_" + elem.Sort).parents('li').css("padding-bottom", "10px");
        });
    }

    //혼주정보
    if ($("#Parents_Information_Use_YN").val() == "N") {
        $("#area4 .list_wrap").hide();
    } else {
        $("#area4 .list_wrap").show();
    }
    //갤러리
    if ($("#Gallery_Use_YN").val() == "N") {
        $("#area5").hide();
        $("#area6").hide();
    } else {
        $("#area5").show();
        $("#area6").show();
    }

    //동영상
    if ($("#Invitation_Video_Use_YN").val() == "N") {
        $("#area7").hide();
        $("#area8").hide();
    } else {
        $("#area7").show();
        $("#area8").show();
    }

    //기타정보
    if ($("#Etc_Information_Use_YN").val() == "N") {
        $("#area11").hide();
    } else {
        $("#area11").show();
    } 

    var accounts = [];
    if ($("#Accounts").text() != "") {
        accounts = JSON.parse($("#Accounts").text());
    }
    var groom_accounts = [];
    if ($("#GroomAccounts").text() != "") {
        groom_accounts = JSON.parse($("#GroomAccounts").text());
    }
    var bride_accounts = [];
    if ($("#BrideAccounts").text() != "") {
        bride_accounts = JSON.parse($("#BrideAccounts").text());
    }


    //축의금
    if ($("#MoneyGift_Remit_Use_YN").val() == "N" && $("#MoneyAccount_Remit_Use_YN").val() == "N" && $("#MoneyAccount_Div_Use_YN").val() == "N") {
        $("#area12").hide();
        $("#area13").hide();
    } else {

        //비회원
        if ($("#User_ID").val() == "") {
            $("#area12").show();
            $("#area13").show();

            //카카오
            $(".remittance_btn").hide();

            //통합
            if ($("#MoneyAccount_Remit_Use_YN").val() == "Y") {
                $(".an_btn.account").show();
            } else {
                $(".an_btn.account").hide();
            }

            //신랑측
            if ($("#MoneyAccount_Div_Use_YN").val() == "Y" && groom_accounts.length > 0) {
                $(".an_btn.groom").show();
            } else {
                $(".an_btn.groom").hide();
            }

            //신부측
            if ($("#MoneyAccount_Div_Use_YN").val() == "Y" && bride_accounts.length > 0) {
                $(".an_btn.bride").show();
            } else {
                $(".an_btn.bride").hide();
            }

        } else {
            $("#area12").show();
            $("#area13").show();

            //카카오
            if ($("#MoneyGift_Remit_Use_YN").val() == "Y") {
                $(".remittance_btn").show();
            } else {
                $(".remittance_btn").hide();
            }

            
            //통합
            if ($("#MoneyAccount_Remit_Use_YN").val() == "Y") {
                $(".an_btn.account").show();
            } else {
                $(".an_btn.account").hide();
            }

            //신랑측
            if ($("#MoneyAccount_Div_Use_YN").val() == "Y" && groom_accounts.length > 0) {
                $(".an_btn.groom").show();
            } else {
                $(".an_btn.groom").hide();
            }

            //신부측
            if ($("#MoneyAccount_Div_Use_YN").val() == "Y" && bride_accounts.length > 0) {
                $(".an_btn.bride").show();
            } else {
                $(".an_btn.bride").hide();
            }

        }
    }

    //방명록
    if ($("#GuestBook_Use_YN").val() == "N") {
        $("#area14").hide();
        $("#area15").hide();
    } else {
        $("#area14").show();
        $("#area15").show();
    }


    fonts.forEach(function (elem) {
        $("#selFont").append("<option value=\"" + elem.key + "\">" + elem.name + "</option>");
        $("#selFont option:eq(" + font_cnt + ")").css({ "font-family": elem.key });
        font_cnt++;
    });
    $("#selFont").on('change', function () {
        invitation.changeTextCss(this);
    });
    $("#Delete").on('click', function () {
        if ($(".item.selected").hasClass('photo-image')) {
            $('#btnphotoupload').addClass('btn-primary');
            $('#btnphotoupload').removeClass('btn-secondary');
            $('#btnphotoupload').addClass('empty');
            $('#btnphotoupload').text('추가');
        }

        invitation.pid = $(".item.selected").parent('div').attr('id');
        $(".item.selected").remove();
        $(".matchinfo.selected").remove();
        invitation.delObject();
        invitation.initMode();
        if ($("#" + invitation.pid + " .item").length < 1) {
            invitation.resizeArea('delete', invitation.pid);
        }
        invitation.showPlaceholder($('#' + invitation.pid));
    })
    $(document).on("keydown", function (event) {
        if (event.keyCode == 46) {

            if ($(".item.selected").hasClass('photo-image')) {
                $('#btnphotoupload').addClass('btn-primary');
                $('#btnphotoupload').removeClass('btn-secondary');
                $('#btnphotoupload').addClass('empty');
                $('#btnphotoupload').text('추가');
            }

            invitation.pid = $(".item.selected").parent('div').attr('id');
            $(".item.selected").remove();
            $(".matchinfo.selected").remove();
            invitation.delObject();
            invitation.initMode();
            if ($("#" + invitation.pid + " .item").length < 1) {
                invitation.resizeArea('delete', invitation.pid);
            }
            invitation.showPlaceholder($('#' + invitation.pid));


        }
    });
    $("input").on("focus", function (e) {
        $(document).unbind('keydown');

        var id = $(this).attr('indx');

        if (id != null) {
            var position = $("#area" + id).position().top;
            var scroll = $(".dvView>.card").scrollTop();
            var top = scroll + position;
            $(".dvView>.card").scrollTop(top);
        }
    });
    $("input").on("blur", function (e) {
        $(document).on("keydown", function (event) {
            if (event.keyCode == 46) {

                if ($(".item.selected").hasClass('photo-image')) {
                    $('#btnphotoupload').addClass('btn-primary');
                    $('#btnphotoupload').removeClass('btn-secondary');
                    $('#btnphotoupload').addClass('empty');
                    $('#btnphotoupload').text('추가');
                }

                invitation.pid = $(".item.selected").parent('div').attr('id');
                $(".item.selected").remove();
                $(".matchinfo.selected").remove();
                invitation.delObject();
                invitation.initMode();
                if ($("#" + invitation.pid + " .item").length < 1) {
                    invitation.resizeArea('delete', invitation.pid);
                }
                invitation.showPlaceholder($('#' + invitation.pid));


            }
        });
    });
    $("#Greetings",).on("focus", function (e) {
        $(document).unbind('keydown');

        var position = $("#area2").position().top;
        var scroll = $(".dvView>.card").scrollTop();
        var top = scroll + position;
        $(".dvView>.card").scrollTop(top);
    });
    $("#Greetings",).on("blur", function (e) {
        $(document).on("keydown", function (event) {
            if (event.keyCode == 46) {

                if ($(".item.selected").hasClass('photo-image')) {
                    $('#btnphotoupload').addClass('btn-primary');
                    $('#btnphotoupload').removeClass('btn-secondary');
                    $('#btnphotoupload').addClass('empty');
                    $('#btnphotoupload').text('추가');
                }

                invitation.pid = $(".item.selected").parent('div').attr('id');
                $(".item.selected").remove();
                $(".matchinfo.selected").remove();
                invitation.delObject();
                invitation.initMode();
                if ($("#" + invitation.pid + " .item").length < 1) {
                    invitation.resizeArea('delete', invitation.pid);
                }
                invitation.showPlaceholder($('#' + invitation.pid));


            }
        });
    });
    $('.invitationarea').mousedown(function (e) {

        $(".ui-resizable-handle").removeClass('resizabled');
        $(".resizable").removeClass("selected");
        $(".matchinfo").removeClass("selected");
        $(this).addClass("selected");
        area.id = $(this).attr('id');
        invitation.x = e.pageX - $(this).offset().left;
        invitation.y = e.pageY - $(this).offset().top;
        area.width = $(this).width();
        area.height = $(this).height();

        $(".ui-resizable-handle").hide();


        invitation.initMode();
        $("#divDisabled3").css("display", "none");
        $("#object_bgcolor").val($(this).attr("color_val"));
    });
    $("#selHour").find('option').each(function () {

        if ($(this).val() == $("#WeddingHour").val()) {
            $(this).attr("selected", "selected");
            $("#WeddingHour").val(this.value);
        }
    });
    $("#selMin").find('option').each(function () {
        if ($(this).val() == $("#WeddingMin").val()) {
            $(this).attr("selected", "selected");
            $("#WeddingMin").val(this.value);
        }
    });
    $('[match]').bind('change, keyup', function () {
        $('.matchinfo').trigger('keyup');

    })
    $("#save").click(function () {
        invitation_areas = [];
        tb_area.forEach(function (elem) {
            var sort = 1;
            $("#wrap").find(">div").each(function () {
                if ($(this).attr('idx') == elem.Area_ID) {
                    $("#wrap>div[idx='" + elem.Area_ID + "']").css("display", "block");
                    if ($(this).hasClass('invitationarea')) {
                        var obj = new Object();
                        obj.Area_ID = elem.Area_ID;
                        obj.Size_Height = $(this).height();
                        obj.Size_Width = $(this).width();
                        obj.Color = $(this).attr("color_val");
                        obj.Sort = sort;
                        invitation_areas.push(obj);
                    }
                }
                sort++;
            });
        });
        invitation.initClass();


        if (validation_check()) {
            var result = confirm('저장 하시겠습니까?');
            if (result) {
                fn_save();
            }
        }
    });
    $("#close").click(function () {
        self.close();
    });
    $('.crop_btn').click(function () {
        $('.crop_btn').hide();
        var img_uri = cropper.getCroppedCanvas().toDataURL('image/jpeg').replace(/^data[:]image\/(png|jpg|jpeg)[;]base64,/i, "");

        fn_photoImageUpload(img_uri);
    });
    $("#selHour").change(function () {
        $("#selHour").find('option').each(function () {
            if ($(this).val() == $("#selHour option:selected").val()) {
                $(this).attr("selected", "selected");
                $("#WeddingHour").val(this.value);
                $("#WeddingHour").trigger("keyup");

            } else {
                $(this).attr("selected", false);
            }
        });
    });
    $("#selMin").change(function () {
        $("#selMin").find('option').each(function () {
            if ($(this).val() == $("#selMin option:selected").val()) {
                $(this).attr("selected", "selected");
                $("#WeddingMin").val(this.value);
                $("#WeddingMin").trigger("keyup");
            } else {
                $(this).attr("selected", false);
            }
        });
    });
    $("input[name='Time_Type_Code']:radio").change(function () {
        $("#Time_Type_Code").val(this.value);
        $("#Time_Type_Name").val(this.value);
        $("#Time_Type_Name").trigger("keyup");
    });
    $("input[name='Time_Type_Eng_YN']:radio").change(function () {
        $("#Time_Type_Eng_YN").val(this.value);

        if (this.value == "Y") {
            if ($("#Time_Type_Name").val() == "오후" || $("#Time_Type_Name").val() == "PM") {
                $("#Time_Type_Name").val("PM");
            } else {
                $("#Time_Type_Name").val("AM");
            }
        } else {
            if ($("#Time_Type_Name").val() == "오후" || $("#Time_Type_Name").val() == "PM") {
                $("#Time_Type_Name").val("오후");
            } else {
                $("#Time_Type_Name").val("오전");
            }
        }

        $("#Time_Type_Name").trigger("keyup");
    });
    $("input[name='WeddingWeek_Eng_YN']:radio").change(function () {
        $("#WeddingWeek_Eng_YN").val(this.value);

        $('#WeddingDate').trigger('change');
    });
    $('body').on('keyup', '.matchinfo', function () {
        invitation.changeText(this);
    });
    $('body').on('change', '.matchinfo.selected', function () {
        invitation.changeText(this);
    });
    $('body').on('change', '#FontSize', function () {
        invitation.changeTextCss(this);
    });
    $('body').on('keyup', '#FontSize', function () {
        invitation.changeTextCss(this);
    });
    $("#selMatchInfo").on('change', function () {
        $(".matchinfo.selected").val($(".matchinfo.selected").val() + this.value);
        $('.matchinfo').trigger('change');

        $("#selMatchInfo option:eq(0)").prop("selected", true);

        $(".matchinfo.selected").focus();
    });
    $('.btn-plus, .btn-minus').on('click', function (e) {
        const isNegative = $(e.target).closest('.btn-minus').is('.btn-minus');
        const input = $(e.target).closest('.inline-group').find('input');
        if (input.is('input')) {
            input[0][isNegative ? 'stepDown' : 'stepUp']()

            $('#FontSize').trigger('change');
        }
    })
    $('#Bold').on('click', function (e) {
        $(this).hasClass("selected") ? $(this).removeClass("selected") : $(this).addClass("selected");
        invitation.changeTextCss(this);
    });
    $('#Italic').on('click', function (e) {
        $(this).hasClass("selected") ? $(this).removeClass("selected") : $(this).addClass("selected");
        invitation.changeTextCss(this);
    });
    $('#Underline').on('click', function (e) {
        $(this).hasClass("selected") ? $(this).removeClass("selected") : $(this).addClass("selected");
        invitation.changeTextCss(this);
    });
    $('#Left').on('click', function (e) {
        $(this).addClass("selected");
        $('#Center').removeClass("selected");
        $('#Right').removeClass("selected");
        invitation.changeTextCss(this);
    });
    $('#Center').on('click', function (e) {
        $(this).addClass("selected");
        $('#Left').removeClass("selected");
        $('#Right').removeClass("selected");
        invitation.changeTextCss(this);
    });
    $('#Right').on('click', function (e) {
        $(this).addClass("selected");
        $('#Left').removeClass("selected");
        $('#Center').removeClass("selected");
        invitation.changeTextCss(this);
    });
    $('#Top').on('click', function (e) {
        $(this).addClass("selected");
        $('#Middle').removeClass("selected");
        $('#Bottom').removeClass("selected");
        invitation.changeTextCss(this);
    });
    $('#Middle').on('click', function (e) {
        $(this).addClass("selected");
        $('#Top').removeClass("selected");
        $('#Bottom').removeClass("selected");
        invitation.changeTextCss(this);
    });
    $('#Bottom').on('click', function (e) {
        $(this).addClass("selected");
        $('#Top').removeClass("selected");
        $('#Middle').removeClass("selected");
        invitation.changeTextCss(this);
    });
    $('#Between_Text_Calc').on('input change', function (e) {
        $("#Between_Text").val($(this).val())
        $("#Between_Text").trigger("blur");
        invitation.changeTextCss(this);
    });
    $('#Between_Line_Calc').on('input change', function (e) {
        $("#Between_Line").val($(this).val())
        $("#Between_Line").trigger("blur");
        invitation.changeTextCss(this);
    });
    $('body').on('keyup', '#Between_Text', function () {
        $("#Between_Text_Calc").val($(this).val())
        invitation.changeTextCss(this);
    });
    $('body').on('keyup', '#Between_Line', function () {
        $("#Between_Line_Calc").val($(this).val())
        invitation.changeTextCss(this);
    });
    $('#WeddingDate').change(function () {
        var d = new Date($('#WeddingDate').val());

        var week2;

        week = new Array('일', '월', '화', '수', '목', '금', '토', '일');

        if ($("#WeddingWeek_Eng_YN").val() == "Y") {
            week2 = new Array('SUN', 'MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT', 'SUN');
        } else {
            week2 = new Array('일', '월', '화', '수', '목', '금', '토', '일');
        }

        $('#WeddingYY').val(d.getFullYear());
        $('#WeddingDate').trigger('keyup');
        $('#WeddingMM').val(d.getMonth() + 1);
        $('#WeddingDD').val(d.getDate());
        $('#WeddingWeek').val(week[d.getDay()]);
        $('#WeddingWeekName').val(week2[d.getDay()]);
        $('#WeddingWeekName').trigger('keyup');
    });
    $('.addition_image_select').change(function () {
        if ($(this).val()) {
            var img_file = $(this).get(0).files;

            var img_URL = URL.createObjectURL(img_file[0]);

            var modal_con = '';

            modal_con += '<div class="crop_container" >';
            modal_con += '<img id="crop_preview" src="' + img_URL + "\" style=\" max-height: 400px;\">";
            modal_con += '</div>';

            var scale_x = 1;
            var scale_y = 1;

            if ($(window).width() > 750) {
                $('#call-cropImg').find('.modal-body').html(modal_con);

                var image = document.getElementById('crop_preview');


                var minWidth = $('#call-cropImg .modal-dialog').width() - 30;
                var minHeight = minWidth * scale_y;
                cropper = new Cropper(image, {
                    //aspectRatio: scale_x / scale_y,
                    minContainerWidth: minWidth,
                    minContainerHeight: minHeight,
                });

                $('#call-cropImg').modal('show');
            }
        }

    });
    $('#btnphotoupload').on('click', function () {

        if ($(this).hasClass('empty')) {
            $('#area1').trigger('mousedown');
            $('.crop_btn').show();
            $('.addition_image_select').val("");
            $('.addition_image_select').trigger('click');
        } else {
            $('#area1>.photo-image').remove();
            invitation.delObject();
            invitation.initMode();

            $('#btnphotoupload').addClass('btn-primary');
            $('#btnphotoupload').removeClass('btn-secondary');
            $('#btnphotoupload').addClass('empty');
            $('#btnphotoupload').text('추가');

        }
    });
    $('#btncssupload').on('click', function () {
        if ($(this).hasClass('empty')) {
            fn_addCSS();
        } else {
            fn_delCSS();
        }
    });
    $('#btnjsupload').on('click', function () {
        if ($(this).hasClass('empty')) {
            fn_addJS();
        } else {
            fn_delJS();
        }
    });
    $('#invitation_bgcolor').colorpicker({
        format: "hex"
    });
    $('#invitation_bgcolor').colorpicker().on('changeColor', function (event) {
        $('#wrap').css('background-color', event.color.toString());

        if ($('#invitation_bgcolor').val() == '') {
            $('#wrap').css('background-color', '');
        }
    });
    $('#object_txtcolor').colorpicker({
        format: "hex"
    });
    $('#object_txtcolor').colorpicker().on('changeColor', function (event) {
        objects.forEach(function (elem) {
            if (elem.id == $(".item.selected").attr('id')) {
                elem.fontcolor = $("#object_txtcolor").val().toUpperCase()
            }
        });
        $('.item.selected>.text').css('color', $("#object_txtcolor").val().toUpperCase());
    });
    $('#object_bgcolor').colorpicker({
        format: "hex"
    });
    $('#object_bgcolor').colorpicker().on('changeColor', function (event) {

        if ($(".selected").attr('id').substring(0, 4) != 'area') {

            objects.forEach(function (elem) {
                if (elem.id == $(".item.selected").attr('id')) {
                    elem.bgcolor = $("#object_bgcolor").val().toUpperCase()
                }
            });

            $('.item.selected>.text').css('background-color', $("#object_bgcolor").val().toUpperCase());
            $('.item.selected').css('background-color', $("#object_bgcolor").val().toUpperCase());
            if ($('#object_bgcolor').val() == '') {
                $('.item.selected>.text').css('background-color', '');
                $('.item.selected').css('background-color', '');
            }
        } else {
            $('.invitationarea.selected').css('background-color', $("#object_bgcolor").val().toUpperCase());
            $('.invitationarea.selected').attr("color_val", $("#object_bgcolor").val().toUpperCase());
        }

    });
    $('.list_con').slideDown();
    $('.info_detail').on('click', function () {
        $(this).parent('.list_wrap').toggleClass('on');
        $('.list_con').slideToggle();
    });
    $('#gallery').find('figure').each(function () {
        var $link = $(this).find('a'),
            item = {
                src: $link.attr('href'),
                w: $link.data('width'),
                h: $link.data('height'),
                title: $link.data('caption')
            };
        container.push(item);
    });
    $('#gallery').find('figure').each(function () {
        var $link = $(this).find('a'),
            item = {
                src: $link.attr('href'),
                w: $link.data('width'),
                h: $link.data('height'),
                title: $link.data('caption')
            };
        container.push(item);
    });
    $('.remittance_btn.type01').click(function () {
        $('.remittance_pop_h').show();
    });
    $('.remittance_btn.type02').click(function () {
        $('.remittance_pop_w').show();
    });
    $('.close_btn').click(function () {
        $('.remittance_pop_h, .remittance_pop_w').hide();
    });
    $(".message_del").on('click', function () {
        $(this).parents('li').find(".password_check").slideToggle();
    });
    $("#chk_effect").on('click', function () {
        var chk = $(this).is(":checked")

        if (chk) {
            $("#area1").find(">div").not(".item").not(".ui-resizable-handle").not(".ui-resizable-s").each(function () {
                $(this).show();
            });

            if (typeof $(".d_day").val() != 'undefined') {
                $(".d_day").show();
            }

        } else {
            $("#area1").find(">div").not(".item").not(".ui-resizable-handle").not(".ui-resizable-s").each(function () {
                $(this).hide();
            });

            if (typeof $(".d_day").val() != 'undefined') {
                $(".d_day").hide();
            }
        }
    });
    $("#Weddinghall_Name").on("keyup", function () {
        $('#incNaverMap')[0].contentWindow.changeMarker();
    });
    $(".input_match").on("keyup", function () {
        var keyInput = $(this).attr("id");
        var inputValue = $(this).val();
        fn_match(keyInput, inputValue);
    });
    var galleryThumbs = new Swiper('.gallery-thumbs', {
        spaceBetween: 10,
        slidesPerView: 3,
        loop: true,
        freeMode: true,
        loopedSlides: 5,
        watchSlidesVisibility: true,
        watchSlidesProgress: true,
    });
    var galleryTop = new Swiper('.gallery-top', {
        spaceBetween: 10,
        loop: true,
        loopedSlides: 5,
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        },
        thumbs: {
            swiper: galleryThumbs,
        },
    });
    $('.invitationarea').mousedown(function (e) {
        $(".ui-resizable-handle").removeClass('resizabled');
        $(".resizable").removeClass("selected");
        $(this).addClass("selected");
        $(this).children(".ui-resizable-handle").addClass('resizabled');
        $(".ui-resizable-handle").hide();
        $(this).children(".ui-resizable-handle").show();
        $('.ui-resizable-handle.ui-resizable-s.resizabled').css("display", "block");
        $('.ui-resizable-handle.ui-resizable-s.resizabled').css('height', 0);
        $('.ui-resizable-handle.ui-resizable-s.resizabled').css('bottom', -2);

        e.stopPropagation();
    });
    $('.invitationarea').resizable({
        handles: 's'
    });
    $('.ui-resizable-handle.ui-resizable-s').css("display", "none");
    $(".guideline").css("height", $("#wrap").outerHeight())
    if ($("#Product_Category_Name").val() == "M감사장") {
        $("#area4 .list_wrap").hide();
    }
    if ($("#Attached_File1_URL").val() != "") {
        $('#btncssupload').removeClass('btn-primary');
        $('#btncssupload').addClass('btn-secondary');
        $('#btncssupload').removeClass('empty');
        $('#btncssupload').text('삭제');
    }
    if ($("#Attached_File2_URL").val() != "") {
        $('#btnjsupload').removeClass('btn-primary');
        $('#btnjsupload').addClass('btn-secondary');
        $('#btnjsupload').removeClass('empty');
        $('#btnjsupload').text('삭제');
    }
    if ($("#Attached_File1_URL").val() != "" && $("#Attached_File2_URL").val() != "") {
        $('#design_header span').show();
    }

    $("#WeddingDate").on("keyup", function () {
        setDday($("#WeddingDate").val());
    });

    $(".invitation_open").on("click", function () {
        window.open($(".td_Invitation_URL strong").html(), "_blank");
    });

    $(".invitation_copy").on("click", function () {

        var url = $(".td_Invitation_URL").find('strong').text();

        copyToClipboardByText(url);
    });

    var Invitation_ID = $('#Invitation_ID').val();
    var Outline_Type_Code = $('#Outline_Type_Code').val();
    var Outline_Image_URL = $('#Outline_Image_URL').val();


    if (Outline_Type_Code == "OTC01") {
        strMapMedia = '<iframe id="incNaverMap" src="/Admin/Invitation/NaverMap/' + Invitation_ID + '" width="750" height="320" frameborder="0" style="border: 0" allowfullscreen></iframe>';
    }
    else {
        strMapMedia = '<img src="' + Outline_Image_URL + '" class="mapImg" id="PreviewMap" style="border: 1px solid #ccc;" onerror="this.style.display=\'none\';">';
    }

    $(".map_wrap").html(strMapMedia);


    if ($("#Invitation_Video_Use_YN").val() == "Y") {
        setVideo($("#Invitation_Video_Type_Code").val());
    }

    if ($("#Product_Category_Code").val() == "PCC02") {
        //감사장
        $("#area4 .list_wrap").hide();
        $("#area5").hide();
        $("#area6").hide();
        $("#area7").hide();
        $("#area8").hide();
        $("#area9").hide();
        $("#area10").hide();
        $("#area11").hide();
        $("#area12").hide();
        $("#area13").hide();
        $("#area14").hide();
        $("#area15").hide();
    }
});
$.ajax({
    async: false,
    url: '/Admin/Template/GetContextMenuTextItemList',
    type: 'POST',
    dataType: 'json',
    success: function (json) {
        var list = json.result;
        for (var i = 0; i < list.length; i++) {
            mappingfield[list[i].reserveWord] = { "name": fn_replace(list[i].reserveWord, '#', '') };

            $("#selMatchInfo").append("<option value=" + list[i].reserveWord + ">" + fn_replace(list[i].reserveWord, '#', '') + "</option>");
        }
    }
});
function copyToClipboard(elementId) {
    var aux = document.createElement("input");
    aux.setAttribute("value", document.getElementById(elementId).innerHTML);
    document.body.appendChild(aux);
    aux.select();
    document.execCommand("copy");
    document.body.removeChild(aux);
}

function copyToClipboardByText(text) {
    var aux = document.createElement("textarea");
    aux.value = text;
    document.body.appendChild(aux);
    aux.select();
    aux.setSelectionRange(0, 9999);
    document.execCommand("copy");
    document.body.removeChild(aux);
}

function validation_check() {
    if ($('.tdphoto').css('display') != 'none' && $("#btnphotoupload").hasClass('empty')) {
        alert("포토이미지를 추가하세요.");
        return false;
    }

    return true;
}

function dataURLtoFile(dataurl, filename) {

    var arr = dataurl.split(','),
        mime = arr[0].match(/:(.*?);/)[1],
        bstr = atob(arr[1]),
        n = bstr.length,
        u8arr = new Uint8Array(n);

    while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
    }

    return new File([u8arr], filename, { type: mime });
}
function fn_save() {
    $(".loader_preview").show();
    var invitation = new Object();
    var invitation_detail = new Object();


    invitation.invitation_id = $("#Invitation_ID").val();

    invitation_detail.invitation_id = $("#Invitation_ID").val();
    invitation_detail.Invitation_URL = $("#Invitation_URL").val();
    invitation_detail.Invitation_Title = $("#Invitation_Title").val();
    invitation_detail.greetings = $("#Greetings").val();
    invitation_detail.groom_name = $("#Groom_Name").val();
    invitation_detail.groom_phone = $("#Groom_Phone").val();
    invitation_detail.bride_name = $("#Bride_Name").val();
    invitation_detail.bride_phone = $("#Bride_Phone").val();
    invitation_detail.groom_parents1_name = $("#Groom_Parents1_Name").val();
    invitation_detail.groom_parents1_phone = $("#Groom_Parents1_Phone").val();
    invitation_detail.groom_parents2_name = $("#Groom_Parents2_Name").val();
    invitation_detail.groom_parents2_phone = $("#Groom_Parents2_Phone").val();
    invitation_detail.bride_parents1_name = $("#Bride_Parents1_Name").val();
    invitation_detail.bride_parents1_phone = $("#Bride_Parents1_Phone").val();
    invitation_detail.bride_parents2_name = $("#Bride_Parents2_Name").val();
    invitation_detail.bride_parents2_phone = $("#Bride_Parents2_Phone").val();
    invitation_detail.weddingdate = $("#WeddingDate").val();
    invitation_detail.weddinghhmm = $("#WeddingHHmm").val();
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
    invitation_detail.WeddingWeek_Eng_YN = $("#WeddingWeek_Eng_YN").val();
    invitation_detail.Time_Type_Eng_YN = $("#Time_Type_Eng_YN").val();

    var myData = {
        "invitation": invitation,
        "invitation_detail": invitation_detail,
        "invitation_areas": invitation_areas,
        "invitation_item_resources": objects,
        "Delegate_Image_URL": $("#Delegate_Image_URL").val(),
        "Delegate_Image_Height": $("#Delegate_Image_Height").val(),
        "Delegate_Image_Width": $("#Delegate_Image_Width").val(),

    };

    $.ajax({
        url: "/Admin/Invitation/Save",
        data: myData,
        type: 'POST',
        dataType: 'json',
        success: function (result) {
            if (result.success == "Y") {
                location.reload();
            } else {
                $(".loader_preview").hide();
                alert("오류가 발생하였습니다.");
            }
        }
    });
}
function fn_photoImageUpload(img_uri) {
    $.ajax({
        url: '/Admin/Invitation/PhotoImageUpload',
        data: {
            "imageData": img_uri,
            "upload_path" : $('#upload_path').val()
        },
        type: 'POST',
        success: function (result) {
            invitation.org_width = result.width;
            invitation.org_height = result.height

            if (result.width >= area.width) {
                invitation.x = 0;
                invitation.y = 0;

                result.height = parseFloat(result.height * area.width / result.width)

                result.width = parseFloat(area.width)

                if (area.height < result.height) {
                    $('#area1').height(result.height);
                    area.height = result.height;
                }
            }

            invitation.resource_url = result.resource_url;
            invitation.width = result.width;
            invitation.height = result.height;

            $("#Delegate_Image_URL").val(result.resource_url);
            $("#Delegate_Image_Height").val(result.width);
            $("#Delegate_Image_Width").val(result.height);

            invitation.setInvitationIdx();

            invitation.id = 'item_' + invitation.idx;
            var div = "<div id='" + invitation.id + "' idx='" + invitation.idx + "' class='item ui-widget-content selected resizable photo-image' style='top: " + invitation.y + "px; left: " + invitation.x + "px;  position:absolute;'><span class='topline'></span><span class='rightline'></span><span class='botline'></span><span class='leftline'></span><img class='img' src='" + invitation.resource_url + "' width='" + invitation.width + "px' height='" + invitation.height + "px'  /><span class='img-info'></span></div>";
            $('#area1').append(div);
            $('#area1').removeClass('selected');


            $('#btnphotoupload').removeClass('btn-primary');
            $('#btnphotoupload').addClass('btn-secondary');
            $('#btnphotoupload').removeClass('empty');
            $('#btnphotoupload').text('삭제');

            invitation.addObject($(this), 'photo');
            invitation.imageMode();
            invitation.addImageEvent();

            $('.close[data-dismiss="modal"]').trigger('click');
        }
    });
}
function fn_replace(str, searchStr, replaceStr) {
    return str.split(searchStr).join(replaceStr);
}
function fn_match(target, txt) {
    $("#" + target).val(txt);
    if ($("span.i_" + target).length > 0) {
        $("span.i_" + target).text(txt);
    }
    $('.matchinfo').trigger('keyup');
}

function uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
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

function setVideo(id) {
    switch (id) {
        case "VTC01"://Youtube
            var htmlInput = $("#Invitation_Video_URL").val();
            var pattern = /(?:http?s?:\/\/)?(?:www\.)?(?:youtube\.com|youtu\.be)\/(?:watch\?v=)?(.+)/g;

            if (pattern.test(htmlInput)) {
                var replacement = '<iframe src="//www.youtube.com/embed/$1" frameborder="0" class="embed-container" allowfullscreen></iframe>';


                htmlInput = htmlInput.replace(pattern, replacement);
            }

            $(".iframe_wrap").html(htmlInput);

            break;
        case "VTC02": //Vimeo
            $(".iframe_wrap").html($("#Invitation_Video_URL").val());
            break;
        case "VTC03"://FEELMAKER
            $(".iframe_wrap").html($("#Invitation_Video_URL").val());
            break;

    }

}

$(window).load(function () {
    if (typeof $(".d_day").val() != 'undefined') {
        $(".d_day").css("font-size", parseInt($(".d_day").css("font-size").replace("px", ""))  + "px");
        $(".d_day").show();
    }
    setDday($("#WeddingDate").val());
    //$("#wrap").find(">div").show();
    $("#wrap").find("#area3").hide();
    $(".loader_preview").hide();
});














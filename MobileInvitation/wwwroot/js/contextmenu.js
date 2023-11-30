$.contextMenu({
  selector: '.templatearea', 
  trigger: 'right',
  callback: function(key, options, e) {
    if(e.which == 1)
    {
        template.initClass();
      
      if(key == "add_image") {
        template.addImage();     
      }
      else {
        template.addText(key);
      }
    }
    else {
      return false;
    }
  },
  items: {
      "add_text": {
          name: "텍스트 추가",
          items: mappingfield
        },
      "add_image": {name: "이미지 추가"}
      }
});
$.contextMenu({
  selector: '.item', 
  trigger: 'right',
  callback: function(key, options, e) {

      if (e.which == 1)
    

        switch (key) {
            case "add_image":
                template.initClass();
                template.addImage();     
                break;
            case "trash" :
                template.pid = $(".item.selected").parent('div').attr('id');
                $(".item.selected").remove();
                $(".matchinfo.selected").remove();
                template.delObject();
                template.initMode();
                if ($("#" + template.pid + " .item").length < 1) {
                    template.resizeArea('delete', template.pid);
                }
                template.showPlaceholder($('#' + template.pid));
                break;
            case "bring_front" :
                template.bring_front();
              
                break;
            case "bring_forward" :
                template.bring_forward();
                break;  
            case "send_back" :
                template.send_back();
                break;
            case "send_backward" :
                template.send_backward();
                break;
            default:
                template.initClass();
                template.addText(key);
                break;
      }

  },
    items: {
        "add_text": {
            name: "텍스트 추가",
            items: mappingfield
        },
      "add_image": { name: "이미지 추가" },
      "sep1": "---------",
      "trash": {name: "삭제"},
        "sep2": "---------",
        "bring_front": {
            name: "맨 앞으로 가져오기",
            disabled: function () {

                var pid = $(".item.selected").parent('div').attr('id');

                return $("#" + pid + " > .item.selected").attr('id') == $("#" + pid + " > .item").last().attr('id') ? true : false;
             
            }
        }, 
        "bring_forward": {
            name: "앞으로 가져오기",
            disabled: function () {

                var pid = $(".item.selected").parent('div').attr('id');

                return $("#" + pid + " > .item.selected").attr('id') == $("#" + pid + " > .item").last().attr('id') ? true : false;
         
            }
        }, 
        "send_back": {
            name: "맨 뒤로 보내기",
            disabled: function () {

            
                var pid = $(".item.selected").parent('div').attr('id');

                return $("#" + pid + " > .item.selected").attr('id') == $("#" + pid + " > .item").first().attr('id') ? true : false;
        
            }
        }, 
        "send_backward": {
            name: "뒤로 보내기",
            disabled: function () {

                var pid = $(".item.selected").parent('div').attr('id');
                return $("#" + pid + " > .item.selected").attr('id') == $("#" + pid + " > .item").first().attr('id') ? true : false;
        
            }
        }
    }
 

   
});





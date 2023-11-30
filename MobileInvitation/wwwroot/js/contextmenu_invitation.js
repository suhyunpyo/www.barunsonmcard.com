$.contextMenu({
  selector: '.invitationarea', 
  trigger: 'right',
  callback: function(key, options, e) {
    if(e.which == 1)
    {
        invitation.initClass();
      
      if(key == "add_image") {
        invitation.addImage();     
      }
      else {
        invitation.addText(key);
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
                invitation.initClass();
                invitation.addImage();     
                break;
            case "trash" :
                invitation.pid = $(".item.selected").parent('div').attr('id');
                $(".item.selected").remove();
                $(".matchinfo.selected").remove();
                invitation.delObject();
                invitation.initMode();
                if ($("#" + invitation.pid + " .item").length < 1) {
                    invitation.resizeArea('delete', invitation.pid);
                }
                invitation.showPlaceholder($('#' + invitation.pid));
                break;
            case "bring_front" :
                invitation.bring_front();
              
                break;
            case "bring_forward" :
                invitation.bring_forward();
                break;  
            case "send_back" :
                invitation.send_back();
                break;
            case "send_backward" :
                invitation.send_backward();
                break;
            default:
                invitation.initClass();
                invitation.addText(key);
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





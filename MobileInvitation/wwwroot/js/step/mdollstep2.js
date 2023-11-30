
function fn_save(type) {
    $(".loader").show();
    var invitation_detail = new Object();
    invitation_detail.invitation_id = $("#Invitation_Id").val();
    invitation_detail.Invitation_Video_Use_YN = $("#Invitation_Video_Use_YN").val();
    invitation_detail.Invitation_Video_Type_Code = $("#Invitation_Video_Type_Code").val();
    invitation_detail.Invitation_Video_URL = $("#Invitation_Video_URL").val();
    invitation_detail.Gallery_Use_YN = $("#Gallery_Use_YN").val();
    invitation_detail.Gallery_Type_Code = $("#Gallery_Type_Code").val();
    invitation_detail.GalleryPreventPhoto_YN = $("#GalleryPreventPhoto_YN").val();

    var myData = {
        "invitation_detail": invitation_detail
    };
    $.ajax({
        url: "/Order/MDollStep2_Save",
        data: myData,
        type: 'POST',
        dataType: 'json',
        success: function (result) {
            if (result.success == "Y") {
                //$(".loader").hide();
                //$(".saved").css("display", "flex");
                if (type == "next") {
                    setTimeout(function () {
                        location.href = "/Order/StepLast/" + $("#Order_Id").val();
                    }, 1000);
                } else {
                    setTimeout(function () {
                        location.href = "/Order/MDollStep1/" + $("#Order_Id").val();
                    }, 1000);
                }
            } else {
                if (result.auth) {
                    $(".loader").hide();
                    $(".error").css("display", "flex").delay(1000).fadeOut();
                } else {
                    location.reload();
                }
            }
        }
    });
};


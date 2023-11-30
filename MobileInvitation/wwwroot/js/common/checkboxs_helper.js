var chklistArr = [];

$(function () {
    $("#Allgroup").prop("checked", $(".chk-style").length == $(".chk-style:checked").length);

    // 전체를 클릭하면 나머지 체크박스 전부 체크 처리 
    $("#Allgroup").click(function () {
        $(".chk-style").prop("checked", this.checked);
    });

    $(".chk-style").click(function () {
        $("#Allgroup").prop("checked", $(".chk-style").length == $(".chk-style:checked").length);
    });
})
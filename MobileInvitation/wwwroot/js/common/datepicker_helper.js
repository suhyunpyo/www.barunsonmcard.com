$().ready(function (e) {

    var url = location.href.toLocaleUpperCase();

    if (url.indexOf("ADMIN/BANNER") < 0 && url.indexOf("ADMIN/POPUP") < 0 && url.indexOf("ADMIN/NOTICE") < 0 && url.indexOf("ADMIN/FAQ") < 0) {

    // 달력이미지 클릭을 위해 button 컨트롤을 대신 숨겨서 클릭이벤트를 설정
    $("input:button").click(function () {

        var btnid = $(this).attr("id");
        $('#' + btnid.split("_")[1]).datepicker('show');
    });

    $(".fa-calendar-alt").click(function () {

        var eq = $(".fa-calendar-alt").index(this);
        $("input:button:eq(" + eq + ")").trigger("click");

    });

    $(".input-daterange").each(function () {
        var $inputs = $(this).find('input:text');
        $inputs.datepicker({
            showOn: "both", //focus, button, both 중에 선택할 수 있습니다.focus 는 포커스가 오면 달력이 팝업 됩니다.button 은 버튼을 클릭하면 달력이 팝업 됩니다.both는 두 가지 경우 모두에서 팝업 됩니다.
            todayBtn: "linked",
            autoclose: true,
            language: "ko",
            format: "yyyy-mm-dd"
        });

        if ($inputs.length >= 2) {
            var $from = $inputs.eq(0);
            var $to = $inputs.eq(1);

            $from.on('changeDate', function (e) {
                var d = new Date(e.date.valueOf());
                $to.datepicker('setStartDate', d); // 종료일은 시작일보다 빠를 수 없다.
            });
            $to.on('changeDate', function (e) {

                var d = new Date(e.date.valueOf());
                $from.datepicker('setEndDate', d); // 시작일은 종료일보다 늦을 수 없다.
            });
        }
    })


}

});


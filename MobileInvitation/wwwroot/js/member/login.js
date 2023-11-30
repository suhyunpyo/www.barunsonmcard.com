$(function(){

	//login banner
	var loginBanner = new Swiper(".login_banner .swiper-container", {
		spaceBetween: 0,
		slidesPerView: 1,
		loop: true,
		autoplay : {
			delay:5000,
		},
		pagination: {
			el :  '.swiper-pagination',
			clickable: true,
		}
	});

	//login banner hover event
	$('.login_banner').hover(function(){
		loginBanner.autoplay.stop();
	}, function(){
		loginBanner.autoplay.start();
	});
	$(".agree_btn").on('click', function () {
		$('.agree_pop').show();
	});
});

//팝업 열기 이벤트
function popOpen(popNum){
	var $popWrap = $('.find_pop .pop_wrap'),
		$popLi = $popWrap.find('li').eq(popNum),
		$popCon = $popWrap.find('.tab_con').eq(popNum);

	$popWrap.show();
	$popLi.addClass('active').siblings('li').removeClass('active');
	$popCon.show().siblings('div').hide();
}
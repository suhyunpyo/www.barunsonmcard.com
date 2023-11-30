$(function(){

	//인사말 샘플
	var popList = new Swiper(".pop_menu_list .swiper-container", {
		spaceBetween: 8,
		slidesPerView: 'auto',
	});

	$('.pop_menu_list .swiper-slide').on('click', function(){
		var idx = $(this).index();
		$(this).siblings('.swiper-slide').find('a').removeClass('active');
		$(this).find('a').addClass('active');

		$('.sample_con').not(idx).hide();
		$('.sample_con').eq(idx).show();
		
	});

});
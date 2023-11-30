$(function(){

	if( $('.complate_banner').length > 0 ){
		new Swiper(".complate_banner", {
			spaceBetween: 0,
			slidesPerView: 1,
			centeredSlides: true,
			roundLengths: true,
			loop: true,
			autoplay: {
				delay: 3000,
				disableOnInteraction: false,
			},
			speed: 300,
			loopAdditionalSlides: 30,
			pagination: {
				el: '.complate_banner .swiper-pagination',
				type: 'custom',
				renderCustom: function (swiper, current, total) {
					return "<span class='swiper-pagination-current mont'>"+('' + current).slice(-2)+"</span>" + ' / ' + "<span class='swiper-pagination-total mont'>"+('' + total).slice(-2)+"</span>";
				},
			},
			
		});
	}
});
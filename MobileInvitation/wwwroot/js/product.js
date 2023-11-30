$(function(){

	//preview_frame
	var agent = navigator.userAgent.toLowerCase();
	if (agent.indexOf("whale") !== -1) {
		$('#preview_frame').css('width', '100%');
    }


	//좋아요 버튼 클릭 이벤트
	$(".wish").on('click', function(e){
		e.preventDefault();
		//$(this).toggleClass('active');
	});

	$('.sub_con_wrap .list_wrap li .img_con').on('click', function(){
		
		$('.sub_con_wrap .list_wrap li').removeClass('active');
		$(this).parents('li').toggleClass('active');
	});

	//productSlide
	var productSlide = new Swiper(".category_button.swiper-container", {
		slidesPerView: 'auto',
		slidesPerGroup: 10,
		scrollbarDraggable: true,
		watchSlidesVisibility: true,
		watchSlidesProgress: true,
		centeredSlides: false,
		grabCursor: true,
		observer: true,
		observeParents: true,
	});


	$('.preview_box .img_con').on('scroll touchmove mousewheel', function (event) {
		var prevScroll = $(this).scrollTop();
		var innerHeight = $(this).innerHeight();
		var scrollHeight = $(this).prop('scrollHeight');
		var delta = 0;

		if (prevScroll + innerHeight >= scrollHeight) {
			//alert('aa');
			event.preventDefault();
			event.stopPropagation();

			//return false;
		} else {
			$('.preview_box .img_con').unbind();
		}

	});



	$('.preview_info_box').on('mousedown touchstart', function () {
		$(this).addClass('hide');
	});


	var autoplay = 5000;

	var detailSlide = new Swiper(".detail_slide", {
		spaceBetween: 0,
		slidesPerView: 1,
		centeredSlides: true,
		roundLengths: true,
		loop: true,
		autoplay: {
			delay: autoplay,
			disableOnInteraction: false,
		},
		speed: 300,
		loopAdditionalSlides: 30,
		pagination: {
			el: '.detail_slide .swiper-pagination',
			type: 'custom',
			renderCustom: function (swiper, current, total) {
				return "<span class='swiper-pagination-current mont'>" + ('' + current).slice(-2) + "</span>" + ' / ' + "<span class='swiper-pagination-total mont'>" + ('' + total).slice(-2) + "</span>";
			},
		},

	});



	$(".loader").hide();
});

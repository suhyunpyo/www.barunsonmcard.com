$(function(){
	
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
		scrollbarDraggable: true,
		watchSlidesVisibility: true,
		watchSlidesProgress: true,
		centeredSlides: false,
		grabCursor: true,
	});

	//product scroll event
	//var productBanner = $('.product_banner').offset().top + 220;
	$(window).scroll(function(){

		var windowScroll = $(window).scrollTop() + 400;
		var pointTop = $('.footer').offset().top - 380;
		
		if( $('.product_detail_wrap').length ){
			var productBanner = $('.product_banner').offset().top + 220;
			if( windowScroll > productBanner){
				$('.ord_btn').addClass('fixed');
			} else {
				$('.ord_btn').removeClass('fixed');
			}
		}
		
		if(  windowScroll >= pointTop ){
			$('.sub_con_wrap .product_preview').removeClass("fixed").css('bottom','15%');
			
		} else {
			$('.sub_con_wrap .product_preview').addClass("fixed").css('bottom','');
			$('.sub_contents').css('margin-left','');
		}

	});

	$('.preview_box .img_con').on('scroll touchmove mousewheel', function(event) {
		var prevScroll = $(this).scrollTop();
		var innerHeight = $(this).innerHeight();
		var scrollHeight = $(this).prop('scrollHeight');
        var delta = 0;

        if( prevScroll + innerHeight >= scrollHeight ){

			event.preventDefault();
			event.stopPropagation();
			return false;
		} else {
			$('.preview_box .img_con').unbind();
		}

	});


	
	$('.preview_info_box').on('mousedown touchstart', function(){
		$(this).addClass('hide');
	});

});

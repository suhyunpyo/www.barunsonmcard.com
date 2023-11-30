$(function(){
	AOS.init();
	//접속 디바이스 구분
	var filter = "win16|win32|win64|mac";
	if( navigator.platform  ){
		if( filter.indexOf(navigator.platform.toLowerCase()) < 0 ){ //mobile 접속
			
			$('.follow_btn').hide();

		}else{ //PC 접속
		}
	}
	
	//mobile header scroll
	var moSize = $(window).outerWidth();
	var lastScrollTop = 0;
	if (moSize <= 800) {
		
		$(window).on('scroll', function() {
			var scrollTop = $(this).scrollTop();
			var $header = $('.header');
			var hedaerTop = $header.height();

			if(scrollTop < lastScrollTop || scrollTop < hedaerTop){
				//스크롤 올릴때
				$('.header .m_nav').fadeIn();
				$('.header').css('padding-bottom', '14px');
				
			}
			else {
				$('.header').css('padding-bottom', '6px');
				$('.header .m_nav').hide();
			}
			lastScrollTop = scrollTop;
		});
    }

	//마우스 커서 초기 값
	var mouseX = 0, mouseY = 0;

	//마우스 커서 위치 값 구하기
	$(document).on('mousemove', function(e){
		mouseX = e.clientX || e.pageX;
		mouseY = e.clientY || e.pageY;
		$(".circle").css({left: mouseX - 30 +'px', top: mouseY -30 +'px'});
	});

	//slide hover cursor event
	$('.follow_btn').hover(function(){
		$(".circle").addClass('over');
		if( $(this).hasClass("swiper-button-next") ){
			$('.circle').text('NEXT');
		} else {
			$('.circle').text('PREV');
		}
	}, function(){
		$(".circle").removeClass('over');
	});
	
	//main slide
	var autoplay1 = 3000;
	var autoplay2 = 2000;

	var mainSlide = new Swiper(".main_banner .swiper-container", {
		spaceBetween: 0,
		slidesPerView: 1,
		centeredSlides: true,
		roundLengths: true,
		loop: true,
		autoplay: {
			delay: autoplay1,
			disableOnInteraction: false,
		},
		speed: 300,
		loopAdditionalSlides: 30,
		navigation: {
			prevEl: ".swiper-button-prev.mb_btn",
			nextEl: ".swiper-button-next.mb_btn",
		},
		breakpoints: {
			3000: {
				pagination: {
					el: '.swiper-pagination.mb',
					type: 'custom',
					renderCustom: function (swiper, current, total) {
						return "<span class='swiper-pagination-current mont'>"+('' + current).slice(-2)+"</span>" + ' | ' + "<span class='swiper-pagination-total mont'>"+('' + total).slice(-2)+"</span>";
					},
				},
			},
			768: {
				pagination: {
					el: '.swiper-pagination.mb',
				},
			},
		},
		
	});

	//best list
	/*
	$('.best_list li').each(function () {
		var bestIdx = $('.best_list li').size();
		var arr = [bestIdx];
		$('.best_list li').slice(10,15).hide();
	});
	$('.best .btn_wrap a').on('click', function(){
		
		if( !$(this).hasClass('active') ){
			$(this).addClass('active');
			$('.best_list li').show();
		} else {
			$(this).attr('href','http://publish.bhandscard.com/MobileInvitation/product/product_list.html');
		}
	});
	*/

	//intro slide
	var bullet = ['미리보기', '다양한 스킨', '원하는 기능만', '빠른 대응'];
	new Swiper(".intro_slide", {
		cssWidthAndHeight: true,
		slidesPerView: 'auto',
		visibilityFullFit: true,
		autoResize: false,
		observer: true,
		observeParents: true,
		spaceBetween: 0,
		slidesPerView: 1,
		loop: true,
		autoplay: {
			delay: autoplay2,
			disableOnInteraction: false,
		},
		speed: 1000,
		navigation: {
			prevEl: ".swiper-button-prev.it_btn",
			nextEl: ".swiper-button-next.it_btn",
		},
		pagination: {
			el: '.swiper-pagination.it',
			clickable: true,
			renderBullet: function (index, className) {
				return '<div class="' + className + '"><span>' + (bullet[index]) + '</span></div>';
			}
		},
	});

	//찜하기 버튼 클릭 이벤트
	$('.ico.wish').on('click', function(){
	//	$(this).toggleClass('active');
	});


	//set slide
	$('.set_obj_con').eq(0).show();

	var setSlide = new Swiper(".set_slide", {
		
		//effect: 'fade',
		//fadeEffect: { crossFade: true },
		loop: true,
		autoplay: {
			delay: autoplay1,
			disableOnInteraction: false,
		},
		slidesPerView: 1,
		navigation: {
			prevEl: ".swiper-button-prev.st_btn",
			nextEl: ".swiper-button-next.st_btn",
        },
		breakpoints: {
			800: {
				pagination: {
					el: '.swiper-pagination.st',
				}
			},
		},
		on: {
			slideChange: function () {
				$('.set_obj_con').eq(setSlide.realIndex).show().not(this).siblings('div').hide();
			}
		}
	});
	/*
	setSlide.on('transitionEnd', function () {
		$('.set_obj_con').eq(setSlide.realIndex).show().not(this).siblings('div').hide();
	});
	*/

	//new slide
	var newOptions = {};

	if( $(".new .swiper-slide").length == 1 ){
		newOptions = {
			slideToClickedSlide:false,
			speed: 900,
			loop: false,
			autoplay: false,
			breakpoints: {
				3000: {
					slidesPerView: 4.5,
					centeredSlides: true,
					spaceBetween: 0,
				},
				1200: {
					slidesPerView: 1.2,
					spaceBetween: 30,
					centeredSlides: false,
				},
				640: {
					slidesPerView: 1.2,
					spaceBetween: 30,
					centeredSlides: false,
				}
			},
			navigation: false
		}
		$('.new .arrow_area').hide();
	} else {
		newOptions = {
			slideToClickedSlide:true,
			speed: 900,
			loop: true,
			autoplay: {
				delay: autoplay2,
				disableOnInteraction: false,
			},
			loopedSlides:1000,
			breakpoints: {
				3000: {
					slidesPerView: 4.5,
					centeredSlides: true,
					spaceBetween: 0,
				},
				1200: {
					slidesPerView: 1.2,
					spaceBetween: 30,
					centeredSlides: false,
				},
				640: {
					slidesPerView: 1.2,
					spaceBetween: 30,
					centeredSlides: false,
				}
			},
			navigation: {
				prevEl: ".swiper-button-prev.new_btn",
				nextEl: ".swiper-button-next.new_btn",
			},
		}
	}
	
	var newSlide = new Swiper(".new .swiper-container", newOptions);

	// $("section .swiper-container").hover(function() {
	// 	(this).swiper.autoplay.stop();
	// }, function() {
	// 	(this).swiper.autoplay.start();
	// });

	//쿠키값이 Y가 아니라면 메인팝업 오픈
	// if(getCookie("notToday")!="Y"){
	// 	$("#mainPopup").show();
	// }
	//main popup
	var mainPop = new Swiper("#mainPopup .swiper-container", {
		slideToClickedSlide:true,
		speed: 900,
		loop: true,
		slidesPerView: 1,
		centeredSlides: true,
		spaceBetween: 0,
		navigation: {
			prevEl: ".swiper-button-prev.mp_btn",
			nextEl: ".swiper-button-next.mp_btn",
		},
	});
	
	var middleOptions = {};
	middleOptions = {
		loop: false,
		loopedSlides:1,
		slidesPerView: 1,
		spaceBetween: 0,
		pagination: {
			el: '.swiper-pagination.middle',
			clickable: true,
		},
		autoplay:false,
		observer: true,
		observeParents: true,
	}

	
	new Swiper('.middle_banner', middleOptions);
});

//메인팝업 쿠키값 셋팅
function closePopupNotToday(){	             
	setCookie('notToday','Y', 1);
	$("#mainPopup").hide();
}
function setCookie(name, value, expiredays) {

	var today = new Date();
	today.setDate(today.getDate() + expiredays);

	document.cookie = name + '=' + escape(value) + '; path=/; expires=' + today.toGMTString() + ';'
}

function getCookie(name) { 

	var cName = name + "="; 
	var x = 0; 

	while ( x <= document.cookie.length ) { 

		var y = (x+cName.length); 

		if ( document.cookie.substring( x, y ) == cName ) { 

			if ( (endOfCookie=document.cookie.indexOf( ";", y )) == -1 ) 
				endOfCookie = document.cookie.length;

			return unescape( document.cookie.substring( y, endOfCookie ) ); 
		} 

		x = document.cookie.indexOf( " ", x ) + 1; 

		if ( x == 0 ) {
			break;
		}
			
	} 

	return ""; 
}

$(function(){

	//paper slide
	var paperOptions = {};
	var skinLength = $(".skin_banner .swiper-slide").length;

	if (skinLength == 1) {
		paperOptions = {
			slideToClickedSlide: false,
			speed: 900,
			loop: false,
			autoplay: false,
			navigation: false
		},
		$('.skin_banner .arrow_area').hide();
	} else if (skinLength > 1 && skinLength < 4) {
		paperOptions = {
			slidesPerView: skinLength,
			loop: true,
			autoplay: {
				delay: 1500,
				disableOnInteraction: false,
			},
			navigation: {
				prevEl: ".swiper-button-prev.pp_btn",
				nextEl: ".swiper-button-next.pp_btn",
			},
			pagination: {
				el: '.paper_paging',
				clickable: true,
			},
			breakpoints: {
				2000: {
					spaceBetween: 50
				},
				800: {
					slidesPerView: 2.5,
					spaceBetween: 40,
					centeredSlides: true
				}

			}
		}

	} else {
		paperOptions = {
			loop: true,
			autoplay: {
				delay: 1500,
				disableOnInteraction: false,
			},
			navigation: {
				prevEl: ".swiper-button-prev.pp_btn",
				nextEl: ".swiper-button-next.pp_btn",
			},
			pagination: {
				el: '.paper_paging',
				clickable: true,
			},
			breakpoints: {
				3000: {
					slidesPerView: 4,
					spaceBetween: 20
				},
				800: {
					slidesPerView: 2.5,
					spaceBetween: 20,
					centeredSlides: true
				}

			}
		}
	}

	//minvitation slide
	var minvitationOptions = {};
		minvitationOptions = {
			loop: true,
			slidesPerView: 1.8,
			centeredSlides: true,
			spaceBetween: 25,
			autoplay: {
				delay: 3000
			},
			pagination: {
				el: ".swiper-pagination",
			},
		}

	//setcard slide
	var setCardOptions = {};
		setCardOptions = {
			loop: true,
			autoplay: {
				delay: 1500,
				disableOnInteraction: false,
			},
			navigation: {
				prevEl: ".swiper-button-prev",
				nextEl: ".swiper-button-next",
			},
			pagination: {
				el: '.swiper-pagination',
				clickable: true,
			},
			breakpoints: {
				2000: {
					slidesPerView : 3,
                },
				800: {
					slidesPerView: 1.5,
					spaceBetween: 0,
					centeredSlides: true
				}

			}
	}
	//답례품 소개페이지 슬라이드
	var giftshopOptions = {};
		giftshopOptions = {
			slidesPerView: '1',
			loop: true,
			autoplay: false,
			pagination: {
				el: '.swiper-pagination',
				clickable: true,
			},
		}
	

	var paperSlide = new Swiper(".skin_banner .swiper-container", paperOptions);
	var minvitationSlide = new Swiper('.minvitation .event_slide', minvitationOptions);
	var setCardSlide = new Swiper('.set_card .set_slide', setCardOptions);
	var giftshopSlide = new Swiper('.giftshop .event_slide', giftshopOptions);

	//FAQ
	$(document).on('click', '.event_faq li', function (e) {
        $(this).toggleClass('open').siblings('li').removeClass('open');
        $(this).find(".event_faq_contents").slideToggle().parents('li').siblings('li').find('.event_faq_contents').slideUp();
    });
});

var price = new Array();

price[0],
price[1],
price[2],
price[3] = undefined;

$(window).on("load", function (e) {
	var windowWidth = $(window).width();
;	if (windowWidth < 1000) {

		priceSlide();
	} else {
		if (price[0] != undefined) {
			price[0].destroy();
        }
    }
});
function priceSlide() {

	$('.item_wrap').each(function (i) {
		try {
			var priceOptions = {};
			priceOptions = {
				loop:false,
				allowTouchMove: true,
				pagination: {
					el: '.price'+i+' + .swiper-pagination',
					clickable: true,
				},
				breakpoints: {
					3000: {
						
                    },
					1000: {
						loop: true,
						slidesPerView: 2.2,
						spaceBetween: 30,
						centeredSlides: true,
					}
				},
				observer: true,
				observeParents: true,
			}

			price[i] = new Swiper('.event_price .price_slide.price' + i + '', priceOptions);
		}
		catch (e) { }
	});
	
}

function CountBanner(id, url, target) {
	// ��� Ŭ���� ������Ʈ
	$.ajax({
		type: "POST",
		url: "/User/Banner/Click_Update/" + id,
		dataType: "json",
		async: false,
		contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
		success: function (result) {	
			window.open(url, target);
		} 
	});
}
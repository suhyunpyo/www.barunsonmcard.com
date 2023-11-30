$(function(){

	//����� ������ �����̵�
	var mthanksOptions = {};
	var skinLength = $(".skin_banner .swiper-slide").length;

		mthanksOptions = {
			slidesPerView: 4,
			spaceBetween: 10,
			loop: true,
			autoplay: {
				delay: 1500,
				disableOnInteraction: false,
			},
			pagination: {
				el: '.swiper-pagination.sk',
				clickable: true,
			},
			navigation: {
				prevEl: ".swiper-button-prev.sk",
				nextEl: ".swiper-button-next.sk",
			},
			breakpoints: {
				800: {
					slidesPerView: 2.5,
					spaceBetween: 20,
					centeredSlides: true
				},
				500: {
					slidesPerView: 1.5,
					spaceBetween: 20,
					centeredSlides: true
				}

			}
		}

	//BEST ���� ������ �����̵�
	var ptOptions = {};
		ptOptions = {
			slidesPerView: 2,
			spaceBetween: 50,
			loop: true,
			autoplay: {
				delay: 1500,
				disableOnInteraction: false,
			},
			pagination: {
				el: '.swiper-pagination.pt',
				clickable: true,
			},
			navigation: {
				prevEl: ".swiper-button-prev.pt",
				nextEl: ".swiper-button-next.pt",
			},
			breakpoints: {
				450: {
					slidesPerView: 1,
					centeredSlides: true,
					navigation: {
						prevEl: false,
						nextEl: ".swiper-button-next.pt",
					},
				}

			}
		}

	new Swiper(".skin_banner .swiper-container", mthanksOptions);
	new Swiper('.paper_thanks .swiper-container', ptOptions);
});


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
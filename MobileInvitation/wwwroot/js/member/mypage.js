$(function(){
	

	//product scroll event
	$(window).scroll(function(){

		var windowScroll = $(window).scrollTop() + 400;
		var pointTop = $('.footer').offset().top - 380;
		
		if(  windowScroll >= pointTop ){
			$('.sub_con_wrap .mypage_list').removeClass("fixed").css('bottom','0');
			$('.product_img').removeClass('fixed').css('bottom','6%');
		} else {
			$('.sub_con_wrap .mypage_list').addClass("fixed").css('bottom','');
			$('.sub_contents').css('margin-left','');
			$('.product_img').addClass('fixed');
		}
	});

	/** 모바일 툴팁 이벤트 **/
	//$('.box_bottom.tip_box .bn_tip').css('display','none');
	$('.box_bottom.tip_box a').on('click', function (e) {

		$(this).addClass('open');
	});
	$('html').on('click', function (e) {

		if (!$(e.target).hasClass('open')) {
			$('.bn_tip').hide();
			$('.box_bottom.tip_box a').removeClass('open');
		}
	});

	//전체선택
	var $wishList = $('.wish_list');
	var $childCheck;

	$('.all_check').on('click', function(){
		
		var $allCheck = $(this).find('input[type="checkbox"]');
		$childCheck = $wishList.find('input[type="checkbox"]');

		if( $allCheck.prop('checked') == true){

			$childCheck.prop('checked',true).parents('li').addClass('select');
			
		} else {
			$childCheck.prop('checked',false).parents('li').removeClass('select');
		}

	});

	//부분 선택
	/*
	$('.wish_list li').on('click', function(){
		$childCheck = $(this).find('input[type="checkbox"]');

		if( $childCheck.prop('checked') == false ){
			$childCheck.prop('checked', true).parents('li').addClass('select');
		} else {
			$childCheck.prop('checked', false).parents('li').removeClass('select');
		}
	});
	*/

	//모바일 마이페이지 메뉴 
	$('.mobile .mypage_box li > a').on('click', function(){

		var $mypageMenu = $(this).siblings('.mypage_depth02');
		
		if( $mypageMenu.length > 0 ){
			$(this).toggleClass('on');
		}
	});

	//마이페이지 배너
	var mpBanner = new Swiper(".mypage_banner .swiper-container", {
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

	//모바일 마이페이지 탭메뉴
	$('.tab_list li').on('click', function(){
		var idx = $(this).index();

		$('.tab_list li').not(this).removeClass('active');
		$(this).addClass('active');
		
		$('.tab_con').hide();
		$('.tab_con').eq(idx).fadeIn(500);
	});

	//모바일 마이페이지 1:1 문의 내용 펼쳐보기
	$('.ask_open_btn').on('click', function(){
		var $this = $(this).parents('li');
		
		if( !$this.hasClass('on') ){
			$this.addClass('on');
			$(this).text('접기');
		} else {
			$this.removeClass('on');
			$(this).text('펼쳐보기');
		}
	});

	//회원 정보 수정
	$("a.EditProfile").on('click', function (e) {
		e.preventDefault();
		$.ajax({
			url: $(this).attr('href'),
			method: 'GET'
		}).done(function (data, textStatus, jqXHR) {
			if (data.status == true) {
				var url = data.message;
				window.name = "'Parent_window";
				window.open(url, 'UserProfile', 'width=1200, height=900, scrollbars=yes');

			} else {
				var errors = data.message;
				alert(errors);
			}
		});
	});

	$(".loader").hide();
});

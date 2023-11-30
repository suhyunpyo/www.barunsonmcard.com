$(function(){
	
	//고객센터 FAQ
	$('.faq_con_title a').on('click', function(){
		$(this).parents('tr').toggleClass('open');
		$(this).parents('tr').siblings('tr').removeClass('open');
	});

	//모바일 고객센터 FAQ
	$('.mobile .faq_con_title').on('click', function(){
		$(this).parents('li').toggleClass('open');
		$(this).parents('li').siblings('li').removeClass('open');
	});

	//모바일 마이페이지 메뉴 
	$('.mobile .mypage_box li > a').on('click', function(){

		var $mypageMenu = $(this).siblings('.mypage_depth02');
		
		if( $mypageMenu.length > 0 ){
			$(this).toggleClass('on');
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

});

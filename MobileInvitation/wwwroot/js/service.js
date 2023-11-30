// $(function() {
// 	var pnls = document.querySelectorAll('.panel').length,
// 		scdir, hold = false;

// 	function _scrollY(obj) {
// 		var slength, plength, pan, step = 100,
// 			vh = window.innerHeight / 100,
// 			vmin = Math.min(window.innerHeight, window.innerWidth) / 100;
// 		if ((this !== undefined && this.id === 'well') || (obj !== undefined && obj.id === 'well')) {
// 			pan = this || obj;
// 			plength = parseInt(pan.offsetHeight / vh);
// 		}
// 		if (pan === undefined) {
// 			return;
// 		}
// 		plength = plength || parseInt(pan.offsetHeight / vmin);
// 		slength = parseInt(pan.style.transform.replace('translateY(', ''));
// 		if (scdir === 'up' && Math.abs(slength) < (plength - plength / pnls)) {
// 			slength = slength - step;
// 		} else if (scdir === 'down' && slength < 0) {
// 			slength = slength + step;
// 		} else if (scdir === 'top') {
// 			slength = 0;
// 		}
// 		if (hold === false) {
// 			hold = true;
// 			pan.style.transform = 'translateY(' + slength + 'vh)';
// 			setTimeout(function() {
// 				hold = false;
// 			}, 1000);
// 		}
// 		console.log(scdir + ':' + slength + ':' + plength + ':' + (plength - plength / pnls));
// 	}
	
// 	function _swipe(obj) {
// 		var swdir,
// 			sX,
// 			sY,
// 			dX,
// 			dY,
// 			threshold = 100,
// 			slack = 50,
// 			alT = 200,
// 			elT,
// 			stT;
// 		obj.addEventListener('touchstart', function(e) {
// 			var tchs = e.changedTouches[0];
// 			swdir = 'none';
// 			sX = tchs.pageX;
// 			sY = tchs.pageY;
// 			stT = new Date().getTime();
// 		}, false);

// 		obj.addEventListener('touchmove', function(e) {
// 			e.preventDefault();
// 		}, false);

// 		obj.addEventListener('touchend', function(e) {
// 			var tchs = e.changedTouches[0];
// 			dX = tchs.pageX - sX;
// 			dY = tchs.pageY - sY;
// 			elT = new Date().getTime() - stT;
// 			if (elT <= alT) {
// 				if (Math.abs(dX) >= threshold && Math.abs(dY) <= slack) {
// 					swdir = (dX < 0) ? 'left' : 'right';
// 				} else if (Math.abs(dY) >= threshold && Math.abs(dX) <= slack) {
// 					swdir = (dY < 0) ? 'up' : 'down';
// 				}
// 				if (obj.id === 'well') {
// 					if (swdir === 'up') {
// 						scdir = swdir;
// 						_scrollY(obj);
// 					} else if (swdir === 'down' && obj.style.transform !== 'translateY(0)') {
// 						scdir = swdir;
// 						_scrollY(obj);

// 					}
// 					e.stopPropagation();
// 				}
// 			}
// 		}, false);
// 	}

// 	var well = document.getElementById('well');
// 	well.style.transform = 'translateY(0)';
// 	well.addEventListener('wheel', function(e) {
// 		if (e.deltaY < 0) {
// 			scdir = 'down';
// 		}
// 		if (e.deltaY > 0) {
// 			scdir = 'up';
// 		}
// 		e.stopPropagation();
// 	});
// 	well.addEventListener('wheel', _scrollY);
// 	_swipe(well);
// 	var tops = document.querySelectorAll('.top');
// 	for (var i = 0; i < tops.length; i++) {
// 		tops[i].addEventListener('click', function() {
// 			scdir = 'top';
// 			_scrollY(well);
// 		});
// 	}


// });

$(function(){
	
	$('#fullpage').fullpage({
		//sectionsColor: ['#B8AE9C', '#348899', '#F2AE72', '#5C832F', '#B8B89F'],
		sectionSelector: '.fp_section',
		slideSelector: '.horizontal-scrolling',
		navigation: false,
		slidesNavigation: true,
		controlArrows: false,
		anchors: ['firstSection', 'secondSection', 'thirdSection', 'fourthSection', 'fifthSection'],
	}); 

	var autoplay = 2000;
	var introSlide = new Swiper(".intro_slide .swiper-container", {
		loop: true,
		loopedSlides:10,
		autoplay : {
			delay:autoplay,
		},
		navigation: {
			nextEl: '.swiper-button-next.nb',
			prevEl: '.swiper-button-prev.nb'
		},
		pagination: {
			el :  '.swiper-pagination.nb',
			clickable: true,
		},
		breakpoints: {
			3000 : {
				slidesPerView: 2,
				slidesPerGroup:2,
				spaceBetween: 36,
			},
			800 : {
				slidesPerView: 1.2,
				spaceBetween: 20,
				slidesPerGroup: 1,
			}
		}
	});
	/*
	$(".intro_slide .swiper-container").hover(function () {
		(this).swiper.autoplay.stop();
	}, function() {
		(this).swiper.autoplay.start();
	});
	*/
});
window.addEventListener('load', function() { window.scrollTo(0,1); }, false);//주소창 숨기기	
//스크롤 방지 이벤트
function scrollDisable(){
    $('#fullpage').addClass('scroll_off').on('scroll touchmove mousewheel', function(e){
        e.preventDefault();
    });
}
//스크롤 방지해제 이벤트
function scrollAble(){
    $('#fullpage').removeClass('scroll_off').off('scroll touchmove mousewheel');
}
; (function($) {
  $.fn.extend({
    silder: function(opt) {
      opt = jQuery.extend({
        silderbox: ".silder-box .animate",
				silderitem: ".item"
      },opt);
	  var MyTime = setInterval(function(){
		      $(".next").trigger("click");
		 } , 6000);	
      var $animate = $(opt.silderbox),
					$item = $animate.children(opt.silderitem);
			var bind = false,processTime = 1000;
			var array = new Array();
			var n = $.browser.msie,
					G = parseInt($.browser.version, 10);
					
			$item.each(function(index){
				var $this = $(this),aheight = $this.height();
				array.push($this.position().left);
				index%2 ? $this.css({top:-aheight}) : $this.css({top:aheight});
				$this.animate({top:0},650,function(){
					process($this, processTime);
					$this.removeAttr("style");
					setTimeout(function(){if(index == $item.length-1){bind = !bind;}},processTime)
				})
			});
			
			/*事件*/
			$(opt.silderbox).find(opt.silderitem).live("click",function(){
				var $this = $(this),index = $this.index()+1;
				arrow(index - Math.ceil($item.length/2),$this);
				bind = false;
			});
			$(".next").click(function(){
				var obj = $(opt.silderbox).find(opt.silderitem).eq(parseInt($item.length/2)-1);
				arrow(-1,obj)
				bind = false;
			});
			$(".prev").click(function(){
				var obj = $(opt.silderbox).find(opt.silderitem).eq(parseInt($item.length/2)+1);
				arrow(1,obj);
				bind = false;
			});
			
			 
//			
			/*运动*/
			function arrow(num,obj){
				if(!bind){return;}
				var direct = String(num).length > 1 ? 1 : 0;	//方向1-next(加前) 0-prev（加后）
				var $clone = null;
				
				if(direct){
					$clone = $animate.children(".item").slice(num).clone();
					IEprocess($clone);
					$animate.prepend($clone);
				}else{
					$clone = $animate.children(".item").slice(0,num).clone();
					IEprocess($clone);
					$clone.appendTo($animate);
				}
					
				var $anItem = $animate.children(opt.silderitem),trajectory=null,space=Math.abs(num)*200+200;	//trajectory-轨迹
				if(direct){
					$anItem.css({left:-array[Math.abs(num)]});
					trajectory = 0;
				}else{
					trajectory = -array[Math.abs(num)];
				}

				$anItem.each(function(index){
					
					var $this = $(this);
					$this.find(".process").html("");
					process($this, 1000);
					if(index != $anItem.length-1){
						$this.animate({left:trajectory},space);
					}else{
						obj.addClass("active").siblings().removeClass("active");
						$this.animate({left:trajectory},space,function(){
							direct ? $anItem.slice(num).remove() : $anItem.slice(0,num).remove();
							$anItem.removeAttr("style");
							bind = true;
						});
					}
				});
			}
			//ie7 8
			function IEprocess(obj){
				if(n && G <= 8){
					obj.each(function(){
						$(this).find(".process").html("");
						process($(this), 0);	
					})
				}
			}
			//圆环
			function process(th, t) {
				th.find(".process").each(function() {
					var progressValue = $(this).attr("alt"),
					r1 = 93,
					b1 = 10;
					if ($(this).data("alt") == "b") {
						r1 = 177.5;
						b1 = 20
					}
					if (!isNaN(progressValue)) {
						$.svgCircle({
							id: $(this)[0],
							r: r1,
							b: b1,
							perent: progressValue,
							color: ["#d0d0d0", "#ff9d9a", "#e75751"],
							speed: t,
							delay: 100
						});
					}
				});
			}
    }
  })
})(jQuery);

$(function() {
  //大图
  $(".flexslider").flexslider({
    directionNav: false
  });
  setInterval('AutoScroll(".newsbox")', 6000);
  /*百分圆*/
  //  setTimeout(function(){process()},1200)
  /*合作机构*/
  cooperation();
  $(".silder-box").silder();
  //安全保障
  var verticalOpts = [{
    'width': 0
  },
  {
    'width': '177px'
  }];
  turn($('.mod-insurance'), 100, verticalOpts);
});
//安全保障
var turn = function(target, time, opts) {
  target.find('li').hover(function() {
    $(this).find('img.a').stop().animate(opts[0], time,
    function() {
      $(this).hide().next("img.b").css("display", "block");
      $(this).next().animate(opts[1], time);
    });
  },
  function() {
    $(this).find('img.b').animate(opts[0], time,
    function() {
      $(this).hide().prev("img.a").css("display", "block");
      $(this).prev().animate(opts[1], time);
    });
  });
}

/*合作机构*/
function cooperation() {
  var $cdiv = $(".mod-cooperation"),
  $ccur = 1,
  $ci = 7,
  $clen = $cdiv.find("li").length,
  $cpage = Math.ceil($clen / $ci),
  $cw = $cdiv.find(".slides").width(),
  $cbox = $cdiv.find("ul");
  $cdiv.find(".aleft").click(function() {
    if (!$cbox.is(':animated')) {
      if ($ccur == 1) {
        $cbox.animate({
          left: '-=' + $cw * ($cpage - 1)
        },
        500);
        $ccur = $cpage;
      } else {
        $cbox.animate({
          left: '+=' + $cw
        },
        500);
        $ccur--;
      }
    }
  });
  $cdiv.find(".aright").click(function() {
    if (!$cbox.is(':animated')) {
      if ($ccur == $cpage) {
        $cbox.animate({
          left: 0
        },
        500);
        $ccur = 1;
      } else {
        $cbox.animate({
          left: '-=' + $cw
        },
        500);
        $ccur++;
      }

    }
  });
}
//滚动新闻
function AutoScroll(obj) {
  $(obj).find("ul:first").animate({
    marginTop: "-30px"
  },
  500,
  function() {
    $(this).css({
      marginTop: "0px"
    }).find("li:first").appendTo(this);
  });
}
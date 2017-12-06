; (function($) {
  jQuery.svgCircle = function(opt) {
    opt = jQuery.extend({
      id: null,
      r: 100,
      b: 1,
      color: ["#ccc", "#000", "#f00"],
      perent: 0,
      speed: 1000,
      delay: 100
    },
    opt);
    if (opt.id != null) {
      var r = Raphael(opt.id, 2 * opt.r, 2 * opt.r),
      R = opt.r - (opt.b / 2);
      r.customAttributes.arc = function(value, R) {
        if (value == 100) {
          value = 99.999
        }
        var alpha = 3.6 * value,
        a = (90 - alpha) * Math.PI / 180,
        x = opt.r + R * Math.cos(a),
        y = opt.r - R * Math.sin(a),
        path;
        path = [["M", opt.r, opt.r - R], ["A", R, R, 0, +(alpha > 180), 1, x, y]];
        return {
          path: path
        }
      };
      r.path().attr({
        stroke: opt.color[0],
        "stroke-width": opt.b
      }).attr({
        arc: [100, R]
      });
	  if(opt.perent>0){
      var c = r.path().attr({
        stroke: opt.color[1],
        "stroke-width": opt.b
      }).attr({
        arc: [0.01, R]
      });
	  
      setTimeout(function() {
        c.animate({
          stroke: opt.color[2],
          arc: [opt.perent, R]
        },
        opt.speed)
      },
      opt.delay)
    }
	}
  }
})(jQuery);
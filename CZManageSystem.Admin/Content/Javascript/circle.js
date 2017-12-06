/* @update: 2015-1-9 16:55:55 */ 
!
function($) {
  $.fn.svgCircle = function(t) {
    return t = $.extend({
      parent: null,
      w: 75,//52
      R: 30,//55
      sW: 20,//6
      color: ["#000", "#000"],
      perent: [100, 100],
      speed: 0,
      delay: 1e3
    },
    t),
    this.each(function() {
      var e = t.parent;
      if (!e) return ! 1; {
        var r = t.w,
        a = Raphael(e, r, r),
        n = t.R,
        o = {
          stroke: "#d7d7d7"
        };
        document.location.hash
      }
      a.customAttributes.arc = function(e, a, n) {
        {
          var o, c = 360 / a * e,
          s = (90 - c) * Math.PI / 180,
          i = r / 2 + n * Math.cos(s),
          h = r / 2 - n * Math.sin(s);
          t.color
        }
        return o = a == e ? [["M", r / 2, r / 2 - n], ["A", n, n, 0, 1, 1, r / 2 - .01, r / 2 - n]] : [["M", r / 2, r / 2 - n], ["A", n, n, 0, +(c > 180), 1, i, h]],
        {
          path: o
        }
      };
      var c = (a.path().attr({
        stroke: "#d7d7d7",
        "stroke-width": t.sW
      }).attr({
        arc: [100, 100, n]
      }), 
	  a.path().attr({
        stroke: "#000",
        "stroke-width": t.sW
      }).attr(o).attr({
        arc: [.01, t.speed, n]
      }));
      t.perent[1] > 0 ? setTimeout(function() {
        c.animate({
          stroke: t.color[1],
          arc: [t.perent[1], 100, n]
        },
        900, ">")
      },
      t.delay) : c.hide()
    })
  }
} (jQuery);
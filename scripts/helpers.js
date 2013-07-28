define([], function () {

    var Helpers = {
      dist2D: function (pos1, pos2) {
        var x_diff = pos1.x - pos2.x;
        var y_diff = pos1.y - pos2.y;
        return Math.sqrt(x_diff * x_diff + y_diff * y_diff);
      }
    }

    return Helpers;
  });
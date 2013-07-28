define(['debug', 'constants'], function (Debug, Constants) {

    var Node = {
      makeNode: function(x, y, quantity) {
        return {
          pos: {x: x, y: y}
        , quantity: quantity
        , connections: []
        }
      }
    , getQuantityFor: function (node) {
        if (node.quantity > 0) {
          return node.quantity;
        }
        return 0;
      }
    , getRadiusFor: function (node) {
        return this.getQuantityFor(node);
      }
    , reduceQuantityBy: function (node, val) {
        if (node.quantity >= val) {
          node.quantity -= val;
        }
      }
    , increaseQuantityBy: function (node, val) {
        node.quantity += val;
      }
    , sharesConnection: function (node, conn) {
        return node.connections.indexOf(conn) !== -1;
      }
    , sharesConnectionWith: function (node, other) {
        var i;
        for (i = 0; i < other.connections.length; i++) {
          var conn = other.connections[i];
          if (this.sharesConnection(node, conn)) {
            return true;
          }
        }
        return false;
      }
    , draw: function (ctx, node, options) {
        options = options || {};
        var highlighted = options['highlighted'] || false;
        ctx.beginPath();
        ctx.fillStyle = "black";
        ctx.arc(node.pos.x, node.pos.y, this.getRadiusFor(node), 0, Math.PI*2, true);
        ctx.fill();

        ctx.beginPath();
        if (highlighted)
          ctx.fillStyle = "black";
        else
          ctx.fillStyle = "white";

        ctx.arc(node.pos.x, node.pos.y, Constants.MIN_QUANTITY, 0, Math.PI*2, true);
        ctx.fill();
        ctx.stroke();

        if (Debug.enabled) {
          ctx.beginPath();
          ctx.fillStyle = "black";
          ctx.fillText(""+node.quantity, node.pos.x + this.getRadiusFor(node), node.pos.y + this.getRadiusFor(node));
          ctx.fill();
        }
      }
    }

    return Node;
  });
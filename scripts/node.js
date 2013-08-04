define(['debug', 'constants'], function (Debug, Constants) {

    var makeDerivative = function (d_pos, d_velocity, d_quantity) {
      var deriv = {
        d_pos: d_pos
      , d_velocity: d_velocity
      };

      if (d_quantity) {
        deriv.d_quantity = d_quantity;
      }

      return deriv;
    }

    var Node = {
      makeNode: function(x, y, quantity) {
        // return {
        //   pos: {x: x, y: y}
        // , quantity: quantity
        // , connections: []
        // }
        return {
          previous_state: {
            pos: {x: x, y: y}
          , velocity: {x: x, y: y}
          , quantity: quantity
          }
        , current_state: {
            pos: {x: x, y: y}
          , velocity: {x: x, y: y}
          , quantity: quantity
          }
        , connections: []
        };
      }
    , getRealQuantityFor: function (node) {
        return node.current_state.quantity;
      }
    , getQuantityFor: function (node) {
        if (node.current_state.quantity > 0) {
          return node.current_state.quantity;
        }
        return 0;
      }
    , getRadiusFor: function (node) {
        return this.getQuantityFor(node);
      }
    , reduceQuantityBy: function (node, val) {
        if (node.current_state.quantity >= val) {
          node.current_state.quantity -= val;
        }
      }
    , increaseQuantityBy: function (node, val) {
        node.current_state.quantity += val;
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
    , getPosFor: function (node) {
        return node.current_state.pos;
      }
    , draw: function (ctx, node, options) {
        // TODO: Refactor to use state interpolation
        options = options || {};
        var highlighted = options['highlighted'] || false;

        var pos = this.getPosFor(node);

        ctx.beginPath();
        ctx.fillStyle = "black";
        ctx.arc(pos.x, pos.y, this.getRadiusFor(node), 0, Math.PI*2, true);
        ctx.fill();

        ctx.beginPath();
        if (highlighted)
          ctx.fillStyle = "black";
        else
          ctx.fillStyle = "white";

        ctx.arc(pos.x, pos.y, Constants.MIN_QUANTITY, 0, Math.PI*2, true);
        ctx.fill();
        ctx.stroke();

        if (Debug.enabled) {
          ctx.beginPath();
          ctx.fillStyle = "black";
          ctx.fillText(""+this.getRealQuantityFor(node), pos.x + this.getRadiusFor(node), pos.y + this.getRadiusFor(node));
          ctx.fill();
        }
      }
    }

    return Node;
  });
define(['constants', 'helpers', 'node'], function (Constants, Helpers, Node) {

    var Connection = {
      makeConnection: function (a, b) {
        var a_pos = Node.getPosFor(a);
        var b_pos = Node.getPosFor(b);
        var length = Helpers.dist2D(a_pos, b_pos) - Node.getRadiusFor(a) - Node.getRadiusFor(b);
        var quantity = length * Constants.LENGTH_FACTOR; // TODO: REFACTOR
        var conn = {
          a: a
        , b: b
        , quantity: quantity
        }

        a.connections.push(conn);
        b.connections.push(conn);

        return conn;
      }
    , simulate: function (conn) {
        var difference = Math.abs(Node.getQuantityFor(conn.a) - Node.getQuantityFor(conn.b));
        if (difference > Constants.TRANSFER_SPEED) {
          if (Node.getQuantityFor(conn.a) > Node.getQuantityFor(conn.b)) {
            Node.reduceQuantityBy(conn.a, Constants.TRANSFER_SPEED);
            Node.increaseQuantityBy(conn.b, Constants.TRANSFER_SPEED);
          } else if (Node.getQuantityFor(conn.a) < Node.getQuantityFor(conn.b)) {
            Node.increaseQuantityBy(conn.a, Constants.TRANSFER_SPEED);
            Node.reduceQuantityBy(conn.b, Constants.TRANSFER_SPEED);
          }
        } else {
          if (Node.getQuantityFor(conn.a) > Node.getQuantityFor(conn.b)) {
            Node.reduceQuantityBy(conn.a, difference/2);
            Node.increaseQuantityBy(conn.b, difference/2);
          } else {
            Node.increaseQuantityBy(conn.a, difference/2);
            Node.reduceQuantityBy(conn.b, difference/2);
          }
        }
      }
    , draw: function (ctx, connection) {
        var a_pos = Node.getPosFor(connection.a);
        var b_pos = Node.getPosFor(connection.b);
        ctx.moveTo(a_pos.x, a_pos.y);
        ctx.lineTo(b_pos.x, b_pos.y);
      }
    }

    return Connection;
  });
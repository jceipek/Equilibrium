define(['constants', 'helpers', 'node'], function (Constants, Helpers, Node) {

    var Connection = {
      makeConnection: function (a, b) {
        var length = Helpers.dist2D(a.pos, b.pos) - Node.getRadiusFor(a) - Node.getRadiusFor(b);
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
        ctx.moveTo(connection.a.pos.x, connection.a.pos.y);
        ctx.lineTo(connection.b.pos.x, connection.b.pos.y);
      }
    }

    return Connection;
  });
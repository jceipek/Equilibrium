define(['zepto', 'constants', 'debug', 'node', 'connection', 'helpers', 'handlers'],
  function ($, Constants, Debug, Node, Connection, Helpers, Handlers) {

    var G = {
        ctx: null
      , canvas: null
      , connections: []
      , nodes: []
      , context: {
          selected_node: null
        , hovered_node: null
        , curr_pos: {x: 0, y: 0} // Mouse location
        , end_point_pos: {x: 0, y: 0} // Where connection will end
        , quantity: 0
        , offset: {x: 0, y: 0}
        }
      , init: function () {
          var _g = this;
          _g.canvas = $('#game')[0];
          if (_g.canvas.getContext) {
            _g.ctx = _g.canvas.getContext('2d');
            _g.ctx.lineWidth = Constants.visual.LINE_WIDTH;
            _g.canvas.onmousemove = Handlers.move.bind(_g);
            _g.canvas.onmouseup = Handlers.clickUp.bind(_g);
            _g.canvas.onmouseleave = Handlers.clickUp.bind(_g);
            _g.canvas.onmousedown = Handlers.clickDown.bind(_g);
            document.body.onkeydown = Handlers.keyDown.bind(_g);
            _g.populateGame();
          } else {
            // CANVAS IS NOT SUPPORTED
          }
        }
      , selectNode: function (node) {
          var _g = this;
          _g.context.selected_node = node;
        }
      , deselectNode: function () {
          var _g = this;
          _g.context.selected_node = null;
        }
      , populateGame: function () {
          var _g = this;
          // var node_0 = Node.makeNode(100, 90, 25);
          // var node_1 = Node.makeNode(404, 370, 30);
          // var node_2 = Node.makeNode(429, 510, 30);
          // var node_4 = Node.makeNode(522, 493, 30);
          // _g.addNode(node_0);
          // _g.addNode(node_1);
          // _g.addNode(node_2);
          // _g.addNode(node_4);
          var node_0 = Node.makeNode(100, 90, 42);
          var node_1 = Node.makeNode(404, 370, 30);
          var node_2 = Node.makeNode(429, 510, 30);
          var node_3 = Node.makeNode(522, 493, 30);
          var node_4 = Node.makeNode(737, 219, 30);
          _g.addNode(node_0);
          _g.addNode(node_1);
          _g.addNode(node_2);
          _g.addNode(node_3);
          _g.addNode(node_4);
        }
      , addNode: function (node) {
          var _g = this;
          _g.nodes.push(node);
        }
      , removeNode: function (node) { // UNSAFE (quantity is lost)
          var _g = this;
          var index;
          var connectionsToRemove = [];
          for (index = 0; index < node.connections.length; index++) {
            var conn = node.connections[index];
            connectionsToRemove.push(conn);
          }
          for (index = 0; index < connectionsToRemove.length; index++) {
            var conn = connectionsToRemove[index];
            _g.removeConnection(conn);
          }

          index = _g.nodes.indexOf(node);
          if (index !== -1) {
            _g.nodes.splice(index, 1);
          }
        }
      , removeConnection: function (conn) { // UNSAFE (quantity is lost)
          Debug.log("REMOVE Connection");
          var _g = this;
          var index = _g.connections.indexOf(conn);
          if (index !== -1) {
            _g.connections.splice(index, 1);
          }
          index = conn.a.connections.indexOf(conn);
          if (index !== -1) {
            conn.a.connections.splice(index, 1);
          }
          index = conn.b.connections.indexOf(conn);
          if (index !== -1) {
            conn.b.connections.splice(index, 1);
          }
        }
      , addConnection: function (conn) {
          var _g = this;
          _g.connections.push(conn);
        }
      , computeTotalQuantity: function () {
          var _g = this;
          var total = 0;
          _g.connections.forEach(function (connection) {
            total += Connection.getQuantityFor(connection);
            Debug.log("\t" + Connection.getQuantityFor(connection));
          });
          _g.nodes.forEach(function (node) {
            total += Node.getRealQuantityFor(node);
            Debug.log("\t" + Node.getRealQuantityFor(node));
            Debug.log("\t" + Node.getRealQuantityFor(node));
          });
          return total;
        }
      , render: function () {
          var _g = this;
          _g.ctx.clearRect(0, 0, G.canvas.width, G.canvas.height);

          _g.ctx.beginPath();

          _g.connections.forEach(function (connection) {
            Connection.draw(_g.ctx, connection);
          });

          if (_g.context.selected_node !== null) {
            var selected_node_pos = Node.getPosFor(_g.context.selected_node);
            var end_point_pos = _g.context.end_point_pos;
            _g.ctx.moveTo(selected_node_pos.x, selected_node_pos.y);
            _g.ctx.lineTo(end_point_pos.x, end_point_pos.y);
          }

          _g.ctx.stroke();

          _g.nodes.forEach(function (node) {
            Node.draw(_g.ctx, node);
          });

          if (_g.context.hovered_node) {
            Node.draw(_g.ctx, _g.context.hovered_node, {highlighted: true});
          }
        }
      };

    window.G = G;
    return G;
  });
define(['zepto', 'constants', 'debug', 'node', 'connection', 'helpers', 'handlers'],
  function ($, Constants, Debug, Node, Connection, Helpers, Handlers) {

    var G = {
        ctx: null
      , time: {
          physics_time: 0.0
        , PHYSICS_DT: 10.0
        , current_time: (new Date()).getTime()
        , time_accumulator: 0.0
        }
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
          var canvas = $('#game')[0];
          if (canvas.getContext) {
            var _g = this;
            _g.ctx = canvas.getContext('2d');
            _g.context.offset.x = -canvas.offsetLeft;
            _g.context.offset.y = -canvas.offsetTop;
            canvas.onmousemove = Handlers.move.bind(_g);
            canvas.onmouseup = Handlers.clickUp.bind(_g);
            canvas.onmouseleave = Handlers.clickUp.bind(_g);
            canvas.onmousedown = Handlers.clickDown.bind(_g);
            document.body.onkeydown = Handlers.keyDown.bind(_g);
            _g.populateGame();
          } else {
            // CANVAS IS NOT SUPPORTED
          }
        }
      , populateGame: function () {
          var _g = this;
          var a = Node.makeNode(200,300,30);
          var b = Node.makeNode(100,90,21);
          var c = Node.makeNode(500,300,10);
          _g.addNode(a);
          _g.addNode(b);
          _g.addNode(c);
          _g.addConnection(Connection.makeConnection(a,b));
          //_g.addConnection(Connection.makeConnection(a,c));
          //_g.addConnection(Connection.makeConnection(b,c));
          //_g.context.selected_node = c;
        }
      , addNode: function (node) {
          var _g = this;
          _g.nodes.push(node);
        }
      , removeNode: function (node) {
          var _g = this;
          node.connections.forEach(function (conn) {
            _g.removeConnection(conn);
          });
          var index = _g.nodes.indexOf(node);
          if (index !== -1) {
            delete _g.nodes[index];
          }
        }
      , removeConnection: function (conn) {
          var _g = this;
          var index = _g.connections.indexOf(conn);
          if (index !== -1) {
            delete _g.connections[index];
          }
          index = conn.a.connections.indexOf(conn);
          if (index !== -1) {
            delete conn.a.connections[index];
          }
          index = conn.b.connections.indexOf(conn);
          if (index !== -1) {
            delete conn.b.connections[index];
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
            total += connection.quantity;
            Debug.log("\t"+connection.quantity);
          });
          _g.nodes.forEach(function (node) {
            total += node.quantity;
            Debug.log("\t"+node.quantity);
          });
          return total;
        }
      , gameLoop: function () {
          var _g = this;
          var new_time = (new Date).getTime();
          var time_since_last_frame = new_time - _g.time.current_time;
          // Avoid the spiral of death:
          time_since_last_frame = Math.min(time_since_last_frame, Constants.MAX_TIME_FOR_FRAME);
          _g.time.current_time = new_time;

          _g.time.time_accumulator += time_since_last_frame;

          while (_g.time.time_accumulator >= _g.time.PHYSICS_DT) {
            // previous_state = current_state
            _g.simulate();
            _g.time.physics_time += _g.time.PHYSICS_DT;
            _g.time.time_accumulator -= _g.time.PHYSICS_DT;
          }

          var state_blending_factor = _g.time.time_accumulator / _g.time.PHYSICS_DT;

          //State state = current_state*state_blending_factor + previous_state * ( 1.0 - state_blending_factor );

          _g.render();

          setTimeout(_g.gameLoop.bind(_g), Constants.IDEAL_TIME_FOR_FRAME);
        }
      , simulate: function () {
          var _g = this;
          _g.connections.forEach(function (connection) {
            Connection.simulate(connection);
          });
          _g.simulateNewConnection();
        }
      , simulateNewConnection: function () {
          var _g = this;
          if (_g.context.selected_node !== null) {
            var length = Helpers.dist2D(_g.context.selected_node.pos, _g.context.curr_pos);
            var quantity = length * Constants.LENGTH_FACTOR; // TODO: REFACTOR
            var max_quantity = _g.context.selected_node.quantity + _g.context.quantity - Constants.MIN_QUANTITY;
            var max_length = max_quantity / Constants.LENGTH_FACTOR;
            var des_length = Math.min(length, max_length);
            _g.context.selected_node.quantity += _g.context.quantity;
            _g.context.quantity = Math.min(max_quantity, quantity);
            _g.context.selected_node.quantity -= _g.context.quantity;

            if (des_length > 0) {
              var real_x_diff = _g.context.curr_pos.x - _g.context.selected_node.pos.x;
              var real_y_diff = _g.context.curr_pos.y - _g.context.selected_node.pos.y;
              var des_x_diff = real_x_diff * des_length/length;
              var des_y_diff = real_y_diff * des_length/length;
              _g.context.end_point_pos.x = _g.context.selected_node.pos.x + des_x_diff;
              _g.context.end_point_pos.y = _g.context.selected_node.pos.y + des_y_diff;
            }
          }
        }
      , render: function () {
          var _g = this;
          _g.ctx.clearRect(0, 0, 1000, 1000);

          _g.ctx.beginPath();

          _g.connections.forEach(function (connection) {
            Connection.draw(_g.ctx, connection);
          });

          if (_g.context.selected_node !== null) {
            _g.ctx.moveTo(_g.context.selected_node.pos.x, _g.context.selected_node.pos.y);
            _g.ctx.lineTo(_g.context.end_point_pos.x, _g.context.end_point_pos.y);
          }

          _g.ctx.stroke();

          _g.ctx.beginPath();
          _g.ctx.fillStyle = "black";
          _g.nodes.forEach(function (node) {
            Node.draw(_g.ctx, node);
          });
          _g.ctx.fill();

          if (_g.context.hovered_node) {
            _g.ctx.beginPath();
            _g.ctx.fillStyle = "green";
            Node.draw(_g.ctx, _g.context.hovered_node);
            _g.ctx.fill();
          }
        }
      };

    return G;
  });
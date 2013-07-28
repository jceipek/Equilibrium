var TRANSFER_SPEED = 1;
var LENGTH_FACTOR = 0.1;
var MIN_QUANTITY = 10;
var IDEAL_TIME_FOR_FRAME = 50;
var MAX_TIME_FOR_FRAME = 250.0;
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
  , sel: {
      selected_node: null
    , curr_pos: {x: 0, y: 0} // Mouse location
    , end_point_pos: {x: 0, y: 0} // Where connection will end
    , quantity: 0
    , offset: {x: 0, y: 0}
    }
  , init: function () {
      var canvas = document.getElementById('game');
      if (canvas.getContext) {
        var _g = this;
        _g.ctx = canvas.getContext('2d');
        _g.sel.offset.x = -canvas.offsetLeft;
        _g.sel.offset.y = -canvas.offsetTop;
        canvas.onmousemove = _g.moveHandler.bind(_g);
        canvas.onmouseup = _g.clickUpHandler.bind(_g);
        canvas.onmouseleave = _g.clickUpHandler.bind(_g);
        canvas.onmousedown = _g.clickDownHandler.bind(_g);
        _g.populateGame();
      } else {
        // CANVAS IS NOT SUPPORTED
      }
    }
  , populateGame: function () {
      var _g = this;
      var a = makeNode(200,300,30);
      var b = makeNode(100,90,21);
      var c = makeNode(500,300,10);
      _g.nodes.push(a);
      _g.nodes.push(b);
      _g.nodes.push(c);
      _g.connections.push(makeConnection(a,b));
      //_g.connections.push(makeConnection(a,c));
      //_g.connections.push(makeConnection(b,c));
      //_g.sel.selected_node = c;
    }
  , computeTotalQuantity: function () {
      var _g = this;
      var total = 0;
      _g.connections.forEach(function (connection) {
        total += connection.quantity;
        console.log(connection.quantity);
      });
      _g.nodes.forEach(function (node) {
        total += node.quantity;
        console.log(node.quantity);
      });
      return total;
    }
  , gameLoop: function () {
    var _g = this;
    var new_time = (new Date).getTime();
    var time_since_last_frame = new_time - _g.time.current_time;
    // Avoid the spiral of death:
    time_since_last_frame = Math.min(time_since_last_frame, MAX_TIME_FOR_FRAME);
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

    setTimeout(_g.gameLoop.bind(_g), IDEAL_TIME_FOR_FRAME);
  }
  , simulate: function () {
      var _g = this;
      _g.connections.forEach(function (connection) {
        connection.simulate();
      });
      _g.simulateNewConnection();
    }
  , simulateNewConnection: function () {
      var _g = this;
      if (_g.sel.selected_node !== null) {
        var length = dist2D(_g.sel.selected_node.pos, _g.sel.curr_pos);
        var quantity = length * LENGTH_FACTOR; // TODO: REFACTOR
        var max_quantity = _g.sel.selected_node.quantity + _g.sel.quantity - MIN_QUANTITY;
        var max_length = max_quantity / LENGTH_FACTOR;
        var des_length = Math.min(length, max_length);
        _g.sel.selected_node.quantity += _g.sel.quantity;
        _g.sel.quantity = Math.min(max_quantity, quantity);
        _g.sel.selected_node.quantity -= _g.sel.quantity;

        if (des_length > 0) {
          var real_x_diff = _g.sel.curr_pos.x - _g.sel.selected_node.pos.x;
          var real_y_diff = _g.sel.curr_pos.y - _g.sel.selected_node.pos.y;
          var des_x_diff = real_x_diff * des_length/length;
          var des_y_diff = real_y_diff * des_length/length;
          _g.sel.end_point_pos.x = _g.sel.selected_node.pos.x + des_x_diff;
          _g.sel.end_point_pos.y = _g.sel.selected_node.pos.y + des_y_diff;
        }
      }
    }
  , render: function () {
      var _g = this;
      _g.ctx.clearRect(0, 0, 1000, 1000);

      _g.ctx.beginPath();

      _g.connections.forEach(function (connection) {
        _g.drawConnection(_g.ctx, connection);
      });

      if (_g.sel.selected_node !== null) {
        _g.ctx.moveTo(_g.sel.selected_node.pos.x, _g.sel.selected_node.pos.y);
        _g.ctx.lineTo(_g.sel.end_point_pos.x, _g.sel.end_point_pos.y);
      }

      _g.ctx.stroke();

      _g.nodes.forEach(function (node) {
        _g.ctx.beginPath();
        _g.ctx.fillStyle = "black";
        _g.drawNode(_g.ctx, node);
        _g.ctx.fill();

        _g.ctx.beginPath();
        // Is hovering over node?
        if (dist2D(node.pos, _g.sel.curr_pos) < node.getQuantity()) {
          _g.ctx.fillStyle = "green";
          _g.drawNode(_g.ctx, node);
          _g.ctx.fill();
        }
      });

    }
  , drawNode: function (ctx, node) {
      ctx.arc(node.pos.x, node.pos.y, node.getQuantity(), 0, Math.PI*2, true);
      ctx.fillText(""+node.quantity, node.pos.x + node.getRadius(), node.pos.y + node.getRadius());
    }
  , drawConnection: function (ctx, connection) {
      ctx.moveTo(connection.a.pos.x,connection.a.pos.y);
      ctx.lineTo(connection.b.pos.x,connection.b.pos.y);
    }
  , moveHandler: function (e) {
      var _g = this;
      _g.sel.curr_pos.x = e.x + _g.sel.offset.x;
      _g.sel.curr_pos.y = e.y + _g.sel.offset.y;
    }
  , clickDownHandler: function (e) {
      var _g = this;
      if (_g.sel.selected_node === null) {
        // Is clicking on node?
        _g.nodes.some(function (node) {
          if (dist2D(node.pos, _g.sel.curr_pos) < node.getQuantity()) {
            _g.sel.selected_node = node;
            _g.sel.end_point_pos.x = node.pos.x;
            _g.sel.end_point_pos.y = node.pos.y;
            console.log("SELECT");
            return true;
          }
        });
      }
      console.log(_g.computeTotalQuantity());
    }
  , clickUpHandler: function (e) {
      var _g = this;
      if (_g.sel.selected_node !== null) {
        var is_proper_release = false;
        _g.nodes.some(function (node) {
          // Is releasing on node?
          if (_g.sel.selected_node !== node
            && dist2D(node.pos, _g.sel.end_point_pos) < node.getQuantity()) {
            console.log("RELEASE PROPER");
            var conn = makeConnection(_g.sel.selected_node, node);
            node.increaseQuantity(_g.sel.quantity - conn.quantity);
            _g.connections.push(conn);
            is_proper_release = true;
            return true;
          }
        });
        if (!is_proper_release) {
          console.log("RELEASE");
          _g.sel.selected_node.quantity += _g.sel.quantity;
        }
        _g.sel.quantity = 0;
        _g.sel.selected_node = null;
      }
      console.log(_g.computeTotalQuantity());
    }
  };

function makeNode(x, y, quantity) {
  return {
    pos: {x: x, y: y}
  , quantity: quantity
  , connections: []
  , getQuantity: function () {
      if (this.quantity > 0) {
        return this.quantity;
      }
      return 0;
    }
  , getRadius: function () {
      return this.getQuantity();
    }
  , reduceQuantity: function (val) {
      if (this.quantity >= val) {
        this.quantity -= val;
      }
    }
  , increaseQuantity: function (val) {
      this.quantity += val;
    }
  }
}

function makeConnection(a, b) {
  var length = dist2D(a.pos, b.pos) - a.getRadius() - b.getRadius();
  var quantity = length * LENGTH_FACTOR; // TODO: REFACTOR
  return {
    a: a
  , b: b
  , quantity: quantity
  , simulate: function () {
      if (Math.abs(this.a.getQuantity() - this.b.getQuantity()) > TRANSFER_SPEED) {
        if (this.a.getQuantity() > this.b.getQuantity()) {
          this.a.reduceQuantity(TRANSFER_SPEED);
          this.b.increaseQuantity(TRANSFER_SPEED);
        } else if (this.a.getQuantity() < this.b.getQuantity()) {
          this.a.increaseQuantity(TRANSFER_SPEED);
          this.b.reduceQuantity(TRANSFER_SPEED);
        }
      }
    }
  }
}

function dist2D(a, b) {
  var x_diff = a.x - b.x;
  var y_diff = a.y - b.y;
  return Math.sqrt(x_diff * x_diff + y_diff * y_diff);
}

G.init();
G.gameLoop();
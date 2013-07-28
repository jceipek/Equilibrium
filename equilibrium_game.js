var TRANSFER_SPEED = 1;
var LENGTH_FACTOR = 0.1;
var MIN_QUANTITY = 10;
var G = {
    ctx: null
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
        this.ctx = canvas.getContext('2d');
        this.sel.offset.x = -canvas.offsetLeft;
        this.sel.offset.y = -canvas.offsetTop;
        canvas.onmousemove = this.moveHandler.bind(this);
        canvas.onmouseup = this.clickUpHandler.bind(this);
        canvas.onmousedown = this.clickDownHandler.bind(this);
        this.populateGame();
      } else {
        // CANVAS IS NOT SUPPORTED
      }
    }
  , populateGame: function () {
      var a = makeNode(200,300,30);
      var b = makeNode(100,90,21);
      var c = makeNode(500,300,10);
      this.nodes.push(a);
      this.nodes.push(b);
      this.nodes.push(c);
      this.connections.push(makeConnection(a,b));
      //this.connections.push(makeConnection(a,c));
      //this.connections.push(makeConnection(b,c));
      //this.sel.selected_node = c;
    }
  , computeTotalQuantity: function () {
      var total = 0;
      this.connections.forEach(function (connection) {
        total += connection.quantity;
      });
      this.nodes.forEach(function (node) {
        total += node.quantity;
      });
      return total;
    }
  , simulate: function () {
      this.connections.forEach(function (connection) {
        connection.simulate();
      });
      this.simulateNewConnection();
    }
  , simulateNewConnection: function () {
      var that = this;
      if (that.sel.selected_node !== null) {
        var length = dist2D(that.sel.selected_node.pos, that.sel.curr_pos);
        var quantity = length * LENGTH_FACTOR; // TODO: REFACTOR
        var max_quantity = that.sel.selected_node.quantity + that.sel.quantity - MIN_QUANTITY;
        var max_length = max_quantity / LENGTH_FACTOR;
        var des_length = Math.min(length, max_length);
        that.sel.selected_node.quantity += that.sel.quantity;
        that.sel.quantity = Math.min(max_quantity, quantity);
        that.sel.selected_node.quantity -= that.sel.quantity;

        if (des_length > 0) {
          var real_x_diff = that.sel.curr_pos.x - that.sel.selected_node.pos.x;
          var real_y_diff = that.sel.curr_pos.y - that.sel.selected_node.pos.y;
          var des_x_diff = real_x_diff * des_length/length;
          var des_y_diff = real_y_diff * des_length/length;
          that.sel.end_point_pos.x = that.sel.selected_node.pos.x + des_x_diff;
          that.sel.end_point_pos.y = that.sel.selected_node.pos.y + des_y_diff;
        }
      }
    }
  , visualize: function () {
      var that = this;
      that.ctx.clearRect(0, 0, 1000, 1000);

      that.ctx.beginPath();

      that.connections.forEach(function (connection) {
        that.drawConnection(that.ctx, connection);
      });

      if (that.sel.selected_node !== null) {
        that.ctx.moveTo(that.sel.selected_node.pos.x, that.sel.selected_node.pos.y);
        that.ctx.lineTo(that.sel.end_point_pos.x, that.sel.end_point_pos.y);
      }

      that.ctx.stroke();

      that.nodes.forEach(function (node) {
        that.ctx.beginPath();
        that.ctx.fillStyle = "black";
        that.drawNode(that.ctx, node);
        that.ctx.fill();

        that.ctx.beginPath();
        // Is hovering over node?
        if (dist2D(node.pos, that.sel.curr_pos) < node.getQuantity()) {
          that.ctx.fillStyle = "green";
          that.drawNode(that.ctx, node);
          that.ctx.fill();
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
      this.sel.curr_pos.x = e.x + this.sel.offset.x;
      this.sel.curr_pos.y = e.y + this.sel.offset.y;
    }
  , clickDownHandler: function (e) {
      var that = this;
      if (that.sel.selected_node === null) {
        // Is clicking on node?
        that.nodes.some(function (node) {
          if (dist2D(node.pos, that.sel.curr_pos) < node.getQuantity()) {
            that.sel.selected_node = node;
            that.sel.end_point_pos.x = node.pos.x;
            that.sel.end_point_pos.y = node.pos.y;
            console.log("SELECT");
            return true;
          }
        });
      }
      console.log(that.computeTotalQuantity());
    }
  , clickUpHandler: function (e) {
      var that = this;
      if (that.sel.selected_node !== null) {
        var is_proper_release = false;
        that.nodes.some(function (node) {
          // Is releasing on node?
          if (dist2D(node.pos, that.sel.end_point_pos) < node.getQuantity()) {
            // There's a problem here:
            // Theoretically, a connection should not be possible if
            // the connection can't reach to the middle (based on current ruleset).
            // Right now, quantity is being destroyed, I think.
            // However, this is bad from an interaction perspective.
            // Proposed fix: Change ruleset to take radius into account at all times.
            // Treat connections as points on the circumference, not the middle, while
            // transferring quantity.
            console.log("RELEASE PROPER");
            var conn = makeConnection(that.sel.selected_node, node);
            // TODO: fix leak?
            //console.log(that.sel.quantity - conn.quantity);
            node.increaseQuantity(that.sel.quantity - conn.quantity);
            that.connections.push(conn);
            is_proper_release = true;
            return true;
          }
        });
        if (!is_proper_release) {
          console.log("RELEASE");
          that.sel.selected_node.quantity += that.sel.quantity;
        }
        that.sel.quantity = 0;
        that.sel.selected_node = null;
      }
      console.log(that.computeTotalQuantity());
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
console.log(G.computeTotalQuantity());
setInterval(function () {
  G.visualize();
  G.simulate();
}, 50);
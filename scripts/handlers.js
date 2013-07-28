define(['helpers', 'constants', 'debug', 'node', 'connection'], function (Helpers, Constants, Debug, Node, Connection) {

    var Handlers = {
      move: function (e) {
        var _g = this;
        _g.context.offset.x = -_g.canvas.offsetLeft;
        _g.context.offset.y = -_g.canvas.offsetTop;
        _g.context.curr_pos.x = e.x + _g.context.offset.x;
        _g.context.curr_pos.y = e.y + _g.context.offset.y;

        _g.context.hovered_node = null;
        _g.nodes.some(function (node) {
          if (Helpers.dist2D(node.pos, _g.context.curr_pos) < Node.getQuantityFor(node)) {
            _g.context.hovered_node = node;
            return true;
          }
        });
      }
    , clickDown: function (e) {
        var _g = this;
        if (_g.context.selected_node === null) {
          // Is clicking on node?
          _g.nodes.some(function (node) {
            if (Helpers.dist2D(node.pos, _g.context.curr_pos) < Node.getQuantityFor(node)) {
              _g.context.selected_node = node;
              _g.context.end_point_pos.x = node.pos.x;
              _g.context.end_point_pos.y = node.pos.y;
              Debug.log("SELECT");
              return true;
            }
          });
        }
        Debug.log(_g.computeTotalQuantity());
      }
    , clickUp: function (e) {
        var _g = this;
        if (_g.context.selected_node !== null) {
          var is_proper_release = false;
          _g.nodes.some(function (node) {
            // Is releasing on node?
            if (_g.context.selected_node !== node
              && Helpers.dist2D(node.pos, _g.context.end_point_pos) < Node.getQuantityFor(node)
              && !Node.sharesConnectionWith(_g.context.selected_node, node)) {
              Debug.log("RELEASE PROPER");
              var conn = Connection.makeConnection(_g.context.selected_node, node);
              Node.increaseQuantityBy(node, _g.context.quantity - conn.quantity);
              _g.connections.push(conn);
              is_proper_release = true;
              return true;
            }
          });
          if (!is_proper_release) {
            Debug.log("RELEASE");
            _g.context.selected_node.quantity += _g.context.quantity;
          }
          _g.context.quantity = 0;
          _g.context.selected_node = null;
        }
        Debug.log(_g.computeTotalQuantity());
      }
    , keyDown: function (e) {
        var _g = this;
        var keys = {
          68: DBG_toggle // d key
        , 67: DBG_createNode.bind(_g) // c key
        , 88: DBG_deleteNode.bind(_g) // x key
        , 219: DBG_reduceHoveredNodeQuantity.bind(_g) // [ key
        , 221: DBG_increaseHoveredNodeQuantity.bind(_g) // ] key
        , 83: DBG_saveLevel.bind(_g) // s key
        }

        if (keys[e.which]) keys[e.which]();

        Debug.logObject(e);
      }
    };

    function DBG_toggle () {
      Debug.toggle();
    }

    function DBG_reduceHoveredNodeQuantity () {
      var _g = this;
      Debug.log('Dec');
      if (Debug.enabled && _g.context.hovered_node) {
        Node.reduceQuantityBy(_g.context.hovered_node, Constants.TRANSFER_SPEED);
      }
    }

    function DBG_increaseHoveredNodeQuantity () {
      var _g = this;
      Debug.log('Inc');
      if (Debug.enabled && _g.context.hovered_node) {
        Node.increaseQuantityBy(_g.context.hovered_node, Constants.TRANSFER_SPEED);
      }
    }

    function DBG_createNode () {
      var _g = this;
      if (Debug.enabled) {
        var node = Node.makeNode(_g.context.curr_pos.x, _g.context.curr_pos.y, 30);
        _g.addNode(node);
        _g.context.hovered_node = node;
      }
    }

    function DBG_deleteNode () {
      var _g = this;
      if (Debug.enabled && _g.context.hovered_node) {
        _g.removeNode(_g.context.hovered_node);
        _g.context.hovered_node = null;
      }
    }

    function DBG_saveLevel () {
      var _g = this;
      if (Debug.enabled) {
        var nodeCreation = "";
        var nodeAddition = "";
        var connectionCreationAndAddition = "";
        var i;
        for (i = 0; i < _g.nodes.length; i++) {
          var node = _g.nodes[i];
          node.__DBG_id = i;
          nodeCreation += "var node_" + i + " = " + "Node.makeNode("
           + node.pos.x + ", " + node.pos.y + ", " + node.quantity + ");\n";
          nodeAddition += "_g.addNode(node_" + i + ");\n";
        }
        for (i = 0; i < _g.connections.length; i++) {
          var connection = _g.connections[i];
          connectionCreationAndAddition += "_g.addConnection(Connection.makeConnection(node_"
           + connection.a.__DBG_id + ", node_" + connection.b.__DBG_id + ");\n";
        }
        Debug.displaySaveData(nodeCreation + nodeAddition + connectionCreationAndAddition);
      }
    }

    return Handlers;
  });
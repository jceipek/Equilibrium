define(['helpers', 'debug', 'node', 'connection'], function (Helpers, Debug, Node, Connection) {

    var Handlers = {
      move: function (e) {
        var _g = this;
        _g.sel.curr_pos.x = e.x + _g.sel.offset.x;
        _g.sel.curr_pos.y = e.y + _g.sel.offset.y;
      }
    , clickDown: function (e) {
        var _g = this;
        if (_g.sel.selected_node === null) {
          // Is clicking on node?
          _g.nodes.some(function (node) {
            if (Helpers.dist2D(node.pos, _g.sel.curr_pos) < Node.getQuantityFor(node)) {
              _g.sel.selected_node = node;
              _g.sel.end_point_pos.x = node.pos.x;
              _g.sel.end_point_pos.y = node.pos.y;
              Debug.log("SELECT");
              return true;
            }
          });
        }
        Debug.log(_g.computeTotalQuantity());
      }
    , clickUp: function (e) {
        var _g = this;
        if (_g.sel.selected_node !== null) {
          var is_proper_release = false;
          _g.nodes.some(function (node) {
            // Is releasing on node?
            if (_g.sel.selected_node !== node
              && Helpers.dist2D(node.pos, _g.sel.end_point_pos) < Node.getQuantityFor(node)
              && !Node.sharesConnectionWith(_g.sel.selected_node, node)) {
              Debug.log("RELEASE PROPER");
              var conn = Connection.makeConnection(_g.sel.selected_node, node);
              Node.increaseQuantityBy(node, _g.sel.quantity - conn.quantity);
              _g.connections.push(conn);
              is_proper_release = true;
              return true;
            }
          });
          if (!is_proper_release) {
            Debug.log("RELEASE");
            _g.sel.selected_node.quantity += _g.sel.quantity;
          }
          _g.sel.quantity = 0;
          _g.sel.selected_node = null;
        }
        Debug.log(_g.computeTotalQuantity());
      }
    , keyDown: function (e) {
        var _g = this;
        var keys = {
          68: function () { // d key
            Debug.toggle();
          }
        , 67: function () { // c key
            if (Debug.enabled) {
              var node = Node.makeNode(_g.sel.curr_pos.x, _g.sel.curr_pos.y, 30);
              _g.addNode(node);
            }
          }
        }

        if (keys[e.which]) keys[e.which]();

        Debug.logObject(e);
      }
    };

    return Handlers;
  });
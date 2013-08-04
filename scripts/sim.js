define(['constants', 'helpers', 'game', 'connection', 'node'], function (Constants, Helpers, G, Connection, Node) {

    var Sim = {
      time: {
          physics_time: 0.0
        , PHYSICS_DT: Constants.PHYSICS_DT
        , current_time: (new Date()).getTime()
        , time_accumulator: 0.0
        }
    , gameLoop: function () {
      var _s = this;
      var new_time = (new Date).getTime();
      var time_since_last_frame = new_time - _s.time.current_time;
      // Avoid the spiral of death:
      time_since_last_frame = Math.min(time_since_last_frame, Constants.MAX_TIME_FOR_FRAME);
      _s.time.current_time = new_time;

      _s.time.time_accumulator += time_since_last_frame;

      while (_s.time.time_accumulator >= _s.time.PHYSICS_DT) {
        // previous_state = current_state
        _s.simulate();
        _s.time.physics_time += _s.time.PHYSICS_DT;
        _s.time.time_accumulator -= _s.time.PHYSICS_DT;
      }

      var state_blending_factor = _s.time.time_accumulator / _s.time.PHYSICS_DT;

      //State state = current_state*state_blending_factor + previous_state * ( 1.0 - state_blending_factor );

      G.render();

      setTimeout(_s.gameLoop.bind(_s), Constants.IDEAL_TIME_FOR_FRAME);
    }
    , evaluate: function (current_state, t, dt, deriv) {
        var _s = this;
        var new_state = {pos: {}, velocity: {}};
        new_state.pos.x = current_state.pos.x + deriv.d_pos.x * dt;
        new_state.pos.y = current_state.pos.y + deriv.d_pos.y * dt;
        new_state.velocity.x = current_state.d_velocity.x + deriv.d_velocity.x * dt;
        new_state.velocity.y = current_state.d_velocity.y + deriv.d_velocity.y * dt;

        return output_deriv = {
          d_pos: {
            x: new_state.velocity.x
          , y: new_state.velocity.y
          }
        , d_velocity: _s.acceleration(new_state, t + dt)
        };
      }
    , acceleration: function (state, t) {
        // TODO: Implement
        return {
          x: 1
        , y: 1
        };
      }
    , integrate: function (state, t, dt) {
        // RK4 Integration
        var _s = this;
        var a = _s.evaluate(state, t, 0.0, {d_pos: {x: 0, y: 0}, d_velocity: {x: 0, y: 0}});
        var b = _s.evaluate(state, t, dt * 0.5, a);
        var c = _s.evaluate(state, t, dt * 0.5, b);
        var d = _s.evaluate(state, t, dt, c);

        // XXX: FIXME!!!!
        var step = {
          d_pos_dt: {
            x: 1.0/6.0 * (a.d_pos.x + 2.0 * (b.d_pos.x + c.d_pos.x) + d.d_pos.x)
          , y: 1.0/6.0 * (a.d_pos.y + 2.0 * (b.d_pos.y + c.d_pos.y) + d.d_pos.y)
          }
        , d_velocity_dt: {
            x: 1.0/6.0 * (a.d_velocity.x + 2.0 * (b.d_velocity.x + c.d_velocity.x) + d.d_velocity.x)
          , y: 1.0/6.0 * (a.d_velocity.y + 2.0 * (b.d_velocity.y + c.d_velocity.y) + d.d_velocity.y)
          }
        }

        var new_state = {
          pos: {
            x: state.pos.x + step.d_pos_dt.x * dt
          , y: state.pos.y + step.d_pos_dt.y * dt
          }
        , velocity: {
            x: state.velocity.x + step.d_velocity_dt.x * dt
          , y: state.velocity.y + step.d_velocity_dt.y * dt
          }
        }
        return new_state;
      }
    , simulate: function () {
        G.connections.forEach(function (connection) {
          Connection.simulate(connection);
        });
        this.simulateNewConnection();
      }
    , simulateNewConnection: function () {
        var selected_node = G.context.selected_node;
        if (selected_node !== null) {
          var selected_node_pos = Node.getPosFor(selected_node);
          var length = Helpers.dist2D(selected_node_pos, G.context.curr_pos);
          var quantity = length * Constants.LENGTH_FACTOR; // TODO: REFACTOR
          var max_quantity = Node.getRealQuantityFor(selected_node) + G.context.quantity - Constants.MIN_QUANTITY;
          var max_length = max_quantity / Constants.LENGTH_FACTOR;
          var des_length = Math.min(length, max_length);

          Node.increaseQuantityBy(selected_node, G.context.quantity);
          G.context.quantity = Math.min(max_quantity, quantity);
          Node.reduceQuantityBy(selected_node, G.context.quantity);

          if (des_length > 0) {
            var real_x_diff = G.context.curr_pos.x - selected_node_pos.x;
            var real_y_diff = G.context.curr_pos.y - selected_node_pos.y;
            var des_x_diff = real_x_diff * des_length/length;
            var des_y_diff = real_y_diff * des_length/length;
            G.context.end_point_pos.x = selected_node_pos.x + des_x_diff;
            G.context.end_point_pos.y = selected_node_pos.y + des_y_diff;
          }
        }
      }
    }

    return Sim;
  });
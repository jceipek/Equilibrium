define(['constants', 'game', 'connection'], function (Constants, G, Connection) {

    var Sim = {
      time: {
          physics_time: 0.0
        , PHYSICS_DT: 10.0
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
    , simulate: function () {
        G.connections.forEach(function (connection) {
          Connection.simulate(connection);
        });
        G.simulateNewConnection();
      }
    }

    return Sim;
  });
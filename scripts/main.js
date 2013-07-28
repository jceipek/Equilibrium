require.config({
  paths: {
    zepto: 'zepto.min',
  },
  shim: {
    zepto: {
      exports: '$'
    }
  }
});

require(['domReady!', 'game', 'sim'], function (_, G, Sim) {
    G.init();
    Sim.gameLoop();
  });
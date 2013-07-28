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

require(['objectwatch-polyfill', 'domReady!', 'game', 'sim'], function (_, _, G, Sim) {
    G.init();
    Sim.gameLoop();
  });
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

require(['objectwatch-polyfill', 'domReady!', 'game'], function (_, _, G) {
    G.init();
    G.gameLoop();
  });
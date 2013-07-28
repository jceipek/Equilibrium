define(['zepto', 'constants'], function ($, Constants) {

    $('.js-debug-min-node-value').val(Constants.MIN_QUANTITY);
    $('.js-debug-min-node-value').on('keypress blur focus', function (e) {
      Constants.MIN_QUANTITY = parseFloat(e.target.value);
    })

    var Debug = {
      enabled: true
    , toggle: function () {
        this.enabled = !this.enabled;

        if (this.enabled) {
          $('.js-debug-enabled').html('Enabled');
          $('.js-debug__panel').addClass('is-shown');
        } else {
          $('.js-debug-enabled').html('Disabled');
          $('.js-debug__panel').removeClass('is-shown');
        }
      }
    , log: function (text) {
        if (this.enabled) {
          console.log("DEBUG: " + text);
        }
      }
    , logObject: function (obj) {
        if (this.enabled) {
          console.log("DEBUG: ");
          console.log(obj);
        }
      }
    }

    return Debug;
  });
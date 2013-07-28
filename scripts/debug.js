define(['zepto'], function ($) {

    var Debug = {
      enabled: true
    , toggle: function () {
        this.enabled = !this.enabled;

        if (this.enabled) {
          $('.js-debug-enabled').html('Enabled');
          $('.js-debug__commands').addClass('is-shown');
        } else {
          $('.js-debug-enabled').html('Disabled');
          $('.js-debug__commands').removeClass('is-shown');
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
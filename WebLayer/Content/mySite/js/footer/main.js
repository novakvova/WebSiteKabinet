//CONSTANTS
const VALID_ERROR_CLASS = 'input-validation-error';
const VALID_ERROR_CLASS_SELECTOR = '.' + VALID_ERROR_CLASS;
const VALID_ERROR_SCROLL_OFFSET = 100;
const VALID_ERROR_SCROLL_SPEED = 600;

$body = $("body");

//$(document).ready(function () {

    

//});


//ajax loader global event
$(document).on({
    ajaxStart: function () { $body.addClass("loading"); },
    ajaxStop: function () {
        setTimeout(function () {
            $body.removeClass("loading");
        }, 250);

    }
});

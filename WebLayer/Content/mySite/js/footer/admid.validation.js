function validText(input, rules) {
	var oldPlaceholder = input.attr('placeholder');
	


	//check if value required
	if (rules.required) {
	    if (input.val().length > 0) {
	        input.removeClass(VALID_ERROR_CLASS);
		}
		else {
	        input.addClass(VALID_ERROR_CLASS);
	        input.data('old-val', input.val());
			input.attr('placeholder', rules.required);
			input.on('focusin', function () {
			    input.removeClass(VALID_ERROR_CLASS);
			    input.attr('placeholder', oldPlaceholder);
			});
			return false;
		}
	}

	//check max length
	if (rules.maxLength > 0) {
	    if (input.val().length > rules.maxLength) {
	        input.addClass(VALID_ERROR_CLASS);
		    input.data('old-val', input.val());
			input.val('');
			input.attr('placeholder', input.data('val-length'));
			input.on('focusin', function () {
			    input.removeClass(VALID_ERROR_CLASS);
			    input.attr('placeholder', oldPlaceholder);
			    input.val(input.data('old-val'));
			});
			return false;
		}
		else {
	        input.removeClass(VALID_ERROR_CLASS);
		}
	}


	return true;

}

function validSelect(input, rules) {
    if (rules.required) {
        if (input.val() > 0) {
            input.removeClass(VALID_ERROR_CLASS);
        }
        else {
            input.addClass(VALID_ERROR_CLASS);
            input.change(function () {
                if (input.val() > 0)
                    input.removeClass(VALID_ERROR_CLASS);
            });
            return false;
        }
    }

    return true;

}

function validCropper(input, box, rules) {

    if (rules.required) {
        if (input.val().length > 0) {
            box.removeClass(VALID_ERROR_CLASS);
        }
        else {
            box.addClass(VALID_ERROR_CLASS);
            input.keyup(function () {
                if (input.val().length > 0) {
                    box.removeClass(VALID_ERROR_CLASS);
                }
                    
            });
            return false;
        }
    }

    return true;

}

function validEditor(div) {

    div.on('DOMSubtreeModified', function () {
        valid(div);
    });

    return valid(div);

    function valid(div) {
        var boxShadow = '0 1px 3px rgba(0,0,0,.12), 0 1px 1px 1px rgba(0,0,0,.16)';
        if (div.parent().froalaEditor('html.get').length == 0) {
            div.children().first().addClass('input-validation-error');
            div.css('-webkit-box-shadow', 'none');
            div.css('-moz-box-shadow', 'none');
            div.css('box-shadow', 'none');
            return false;
        }
        else {
            div.children().first().removeClass('input-validation-error');
            div.css('-webkit-box-shadow', boxShadow);
            div.css('-moz-box-shadow', boxShadow);
            div.css('box-shadow', boxShadow);
            return true;
        }
    }

}
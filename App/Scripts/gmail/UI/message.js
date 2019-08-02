function showMessage(title, msg, type, callback, buttons, buttonInjectType){
    showModal(
        '<div class="message ' + type + '">' + 
            '<h4>' + title + '</h4>' +  msg +
            '<div class="buttons">' + 
            (buttons != null && (buttonInjectType == 'prepend') ? buttons : '') +
            (
                type == '' || type == 'message' || type == 'warning' || 
                type == 'error' ? 
                '<a class="button btn-okay">Okay</a>' :
                type == 'confirm' ?
                '<a class="button btn-okay">Okay</a>' +
                '<a class="button btn-cancel">Cancel</a>' :
                type == 'save' ?
                '<a class="button btn-okay">Save</a>' +
                '<a class="button btn-cancel">Cancel</a>' :
                type == 'question' ? 
                '<a class="button btn-yes">Yes</a>' +
                '<a class="button btn-no">No</a>' :
                type == 'terms' ?
                '<a class="button btn-accept">Accept</a>' +
                '<a class="button btn-decline">Decline</a>' : 
                type == 'pay' ? 
                    (
                        hasSubscription() == true && 
                        (
                            (outstanding_info != null && outstanding_info.status == 1) || 
                            outstanding_info == null
                        ) ? 
                        '<span class="foot-note">Select payment method</span>' +
                        '<a class="button btn-saved">Saved Credit Card</a>' +
                        '<a class="button btn-credit">New Credit Card</a>' :
                        '<a class="button btn-credit">Pay with Credit Card</a>'
                    ) : ''
            ) +
            (buttons != null && (buttonInjectType == 'append' || buttonInjectType == null) ? buttons : '') +
            '</div>' +
        '</div>'
    );

    var modal = $('.gmaster-content .message');
    var response = true;
    switch(type){
        case '': case 'warning': case 'message': case 'confirm': case 'error': case 'save':
            modal.find('.btn-okay').on('click', function () {
                hideModalMessage();
                if(typeof callback == 'function'){response = callback(true);}
                if(response !== false){hideModal();}
            });
            modal.find('.btn-cancel').on('click', hideModal);
            break;

        case 'question': // Question *Yes, No) /////////////////////////////////////
            modal.find('.btn-yes').on('click', function () {
                hideModalMessage();
                if(typeof callback == 'function'){response = callback(true);}
                if(response !== false){hideModal();}
            });
            modal.find('.btn-no').on('click', function () {
                hideModalMessage();
                if(typeof callback == 'function'){response = callback(false);}
                if(response !== false){hideModal();}
            });
            break;

        case 'terms': // Terms (Accept, Decline) /////////////////////////////////////
            modal.find('.btn-accept').on('click', function () {
                hideModalMessage();
                if(typeof callback == 'function'){response = callback(true);}
                if(response !== false){hideModal();}
            });
            modal.find('.btn-decline').on('click', function () {
                hideModalMessage();
                if(typeof callback == 'function'){response = callback(false);}
                if(response !== false){hideModal();}
            });
            break;
        
        case 'pay': // Pay (Credit Card, PayPal) /////////////////////////////////////
            modal.find('.btn-credit').on('click', function () {
                hideModalMessage();
                if(typeof callback == 'function'){response = callback('credit');}
                if(response !== false){hideModal();}
            });
            modal.find('.btn-saved').on('click', function () {
                hideModalMessage();
                if(typeof callback == 'function'){response = callback('saved');}
                if(response !== false){hideModal();}
            });
            modal.find('.btn-paypal').on('click', function () {
                hideModalMessage();
                if(typeof callback == 'function'){response = callback('paypal');}
                if(response !== false){hideModal();}
            });
            break;
    }
}
var stripe = null;

$(document).ready(() => {
    //generate Stripe elements
    stripe = Stripe(stripeKey);
    $('.submit-payment').hide();
    var isError = false;

    //set up Stripe elements
    var elements = stripe.elements();
    var style = {
        base: {
            color: '#32325d',
            fontSize: '16px',
            '::placeholder': {
                color: '#aab7c4'
            }
        },
        invalid: {
            color: '#fa755a',
            iconColor: '#fa755a'
        }
    };

    // create an instance of Stripe's card element
    var card = elements.create('card', { style: style });
    card.mount('#stripe-card-element');

    //cc error handler
    card.addEventListener('change', ({ error }) => {
        if (error) {
            error(error.message);
            $('.submit-payment').hide();
            isError = true;
        } else {
            message('Secure Pay with Stripe');
            $('.submit-payment').show();
            isError = false;
        }
    });

    //cc form submit handler
    const form = $('#payment-form');
    form.on('submit', async (event) => {
        if (isError == true) { return; }
        event.preventDefault();
        $('.submit-payment').hide();

        const { token, error } = await stripe.createToken(card);

        if (error) {
            //error occurred
            error(error.message);
            $('.submit-payment').show();
        } else {
            // Send the token to server
            paymentSuccess(token);
        }
    });
});

function paymentSuccess(token) {
    message('Processing Payment...');
    chrome.runtime.sendMessage(extensionId,
        { path: 'subscribe', method: 'stripe', zipcode: token.card.address_zip || '', token: token.id },
        function (response) {
            if (response == 'success') {
                window.close();
            } else {
                error('Payment Error. Please try again.');
                $('.submit-payment').show();
            }
        }
    );
}

function message(msg) {
    $('.pay-msg').removeClass('error').html(msg);
}

function error(msg) {
    $('.pay-msg').addClass('error').html(msg);
} 
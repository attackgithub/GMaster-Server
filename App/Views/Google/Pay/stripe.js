var stripe = null;

$(document).ready(() => {
    //generate Stripe elements
    stripe = Stripe(stripeKey);

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
        } else {
            message('Secure Pay with Stripe');
        }
    });

    //cc form submit handler
    const form = $('#payment-form');
    form.on('submit', async (event) => {
        event.preventDefault();

        const { token, error } = await stripe.createToken(card);

        if (error) {
            //error occurred
            error(error.message);
        } else {
            // Send the token to server
            paymentSuccess(token);
        }
    });
});

function pay() {

}

function paymentSuccess(token) {
    message('Processing Payment...');
    console.log(token);
    S.ajax.post('Subscriptions/Subscribe',
        { 
            devkey: devkey,
            email: email,
            planId: planId,
            users: users,
            zipcode: token.card.address_zip || '',
            stripeToken: token.id
        },
        function () {
            message('Payment success! please wait...');
            chrome.runtime.sendMessage(extensionId, { path: 'subscribe', method:'stripe' },
                function (response) {
                    if (response == 'success') {
                        window.close();
                    } else {
                        error('Payment Error. Please try again.');
                    }
                }
            );
        },
        function (err) {
            error('Payment error. Please try again.<div class="summary">' + err.responseText + '</div>');
        }
    );
}

function message(msg) {
    $('.pay-msg').removeClass('error').html(msg);
}

function error(msg) {
    $('.pay-msg').addClass('error').html(msg);
} 
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
});

function pay() {

}

function paymentSuccess() {
    $('.auth-msg').html('Authenticating...');
    S.ajax.post('Pay/Stripe', response,4621 9210 0236 0236
        function () {
            $('.auth-msg').html('Payment success! please wait...');
            chrome.runtime.sendMessage(extensionId, { path: 'subscribe', method:'stripe' },
                function (response) {
                    if (response == 'success') {
                        window.close();
                    } else {
                        error('Error authenticating. Please try again.');
                    }
                }
            );
        },
        function (err) {
            error('Payment error. Please try again.<div class="summary">' + err.responseText + '</div>');
        }
    );
}

function error(msg) {
    $('.auth-msg').html(msg + '<div class="retry"><a href="javascript:" onclick="pay()">Retry</a></div>');
} 
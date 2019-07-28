function handleSettingsPage(view, menu, sub){
    var name = 'settings';
    loadSubscriptionPage(sub.subscriptionId, name, view, settingsPageCallback);

    function settingsPageCallback(subscriptionId) {
        //bind all buttons on subscription page //////////////////////////////////
        var div = $('.subscription-page');
        $(div).find('section.modify button.btn-change').on('click', () => {
            //get list of plans to upgrade / downgrade to
            webApi('Subscriptions/GetModifyInfo', {}, 
                function(mod){
                    var info = mod[0].response;
                    modifySubscription(info);
                },
                function(err){
                    //don't execute callback
                    //<debug>
                    console.log(err);
                    //</debug>
                    showMessage('Error', err.message, 'error');
                }
            );
        });

        $(div).find('section.modify button.modify-team').on('click', () => {
            //skip list of plans and go straight to pay modal when modifying a team
            webApi('Subscriptions/GetModifyInfo', {}, 
                function(mod){
                    var info = mod[0].response;
                    modifySubscriptionPayment(info, info.subscription.planId);
                },
                function(err){
                    //don't execute callback
                    //<debug>
                    console.log(err);
                    //</debug>
                    showMessage('Error', err.message, 'error');
                }
            );
        });

        $(div).find('section.outstanding button.make-payment').on('click', () => {
            //show payment modal and force user to use a new credit card
            var html = '<div class="total-users-form">' +
                    '<div class="owed-today">' + 
                        '<span class="price">$' + outstanding_info.totalOwed + '</span> <span class="charged">will be charged today.</span>' + 
                        '<div class="cycle">Your billing cycle will not change.</div>' +
                    '</div>' +
                '</div>';
            showMessage('Make a payment', html, 'pay', function(){
                var port = chrome.runtime.connect({name: "payment:" + outstanding_info.totalOwed + ':' + db.devkey + ':' + email});
                port.onMessage.addListener(function(response){
                    if(response.success == true){
                        //show success modal
                        html = '<div class="payment-success">' + 
                            '<h2>Thank you for your payment!</h2>' +
                            '<p>' + 
                                'You\'ve successfully made a payment for your subscription. ' +
                            '</p>' +
                            '</div>';
                        showMessage("Payment Success", html, '', () => {
                            //finally, update subscription info
                            subscription_info = null;
                            getSubscriptionInfo(auth_info => {
                                releaseAuthLock(auth_info);
                                replaceSubscriptionMenu(getCurrentSubscription(), subscriptionId);
                                var id = getCurrentSubscription().subscriptionId;
                                loadSubscriptionPage(id, name, view, settingsPageCallback);
                            });
                        });
                    }
                });
                
            });
        });

        $(div).find('section.cancellation button.btn-cancel').on('click', () => {
            //cancel subscription
            if(confirm("Do you really want to cancel your subscription? This cannot be undone.") == true){
                webApi('Subscriptions/Cancel', {}, 
                    function(success){
                        showMessage('Cancellation Success', 'Your subscription has been cancelled. Your refund is being processed and should show up within your credit card account within the next two weeks. Thank you for your business!', '', () => {
                            subscription_info = null;
                            getSubscriptionInfo(auth_info => {
                                releaseAuthLock(auth_info);
                                replaceSubscriptionMenu(getCurrentSubscription(), subscriptionId);
                                var id = getCurrentSubscription().subscriptionId;
                                loadSubscriptionPage(id, name, view, settingsPageCallback);
                            });
                        });
                    },
                    function(err){
                        //don't execute callback
                        //<debug>
                        console.log(err);
                        //</debug>
                        showMessage('Error', err.message, 'error');
                    }
                );
            }
        });
    }

    function modifySubscription(info){
        var html = 
            '<div class="change-plans">' +
                '<h4>Modify Subscription</h4>' +
                '<ul>' +
                info.plans.map(plan => 
                    '<li class="' + plan.schedule + '">' + 
                        '<div class="name"><span>' + plan.name + '</span></div>' +
                        '<div class="select"><button data-id="' + plan.planId + '">Select</button></div>' +
                        '<div class="price"><span>' + 
                            toCurrency(plan.price) + ' / ' + 
                            (plan.minUsers > 1 ? 'user / ' : '') +
                            '<b>' + (plan.schedule == 1 ? 'year' : 'month') + '</b>' +
                        '</span></div>' +
                    '</li>'
                ).join('\n') +
                '</ul>'
            '</div>';
        showModal(html);
        
        $('.change-plans button').on('click', (e) => {
            var id = parseInt(e.target.getAttribute('data-id'));
            modifySubscriptionPayment(info, id);
        });
    }

    function modifySubscriptionPayment(info, planId){
        //displays subscription payment modal 
        //(with options to add/remove email addresses if Team plan)
        var plan = info.plans.filter(a => a.planId == planId)[0];
        plan.members = info.members;
        plan.refund = info.refund;
        var oldid = getCurrentSubscription().subscriptionId;
        Subscribe(info.plans.filter(a => a.planId == planId)[0], () => {
            //<debug>
            console.log('replace subscription menu');
            //</debug>
            replaceSubscriptionMenu(getCurrentSubscription(), oldid);
            var id = getCurrentSubscription().subscriptionId;
            loadSubscriptionPage(id, name, view, settingsPageCallback);
        });
    }
}
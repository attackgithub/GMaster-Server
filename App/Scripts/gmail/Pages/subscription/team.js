function handleTeamPage(view, menu, sub){
    var name = 'team';
    loadSubscriptionPage(sub.subscriptionId, name, view, subscriptionPageCallback);

    function subscriptionPageCallback() {
        //bind all buttons on team page //////////////////////////////////
        $('.subscription-page .btn-manageteam').on('click', () => {
            webApi('Subscriptions/GetModifyInfo', {}, 
                function(mod){
                    var info = mod[0].response;
                    var oldid = info.subscription.subscriptionId;
                    //<debug>
                    console.log(info);
                    //</debug>
                    var plan = info.plans.filter(a => a.planId == info.subscription.planId)[0];
                    plan.members = info.members;
                    Subscribe(plan, () => {
                        //<debug>
                        console.log('replace subscription menu');
                        //</debug>
                        replaceSubscriptionMenu(getCurrentSubscription(), oldid);
                        var id = getCurrentSubscription().subscriptionId;
                        loadSubscriptionPage(id, name, view, subscriptionPageCallback);
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
            
        });
    }
}
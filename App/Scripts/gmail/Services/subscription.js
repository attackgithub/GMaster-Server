var outstanding_info = null;
var subscription_info = null;
var subscription_menus = [];
var subscription_menu_parents = [];

function getSubscriptionInfo(callback, prelocked){
    if(subscription_info == null) {
        //no subscription info exists yet
        if(prelocked === true || lockAuth(callback)){
            //user hasn't retreived plan info yet
            //<debug>
            console.log('get subscription info');
            //</debug>
            webApi('Subscriptions/GetInfo', {}, 
                function(data){
                    var response = data[0].response;
                    outstanding_info = response.outstanding;
                    subscription_info = response.subscriptions;
                    //<debug>
                    console.log(response);
                    //</debug>

                    checkSubscription(() => {
                        releaseAuthLock({devkey:db.devkey, subscription:subscription_info});
                    });
                },
                function(err){
                    //don't execute callback
                    //<debug>
                    console.log(err);
                    //</debug>
                    if(err.status == 401){
                        //developer key is invalid, request a new one
                        db.devkey = null;
                        db.userId = null;
                        chrome.storage.sync.set({devkey: null, userId:null});

                        //set callback to calling all cached callbacks
                        var callbacks = [];
                        for(var x = 0; x < auth_callbacks.length; x++){
                            callbacks.push(auth_callbacks[x]);
                        }
                        auth_callbacks = [];
                        lock_auth = false;
                        //try to authenticate again
                        authenticate('', (auth_info) => {
                            //execute all callbacks
                            for(var x = 0; x < callbacks.length; x++){
                                var item = callbacks[x];
                                if(typeof item.callback == 'function'){
                                    item.callback(auth_info);
                                }
                            }
                        });
                        return;
                    }
                    showMessage('Error', 'Could not retrieve Gmaster subscription details for ' + email, 'error');
                }
            );
        }
    }else{
        //pass-thru since user has dev key & plan info
        checkSubscription(() => {
            callback({devkey:db.devkey, subscription:subscription_info});
        });
    }
}

function checkSubscription(callback){
    var subscription = subscription_info;
    if(subscription.length == 0){
        //show subscription plan page
        //<debug>
        console.log('show available plans');
        //</debug>
        loadPlans();
    }else{
        //subscription exists, load each subscription
        //as a separate left-nav menu item with sub-items
        if(subscription_menus.length == 0){
            var nav_parent = navmenu_gmaster;
            for(var x = 0; x < subscription.length; ++x){
                var sub = subscription[x];
                //determine parent nav menu
                if(subscription.length > 1){
                    nav_parent = navmenu_gmaster.addNavItem({
                        name:sub.teamName,
                        orderHint:100
                    });
                    subscription_menu_parents.push({
                        id:sub.teamName,
                        nav:nav_parent
                    });
                }
                //create menu system for subscription
                createSubscriptionMenu(nav_parent, sub, x);
            }
        }
        
        callback();
    }
}

function hasSubscription(){
    return subscription_info != null ? subscription_info.filter(s => s.userId == db.userId).length >= 1 : false;
}

function getCurrentSubscription(){
    return subscription_info.filter(s => s.userId == db.userId)[0];
}

function Subscribe(plan, finalCallback){
    var msg = '';
    var members = [email];
    var orig_members = [email];
    var refund = null;
    var canSubmitPayment = true;
    var cycleStart = 'begin';

    //get list of emails associated with existing subscription (if possible)
    if(plan.members && plan.members.filter(a => a == email).length == 1){
        members = plan.members.slice(0);
        orig_members = plan.members.slice(0);
    }

    //get refund (if possible)
    if(plan.refund != null){
        refund = plan.refund;
        cycleStart = 'restart';
    }

    if(plan.minUsers > 1){
        //force customer to add team members before paying
        msg = 
            '<div class="total-users-form">' +
                '<div class="row summary">You must include at least ' + plan.minUsers + ' member email addresses</div>' +
                '<div class="row">' +
                    '<div class="col field">Email</div>' +
                    '<div class="col input">' + 
                        '<input type="text" id="member_email" placeholder="person@gmail.com"/>' + 
                    '</div>' +
                    '<div class="col input"><button class="add-member apply">Add Member</button></div>' +
                '</div>' +
                '<div class="sub-total">' + 
                    '<span class="price"></span>' +
                    '<span class="schedule"> / ' + (plan.schedule == 1 ? 'year' : 'month') + '</span>' +
                '</div>' +
                '<div class="member-count"></div>' +
                '<div class="owed-today">' + 
                    '<span class="price"></span> <span class="charged">will be charged today.</span>' + 
                    '<div class="cycle">Your billing cycle will ' + cycleStart + ' today as well.</div>' +
                '</div>' +
            '</div>';
    }else{
        //show sub-total price for all other plans
        msg = 
            '<div class="total-users-form">' +
                '<div class="sub-total">' + 
                    '<span class="price">' + toCurrency(plan.minUsers * plan.price) + '</span>' +
                    '<span> / ' + (plan.schedule == 1 ? 'year' : 'month') + '</span>' +
                '</div>' +
                '<div class="owed-today">' + 
                    '<span class="price"></span> <span class="charged">will be charged today.</span><br/>' + 
                    '<span class="cycle">Your billing cycle will ' + cycleStart + ' today as well.</span>' +
                '</div>' +
            '</div>';
    }

    showMessage('Subscribe to the ' + plan.name + ' plan', msg, 'pay', 
        function(type){
            if(canSubmitPayment == false){
                //user is not allow to submit payment yet
                return false;
            }
            var emails = members.join(',');
            
            function getSuccessMessage(){
                //method to display success message after payment has been processed
                if(subscription_info.length == 0 || subscription_info == null){
                    msg = '<div class="payment-success">' + 
                        '<h2>Thank you for subscribing!</h2>' +
                        '<p>' + 
                            'You\'re well on your way to sending your first campaign with Gmaster. ' + 
                            'To begin, you should review the documentation for ' + 
                            '<a id="gmaster-getting-started" href="javascript:">Getting Started</a>, ' +
                            'which can be found within the <b>Gmaster</b> &gt; <b>Support</b> section ' +
                            'located within the left-hand navigation menu.<br/><br/>' + 
                            'It may take a few minutes before your payment is processed and the Gmaster ' +
                            'features are unlocked within Gmail.' +
                        '</p>' +
                    '</div>'
                    showMessage("Payment Success", msg, '', () => {
                        sdk.Router.goto(sdk.Router.NativeRouteIDs.INBOX);
                    });
                    $('#gmaster-gettings-started').on('click', () => {
                        sdk.Router.goto('gmaster-support-getting-started');
                    });
                    //finally, get user's plan info (again)
                    getSubscriptionInfo(function(auth_info){
                        releaseAuthLock(auth_info);
                    });
                    
                }else if(hasSubscription() == true){
                    msg = '<div class="payment-success">' + 
                            '<h2>Thank you for your payment!</h2>' +
                            '<p>' + 
                                'You\'ve successfully modified your subscription. ' +
                                'You are now subscribed to the <b>' + plan.name + '</b> plan with <b>' + members.length + '</b> member' + (members.length > 1 ? 's' : '') + '.' +
                            '</p>' +
                        '</div>';
                    showMessage("Payment Success", msg, '', () => {
                        //finally, get user's plan info (again)
                        subscription_info = null;
                        getSubscriptionInfo(function(auth_info){
                            releaseAuthLock(auth_info);
                            if(typeof finalCallback == 'function'){finalCallback();}
                        });
                    });
                }
            }
            if(type == 'credit'){
                //show popup window with Stripe credit card processing form
                //<debug>
                console.log('show Stripe popup window "' + "subscribe:" + plan.planId + ':' + members.length + ':' + db.devkey + ':' + email + '"');
                //</debug>
                var port = chrome.runtime.connect({name: "subscribe:" + plan.planId + ':' + members.length + ':' + db.devkey + ':' + email});
                port.onMessage.addListener(function(response){
                    if(response.success == true){
                        webApi('Subscriptions/Subscribe', {planId:plan.planId, emails:emails, zipcode:response.zipcode, stripeToken:response.token}, 
                            function(data){
                                //show success modal
                                getSuccessMessage();
                            },
                            function(err){
                                //don't execute callback
                                //<debug>
                                console.log(err);
                                //</debug>
                                showMessage('Error', err.responseText, 'error');
                            }
                        );
                    }
                });
            }else if(type == 'saved'){
                //don't show any payment form. Simply process the subscription change
                canSubmitPayment = false;
                webApi('Subscriptions/Subscribe', {planId:plan.planId, emails:emails, zipcode:'', stripeToken:''}, 
                function(data){
                    getSuccessMessage();
                },
                function(err){
                    //don't execute callback
                    //<debug>
                    console.log(err);
                    //</debug>
                    showMessage('Error', err.responseText, 'error');
                }
            );
                
            }else if(type == 'paypal'){
                //show PayPal login form
                //<debug>
                console.log('show PayPal popup window');
                //</debug>
            }
        }
    );
    var buttons = $('.gmaster-content .modal .buttons');

    //add event listeners to popup message
    if(plan.minUsers > 1){
        //add member list area below payment buttons
        $('.modal .message').append('<div class="members-list"></div>');

        $('.total-users-form button.add-member').on('click', function(e){
            //add member email to payment process
            var memberEmail = $('#member_email').val();
            if(members.indexOf(memberEmail) >= 0){
                alert('Cannot add a duplicate email to the member list');
                return;
            }else if(isValidEmail(memberEmail) === true){
                members.push(memberEmail);
                renderMembersList();
                $('#member_email').val('');
            }else{
                alert('Please provide a valid email address.');
            }
        });

        function renderMembersList(){
            //update payment buttons
            if(members.length >= plan.minUsers){
                buttons.removeClass('disabled');
            }else{
                buttons.addClass('disabled');
            }
            //update price total
            var total = (plan.price * members.length);
            $('.total-users-form .price').html(toCurrency(total));

            //update total owed today
            if(refund != null){
                var owedAmount = toCurrency(total - refund.Amount);
                $('.total-users-form .owed-today .price').html(owedAmount.replace('-',''));
                if(owedAmount.indexOf('-') == 0){
                    $('.total-users-form .owed-today .charged').html('will be <b>credited</b> today');
                }else{
                    $('.total-users-form .owed-today .charged').html('will be <b>charged</b> today');
                }

            }else{
                $('.total-users-form .owed-today .price').html(toCurrency(total));
                $('.total-users-form .owed-today .charged').html('will be <b>charged</b> today');
            }

            //update member count
            $('.total-users-form .member-count').html('for <b>' + members.length + '</b> user' + (members.length > 1 ? 's' : ''));

            //update billing cycle text
            var cycleStart = 'begin';
            if(refund != null){cycleStart = 'restart';}
            $('.total-users-form .owed-today .cycle').html('Your billing cycle will ' + cycleStart + ' today as well.');
            
            canSubmitPayment = true;
            if(members.length == orig_members.length && members.filter(m => orig_members.indexOf(m) < 0).length > 0){
                //changed emails but no added emails
                $('.total-users-form .owed-today .price').hide();
                $('.total-users-form .owed-today .charged').hide();
                $('.total-users-form .owed-today .cycle').html('You will not be <b>charged</b> today');
            }else if(plan.planId == getCurrentSubscription().planId && members.length == orig_members.length && members.filter(m => orig_members.indexOf(m) >= 0).length == orig_members.length){
                //no email changes or additions
                canSubmitPayment = false;
                buttons.addClass('disabled');
                $('.total-users-form .owed-today .price').hide();
                $('.total-users-form .owed-today .charged').hide();
                $('.total-users-form .owed-today .cycle').html('You must make changes to your email list before continuing.');
            }else{
                $('.total-users-form .owed-today .price')[0].style.display = '';
                $('.total-users-form .owed-today .charged')[0].style.display = '';
            }

            //update member list
            $('.modal .members-list').html(
                members.slice().reverse().map( m => {
                    return '' + 
                        '<div class="new-member" data-id="' + m + '">' + 
                            '<span>' + m + '</span>' +
                            (m != email ? '<div class="remove-member">X</div>' : '') +
                        '</div>';
                }).join('\n')
            );

            $('.modal .remove-member').on('click', (e) => {
                var memberEmail = $(e.target).parents('.new-member')[0].getAttribute('data-id');
                if(memberEmail == email){
                    alert('Cannot remove Gmaster account email address from list.');
                }
                members.splice(members.indexOf(memberEmail), 1);
                renderMembersList();
            });
        }
        renderMembersList();
    }else{
        //single-user only plans
        var total = plan.minUsers * plan.price;
        members=[email];
        if(refund != null){
            var owedAmount = toCurrency(total - refund.Amount);
            $('.total-users-form .owed-today .price').html(owedAmount.replace('-',''));
            if(owedAmount.indexOf('-') == 0){
                $('.total-users-form .owed-today .charged').html('will be <b>credited</b> today');
            }else{
                $('.total-users-form .owed-today .charged').html('will be <b>charged</b> today');
            }

        }else{
            $('.total-users-form .owed-today .price').html(toCurrency(total));
        }
    }

}

function createSubscriptionMenu(nav, sub, index){
    //set up subscription nav menu object
    var menu = {id:'sub' + sub.subscriptionId, nav:{}, routes:{}};
    var suffix = 'gmaster-subscription-' + menu.id;

    //create subscription nav menu items
    menu.routes.campaigns = suffix + '-campaigns';
    menu.nav.campaigns = nav.addNavItem({
        name:"Campaigns",
        routeID:menu.routes.campaigns,
        orderHint:index + 100
    });
    sdk.Router.handleCustomRoute(menu.routes.campaigns, (view) => {
        //display campaigns for subscription
        handleCampaignsPage(view, menu, sub);
    });

    menu.routes.addressbook = suffix + '-address-book';
    menu.nav.addressbook = nav.addNavItem({
        name:"Address Book",
        routeID:menu.routes.addressbook,
        orderHint:index + 101
    });
    sdk.Router.handleCustomRoute(menu.routes.addressbook, (view) => {
        //display address book for subscription
        handleAddressBookPage(view, menu, sub);
    });

    menu.routes.reports = suffix + '-reports';
    menu.nav.reports = nav.addNavItem({
        name:"Reports",
        routeID:menu.routes.reports,
        orderHint:index + 102
    });
    sdk.Router.handleCustomRoute(menu.routes.reports, (view) => {
        //display reports for subscription
        handleReportsPage(view, menu, sub);
    });

    menu.routes.team = suffix + '-team';
    menu.nav.team = nav.addNavItem({
        name:"Team",
        routeID:menu.routes.team,
        orderHint:index + 103
    });
    sdk.Router.handleCustomRoute(menu.routes.team, (view) => {
        //display team members for subscription
        handleTeamPage(view, menu, sub);
    });

    menu.routes.settings = suffix + '-settings';
    menu.nav.settings = nav.addNavItem({
        name:"Subscription",
        routeID:menu.routes.settings,
        orderHint:index + 104
    });
    sdk.Router.handleCustomRoute(menu.routes.settings, (view) => {
        //display settings for subscription
        handleSettingsPage(view, menu, sub);
    });

    subscription_menus.push(menu);
}

function replaceSubscriptionMenu(sub, oldId){
    var menu = subscription_menus.filter(m => m.id == 'sub' + oldId)[0];
    //remove menus from navigation
    menu.nav.campaigns.remove();
    menu.nav.addressbook.remove();
    menu.nav.reports.remove();
    menu.nav.team.remove();
    menu.nav.settings.remove();
    //remove menu from array
    subscription_menus.splice(subscription_menus.indexOf(menu), 1);
    //add new menu
    if(subscription_menu_parents.length > 0){
        createSubscriptionMenu(subscription_menu_parents.filter(a => a.id == sub.teamName)[0], sub, 100);
    }else{
        createSubscriptionMenu(navmenu_gmaster, sub, 100);
    }
}

function loadSubscriptionPage(subscriptionId, name, view, callback){
    //loads a subscription page by name
    //name: addressbook, campaigns, reports, settings, team
    $('.gmaster-page.subscription-page').remove();
    var div = document.createElement('div');
    var id = name + '-' + subscriptionId;
    div.className = 'gmaster-page subscription-page  ' + id;
    //load subscription page via Gmaster Web API
    $(div).html('<div class="please-wait">Loading content...</div>');
    getUrl(host + 'subscription/' + subscriptionId + '/' + encodeURIComponent(name) + '?nolayout', {devkey:db.devkey, email:email}, 
        function(data){
            $(div).html(data);
            if(typeof callback == 'function'){callback(subscriptionId);}
        }
    );
    view.getElement().appendChild(div);
}
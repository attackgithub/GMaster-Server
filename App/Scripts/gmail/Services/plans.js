var plans_info = [];

function loadPlans(){
    if(plans_info.length == 0){
        webApi('Plans/GetInfo', {}, 
        function(data){
            plans_info = data;
            //<debug>
            console.log(data);
            //</debug>

            navmenu_gmaster.addNavItem({
                name:"Available Plans" ,
                routeID:'gmaster-plans',
                orderHint:1
            });

            viewPlans(plans_info);
        },
        function(err){
            showMessage('Error', 'Could not retrieve Gmaster subscription plans list. Please try again later.', 'error');
        }
    );
    }else{
        viewPlans(plans_info);
    }
}

var plans_routed = false;
var features_list = '';

function viewPlans(plans){
    if(plans_routed == false){
        plans_routed = true;
        sdk.Router.handleCustomRoute('gmaster-plans', (view) => {
            //<debug>
            console.log(plans);
            //</debug>
            //container div
            var div = document.createElement('div');
            div.className = 'gmaster-page gmaster-plans';

            //page header
            var divHead = document.createElement('div');
            divHead.className = 'header';
            divHead.innerHTML = 
                '<div class="logo"><img src="' + extensionUrl + 'logo-xs.png"/></div>' + 
                '<span>Available Subscription Plans</span>';
            div.appendChild(divHead);

            if(plans.length > 0){
                //show switch for monthly/yearly
                divHead.innerHTML += 
                    '<div class="plan-schedule">' +
                        '<div class="schedule-switch">' +
                            '<div class="monthly selected">Monthly</div>' +
                            '<div class="yearly">Yearly</div>' +
                        '</div>' + 
                    '</div>'; 
                $(divHead).find('.monthly').on('click', () => {
                    $('.schedule-switch > div').removeClass('selected');
                    $('.schedule-switch .monthly').addClass('selected');
                    $('.plans .schedule').hide();
                    $('.plans .schedule.monthly').show();
                })
                $(divHead).find('.yearly').on('click', () => {
                    $('.schedule-switch > div').removeClass('selected');
                    $('.schedule-switch .yearly').addClass('selected');
                    $('.plans .schedule').hide();
                    $('.plans .schedule.yearly').show();
                })
            }

            //features list
            var featuresDiv = document.createElement('div');
            featuresDiv.className = 'features';
            featuresDiv.innerHTML = 
                '<ul>' + 
                    '<li>Unlimited Emails</li>' +
                    '<li>Remove Gmaster email branding</li>' + 
                    '<li>Build an Address Book</li>' + 
                    '<li>Import data from Google Sheets</li>' +
                    '<li>Auto-Followup Campaigns</li>' +
                    '<li>Add Surveys to Campaigns</li>' + 
                    '<li>Use 3rd-party SMTP Services</li>' +
                    '<li>Collaborate with other users</li>' +
                '</ul>';
            div.appendChild(featuresDiv);

            //plans list container
            var plansDiv = document.createElement('div');
            plansDiv.className = 'plans';
            div.appendChild(plansDiv);
            
            if(plans.length > 0){
                //render each plan
                var schedule = -1;
                var scheduleDiv = null;
                for(var x = 0; x < plans.length; x++){
                    const plan = plans[x];
                    var planDiv = document.createElement('div');
                    planDiv.className = 'plan';

                    if(scheduleDiv == null || schedule != plan.schedule){
                        scheduleDiv = document.createElement('div');
                        scheduleDiv.className = 'schedule ' + (plan.schedule == 0 ? 'monthly' : 'yearly') ;
                        if(plan.schedule != 0){
                            scheduleDiv.style.display = 'none';
                        }
                        schedule = plan.schedule;
                        plansDiv.appendChild(scheduleDiv);
                    }

                    function renderFeature(enabled){
                        return enabled === true ? 
                            '<img src="' + extensionUrl + 'icon-check.png"/>' 
                        :
                            '<img src="' + extensionUrl + 'icon-close.png"/>';

                    }

                    planDiv.innerHTML = 
                        '<div class="head">' +
                            '<h3>' + plan.name + '</h3>' +
                            '<p>' + 
                                '<span class="price">' + 
                                '<span>$</span>' + plan.price.toFixed(2) + 
                                '</span>' + 
                                (plan.maxUsers > 1 ? ' / user' : ' / ' + (plan.schedule == 1 ? 'year' : 'month')) +
                            '</p>' +
                        '</div>' +
                        '<div class="plan-features">' +
                            '<ul>' + 
                                '<li>' + renderFeature(plan.hasUnlimitedEmails) + '</li>' +
                                '<li>' + renderFeature(!plan.hasAds) + '</li>' +
                                '<li>' + renderFeature(plan.hasAddressBook) + '</li>' +
                                '<li>' + renderFeature(plan.hasGoogleSheets) + '</li>' +
                                '<li>' + renderFeature(plan.hasFollowupCampaigns) + '</li>' +
                                '<li>' + renderFeature(plan.hasQAPolls) + '</li>' +
                                '<li>' + renderFeature(plan.hasSendGrid) + '</li>' +
                                '<li>' + renderFeature(plan.canCollaborate) + '</li>' +
                            '</ul>' + 
                            (plan.minUsers > 1 ? 
                                '<div class="extra-info">* minimum ' + plan.minUsers + ' users ' +
                                '@ <b>$' + (plan.minUsers * plan.price).toFixed(2) + 
                                '/' + (plan.schedule == 1 ? 'year' : 'month') + '</div>'
                                : ''
                            ) +
                        '</div>' +
                        '<div class="buttons">' +
                            '<a class="button btn-subscribe">Subscribe</a>' +
                        '</div>';
                    $(planDiv).find('.btn-subscribe').on('click', function(){Subscribe(plan)});
                    scheduleDiv.appendChild(planDiv);
                }
            }
            view.getElement().appendChild(div);

            //load list of features
            var featuresListDiv = document.createElement('div');
            featuresListDiv.className = 'features-list';

            if(features_list == ''){
                //get features list from Gmaster server
                getUrl(host + 'features?partial', null, 
                    function(data){
                        //<debug>
                        console.log(['features list',data]);
                        //</debug>
                        features_list = data;
                        featuresListDiv.innerHTML = features_list;
                        view.getElement().appendChild(featuresListDiv);
                    }
                );
            }else{
                //load cached features list
                featuresListDiv.innerHTML = features_list;
                view.getElement().appendChild(featuresListDiv);
            }
        });    
    }
    
    sdk.Router.goto('gmaster-plans');
}


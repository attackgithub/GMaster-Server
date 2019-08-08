var compose_inc = 0;

authenticate((auth_info) => {
    sdk.Compose.registerComposeViewHandler(handleComposeView);
});

function handleComposeView(view, campaignId) {
    if (view.isReply() == true) {
        //not eligible for being made into a campaign
        return;
    }

    //increment compose window index
    var index = compose_inc + 1;
    compose_inc += 1;

    //add a campaign-mode bar
    var campaignbar = view.addComposeNotice({});
    $(campaignbar.el).append(
        '<div class="gmaster campaign-bar-' + index + '">' +
            '<div class="row">' +
                '<div class="col">' +
                    '<div class="button-switch"><div class="knob"></div></div>' +
                '</div>' +
                '<div class="col switch-label">Compose Campaign Message for </div>' +
                '<div class="col switch-subscription">' +
                    '<select id="compose_subscription_' + index + '">' +
                    subscription_info.map(a => {
                        return '<option value="' + a.subscriptionId + '">' + a.teamName + '</option>';
                    }).join('') +
                    '</select>' +
                '</div>' +
            '</div>' +
        '</div>'
    );

    var btnswitch = $('.campaign-bar-' + index + ' .button-switch');

    //compose window UI elements
    var win = btnswitch.parents('table');
    var sendbtn = win.find('[data-tooltip^="Send"]').filter((i, a) => a.innerHTML == 'Send').parents('td').first();
    var buttons = win.find('[command="Files"], [command="docs"], [command="locker"], [command="op.money"]');
    var subject = win.find('[aria-label="Subject"]');
    var msgto = win.find('[aria-label="To"]');
    var recipients = win.find('div').filter((i, a) => a.innerHTML == 'Recipients').parent();
    var msgbody = win.find('[aria-label="Message Body"]');
    var tabindexes = win.find('[tabindex]');
    var ccbccbtns = win.find('[data-tooltip^="Add Cc recipients"]').parents('div').first();
    var cc = win.find('[data-tooltip="Select contacts"').filter((i, a) => a.innerHTML == 'Cc').parents('tr').first();
    var bcc = win.find('[data-tooltip="Select contacts"').filter((i, a) => a.innerHTML == 'Bcc').parents('tr').first();

    //toggle Campaign-mode switch
    btnswitch.on('click', () => {
        btnswitch.toggleClass('selected');
        hideShowComposeUIElements()
    });

    //Compose window UI events
    tabindexes.on('click,focus', () => {
        //focus on any element with a tabindex
        hideShowComposeUIElements()
    });

    subject.on('click,focus', () => {
        //click subject textbox
        hideShowComposeUIElements()
    });

    msgto.on('click,focus', () => {
        //click To textbox
        hideShowComposeUIElements()
    });

    recipients.on('click,focus', () => {
        //click minimized To textbox
        hideShowComposeUIElements()
    });

    msgbody.on('click,focus', () => {
        //click message text area
        hideShowComposeUIElements()
    });

    if (campaignId != null) {
        btnswitch[0].click();
        //load campaign info from Gmaster API

    }

    function hideShowComposeUIElements() {
        setTimeout(() => { 
            //timeout to let Compose UI execute first before showing/hiding UI elements
            if (btnswitch.hasClass('selected')) {
                //toggle campaign mode on, remove unneccessary message settings
                view.setBccRecipients([]);
                view.setCcRecipients([]);
                //hide message-related UI
                sendbtn.hide();
                buttons.hide();
                ccbccbtns.hide();
                cc.hide();
                bcc.hide();
                //show campaign-related UI
                composebtn.show();
            } else {
                //show message-related UI
                sendbtn.show();
                buttons.show();
                ccbccbtns.show();
                //hide campaign-related UI
                composebtn.hide();
            }
        }, 10);
    }


    //create campaign-related buttons
    if (campaignId == null) {
        view.addButton({
            title: 'Create your campaign',
            iconClass: 'compose-button compose-create-button',
            onClick: function (event) {
                //create a new campaign
                var data = {
                    subscriptionId: parseInt($('#compose_subscription_' + index).val()),
                    subject: view.getSubject(),
                    body: view.getHTMLContent(),
                    emails: view.getToRecipients().map(a => a.emailAddress).join(','),
                }

                //get draftID
                view.getCurrentDraftID().then(draftId => {
                    data.draftId = draftId;

                    webApi('Campaigns/Create', data,
                        (response) => {
                            var campaignId = parseInt(response[0].campaignId);
                            sdk.Router.goto('campaign-details/:campaignId', { campaignId: campaignId });
                            view.close();
                        }, (err) => {
                            showMessage("Gmaster Error", err.responseText, 'error');
                        }
                    );
                });
            },
        });
    } else {
        view.addButton({
            title: 'Update your campaign message & email list',
            iconClass: 'compose-button compose-update-button',
            onClick: function (event) {
                //update existing campaign
            },
        });
    }

    if (campaignId != null) {
        //load details about campaign
    }

    //finally, update compose window UI
    var composebtn = win.find('.compose-button').parents('td').first();
    hideShowComposeUIElements();
}
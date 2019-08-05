var compose_inc = 0;

authenticate((auth_info) => {
    sdk.Compose.registerComposeViewHandler(handleComposeView);
});

function handleComposeView(view, campaignId) {
    if (view.isReply() == true) {
        //not eligible for being made into a campaign
        return;
    }
    compose_inc += 1;
    var settings = {
        index: compose_inc,
        campaignId: campaignId
    };

    if (campaignId != null) {
        //load details about campaign
    }

    //add a campaign-mode bar
    var campaignbar = view.addComposeNotice({});
    $(campaignbar.el).append(
        '<div class="gmaster campaign-bar-' + settings.index + '">' +
        '<div class="row">' +
        '<div class="col">' +
        '<div class="button-switch"><div class="knob"></div></div>' +
        '</div>' +
        '<div class="col switch-label">Compose Campaign Message</div>' +
        '<div class="col right">' +

        '</div>' +
        '</div>' +
        '</div>'
    );

    var btnswitch = $('.campaign-bar-' + settings.index + ' .button-switch');

    //compose window UI elements
    var sendbtn = btnswitch.parents('table').find('[data-tooltip^="Send"]').filter((i, a) => a.innerHTML == 'Send').parents('td').first();
    var buttons = btnswitch.parents('table').find('[command="Files"], [command="docs"], [command="locker"], [command="op.money"]');
    var subject = btnswitch.parents('table').find('[aria-label="Subject"]');
    var msgto = btnswitch.parents('table').find('[aria-label="To"]');
    var recipients = btnswitch.parents('table').find('div').filter((i, a) => a.innerHTML == 'Recipients').parent();
    var msgbody = btnswitch.parents('table').find('[aria-label="Message Body"]');
    var tabindexes = btnswitch.parents('table').find('[tabindex]');

    console.log([subject, msgto, recipients, msgbody]);

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
                //hide buttons
                sendbtn.hide();
                buttons.hide();
            } else {
                //show buttons
                sendbtn.show();
                buttons.show();
            }
        }, 10);
    }


    // a compose view has come into existence, do something with it!
    view.addButton({
        title: 'Save your campaign',
        iconClass: 'compose-button compose-save-button',
        onClick: function (event) {

        },
    });
}
authenticate((auth_info) => {
    sdk.Compose.registerComposeViewHandler(function(composeView){

        // a compose view has come into existence, do something with it!
        composeView.addButton({
            title: 'Send this email as a mass mail',
            iconClass:'compose-button compose-send-button',
            onClick: function(event) {
                event.composeView.insertTextIntoBodyAtCursor('Master your Gmail Account.');
            },
        });
    });
});
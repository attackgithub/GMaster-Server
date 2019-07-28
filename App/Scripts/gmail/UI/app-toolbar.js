authenticate((auth_info) => {
    sdk.Toolbars.addToolbarButtonForApp({
        title: 'Gmaster',
        titleClass:'gmaster-button',
        onClick: function(){
            alert('cool brah');
        }
    });
});
navmenu_support.addNavItem({
    name:"Subscriptions" ,
    routeID:'gmaster-support-subscriptions'
});

sdk.Router.handleCustomRoute('gmaster-support-subscriptions', (view) => {
    loadSupportPage('subscriptions', view);
});    
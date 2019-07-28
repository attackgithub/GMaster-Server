navmenu_support.addNavItem({
    name:"Billing" ,
    routeID:'gmaster-support-billing'
});

sdk.Router.handleCustomRoute('gmaster-support-billing', (view) => {
    loadSupportPage('billing', view);
});    
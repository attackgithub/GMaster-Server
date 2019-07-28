navmenu_support.addNavItem({
    name:"Campaigns" ,
    routeID:'gmaster-support-campaigns'
});

sdk.Router.handleCustomRoute('gmaster-support-campaigns', (view) => {
    loadSupportPage('campaigns', view);
});    
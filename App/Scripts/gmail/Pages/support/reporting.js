navmenu_support.addNavItem({
    name:"Reporting" ,
    routeID:'gmaster-support-reporting'
});

sdk.Router.handleCustomRoute('gmaster-support-reporting', (view) => {
    loadSupportPage('reporting', view);
});    
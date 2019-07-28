navmenu_support.addNavItem({
    name:"Teams" ,
    routeID:'gmaster-support-teams'
});

sdk.Router.handleCustomRoute('gmaster-support-teams', (view) => {
    loadSupportPage('teams', view);
});    
navmenu_support.addNavItem({
    name:"Surveys" ,
    routeID:'gmaster-support-surveys'
});

sdk.Router.handleCustomRoute('gmaster-support-surveys', (view) => {
    loadSupportPage('surveys', view);
});    
navmenu_support.addNavItem({
    name:"Getting Started" ,
    routeID:'gmaster-support-getting-started'
});

sdk.Router.handleCustomRoute('gmaster-support-getting-started', (view) => {
    loadSupportPage('getting-started', view);
});    
navmenu_support.addNavItem({
    name:"Importing Data" ,
    routeID:'gmaster-support-import-data'
});

sdk.Router.handleCustomRoute('gmaster-support-import-data', (view) => {
    loadSupportPage('import-data', view);
});    
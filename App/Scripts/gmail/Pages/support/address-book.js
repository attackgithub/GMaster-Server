navmenu_support.addNavItem({
    name:"Address Book" ,
    routeID:'gmaster-support-address-book'
});

sdk.Router.handleCustomRoute('gmaster-support-address-book', (view) => {
    loadSupportPage('address-book', view);
});    
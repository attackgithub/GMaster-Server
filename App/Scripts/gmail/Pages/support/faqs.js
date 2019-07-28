navmenu_support.addNavItem({
    name:"FAQs" ,
    routeID:'gmaster-support-faqs'
});

sdk.Router.handleCustomRoute('gmaster-support-faqs', (view) => {
    loadSupportPage('faqs', view);
});    
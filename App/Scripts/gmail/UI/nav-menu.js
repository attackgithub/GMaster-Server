var navmenu_gmaster = sdk.NavMenu.addNavItem({
    name:"Gmaster"
});

var navmenu_support = navmenu_gmaster.addNavItem({
    name:"Support",
    orderHint:999
});
navmenu_support.setCollapsed(true);
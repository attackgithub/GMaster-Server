//load gmail CSS file
getUrl(host + 'GmailCss', null, (css) => {
    $('head').append('<style type="text/css" id="gmaster_css">' + css + '</style>');
});
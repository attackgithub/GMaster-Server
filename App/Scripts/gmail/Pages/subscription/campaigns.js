function handleCampaignsPage(view, menu, sub){
    loadSubscriptionPage(sub.subscriptionId, "campaigns", view, () => {
        //bind all buttons on addressbook page //////////////////////////////////
        $('.campaign-list tr').on('click', (e) => {
            var id = $(e.target).attr('data-id');
            if (e.target.tagName != 'TR') {
                id = $(e.target).parents('tr').first().attr('data-id');
            }
            loadCampaign(id);
        });
    });
}

sdk.Router.handleCustomRoute('campaign-details/:campaignId', (view) => {
    //called when user requests to load campaign details page
    console.log(view.getParams());
    var params = view.getParams();
    var campaignId = parseInt(params.campaignId);
    

    $('.gmaster-page.campaign-details-page').remove();
    var div = document.createElement('div');
    div.className = 'gmaster-page campaign-details-page campaign-' + campaignId;
    //load subscription page via Gmaster Web API
    $(div).html('<div class="please-wait">Loading content...</div>');
    getUrl(host + 'campaign/' + campaignId + '?nolayout', { devkey: db.devkey, email: email },
        function (data) {
            $(div).html(data);

            //handle button events
        }
    );
    view.getElement().appendChild(div);
})
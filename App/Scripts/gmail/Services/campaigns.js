function loadCampaign(campaignId) {
    sdk.Router.goto('campaign-details/:campaignId', { campaignId: campaignId });
}
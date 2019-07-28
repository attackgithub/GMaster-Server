//cache support pages loaded from Gmaster Web API
var support_cache = {};

function loadSupportPage(name, view){
    var div = document.createElement('div');
    div.className = 'gmaster-page support-page ' + name;
    if(support_cache[name] != null){
        //load support page via cache
        $(div).html(support_cache[name]);
        addSupportPageLinks(div);
    }else{
        //load support page via Gmaster Web API
        $(div).html('<div class="please-wait">Loading content...</div>');
        getUrl(website + 'support/' + encodeURIComponent(name) + '?nolayout', {}, 
            function(data){
                $(div).html(data);
                addSupportPageLinks(div);
            }
        );
    }
    view.getElement().appendChild(div);
}

function addSupportPageLinks(div){
    //adds event handlers to anchor links that
    //link to other support pages so that they
    //can be loaded with Gmail
}

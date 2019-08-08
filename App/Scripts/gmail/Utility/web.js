function webApi(path, data, onComplete, onError){
    var form = {};
    if(data != null && typeof data == 'object'){ 
        form = JSON.parse(JSON.stringify(data));
    }
    form.devkey = db.devkey;
    form.email = email;
    //<debug>
    console.log([api + path, form]);
    //</debug>
    $.ajax(api + path, {
        data: JSON.stringify(form),
        dataType: 'json',
        contentType: "text/plain; charset=utf-8",
        method: 'POST',
        success: (e) => {
            //<debug>
            console.log([api + path, form]);
            //</debug>
            onComplete(e)
        },
        error:onError
    });
}

function getUrl(url, data, onComplete, onError){
    $.ajax(url, {
        data:JSON.stringify(data), 
        dataType:'html',
        contentType: "text/plain; charset=utf-8",
        method:'POST',
        success:onComplete, 
        error:(err) => {
            if(onError == null){
                genericError(err, url);
            }else if(typeof onError == 'function'){
                onError(err);
            }
        }
    });
}

function loadScript(url, id, onload){
    if($('#' + id).length > 0){
        //script already loaded
        if(typeof onload == 'function'){
            onload();
        }
        return;
    }
    var script = document.createElement('script');
    script.onload = onload;
    script.src = url;
    script.setAttribute('id', id);
    document.body.appendChild(script);
}

function genericError(err, url){
    showMessage('Error', 
        (err.status != null ? 'Error ' + err.status + '. ' : '') + 
        'Could not retrieve content from "' + url + '".'
    , 'error');
}
var auth_callbacks = [];
var auth_tries = 0;
var lock_auth = false; //locks all authentication requests

function authenticate(callback){
    auth_tries++;
    if(auth_tries > 5){
        showMessage('Error', 'Tried to authenticate with Gmaster servers 5 times & gave up.', 'error');
    }
    if(!db.devkey || db.devkey == null || db.devkey == ''){
        //no developer key exists
        //<debug>
        console.log('no dev key found in storage');
        //</debug>
        if(lockAuth(callback)){
            //show authentication popup to log into Gmaster
            var port = chrome.runtime.connect({name: "authenticate"});
            port.onMessage.addListener(function(msg){
                //received dev key from extension
                var devkey = msg.devkey;
                //<debug>
                console.log('retrieved dev key from Gmaster server: ' + devkey);
                //</debug>
                db.devkey = devkey;
                db.userId = msg.userId;
                chrome.storage.sync.set({devkey: devkey, userId:msg.userId});
                auth_tries = 0;

                //finally, get user's plan info
                getSubscriptionInfo(function(auth_info){
                    releaseAuthLock(auth_info);
                }, true);
                
            });
        }
    }else{
        //key exists
        //<debug>

            // reset devkey /////////////////////

            //db.devkey = null; 
            //subscription_info = null;
            //authenticate(callback);
            //return;
        //</debug>
        
        /////////////////////////////////////
        if(lockAuth(callback)){
            console.log(['key exists', db.devkey]);
            getSubscriptionInfo(function(auth_info){
                releaseAuthLock(auth_info);
            }, true);
        }
    }
}

function lockAuth(callback){
    //track pending callbacks while locked
    auth_callbacks.push({callback:callback});
    if(lock_auth == false){
        //not locked yet
        lock_auth = true;
        return true;
    }else{
        //already locked
        return false;
    }
}

function releaseAuthLock(auth_info){
    const len = auth_callbacks.length;
    auth_tries = 0;
    for(let x = 0; x < len; x++){
        //execute queued callback
        var item = auth_callbacks.shift();
        if(typeof item.callback == 'function'){
            item.callback(auth_info);
        }
    }
    lock_auth = false;
}
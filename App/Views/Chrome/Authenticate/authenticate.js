var auth2;

$(document).ready(() => {
    gapi.load('auth2', function () {
        auth2 = gapi.auth2.init({
            client_id: clientId,
        });

        auth2.grantOfflineAccess().then(
            function (response) {
                $('.auth-msg').html('Authenticating...');
                S.ajax.post('Chrome/OAuth2', response,
                    function (devkey) {
                        $('.auth-msg').html('Authenticated! Please wait...');
                        chrome.runtime.sendMessage(extensionId, { devkey: devkey },
                            function (response) {
                                console.log(response);
                                
                            }
                        );
                    },

                    function (err) {
                        console.log(err.responseText);
                    }
                );
            }
        );
    });
});
    
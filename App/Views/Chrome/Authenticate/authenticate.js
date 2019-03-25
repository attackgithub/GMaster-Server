var auth2;

$(document).ready(() => {
    gapi.load('auth2', function () {
        auth2 = gapi.auth2.init({
            client_id: clientId,
        });

        auth2.grantOfflineAccess().then(
            function (response) {
                console.log(response);
                S.ajax.post('Chrome/OAuth2', response,
                    function (d) {
                        window.parent.Gmaster_authenticated(d);
                    },

                    function (err) {
                        console.log(err.responseText);
                    }
                );
            }
        );
    });
});
    
﻿var auth2;

$(document).ready(() => {
    authAccount();
});

function authAccount() {
    if (auth2 == null) {
        gapi.load('auth2', function () {
            auth2 = gapi.auth2.init({
                client_id: clientId,
                access_type: 'offline',
                login_hint: email,
                prompt: 'consent',
                scope: [
                    'openid profile email',
                    'https://www.googleapis.com/auth/plus.me',
                    'https://www.googleapis.com/auth/gmail.compose',
                    'https://www.googleapis.com/auth/gmail.readonly',
                    'https://www.googleapis.com/auth/gmail.labels'
                ].join(' ')
            });
            openGoogleSignin();
        });
    } else {
        openGoogleSignin();
    }
}

function openGoogleSignin() {
    auth2.grantOfflineAccess().then(
        function (response) {
            $('.auth-msg').html('Authenticating...');
            S.ajax.post('Google/OAuth2', response,
                function (d) {
                    var data = JSON.parse(d);
                    $('.auth-msg').html('Authenticated! Please wait...');
                    chrome.runtime.sendMessage(extensionId, { devkey: data.devkey, userId:data.userId },
                        function (response) {
                            if (response == 'success') {
                                window.close();
                            } else {
                                error('Error authenticating. Please try again.');
                            }
                        }
                    );
                },

                function (err) {
                    error('Error authenticating. Please try again.<div class="summary">' + err.responseText + '</div>');
                }
            );
        }
    ).catch(function (err) {
        var msg = err.error.replace(/\_/g, ' ');
        msg = msg.charAt(0).toUpperCase() + msg.slice(1);
        error(msg + '. Please try again.');
    });
}

function error(msg) {
    $('.auth-msg').html(msg + '<div class="retry"><a href="javascript:" onclick="authAccount()">Retry</a></div>');
} 
/// <reference path="jquery-1.10.2.min.js" />

function getAccessToken() {
    if (location.hash) {
        if (location.hash.split('access_token=')) {
            var accessToken = location.hash.split('access_token=')[1].split('&')[0];
            if (accessToken) {
                isUserRegistered(accessToken);
            }
        }
    }
}

function isUserRegistered(accessToken) {
    $.ajax({
        url: '/api/Account/UserInfo',
        method: 'GET',
        headers: {
            'content-type': 'application/JSON',
            'Authorization' : 'Bearer ' + accessToken
        },
        succes: function (response) {
            if (response.HasRegistered) {
                localStorage.setItem('accessToken', accessToken);
                localStorage.setItem('userName', response.Email);
                window.location.href = "Data.html";
            }
            else {
                signupExternalUser(accessToken);
            }
        }

    })
}


function isUserRegistered(accessToken) {
    $.ajax({
        url: '/api/Account/UserInfo',
        method: 'GET',
        headers: {
            'content-type': 'application/JSON',
            'Authorization': 'Bearer ' + accessToken
        },
        succes: function (response) {
            if (response.HasRegistered) {
                localStorage.setItem('accessToken', accessToken);
                localStorage.setItem('userName', response.Email);
                window.location.href = "Data.html";
            }
            else {
                signupExternalUser(accessToken);
            }
        }

    })
}
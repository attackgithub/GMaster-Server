function showNewAddressbookEntryModal(subscriptionId, callback){
    var html = '<div class="msg"></div>' +
        '<div class="row">' +
            '<div class="col field">Email</div>' +
            '<div class="col input">' + 
                '<input type="text" id="entry_email" placeholder="person@gmail.com"/>' + 
            '</div>' +
        '</div>' +
        '<div class="row">' +
            '<div class="col field">First Name</div>' +
            '<div class="col input">' + 
                '<input type="text" id="entry_firstname" placeholder="John"/>' + 
            '</div>' +
        '</div>' +
        '<div class="row">' +
            '<div class="col field">Last Name</div>' +
            '<div class="col input">' + 
                '<input type="text" id="entry_lastname" placeholder="Doe"/>' + 
            '</div>' +
        '</div>';
    showMessage('Create a new Address Book Entry', html, 'confirm', (confirmed) => {
        hideModalMessage();
        if(confirmed !== true){return;}
        var data = {
            subscriptionId:subscriptionId,
            entryemail: $('#entry_email').val(),
            firstname: $('#entry_firstname').val(),
            lastname: $('#entry_lastname').val()
        };

        if(!isValidEmail(data.entryemail)){
            showModalMessage('Invalid email address', 'error');
            return false;
        }

        webApi('Addressbook/Create', data, 
            function(response){
                //add new entry to the top of the list
                console.log(response);
                data.html = response[0].html;
                if(typeof callback == 'function'){callback(data);}            
            },
            function(err){
                //don't execute callback
                //<debug>
                console.log(err);
                //</debug>
                showMessage('Error', err.message, 'error');
            }
        );
    });
}
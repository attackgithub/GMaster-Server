function handleAddressBookPage(view, menu, sub){
    var name = "addressbook";
    loadSubscriptionPage(sub.subscriptionId, name, view, addressBookPageCallback);

    function addressBookPageCallback(subscriptionId){
        //bind all buttons on addressbook page //////////////////////////////////
        $('.subscription-page .btn-addcontact').on('click', () => {
            showNewAddressbookEntryModal(subscriptionId, (response) => {
                $('.addressbook-list tbody').prepend(response.html);
             });
        });

        $('.addressbook-list tr').on('click', (e) => {
            var id = $(e.target).attr('data-id');
            if (e.target.tagName != 'TR') {
                id = $(e.target).parents('tr').first().attr('data-id');
            }
            
            console.log('id = ' + id);
            editAddressbookEntry(subscriptionId, id, () => { });
        });
    }
}
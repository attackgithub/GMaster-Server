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

        $('.addressbook-list tr').on('click', (e) => {handleAddressbookEntry(subscriptionId, e);});
    }
}
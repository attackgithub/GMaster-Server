function handleAddressBookPage(view, menu, sub){
    var name = "addressbook";
    loadSubscriptionPage(sub.subscriptionId, name, view, () => { addressBookPageCallback(sub.subscriptionId, view); });
}

function addressBookPageCallback(subscriptionId, view) {
    //bind all buttons on addressbook page //////////////////////////////////
    $('.subscription-page .btn-addcontact').on('click', () => {
        showNewAddressbookEntryModal(subscriptionId, view, (response) => {
            $('.addressbook-list tbody').prepend(response.html);
        });
    });

    $('.addressbook-list tr').on('click', (e) => { handleAddressbookEntry(subscriptionId, view, e); });
}
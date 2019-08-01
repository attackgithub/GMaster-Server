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
    showMessage('Create a new Address Book Entry', html, 'save', (confirmed) => {
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
                if (typeof callback == 'function') { callback(data); }   
                $('.subscription-page tr.is-new')
                $('.subscription-page tr.is-new').on('click', (e) => { handleAddressbookEntry(subscriptionId, e); });
                setTimeout(() => { $('.subscription-page tr.is-new').removeClass('is-new'); }, 5000);
            },
            function(err){
                //don't execute callback
                //<debug>
                console.log(err);
                //</debug>
                showModalMessage(err.responseText, 'error');
            }
        );
    });
}

function editAddressbookEntry(subscriptionId, addressId, callback) {
    var data = {
        subscriptionId: subscriptionId,
        addressId: addressId
    };
    webApi('Addressbook/Entry', data,
        function (response) {
            var entry = response[0];

            function createDeleteButton(id) {
                return '<div class="delete-button hover-only">' +
                    '<div class="button outline btn-delete fill font-icon" data-id="' + id + '" title="delete custom field"><b>-</b></div></div>';
            }

            var html = '<div class="msg"></div>' +
                '<div class="max-height dentry-details">' + 
                //basic fields
                '<div class="row hover longer expand">' +
                    '<div class="row inner">' +
                    '<div class="col field">Email</div>' +
                    '<div class="col input">' +
                        '<input type="text" id="entry_email" value="' + entry.email + '" placeholder="person@gmail.com"/>' +
                    '</div>' +
                    '</div>' +
                '</div>' +
                '<div class="row hover longer expand">' +
                    '<div class="row inner">' +
                    '<div class="col field">First Name</div>' +
                    '<div class="col input">' +
                        '<input type="text" id="entry_firstname" value="' + entry.firstname + '" placeholder="John"/>' +
                    '</div>' +
                    '</div>' +
                '</div>' +
                '<div class="row hover longer expand">' +
                    '<div class="row inner">' +
                    '<div class="col field">Last Name</div>' +
                    '<div class="col input">' +
                        '<input type="text" id="entry_lastname" value="' + entry.lastname + '" placeholder="Doe"/>' +
                    '</div>' +
                    '</div>' +
                '</div>' + 
                '<div class="custom-fields">' +
                //custom fields
                ((entry.fields != null && entry.fields.length > 0) ?
                entry.fields.map((field) => {
                    return '' +
                        '<div class="row hover longer expand">' +
                        createDeleteButton(field.fieldId) +
                        '<div class="row inner">' +
                            '<div class="col field">' + field.label + '</div>' +
                            '<div class="col input text-left">' +
                                (
                                field.datatype == 0 ? '<input type="text" class="entry-field" data-id="' + field.fieldId + '" value="' + (field.value || '') + '"/>' :
                                field.datatype == 1 ? '<input type="number" class="entry-field" data-id="' + field.fieldId + '" value="' + (field.value || '') + '"/>' :
                                field.datatype == 2 ? '<input type="date" class="entry-field" data-id="' + field.fieldId + '" value="' + (field.value || '') + '"/>' :
                                field.datatype == 3 ? '<input type="checkbox" class="entry-field" data-id="' + field.fieldId + '" value="1" ' + (field.value == 'true' ? 'checked="checked"' : '') + '/>' : ''
                                ) +
                            '</div>' +
                            '</div>' +
                        '</div>';
                }).join('')
                    : '') +
                '</div>' +
                '</div>';

            showMessage('Edit Address Book Entry', html, 'save',
                (confirmed) => {
                    //save changes to address book entry
                    if (confirmed == true) {
                        var data = {
                            subscriptionId: subscriptionId,
                            addressId: addressId,
                            entryemail: $('#entry_email').val(),
                            firstname: $('#entry_firstname').val(),
                            lastname: $('#entry_lastname').val(),
                            customids: '',
                            customvalues: '',
                            customtypes: '',
                            newfields: '',
                            newvalues: '',
                            newtypes: ''
                        };

                        var customids = $('input.entry-field');
                        var customchanges = [];

                        if (customids.length > 0) {
                            //get list of fields that have changed
                            customchanges = customids.map((i, a) => {
                                var fid = parseInt($(a).attr('data-id'));
                                var field = entry.fields.filter(f => f.fieldId == fid);
                                if (field.length > 0) {
                                    field = field[0];
                                    var val = '';
                                    switch (a.getAttribute('type')) {
                                        case 'text':
                                            val = $(a).val();
                                            break;
                                        case 'number':
                                            val = parseInt($(a).val()) || null;
                                            break;
                                        case 'date':
                                            var date = new Date($(a).val());
                                            val = date.getFullYear() + '-' +
                                                ('0' + (date.getMonth() + 1)).slice(-2) + '-' +
                                                ('0' + (date.getDate() + 1)).slice(-2);
                                            break;
                                        case 'checkbox':
                                            val = a.checked ? 'true' : 'false';
                                            break;
                                    }
                                    if (val != field.value) {
                                        return true;
                                    }
                                }
                                return false;
                            });

                            var ids = [];
                            var vals = [];
                            var types = [];
                            for (var x = 0; x < customids.length; x++) {
                                if (customchanges[x] == false) { continue; }
                                var el = $(customids[x]);
                                ids.push(el.attr('data-id'));
                                switch (el.attr('type')) {
                                    case 'text':
                                        vals.push(el.val());
                                        types.push(0);
                                        break;
                                    case 'number':
                                        vals.push(el.val());
                                        types.push(1);
                                        break;
                                    case 'date':
                                        vals.push(el.val());
                                        types.push(2);
                                        break;
                                    case 'checkbox':
                                        vals.push(el[0].checked == true ? '1' : '0');
                                        types.push(3);
                                        break;
                                }
                            }
                            data.customids = ids.join(',');
                            data.customvalues = vals.join('|&|');
                            data.customtypes = types.join(',');
                        }
                        
                        var newfields = $('.new-field-name');
                        var newvalues = $('.new-field-value');

                        if (newfields.length > 0) {
                            data.newfields = newfields.map((i, a) => $(a).val()).join(',');
                            data.newvalues = newvalues.map((i, a) => {
                                //get value based on data type
                                switch (a.getAttribute('type')) {
                                    case 'text': case 'number': case 'date':
                                        return $(a).val();
                                    case 'checkbox':
                                        return a.checked == true ? '1' : '0';
                                }
                                return '';

                            }).join(',');
                            data.newtypes = newvalues.map((i, a) => {
                                //get value based on data type
                                switch (a.getAttribute('type')) {
                                    case 'text': return 0;
                                    case 'number': return 1;
                                    case 'date': return 2;
                                    case 'checkbox': return 3;
                                }
                                return 0;

                            }).join(',');
                        }

                        webApi('Addressbook/Update', data,
                            function (response) {
                                editAddressbookEntry(subscriptionId, addressId, () => {
                                    showModalMessage('Address Book Contact successfully updated');
                                });
                            },

                            function (err) {
                                showModalMessage(err.responseText, 'error');
                            }
                        );
                    }
                    return false;
                },
                '<a href="javascript:" class="button btn-special btn-new-field"><b>+</b> New Field</a>' +
                '<a href="javascript:" class="button btn-cancel btn-delete-entry faded"><b>x</b> Delete Contact</a>' +
                '<br/><br/>',
            'prepend');

            //bind element events within modal
            $('.gmaster-content .modal .btn-new-field').on('click', () => {
                //generate a new field within the addressbook entry
                $('.gmaster-content .modal .custom-fields').append('' + 
                    '<div class="row hover longer expand new-field">' +
                        createDeleteButton('new') +
                        '<div class="row inner">' +
                        '<div class="col field input">' +
                            '<input type="text" class="new-field-name" placeholder="New Field Name" />' +
                        '</div> ' +
                        '<div class="col input">' + 
                            '<div class="row input-type">' +
                                '<div class="col input input-element type-text text-left">' +
                                    '<input type="text" class="new-field-value"/>' + 
                                '</div>' +
                                '<div class="col input text-right"><div class="pad-top-sm">' +
                                    '<select class="select-data-type">' + 
                                        '<option value="0">Text</option>' +
                                        '<option value="1">Number</option>' +
                                        '<option value="2">Date</option>' +
                                        '<option value="3">Yes/No</option>' +
                                '</div></div>' +
                            '</div>' +
                        '</div>' +
                        '</div>' +
                    '</div>'
                );

                $('.gmaster-content .modal .custom-fields .select-data-type').last().on('change', (e) => {
                    //change new field's data type
                    var datatype = parseInt($(e.target).val());
                    var newfield = $(e.target).parents('.new-field').first();
                    var el = newfield.find('.input-element');
                    //get value from existing field
                    var oldtype = el.hasClass('type-text') ? 0 :
                        el.hasClass('type-number') ? 1 :
                        el.hasClass('type-date') ? 2 :
                                el.hasClass('type-bit') ? 3 : null;
                    var field = newfield.find('.new-field-value');
                    var val = field.val();
                    if (oldtype == 3) { val = field[0].checked === true ? 1 : 0; }
                    //change field
                    switch (datatype) {
                        case 0: //text
                            el.html('<input type="text" class="new-field-value" value="' + val + '"/>');
                            break;
                        case 1: //number
                            el.html('<input type="number" class="new-field-value" value="' + (parseInt(val) || 0) + '"/>');
                            break;
                        case 2: //date
                            var date;
                            try {
                                date = new Date(val);
                            } catch (ex) { date = new Date();}
                            el.html('<input type="date" class="new-field-value" value="' + date.toLocaleDateString('en-US') + '"/>');
                            break;
                        case 3: //boolean
                            el.html('<input type="checkbox" class="new-field-value" value="1" ' +
                                (val == 1 || val == '1' ? 'checked="checked"' : '') + '/>');
                            break;
                    }
                });
            });

            $('.gmaster-content .modal .btn-delete-entry').on('click', () => {
                //delete addressbook entry
            });

            //finally, execute callback
            if (typeof callback == 'function') {
                callback();
            }

        },
        function (err) {
            //don't execute callback
            //<debug>
            console.log(err);
            //</debug>
            showMessage('Error', err.responseText, 'error');
        }
    );
    
}

function handleAddressbookEntry(subscriptionId, e) {
    var id = $(e.target).attr('data-id');
    if (e.target.tagName != 'TR') {
        id = $(e.target).parents('tr').first().attr('data-id');
    }
    editAddressbookEntry(subscriptionId, id, () => { });
}
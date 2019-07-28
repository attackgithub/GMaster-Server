function showModal(content, width, height){
    $('.gmaster-content').find('.modal, .modal-bg').remove();
    $('.gmaster-content').append('<div class="modal-bg"></div><div class="modal"></div>');
    var modal = $('.gmaster-content .modal');
    modal.append(content);
    var win = {w: window.innerWidth, h: window.innerHeight};
    modal.css({
        width:width, height:height, 
        left: (win.w / 2) - (width / 2), top: (win.h / 2) - (height - 2) 
    });
    $('.gmaster-content .modal-bg').on('click', hideModal);
}

function hideModal(){
    $('.gmaster-content').find('.modal-bg, .modal').remove();
}

function showModalMessage(message, type){
    var msg = $('.modal .msg');
    msg.removeClass('error');
    if(type != null && type != ''){
        msg.addClass(type);
    }
    msg.html(message);
    msg.show();
}

function hideModalMessage(){
    $('.modal .msg').hide();
}
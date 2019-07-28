function toCurrency(num){
    var txt = num.toFixed(2).toString();
    if(txt.indexOf('-') == 0){
        return '-$' + txt.replace('-','');
    }else{
        return '$' + txt;
    }
}
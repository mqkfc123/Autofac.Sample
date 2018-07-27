//选择对话框
/*title:标题名称,content：提示内容,canclename：取消名称,surename：确定名称,surefun：确定后实现功能*/
function dialog_confirm(content, title, canclename, surename, surefun, canclefun) {
    if (!surename) {
        surename = '确定';
    }
    if (!canclename) {
        canclename = '取消';
    }
    var d = dialog({
        title: title,
        fixed: true,
        boxSkin: 'ui-popup-full',
        skin: 'ax-popup',
        content: content,
        button: [
            {
                value: canclename,
                callback: canclefun || function () { d.close().remove();}
            },
            {
                value: surename,
                callback: surefun || function () { }
            }
        ]
    });
    d.showModal();
}

//普通对话框
/*content：提示内容,title:标题名称,surename：确定名称,surefun：确定后实现功能*/
/*如果只有内容,之后的三项可不传*/
function dialog_alert(content, title, surename, surefun) {
    if (!surename) {
        surename = '确定';
    }
    var d = dialog({
        title: title, //带前置图标的标题
        fixed: true,
        boxSkin: 'ui-popup-full', // 增加了一个官方没有的属性boxSkin,参数为Class名称,用于控制整体弹出层的样式。
        content: content,
        ok: surefun || function () { },
        okValue: surename
    });
    d.showModal();
}

//加载对话框
/*content：提示内容*/
function dialog_loading(content) {
    var d = dialog({
        //skin:'bk-popup',//黑色皮肤,有问题
        contentType: 'jsonload',
        content: content,
        closeTime: 2000,
        cancel: false,
    }).showModal();
    return d;
}

//加载对话框关闭
function dialog_loadingClose(e) {
    e.close().remove();
}
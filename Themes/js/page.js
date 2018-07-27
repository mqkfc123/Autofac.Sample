(function ($) {
    //分页
    $.addFlexPage = function (t, p) {
        if (t.pages) return false;   // 若已经存在对象则return
        p = $.extend({
            total: 0,     //总数  
            index: 1,     //  
            size: 10,
            callback: null   //回调函数
        }, p);
        $(t).show();//show if hidden
        var g = {
            initSource: function () {
                g.pageLoad(p.index, p.total);
            },
            //获取当前页码
            pageNum: function () {
                var indexCurrent = $(t).find("a.pageOn").text();
                
                return indexCurrent;
            },
            pageEvent: function () {
                $(t).find('a').on('click', function () {
                    var indexCurrent = 1;
                    if ($(this).hasClass("page")) {
                        indexCurrent = parseInt($(this).text());
                    }
                    else if ($(this).hasClass("prev")) {
                        indexCurrent = parseInt(g.pageNum()) == 1 ? 1 : parseInt(g.pageNum()) - 1;
                    }
                    else if ($(this).hasClass("next")) {
                        indexCurrent = parseInt(g.pageNum()) == (Math.ceil(parseInt(p.total) / parseInt(p.size))) ? parseInt(g.pageNum()) : parseInt(g.pageNum()) + 1;
                    }
                    else if ($(this).hasClass("pageOn")) {
                        indexCurrent = parseInt($(this).text());
                    }
                    else if ($(this).hasClass("sure")) {
                        var pageNum = $(t).find("input.txt-page").val();
                        if (parseInt(pageNum) > Math.ceil(parseInt(p.total) / parseInt(p.size))) {
                            dialog({ contentType: 'tipsbox', skin: 'bk-popup', content: '已超过最大页码', closeTime: 2000 }).show();
                            return;
                        } else if (pageNum < 1) {
                            dialog({ contentType: 'tipsbox', skin: 'bk-popup', content: '已超过最小页码', closeTime: 2000 }).show();
                            return;
                        } else {
                            indexCurrent = parseInt(pageNum);
                        }
                    }
                    //设置分页
                    g.pageLoad(indexCurrent);
                    //回调
                    if (p.callback) {
                        p.callback({ index: indexCurrent });
                    }
                });
            },
            pageLoad: function (indexCurrent) {
                $(t).empty();
                var page = 1;
                /*分页*/
                page = Math.ceil(parseInt(p.total) / parseInt(p.size));

                $(t).append($('<a href="javascript:void(0);" class="prev"></a>'));
                if (page > 10) {
                    var index_p = 1;
                    for (var i = 1; i <= page; i++) {
                        if (indexCurrent < 5) {
                            index_p = i;   //1 2 3 4 5
                            if (index_p <= 5) {
                                $(t).append($('<a href="javascript:void(0);" class="' + (index_p == indexCurrent ? "pageOn" : "page") + '">' + index_p + '</a>'));
                            }
                            else if (index_p > 5 && index_p < page) {
                                continue;
                            }
                            else {
                                $(t).append($('<span></span>'));
                                $(t).append($('<a href="javascript:void(0);" class="page">' + index_p + '</a>'));
                            }
                        }
                        else if (indexCurrent >= 5 && indexCurrent <= (page - 4)) { //前后各显示一次“...”
                            index_p = i;
                            if (index_p == 1) {
                                $(t).append($('<a href="javascript:void(0);" class="' + (i == indexCurrent ? "pageOn" : "page") + '">' + i + '</a>'));
                                $(t).append($('<span></span>'));
                            }
                            if (index_p == indexCurrent || index_p == indexCurrent || index_p == (indexCurrent + 1)) {
                                $(t).append($('<a href="javascript:void(0);" class="' + (index_p == indexCurrent ? "pageOn" : "page") + '">' + index_p + '</a>'));
                            }
                            if (index_p == page) {
                                $(t).append($('<span></span>'));
                                $(t).append($('<a href="javascript:void(0);" class="page">' + index_p + '</a>'));
                            }
                        }
                        else {
                            index_p = i;
                            if (index_p == 1) {
                                $(t).append($('<a href="javascript:void(0);" class="' + (index_p == indexCurrent ? "pageOn" : "page") + '">' + index_p + '</a>'));
                                $(t).append($('<span></span>'));
                            }
                            else if (index_p > 1 && index_p < page - 4) {
                                continue;
                            }
                            else {
                                $(t).append($('<a href="javascript:void(0);" class="' + (index_p == indexCurrent ? "pageOn" : "page") + '">' + index_p + '</a>'));
                            }
                        }
                    }
                }
                else {
                    for (var i = 1; i <= page; i++) {
                        $(t).append($('<a href="javascript:void(0);" class="' + (i == indexCurrent ? "pageOn" : "page") + '">' + i + '</a>'));
                    }
                }

                $(t).append($('<a href="javascript:void(0);" class="next"></a>'));
                $(t).append($('<input type="text" class="txt-page" />'));
                $(t).append($('<a href="javascript:void(0);" class="sure"></a>'));

                g.pageEvent();
            },
            refresh: function (param) {
                p.total = param.total;
                p.index = param.index;
                p.size = param.size;
                g.pageLoad(param.index);
            }
        }
        g.initSource(); 
        t.pages = g;
        return g;
    };

    var docloaded = false; //文档加载完毕标识
    $(document).ready(function () { docloaded = true });
    $.fn.flexPage = function (p) {
        return $.addFlexPage(this, p);
        //return this.each(function () {
        //    if (!docloaded) {
        //        $(this).hide();
        //        var t = this;
        //        //$(document).ready(function () { return $.addFlexPage(t, p); });
        //        $(document).ready(function () { return $.addFlexPage(t, p); });
        //    } else {
        //        return $.addFlexPage(this, p);
        //    }
        //});
    }

})(jQuery);
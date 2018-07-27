
(function ($, win, doc) {
    var CityPicker = function (el, options) {
        this.el = $(el);
        this.options = options;
        this.provinces = provinces;
        this.pro = null;
        this.city = null;
        this.elType = this.el.is('input');

        this.init();
    };

    var p = CityPicker.prototype;

    p.init = function () {
        this.initEvent();
        this.preventPopKeyboard();

    };

    p.preventPopKeyboard = function () {
        if (this.elType) {
            this.el.prop("readonly", true);
        }
    };

    p.initEvent = function () {
        this.el.on("click", function (e) {
            var pickerBox = $(".picker-box");
            if (pickerBox[0]) {
                pickerBox.show();
            } else {
                this.create();
            }
        }.bind(this));
    };

    p.create = function () {
        this.createCityPickerBox();
        this.createProList();
        this.proClick();
        this.createNavBar();
        this.navEvent();
    };

    p.createCityPickerBox = function () {
        var proBox = "<div class='picker-box'></div>";
        $("body").append(proBox);
    };

    p.createProList = function () {
        var provinces = this.provinces;
        var proBox;
        var dl = "";
        for (var letterKey in provinces) { 
            var val = provinces[letterKey];
            if (provinces.hasOwnProperty(letterKey) && val.length > 0) {
                var dt = "<dt id='" + letterKey + "'>" + letterKey + "</dt>";
                var dd = ""; 
                for (var proKey in val) {
                    if (val.hasOwnProperty(proKey)) {
                        dd += "<dd data-letter=" + val[proKey]['c'] + ">" + val[proKey]['n'] + "</dd>";
                    }
                }
                dl += "<dl>" + dt + dd + "</dl>";
            }
        }
        proBox = "<section class='pro-picker'>" + dl + "</section>";
        $(".picker-box").append(proBox);
    };

    p.createCityList = function (letter, pro) {
        //写入cookie
        //var obj = new Object();
        //obj.cityId = letter;
        //obj.cityName = pro; 
        //$.cookie('LBS_Address', JSON.stringify(obj), {
        //    expires: 365,
        //    path: '/',
        //    domain: 'zxsj123.com'
        //});
        //location.href = "/Question/QuesDefault";
        //this.cityClick();
        setAddress(letter, pro, "", "");
    };

    p.proClick = function () {
        var that = this;
        $(".pro-picker").on("click", function (e) {
            var target = e.target;
            if ($(target).is("dd")) {
                that.pro = $(target).html();
                var letter = $(target).data("letter");
                that.createCityList(letter, that.pro); 
                //$(this).hide();
            }
        });
    };

    p.cityClick = function () {
        var that = this;
        $(".city-picker").on("click", function (e) {
            var target = e.target;
            if ($(target).is("li")) {
                that.city = $(target).html();
                if (that.elType) {
                    that.el.val(that.pro + "-" + that.city);
                } else {
                    that.el.html(that.pro + "-" + that.city);
                }

                $(".picker-box").hide();
                $(".pro-picker").show();
                $(this).hide();
            }
        });
    };

    p.createNavBar = function () {
        var str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var arr = str.split("");
        var a = "";
        arr.forEach(function (item, i) {
            a += '<a href="#' + item + '">' + item + '</a>';
        });

        var div = '<div class="navbar">' + a + '</div>';

        $(".picker-box").append(div);
    };

    p.navEvent = function () {
        var that = this;
        var navBar = $(".navbar");
        var width = navBar.find("a").width();
        var height = navBar.find("a").height();
        navBar.on("touchstart", function (e) {
            $(this).addClass("active");
            that.createLetterPrompt($(e.target).html());
        });

        navBar.on("touchmove", function (e) {
            e.preventDefault();
            var touch = e.originalEvent.touches[0];
            var pos = { "x": touch.pageX, "y": touch.pageY };
            var x = pos.x, y = pos.y;
            $(this).find("a").each(function (i, item) {
                var offset = $(item).offset();
                var left = offset.left, top = offset.top;
                if (x > left && x < (left + width) && y > top && y < (top + height)) {
                    location.href = item.href;
                    that.changeLetter($(item).html());
                }
            });
        });

        navBar.on("touchend", function () {
            $(this).removeClass("active");
            $(".prompt").hide();
        })
    };

    p.createLetterPrompt = function (letter) {
        var prompt = $(".prompt");
        if (prompt[0]) {
            prompt.show();
        } else {
            var span = "<span class='prompt'>" + letter + "</span>";
            $(".picker-box").append(span);
        }
    };


    p.changeLetter = function (letter) {
        var prompt = $(".prompt");
        prompt.html(letter);
    };

    $.fn.CityPicker = function (options) {
        return new CityPicker(this, options);
    }
}(window.jQuery, window, document));

(function ($) {
    $.fn.upload = function (config) {
        var modalHtml = '<div class="modal fade div-cropper" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">'
		            + '  <div class="modal-dialog" style="width:1000px; ">'
                    + '  <div class="modal-content">'
                    + '    <div class="modal-header">'
                    + '    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>'
                    + '    <div class="input-group" style="width:260px;">'
                    + '    <span class="input-group-btn">'
                    + '      <span id="btnUpload" class="btnUpload"></span> '
                    + '    </span>'
                    + '  </div>'
                    + '  </div>'
                    + '  <div class="modal-body">'
                    //+ '       <div class="progress" style="margin-bottom:2px;width:650px; height:3px;">'
                    //+ '           <div class="progress-bar" style="width: 0%; line-height:3px;"></div>'
                    //+ '       </div>'
                    + ' </div>'
                    + ' <div class="modal-footer">'
                    + ' <button class="btn-primary btn btnSubmitCropper"  name="保存" type="button">保存</button>'
                    + ' </div>'
                    + ' </div>'
                    + ' </div>'
                    + ' </div>';
         
     
        var img_modal = '<div class="img_container" style="width:100%; height:400px;background-color:#f7f7f7;border:1px solid #eee;overflow:hidden;margin: 0 auto;  position:relative;">'
                    + '    <img class="imgs" src="" alt="pic" style="margin:auto;position: absolute; width:100%;height:100%;display:inline-block;">'
                    + '  </div>';

        var cropper = '<div style="width:100%; height:400px;">'
                    + '  <div class="eg-wrapper img_container" style="width:650px; height:400px; float:left; ">'
                                //@*<img class="cropper cropper-hidden" src="" alt="Picture">*@
                    + '   </div>'
                    + '   <div class="col-xs-12 col-sm-3">'
                    + '       <div class="eg-preview clearfix">'
                    + '           <div class="preview preview-lg"></div>'
                    + '       </div>'
                    + '       <span>上传小于2M的图片</span>'
                    + '   </div>'
                    + '</div>';

        var container = $(this);
        var modal = $(modalHtml);

        container.append(modal);
        var config = $.extend(
         {
             maxCount: 0,
             maxWidth: 640,
             maxHeight: 640
         }, config);

        var image;
         
        if (config.croDefault) {
            $(modal).find("div.modal-body").append(cropper);
            var preview_h = parseInt(config.croHeight) * 0.3;
            var preview_w = parseInt(config.croWidth) * 0.3;
            $(modal).find("div.preview-lg").height(preview_h + "px");
            $(modal).find("div.preview-lg").width(preview_w + "px");
            //裁剪器注册
            image = this.cropperControl(config, container);
        }
        else {
            $(modal).find("div.modal-body").append(img_modal);
            $(modal).find("div.modal-dialog").width("690px");
        }
        var uploadController = new UploadController(modal, config, image);
        uploadController.image = image;
        return uploadController;
    };

    //实例话
    $.fn.uploadControl = function (settings) {
        settings = $.extend({
            uploadSettings: {
            }
        }, settings);

        var buttonElement = settings.button;
        if (buttonElement == null) {
            throw "请指定用于选择的按钮元素。";
        }
        var uploadController = this.upload(settings.uploadSettings);
        var controller = new UploadControlController(uploadController, buttonElement, uploadController.image, settings);
        return controller;
    };

    //裁剪器初始化
    $.fn.cropperControl = function (settings, container) {
        var imgDefault = settings.imgDefault;
        $(container).find(".eg-wrapper").html("<img class=\"cropper\" src='" + imgDefault + "'/>");
        var $image = $(container).find(".cropper");
        var $dataX = $("#dataX");
        var $dataY = $("#dataY");
        var $dataHeight = $("#dataHeight");
        var $dataWidth = $("#dataWidth");
        var console = window.console || { log: $.noop };

        var croHeight = settings.croHeight
        var croWidth = settings.croWidth
        $image.cropper({
            aspectRatio: croWidth / croHeight,
            autoCropArea: 1,
            data: {
                x: 420,
                y: 50,
                width: croWidth,
                height: croHeight
            },
            preview: $(container).find(".preview"),
            zoomable: true,  //禁用图片缩放 
            done: function (data) {
                $dataX.val(data.x);
                $dataY.val(data.y);
                $dataHeight.val(data.height);
                $dataWidth.val(data.width);
            },
            build: function (e) {
                //console.log(e.type);
            },
            built: function (e) {
                //console.log(e.type);
            },
            dragstart: function (e) {
                //console.log(e.type);
            },
            dragmove: function (e) {
                //console.log(e.type);
            },
            dragend: function (e) {
                //console.log(e.type);
            }
        });
        var cropper = $image.data("cropper");
        $image.on({});
        return $image;
    };

    function UploadController(container, config, image) {
        container = $(container);
        container.modal({
            backdrop: "static",
            show: false
        });

        function bindSwfUpload(settings) {
            var buttonAction = (settings.maxCount <= 0 || settings.maxCount) > 1 ? SWFUpload.BUTTON_ACTION.SELECT_FILES : SWFUpload.BUTTON_ACTION.SELECT_FILE;
            return new SWFUpload($.extend({
                //upload_url: (window.ApplicationPath || "/") + "/Upload/UploadImage",
                upload_url: (window.ApplicationPath || "/") + "Upload/UploadImage",
                file_size_limit: 2048,
                file_types: "*.jpg;*.gif;*.png",
                file_types_description: "JPEG Image",
                file_upload_limit: 0,
                file_queue_limit: 0,
                //button_image_url: (window.ApplicationPath || "/") + "Themes/TheAdmin/lib/swfupload/images/uplodfromDnBtnIcon.png",
                button_image_url: (window.ApplicationPath || "/") + "Scrpits/widget/swfupload/images/uplodfromDnBtnIcon.png",
                button_placeholder_id: "btnUpload",
                button_width: 87,
                button_height: 28,
                button_text_top_padding: 0,
                button_cursor: SWFUpload.CURSOR.HAND,
                button_action: buttonAction,
                post_params: { maxWidth: config.maxWidth, maxHeight: config.maxHeight }
            }, settings));
        }

        var swfUpload = bindSwfUpload($.extend({
            upload_start_handler: function () {
                swfUpload.removePostParam("albumId");
                swfUpload.addPostParam("albumId", 100);
            },
            //上传返回结果
            upload_success_handler: function (file, serverData) {
                try {
                    $(container).find("div.loading").remove();
                    var data = eval("(" + serverData + ")");
                    if (config.croDefault) {
                        //cropper裁剪器 
                        $(image).cropper("reset");
                        $(image).cropper("replace", data.Url);
                    }
                    else {
                        $(container).find("img.imgs").attr("src", data.Url);
                    }
                } catch (ex) {
                    this.debug(ex);
                }
            },
            //异步等待中
            upload_progress_handler: function (file, bytesComplete, bytesTotal) {
                try {
                    if (config.croDefault) {
                        //cropper裁剪器 
                        $(image).cropper("reset");
                        $(image).cropper("replace", "");
                        $(container).find(".cropper-container").remove();
                    }
                    var percentage = Math.round((bytesComplete / bytesTotal) * 100);
                    //console.log("当前进度：" + percentage);
                    //$(container).find("div.progress-bar").width(percentage + "%");
                    if ($(container).find("div.loading").length<=0) {
                        var loading = '<div class="loading" style="position:absolute;z-index:9999;width:650px;height:400px;background:#000000;opacity:0.4;text-align:center;">';
                        loading += '        <img style="width:40px;height:40px;margin-top:180px;" src="/Scrpits/widget/swfupload/images/loadingImg.gif" />';
                        loading += '   </div>'; 
                        $(container).find("div.img_container").append(loading);
                    }
                } catch (ex) {
                    this.debug(ex);
                }
            },
            //选择图片后加载
            file_dialog_complete_handler: function (numFilesSelected, numFilesQueued) {
                try {
                    if (numFilesQueued > 0) {
                        //开始上传， 调用swfupload
                        this.startUpload();
                    }
                } catch (ex) {
                    this.debug(ex);
                }
            }
        }, config));

        this.succes = function (fn) {
            $(this).bind("succes", fn);
        };
        this.show = function () {
            container.modal("show");
        }
        this.close = function () {
            container.modal("hide");
        }
        this.toggle = function () {
            container.modal("toggle");
        }
        this.container = container;
    }

    function UploadControlController(uploadController, buttonElement, image, settings) {
        var button = $(buttonElement);
        var createItem = function (item) {
            if (settings.callback) {
                settings.callback(item.url);
            }
            else {
                alert("不存在回调函数callback");
            }
        };
        this.addImage = function (array) {
            if (array == null) {
                return;
            }
            if (!(array instanceof (Array))) {
                array = new Array(array);
            }
            $.each(array, function () {
                if (settings.callback) {
                    createItem(this);
                }
                else {
                    alert("不存在回调函数callback");
                }
            });
        };
        var that = this;
        $(uploadController.container).find("button.btnSubmitCropper").on("click", function () {

            //验证照片是否超出maxCount
            if (settings.selectedImgCount) {
                var currentCout = settings.selectedImgCount();
                if (currentCout >= settings.uploadSettings.maxCount) {
                    dialog({ contentType: 'tipsbox', skin: 'bk-popup', content: '图片超出上限(' + currentCout + ")张", closeTime: 2000 }).show();
                    return false;
                }
            }
            if (!settings.uploadSettings.croDefault) {
                var item = new Object();
                item.path = imgPath;
                item.url = imgPath;
                item.path = $(uploadController.container).find("img.imgs").attr("src");
                item.url = $(uploadController.container).find("img.imgs").attr("src");

                if ($(uploadController.container).find("img.imgs").attr("src")) {
                    that.addImage(item);
                }
                uploadController.close();
            }
            else {
                var dataURL = $(image).cropper("getDataURL");

                var imgPath = $(uploadController.container).find("img.cropper").attr("src");
                var d;
                $.ajax({
                    url: "/Upload/UploadCropperImage",
                    type: "post",
                    async: false,
                    data: {
                        t: new Date().getDate(),
                        Filedata: dataURL,
                        path: imgPath,
                        processData: false,
                        contentType: false,
                        croHeight: settings.uploadSettings.croHeight,
                        croWidth: settings.uploadSettings.croWidth
                    },
                    beforeSend: function () {
                        d = dialog_loading();
                    },
                    error: function (e) {
                        dialog_loadingClose(d);
                        dialog({ contentType: 'tipsbox', skin: 'bk-popup', content: '服务器繁忙', closeTime: 2000 }).show();
                    },
                    success: function (res) { //success
                        dialog_loadingClose(d);
                        var item = new Object();
                        item.path = res.Url;
                        item.url = res.Url;
                        that.addImage(item);
                        uploadController.close();
                    }
                });
            }
        });

        (function () {
            button.click(function (e) {
                uploadController.show();
            });
            uploadController.succes(function (e, list) {
                alert("uploadController");
            });
        })();
    }

})(jQuery);
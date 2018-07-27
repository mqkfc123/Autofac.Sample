   
(function ($) {
    $.fn.upload = function (config, image) {
        var modalHtml = '<div class="modal fade div-cropper" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">'
		            + '  <div class="modal-dialog" style="width:1000px; ">'
                    + '  <div class="modal-content">'
                    + '    <div class="modal-header">'
                    + '    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>'
                    + '    <div class="input-group" style="width: 260px;">'
                    + '   <span class="input-group-btn">'
                    + '      <span id="btnUpload"></span> '
                    + '   </span>'
                    + '  </div>'
                    + '  </div>'
                    + '  <div class="modal-body">'
                    + '   <div style="width:100%; height:450px;">'
                    + '   <div class="eg-wrapper" style="width:650px; height:450px; float:left; ">'
                                //@*<img class="cropper cropper-hidden" src="" alt="Picture">*@
                    + '   </div>'
                    + '   <div class="col-xs-12 col-sm-3">'
                    + '       <div class="eg-preview clearfix">'
                    + '            <div class="preview preview-lg"></div>'
                    + '            <span>上传小于2M的图片</span>'
                    + '       </div>'
                    + '   </div> '
                    + ' </div>'
                    + ' </div>'
                    + ' <div class="modal-footer">'
                    + ' <button class="btn-primary btn btnSubmitCropper"  name="保存" type="button">保存</button>'
                    + ' </div>'
                    + ' </div>'
                    + ' </div>'
                    + ' </div>';

        var container = $(this);
        var modal = $(modalHtml);
        container.append(modal);
        config = $.extend(
        {
            maxCount: 0,
            maxWidth: 640,
            maxHeight: 640,
            uploadSettings: {
            }
        }, config);
         
        //裁剪器注册
        var image = this.cropperControl(config, container);
        var uploadController = new UploadController(modal, config, image);
        uploadController.image = image;
        return uploadController;
    };
	$.fn.uploadControl = function (settings) {
		settings = $.extend({
			uploadSettings: {
			}
		}, settings);

		var buttonElement = settings.button;
		if (buttonElement == null) {
			throw "请指定用于选择的按钮元素。";
		}
		var inputElement = settings.input;
		if (inputElement == null) {
			throw "请指定用于持久化值的控件元素。";
		}
		var viewImageListElement = settings.imageList;
		if (viewImageListElement == null) {
			throw "请指定用于显示的图片列表元素。";
		}
		var oneImage = settings.oneImage;
         
		var uploadController = this.upload(settings.uploadSettings);
		var controller = new UploadControlController(uploadController, buttonElement, inputElement, viewImageListElement, oneImage, uploadController.image, settings);
		return controller;
	};

    //裁剪器初始化
	$.fn.cropperControl = function (settings, container) {
	    $(container).find(".eg-wrapper").html("<img class=\"cropper\" />");
	    var $image = $(".cropper");
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
	        preview: ".preview", 
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
	    var imageList = container.find("#imageList");

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

	    //验证照片是否超出maxCount
	    function isAllowSelect() {
	        var currentCout = getSelectedList().length;
	        return config.maxCount <= 0 || currentCout < config.maxCount;
	    }

	    var swfUpload = bindSwfUpload($.extend({
	        upload_start_handler: function () {
	            swfUpload.removePostParam("albumId");
	            swfUpload.addPostParam("albumId", 100);
	        },
	        //上传返回结果
	        upload_success_handler: function (file, serverData) {
	            try {
	                var data = eval("(" + serverData + ")"); 
	                //cropper裁剪器 
	                $(image).cropper("reset");
	                $(image).cropper("replace", data.Url);
	                //loading 样式移除
	                //imageList.find(".uploading").parents("li:first").replaceWith(li);
	            } catch (ex) {
	                this.debug(ex);
	            }
	        },
	        //异步等待中
	        upload_progress_handler: function () {
	            try {
	                //var uploading;
	                //if ($(".uploading").length > 0) {
	                //	uploading = $(".uploading");
	                //} else {
	                //	uploading = $(".waiting:last").removeClass("waiting").addClass("uploading");
	                //}
	                //uploading.next().removeClass("grey").addClass("green");
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

	function UploadControlController(uploadController, buttonElement, inputElement, viewImageListElement, oneImage, image, config) {
		var button = $(buttonElement);
		var input = $(inputElement);
		var imageList = $(viewImageListElement);

		function syncValue() {
			var value = '';
			var path = "";
			var url = "";
			$.each(imageList.find("li"), function () {
				var li = $(this);
				var result = li.data("path");
				if (result) {
					value += result + ',';
					path += li.data("path") + ",";
					url += li.data("url") + ",";
				}
			});
			value = value.replace(/,$/img, "");
			path = path.replace(/,$/img, "");
			url = url.replace(/,$/img, "");

			input.val(value);
			input.attr("data-uploadControlController-path", path);
			input.attr("data-uploadControlController-url", url);

			input.trigger("uploadControlControllerChange");
			input.change();
		}

		this.addImage = function (array) {
		    if (array == null) {
		        return;
		    }
		    var createItem = function (item) {
		        var li;
		        if (oneImage) {
		            $(imageList).attr("src", item.url);
		        }
		        else if (config.callback) {
		            config.callback(item.url);
		        }
		        else {
		            li = $("<li />", { style: "border: solid 1px #ccc; width: 80px; height: 80px;" });
		            li.data({ url: item.url, path: item.path });
		            var a = $('<a />', { href: item.url, target: "_blank" });
		            a.addClass("example-image-link");
		            a.attr("data-lightbox", "example-set");
		            var img = $("<img />", { width: 80, height: 80, src: item.url });
		            img.addClass("example-image-link");
		            img.appendTo(a);
		            a.appendTo(li);
		            var removeIcon = $('<div class="tags removeIcon" style="cursor: pointer;"><div class="zxsj_file_on_del"><i class="glyphicon glyphicon-remove-circle"></i></div></div>');

		            //移动图片  liuyl
		            var defaultIcon = $('<div class="tags defaultIcon" style="cursor: pointer; width: 40px; height: 40px;top:0;"><button class="btn" style="cursor: pointer;margin:0; display:none;"><i class="ace-icon fa fa-pencil align-top bigger-125"></i></button></div>');
		            defaultIcon.hover(function () {
		                defaultIcon.attr("style", "cursor: pointer;background-color:#ccc;opacity:0.5;width: 40px; height: 40px;top:0;");
		                defaultIcon.find(".btn").show();
		            }, function () {
		                defaultIcon.attr("style", "cursor: pointer;width: 40px; height: 40px;top:0;");
		                defaultIcon.find(".btn").hide();
		            });
		            defaultIcon.find(".btn").on("click", function () {
		                var i = $(this).parents("li:first");
		                i.remove();
		                imageList.prepend(createItem(item));
		                syncValue();
		            });

		            removeIcon.click(function () {
		                var i = $(this).parents("li:first");
		                i.remove();
		                syncValue();
		            });
		            removeIcon.appendTo(li);
		            defaultIcon.appendTo(li);
		        }

		        return li;
		    };

		    if (!(array instanceof (Array))) {
		        array = new Array(array);
		    }
		    //var uploadArray = new Array();
		    $.each(array, function () {
		        if (oneImage) {
		            createItem(this);
		        }
		        else if (config.callback) {
		            createItem(this);
		        }
		        else {
		            createItem(this).appendTo(imageList);
		        }
		        //uploadArray.push(this.path);
		    });
		    syncValue();
		};

		var that = this;
          
		$(uploadController.container).find("button.btnSubmitCropper").on("click", function () {
		    var dataURL = $(image).cropper("getDataURL");
		    var imgPath = $(".cropper").attr("src");
		    if (config.uploadSettings.croDefault) {
		        var item = new Object();
		        item.path = imgPath;
		        item.url = imgPath;
		        that.addImage(item);
		        $(".div-cropper").modal("hide")
		    }
		    else {
		        var d;
		        $.ajax({
		            url: "/Upload/UploadCropperImage",
		            type: "post",
		            async: false,
		            data: {
		                t: new Date().getDate(),
		                Filedata: dataURL,
		                path: imgPath,
		                croHeight: config.uploadSettings.croHeight,
		                croWidth: config.uploadSettings.croWidth
		            },
		            beforeSend: function () {
		                d = dialog_loading();
		            },
		            error: function () {
		                dialog_loadingClose(d);
		                dialog({ contentType: 'tipsbox', skin: 'bk-popup', content: '服务器繁忙', closeTime: 2000 }).show();
		            },
		            success: function (res) { //success
		                dialog_loadingClose(d);
		                var item = new Object();
		                item.path = res.Url;
		                item.url = res.Url;
		                that.addImage(item);
		                $(".div-cropper").modal("hide")
		            }
		        });
		    }

		});

		(function () {
			button.click(function (e) {
				uploadController.show();
			});
			uploadController.succes(function (e, list) {
				var ul = imageList;
				ul.find("li").remove();
				$.each(list, function () {
					that.addImage(this);
				});
			});
		})();
	}

	$(function () {
		//多图片上传的 data-api
		$("[data-csupadm-picup]").each(function () {
			var upimgsEl = $("<div></div>").appendTo("body");
			var uploadControl = upimgsEl.uploadControl({
				input: "#" + $(this).attr("id"),
				button: $(this).attr("data-csupadm-picup-button"),
				imageList: $(this).attr("data-csupadm-picup-imageList"),
				uploadSettings: {
					//如果图片宽度大于该值服务器则会强制裁剪成该值，否则不对宽度进行任何处理。
					maxWidth: parseInt($(this).attr("data-csupadm-picup-maxWidth")),
					//如果图片高度大于该值服务器则会强制裁剪成该值，否则不对高度进行任何处理。
					maxHeight: parseInt($(this).attr("data-csupadm-picup-maxHeight")),
					maxCount: parseInt($(this).attr("data-csupadm-picup-maxCount")),
					//参考SWFUpload插件，这边的配置会传递到SWFUpload。
					0: 0
				}
			});  

			$(this).on("uploadControlControllerChange", function () {
				$(this).attr("data-csupadm-picup-path", $(this).attr("data-uploadControlController-path"));
				$(this).attr("data-csupadm-picup-url", $(this).attr("data-uploadControlController-url"));
				if ($(this).attr("data-csupadm-picup-val") == "url") {
					this.value = $(this).attr("data-csupadm-picup-url");
				}
			});

			(function () {
				var items = [];

				var value = this.value;
				var path = $(this).attr("data-csupadm-picup-path") || value;
				var url = $(this).attr("data-csupadm-picup-url") || value;

				var values = value.split(",");
				var paths = path.split(",");
				var urls = url.split(",");
				for (var i = 0; i < values.length; i++) {
					if (!values[i] || values[i] == "") continue;
					items.push({
						path: paths[i],
						url: urls[i]
					});
				}
				uploadControl.addImage(items);
			}).call(this);
		});
	});
})(jQuery);
var resource_url = "http://resource.zxsj123.com";
var upload_url = "http://admin.zxsj123.com";
(function ($) {
    $.fn.upload = function (config) {
        var modalHtml = '<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">'
            + '<div class="modal-dialog" style="width:730px">'
            + '<div class="modal-content">'
            + '<div class="modal-header">'
            + '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>'
            + '<div class="input-group" style="width: 260px;">'
            + '<span class="input-group-btn">'
            + '<select class="input-medium" id="selectAlbum"></select>'
            + '</span>'
            + '<span class="input-group-btn">'
            + '<span id="btnUpload"></span>'
            + '</span>'
            + '</div>'
            + '</div>'
            + '<div class="modal-body">'
            + '<div class="row-fluid">'
            + '<ul id="imageList" class="ace-thumbnails" style="height: 270px"></ul>'
            + '<div class="paged" style=" clear: both">'
            + '<ul class="pagination" style="margin: 0px; padding: 0px;"></ul>'
            + '</div>'
            + '<div class="hr hr8"></div>'
            + '<ul id="selectedImageList" class="ace-thumbnails"></ul>'
            + '</div>'
            + '<div style="clear: both"></div>'
            + '</div>'
            + '<div class="modal-footer">'
            + '<button class="no-border btn-primary btn" data-dismiss="modal" id="btnOk" name="确定" type="button">确定</button>'
            + '</div>'
            + '</div>'
            + '</div>'
            + '</div>';

        var container = $(this);
        var modal = $(modalHtml);
        container.append(modal);
        config = $.extend(
        {
            maxCount: 0,
            maxWidth: 640,
            maxHeight: 640
        }, config);
        var uploadController = new UploadController(modal, config);
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
        var uploadController = this.upload(settings.uploadSettings);
        var controller = new UploadControlController(uploadController, buttonElement, inputElement, viewImageListElement);
        return controller;
    };

    function UploadController(container, config) {
        container = $(container);
        container.modal({
            backdrop: "static",
            show: false
        });
        var imageList = container.find("#imageList");
        var selectedImageList = container.find("#selectedImageList");
        var paged = new pagedController();
        var pagination = container.find(".pagination");
        var albumSelect = container.find("#selectAlbum");
        var btnOk = container.find("#btnOk");

        function selectImage(aElement, isAddToSelectedList, allowDelete) {
            var paths = getSelectedPaths();

            var a = $(aElement);
            var imgElement = a.find("img");
            var imagePath = imgElement.parents("li:first").data("imagePath");

            a.append('<div class="tags"><span class="label-holder"><span class="label label-warning arrowed"><i class="fa fa-check"></i></span></span></div>');
            a.parent().css({ border: "1px solid orange" });

            if (!isAllowSelect()) {
                //超过选择数量时自动删除最早的一条
                var li2 = selectedImageList.children().first();
                removeSelect(li2.data("imagePath"));
            }

            if (!isAllowSelect()) {
                if (allowDelete) {
                    removeSelect(imagePath);
                }
                return;
            }

            var isOk = true;
            $.each(paths, function () {
                if (imagePath == this) {
                    isOk = false;
                    return false;
                }
                return true;
            });

            if (!isOk) {
                if (allowDelete) {
                    removeSelect(imagePath);
                }
                return;
            }
            if (isAddToSelectedList) {
                var li = a.parents("li:first");
                addImageToSelectedList({
                    Url: li.data("Url"),
                    ImagePath: imagePath
                });
            }
        }

        function addImageToSelectedList(model) {
            if (model == null) {
                return;
            }
            var li =
                $("<li />", { style: "border: solid 1px #ccc; width: 60px; height: 60px" })
                    .data({
                        "imagePath": model.ImagePath,
                        "Url": model.Url
                    });
            var a = $("<a />", { "class": "cboxElement" });
            a.appendTo(li);
            $("<img />", { width: "60px", height: "60px", src: model.Url }).appendTo(a);
            a.append($('<div class="tags"><span class="label-holder"><i class="fa fa-times-circle red" style=" cursor: pointer"></i></span></div>').click(function () {
                removeSelect(model.ImagePath);
            }));
            selectedImageList.append(li);
        }

        function getSelectedPaths() {
            var array = new Array();
            $.each(getSelectedList(), function () {
                array.push(this.path);
            });
            return array;
        }

        function getSelectedList() {
            var array = new Array();
            $.each(selectedImageList.find("li"), function () {
                var li = $(this);

                var imagePath = li.data("imagePath");
                var url = li.data("Url");

                if (imagePath != null && url != null) {
                    array.push({ path: imagePath, url: url });
                }
            });
            return array;
        }

        function removeSelect(imagePath) {
            var li1 = findLiByImagePath(imageList, imagePath);
            var li2 = findLiByImagePath(selectedImageList, imagePath);
            if (li1 != null) {
                li1.css({ border: "1px solid #CCC" });
                li1.find(".tags").remove();
            }
            if (li2 != null) {
                li2.remove();
            }
        }

        function findLiByImagePath(ul, imagePath) {
            imagePath = imagePath.toLowerCase();
            var result;
            $.each($(ul).find("li"), function () {
                var item = $(this);
                var dataImagePath = item.data("imagePath");
                if (dataImagePath != null && dataImagePath.toLowerCase() == imagePath) {
                    result = item;
                    return false;
                }
                return true;
            });
            return result;
        }

        function syncImageList() {
            var paths = getSelectedPaths();
            $.each(paths, function () {
                var li = findLiByImagePath(imageList, this);
                if (li != null) {
                    selectImage(li.find("a"), false, false);
                }
            });
        }

        function pagedController() {
            var pageCount;
            var globalCurrentIndex;

            this.getPageIndex = function () {
                var number = parseInt(globalCurrentIndex);
                if (isNaN(number) || number == null) {
                    return 1;
                }
                return number;
            };

            this.setCurrentIndex = function (currentIndex) {
                if (globalCurrentIndex == currentIndex) {
                    return;
                }
                if (globalCurrentIndex != null) {
                    $(this).trigger("pageChange", [this.getPageIndex(), currentIndex]);
                }

                globalCurrentIndex = currentIndex;

                var lis = pagination.find("li");
                lis.removeClass("active");
                $.each(lis, function () {
                    var li = $(this);
                    if (parseInt(li.data("index")) == currentIndex) {
                        li.addClass("active");
                        return false;
                    }
                    return true;
                });

                var setLiStatus = function (li, fn) {
                    var span = li.find("span");
                    if (fn()) {
                        span.css({
                            color: "#CCC",
                            cursor: "default"
                        });
                    } else {
                        span.css({
                            color: "#2283c5",
                            cursor: "pointer"
                        });
                    }
                }
                setLiStatus(pagination.find("li:first"), function () { return currentIndex <= 1; });
                setLiStatus(pagination.find("li:last"), function () { return currentIndex >= pageCount; });
            };

            this.init = function (count, pageSize, currentIndex) {
                pageCount = Math.ceil(parseInt(count) / (pageSize));
                if (pageCount <= 0) {
                    return;
                }
                globalCurrentIndex = null;
                pagination.find("li").remove();
                var topLi = $('<li><span style="cursor:pointer;color:#2283c5"><i class="fa fa-chevron-left"></i></span></li>');
                var t = this;
                topLi.click(function () {
                    var index = t.getPageIndex();
                    if (index > 1) {
                        t.setCurrentIndex(index - 1);
                    }
                });
                pagination.append(topLi);
                for (var i = 1; i <= pageCount; i++) {
                    var li = $('<li><a href="javascript:void(0)">' + i + '</a></li>');
                    li.data("index", i);
                    li.click(function () {
                        var number = parseInt($(this).data("index"));
                        if (t.getPageIndex() != number) {
                            t.setCurrentIndex(number);
                        }
                    });
                    pagination.append(li);
                }
                var endLi = $('<li><span style="cursor:pointer;color:#2283c5"><i class="fa fa-chevron-right"></i></span></li>');
                endLi.click(function () {
                    var index = t.getPageIndex();
                    if (index < pageCount) {
                        t.setCurrentIndex(index + 1);
                    }
                });
                pagination.append(endLi);

                this.setCurrentIndex(currentIndex);
            };

            this.pageChange = function (fn) {
                $(this).bind("pageChange", fn);
            };
        }

        function createImageListItem(data) {
            var li = $("<li />").css({ border: "solid 1px #ccc" });
            li.data({ imagePath: data.ImagePath, Url: data.Url });
            var a = $("<a />", { href: "javascript:void(0)", "class": "cboxElement" }).appendTo(li);
            a.click(function () {
                selectImage(this, true, true);
            });
            $("<img />", { width: "80px", height: "80px", src: data.Url }).appendTo(a);
            return li;
        }

        function loadImageList(albumId, pageIndex) {
            if (isNaN(albumId) || albumId == null) {
                albumId = parseInt(albumSelect.val());
            }
            imageList.find("li").remove();
            var pageSize = 24;
            $.post((window.ApplicationPath || "/") + "admin/supports/Upload/GetImageList", { albumId: albumId, pageIndex: pageIndex, pageSize: pageSize }, function (result) {
                paged.init(result.Count, pageSize, paged.getPageIndex());
                if (result.List == null) {
                    return;
                }
                $.each(result.List, function () {
                    var li = createImageListItem(this);
                    li.appendTo(imageList);
                });
                selected();
                syncImageList();
            });
        }

        function bindSwfUpload(settings) {
            var buttonAction = (settings.maxCount <= 0 || settings.maxCount) > 1 ? SWFUpload.BUTTON_ACTION.SELECT_FILES : SWFUpload.BUTTON_ACTION.SELECT_FILE;
            return new SWFUpload($.extend({
                //upload_url: (window.ApplicationPath || "/") + "/Upload/UploadImage",
                upload_url: upload_url + "/Upload/UploadImage",
                file_size_limit: 2048,
                file_types: "*.jpg;*.gif;*.png",
                file_types_description: "JPEG Image",
                file_upload_limit: 0,
                file_queue_limit: 0,
                //button_image_url: (window.ApplicationPath || "/") + "Themes/TheAdmin/lib/swfupload/images/uplodfromDnBtnIcon.png",
                button_image_url: resource_url + "/widget/swfupload/images/uplodfromDnBtnIcon.png",
                button_placeholder_id: "btnUpload",
                button_width: 87,
                button_height: 28,
                button_text_top_padding: 0,
                button_cursor: SWFUpload.CURSOR.HAND,
                button_action: buttonAction,
                post_params: { maxWidth: config.maxWidth, maxHeight: config.maxHeight }
            }, settings));
        }

        function isAllowSelect() {
            var currentCout = getSelectedList().length;
            return config.maxCount <= 0 || currentCout < config.maxCount;
        }

        var that = this;

        var swfUpload = bindSwfUpload($.extend({
            upload_start_handler: function () {
                swfUpload.removePostParam("albumId");
                swfUpload.addPostParam("albumId", albumSelect.val());
            },
            upload_success_handler: function (file, serverData) {
                try {
                    var data = eval("(" + serverData + ")");
                    var li = createImageListItem(data);
                    if (isAllowSelect()) {
                        selectImage(li.find("a"), true, false);
                    }
                    imageList.find(".uploading").parents("li:first").replaceWith(li);
                } catch (ex) {
                    this.debug(ex);
                }
            },
            upload_progress_handler: function () {
                try {
                    var uploading;
                    if ($(".uploading").length > 0) {
                        uploading = $(".uploading");
                    } else {
                        uploading = $(".waiting:last").removeClass("waiting").addClass("uploading");
                    }

                    uploading.next().removeClass("grey").addClass("green");
                } catch (ex) {
                    this.debug(ex);
                }
            },
            file_dialog_complete_handler: function (numFilesSelected, numFilesQueued) {
                try {
                    if (numFilesQueued > 0) {
                        var $imageList = imageList.find("li");
                        if ($imageList.length > 0) {
                            var removeCount = numFilesQueued + $imageList.length - 24;
                            if (removeCount > 0) {
                                for (var j = $imageList.length; j >= $imageList.length - removeCount; j--) {
                                    $imageList.eq(j).remove();
                                }
                            }
                        }
                        for (var i = 0; i < numFilesQueued; i++) {
                            imageList.prepend('<li style="border: solid 1px #ccc;"><div class="waiting"></div><i class="fa fa-spinner grey" style="padding: 26px 28.5px; font-size: 200%"></i></li>');
                        }
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

        var selectedArray;
        this.selected = function (array) {
            selectedArray = array;
        }

        function selected() {
            var array = selectedArray;
            var selectedPaths = getSelectedPaths();
            var exits = function (path) {
                var isExits = false;
                $.each(selectedPaths, function () {
                    if (path.toLowerCase() == this.toLowerCase()) {
                        isExits = true;
                        return false;
                    }
                    return true;
                });
                return isExits;
            }
            var convert = function () {
                if (array == null || !(array instanceof (Array)) || array.length == 0) {
                    return null;
                }
                if (array[0] instanceof (String) || typeof (array[0]) == "string") {
                    return array;
                } else {
                    var list = new Array();
                    $.each(array, function () {
                        list.push(this.Path);
                    });
                    return list;
                }
            };
            var imagePaths = convert();
            if (imagePaths == null) {
                return;
            }
            $.each(imagePaths, function () {
                if (exits(this)) {
                    return true;
                }
                var li = findLiByImagePath(imageList, this);
                if (li == null) {
                    var path = this;
                    $.ajax({
                        url: (window.ApplicationPath || "/") + "api/Upload/GetImageByPath",
                        data: { path: this },
                        method: "POST",
                        success: function (result) {
                            addImageToSelectedList(result || { ImagePath: path, Id: -1, Url: path });
                        },
                        error: function () {
                            addImageToSelectedList({ ImagePath: path, Id: -1, Url: path });
                        }
                    });
                } else {
                    selectImage(li.find("a"), true, false);
                }
                return true;
            });
        }

        this.show = function () {
            container.modal("show");
        }
        this.close = function () {
            container.modal("hide");
        }
        this.toggle = function () {
            container.modal("toggle");
        }
        this.removeSelected = function (paths) {
            if (paths == null) {
                return;
            }
            if (paths instanceof (Array)) {
                $.each(paths, function () {
                    removeSelect(this);
                });
            } else {
                removeSelect(paths);
            }
        };
    }

    function UploadControlController(uploadController, buttonElement, inputElement, viewImageListElement) {
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
                var li = $("<li />", { style: "border: solid 1px #ccc; width: 80px; height: 80px;" });
                li.data({ url: item.url, path: item.path });
                var a = $("<a />", { href: item.url });
                a.addClass("example-image-link");
                a.attr("data-lightbox", "example-set");
                var img = $("<img />", { width: 80, height: 80, src: item.url });
                img.addClass("example-image-link");
                img.appendTo(a);
                a.appendTo(li);
                var removeIcon = $('<div class="tags removeIcon" style="cursor: pointer;"><span class="label-holder"><i class="fa fa-times-circle red"></i></span></div>');
                 
                //移动图片  liuyl
                var defaultIcon = $('<div class="tags defaultIcon" style="cursor: pointer; width: 40px; height: 40px;top:0;"><button class="btn" style="cursor: pointer;margin:0; display:none;"><i class="ace-icon fa fa-pencil align-top bigger-125"></i></button></div>');
                defaultIcon.hover(function () {
                    defaultIcon.attr("style", "cursor: pointer;background-color:#ccc;opacity:0.5;width: 40px; height: 40px;top:0;");
                    defaultIcon.find(".btn").show();
                }, function () {
                    defaultIcon.attr("style", "cursor: pointer;width: 40px; height: 40px;top:0;");
                    defaultIcon.find(".btn").hide();
                });
                defaultIcon.find(".btn").on("click",function () {
                    var i = $(this).parents("li:first"); 
                    i.remove();
                    imageList.prepend(createItem(item));
                    syncValue();
                });

                removeIcon.click(function () {
                    var i = $(this).parents("li:first");
                    uploadController.removeSelected(i.data("path"));
                    i.remove();
                    syncValue();
                    //                alert(removeSelect);
                    //                removeSelect($(this).data("path"));
                    //                syncSelected();
                });
                removeIcon.appendTo(li);
                defaultIcon.appendTo(li);
                return li;
            };
            if (!(array instanceof (Array))) {
                array = new Array(array);
            }
            var uploadArray = new Array();
            $.each(array, function () {
                createItem(this).appendTo(imageList);
                uploadArray.push(this.path);
            });
            uploadController.selected(uploadArray);
            syncValue();
        };

        var that = this;
        (function () {
            button.click(function () {
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

    $.fn.uploadOne = function (settings) {
        return new function () {
            settings = $.extend({
                maxWidth: 0,
                maxHeight: 0,
                fileName: "file",
                autoUpload: true,
                enableRemove: settings.url.remove != null && settings.url.remove != "",
                success: function (data) {
                },
                error: function (xhr, status, error) {
                    var message = "上传失败，" + error;
                    if ($.scojs_message == null) {
                        alert(message);
                    } else {
                        $.scojs_message(message, 1);
                    }
                },
                data: {}
            }, settings);
            var form = $("<form />", { enctype: "multipart/form-data" }).append($("<input />", { name: settings.fileName, id: settings.fileName, type: "file" }).hide());
            var uploadForm;
            var image, button, input, imageContainer;
            image = $(settings.image);
            button = $(settings.button);
            input = $(settings.input);
            if (image != null) {
                image.wrap($("<div />"));
                imageContainer = image.parent();
                imageContainer.append($('<i id="loading" class="fa fa-spinner grey" style="padding: 26px 28.5px; font-size: 200%"></i>').hide());
            } else {
            }
            if (button == null) {
                throw "请传入上传按钮控件。";
            }
            if (!settings.url) {
                throw "Url无效。";
            }
            if (!settings.url.upload) {
                throw "上传的Url无效。";
            }

            function changeStatus(status) {
                var loading = imageContainer.find("#loading");
                switch (status) {
                    case 0:
                        if (settings.enableRemove) {
                            image.parents(".tags").hide();
                        }
                        image.hide();
                        loading.show();
                        break;
                    case 1:
                        loading.hide();
                        if (settings.enableRemove) {
                            image.parents(".tags").show();
                        }
                        image.show();
                        break;
                    case 2:
                        if (settings.enableRemove) {
                            image.parents(".tags").hide();
                        }
                        image.hide();
                        loading.hide();
                        break;
                    case 3:
                        if (image.attr("src")) {
                            changeStatus(1);
                        } else {
                            changeStatus(2);
                        }
                        break;
                }
            }

            this.setImage = setImage;

            function setImage(model) {
                if (!model) {
                    //隐藏图片。
                    changeStatus(2);
                } else {
                    image.data({ path: model.path });
                    input.val(model.path);
                    //显示图片。
                    changeStatus(1);
                }
                image.attr("src", model == null ? null : model.url);
            }

            function getUploadModel() {
                if (uploadForm != null) {
                    return { form: uploadForm, file: uploadForm.find("input:first") };
                }
                uploadForm = form.clone();
                form.appendTo($("body"));
                return { form: uploadForm, file: uploadForm.find("input:first") };
            }

            this.upload = function (fn) {
                if (!getUploadModel().file.val()) {
                    return;
                }
                //切换为Loading状态。
                changeStatus(0);
                $(uploadForm).ajaxSubmit({
                    type: "POST",
                    url: settings.url.upload,
                    data: settings.data,
                    success: function (data) {
                        $(uploadForm).remove();
                        uploadForm = null;
                        setImage(data);
                        fn = fn || settings.success;
                        if (fn) {
                            fn(data);
                        }
                    },
                    error: function (xhr, status, error) {
                        if (settings.error) {
                            settings.error(xhr, status, error);
                        }
                        $(uploadForm).remove();
                        uploadForm = null;
                        changeStatus(3);
                    }
                });
            };

            this.remove = function (fn) {
                if (!settings.url.remove) {
                    throw "删除的Url无效。";
                }
                if (!input.val()) {
                    return;
                }
                var isOk = true;
                $.ajax({
                    url: settings.url.remove,
                    type: "POST",
                    cache: false,
                    data: { path: input.val() },
                    success: function () {
                        isOk = true;
                        setImage(null);
                    },
                    error: function () {
                        isOk = false;
                    }
                });
                if (fn) {
                    fn(isOk);
                }
            };

            var that = this;
            (function () {
                if (settings.enableRemove) {
                    var div = $("<div />", { "class": "tags", style: "cursor: pointer;width:auto" }).hide();
                    var removeIcon = $('<span class="label-holder"><i class="fa fa-times-circle red"></i></span>');
                    removeIcon.click(function () {
                        that.remove();
                    });
                    image.wrap(div);
                    image.after(removeIcon);
                    image.after("<br />");
                }
                if (settings.maxWidth) {
                    image.css({ "max-width": settings.maxWidth });
                }
                if (settings.maxHeight) {
                    image.css({ "max-height": settings.maxHeight });
                }
                setImage(null);
                button.click(function () {
                    var file = getUploadModel().file;
                    file.unbind("change");
                    file.bind("change", function () {
                        if (!input.val()) {
                            input.val(file.val());
                        }
                        if (settings.autoUpload) {
                            that.upload();
                        }
                    });
                    file.click();
                });
            })();
        };
    };

    $.simpleUpload = function (settings) {
        return new function () {
            settings = $.extend(
            {
                autoUpload: true,
                fileName: "file",
                success: function (data) {
                },
                error: function (xhr, status, error) {
                    var message = "上传失败，" + error;
                    if ($.scojs_message == null) {
                        alert(message);
                    } else {
                        $.scojs_message(message, 1);
                    }
                },
            }, settings);

            var form = $("<form />", { enctype: "multipart/form-data" }).append($("<input />", { name: settings.fileName, id: settings.fileName, type: "file" }).hide());
            var uploadForm;
            var button = $(settings.button);
            if (button == null) {
                throw "请传入上传按钮控件。";
            }
            if (!settings.url) {
                throw "Url无效。";
            }
            if (!settings.url.upload) {
                throw "上传的Url无效。";
            }

            function getUploadModel() {
                if (uploadForm != null) {
                    return { form: uploadForm, file: uploadForm.find("input:first") };
                }
                uploadForm = form.clone();
                form.appendTo($("body"));
                return { form: uploadForm, file: uploadForm.find("input:first") };
            }

            this.upload = function () {
                if (!getUploadModel().file.val()) {
                    return;
                }
                $(uploadForm).ajaxSubmit({
                    type: "POST",
                    url: settings.url.upload,
                    data: settings.data,
                    success: function (data) {
                        $(uploadForm).remove();
                        uploadForm = null;
                        var fn = settings.success;
                        if (fn) {
                            fn(data);
                        }
                    },
                    error: function (xhr, status, error) {
                        if (settings.error) {
                            settings.error(xhr, status, error);
                        }
                        $(uploadForm).remove();
                        uploadForm = null;
                    }
                });
            };

            this.remove = function (path, fn) {
                if (!settings.url.remove) {
                    throw "删除的Url无效。";
                }
                if (!path) {
                    return;
                }
                var isOk = true;
                $.ajax({
                    url: settings.url.remove,
                    type: "POST",
                    cache: false,
                    data: { path: path },
                    success: function () {
                        isOk = true;
                    },
                    error: function () {
                        isOk = false;
                    }
                });
                if (fn) {
                    fn(isOk);
                }
            };

            this.select = function () {
                var file = getUploadModel().file;
                if (settings.autoUpload) {
                    file.unbind("change");
                    var t = this;
                    file.bind("change", function () {
                        t.upload();
                    });
                }
                file.click();
            }
        };
    };

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
using Auth.Infrastructure.FileSystems;
using Auth.Infrastructure.Utility;
using Auth.Infrastructure.Utility.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Sample.Management.Web.Controllers
{
    public class UploadController : Controller
    {
        private readonly IUploadImageFolder _uploadImageFolder;

        public UploadController(IUploadImageFolder uploadImageFolder)
        {
            _uploadImageFolder = uploadImageFolder;
        }

        public class ImageViewModel
        {
            public string ImagePath { get; set; }

            public string Url { get; set; }
        }

        public ActionResult UploadImage()
        {
            var request = Request;
            var response = Response;

            var file = request.Files["Filedata"];

            var idString = request["albumId"];
            idString = "100";
            long albumId;
            if (!long.TryParse(idString, out albumId) || file == null)
            {
                response.StatusCode = 500;
                return Json(new { ErrorMessage = "出现未知错误" }); 
            }

            try
            {
                if (!new[] { ".jpg", ".png", ".gif" }.Any(item => file.FileName.ToLower().Contains(item)))
                    throw new Exception("图片的格式必须.jpg和.png以及.gif");

                if (!FileHelper.CheckFileFormatIsTrue(file.FileName, file.InputStream))
                    throw new Exception("图片的后缀名和图片的真实格式不一致，请修改图片的后缀名，再尝试上传");

                var maxWidthString = request["maxWidth"];
                var maxHeightString = request["maxHeight"];

                int maxWidth, maxHeight;
                int.TryParse(maxWidthString, out maxWidth);
                int.TryParse(maxHeightString, out maxHeight);

                maxWidth = maxWidth <= 0 ? int.MaxValue : maxWidth;
                maxHeight = maxHeight <= 0 ? int.MaxValue : maxHeight;

                using (var imageHelper = new ImageHelper(file.InputStream))
                {
                    using (var thumbnail = imageHelper.GetPicThumbnail(maxWidth, maxHeight, 100))
                    {
                        var fileName = FileHelper.GetRandomFileNameByFileName(file.FileName);
                        var path = $"~/{albumId}/{fileName}";
                        using (var stream = new MemoryStream())
                        {
                            thumbnail.Save(stream);
                            //_uploadImageFolder.CreateFile(path, stream);
                           var resultJosn = _uploadImageFolder.CreateFileUrl(path, stream);

                            dynamic objPath = JsonConvert.DeserializeObject<dynamic>(resultJosn);
                            if (objPath.Code.Value == 10000)
                                path = objPath.Uri.Value;
                            else
                                path = "";
                        }
                        return Json(new ImageViewModel
                        {
                            ImagePath = path,
                            Url = Url.ImageContent(path)
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                return Json(new { ErrorMessage = exception.Message });
            }
        }

        public void Image(string filePath)
        {
            WebClient mywebclient = new WebClient();
            byte[] bytes = mywebclient.DownloadData(filePath);

            Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开
            Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode("二维码.jpg", System.Text.Encoding.UTF8));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End(); 
        }



        public ActionResult UploadCropperImage()
        { 
            var files = Request.Form["Filedata"];
            var path = Request.Form["path"];

            var maxHeight = Convert.ToDouble( Request.Form["croHeight"]);
            var maxWidth = Convert.ToDouble(Request.Form["croWidth"]);
            var bytes = Convert.FromBase64String(files.Split(',')[1]);
            MemoryStream ms = new MemoryStream(bytes);
            using (var imageHelper = new ImageHelper(ms))
            {
                using (var thumbnail = imageHelper.GetPicThumbnail(maxWidth, maxHeight, 100))
                {
                    //var fileName = FileHelper.GetRandomFileNameByFileName(file.FileName);
                    //var path = $"~/{albumId}/{fileName}";
                    using (var stream = new MemoryStream())
                    {
                        thumbnail.Save(stream); 
                        var resultJosn = _uploadImageFolder.CreateFileUrl(path, stream);
                        dynamic objPath = JsonConvert.DeserializeObject<dynamic>(resultJosn);
                        if (objPath.Code.Value == 10000)
                            path = objPath.Uri.Value;
                        else
                            path = "";

                    }
                    return Json(new ImageViewModel
                    {
                        ImagePath = path,
                        Url = Url.ImageContent(path)
                    });
                }
            }

            //try
            //{   

            //    WebClient clientObj = new WebClient();
            //    NameValueCollection PostVars = new NameValueCollection();
            //    PostVars.Add("File", "ueditor");                                //这些主要是提交的参数和值
            //    PostVars.Add("FileName", Path.GetFileName(path));
            //    PostVars.Add("FileByte", files.Split(',')[1]);

            //    //Post访问接口,返回转为byte[]的josn字符串
            //    var filesUri = CommFunction.StringParse(ConfigurationManager.AppSettings["FilesUri"]);
            //    byte[] byRemoteInfo = clientObj.UploadValues(filesUri + "/Upload/FileUpload.aspx", "POST", PostVars);
            //    string resultstring = Encoding.Default.GetString(byRemoteInfo);
              
            //    dynamic objPath = JsonConvert.DeserializeObject<dynamic>(resultstring);
            //    if (objPath.Code.Value == 10000)
            //        path = objPath.Uri.Value;
            //    else
            //        path = "";

            //    return Json(new { success = true, path = path });
            //}
            //catch (Exception exception)
            //{
            //    return Json(new { success = false, ErrorMessage = exception.Message });
            //}

        }

    }
}
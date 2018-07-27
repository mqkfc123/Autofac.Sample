using Sample.Management.Web.Controllers.Codes;
using Auth.Infrastructure.FileSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sample.Management.Web.Controllers
{
    public class UeditorController : Controller
    {
        private readonly IUploadFolder _uploadFolder; 

        public UeditorController(IUploadFolder uploadFolder)
        {
            _uploadFolder = uploadFolder; 
        }

        public ActionResult Index()
        {
            var context = System.Web.HttpContext.Current;
            Handler action;
            switch (context.Request["action"])
            {
                case "config":
                    action = new ConfigHandler(context);
                    break;

                case "uploadimage":
                    action = new UploadHandler(context, new UploadConfig
                    {
                        AllowExtensions = Config.GetStringList("imageAllowFiles"),
                        PathFormat = Config.GetString("imagePathFormat"),
                        SizeLimit = Config.GetInt("imageMaxSize"),
                        UploadFieldName = Config.GetString("imageFieldName")
                    }, _uploadFolder);
                    break;

                case "uploadscrawl":
                    action = new UploadHandler(context, new UploadConfig
                    {
                        AllowExtensions = new[] { ".png" },
                        PathFormat = Config.GetString("scrawlPathFormat"),
                        SizeLimit = Config.GetInt("scrawlMaxSize"),
                        UploadFieldName = Config.GetString("scrawlFieldName"),
                        Base64 = true,
                        Base64Filename = "scrawl.png"
                    }, _uploadFolder);
                    break;

                case "uploadvideo":
                    action = new UploadHandler(context, new UploadConfig
                    {
                        AllowExtensions = Config.GetStringList("videoAllowFiles"),
                        PathFormat = Config.GetString("videoPathFormat"),
                        SizeLimit = Config.GetInt("videoMaxSize"),
                        UploadFieldName = Config.GetString("videoFieldName")
                    }, _uploadFolder);
                    break;

                case "uploadfile":
                    action = new UploadHandler(context, new UploadConfig
                    {
                        AllowExtensions = Config.GetStringList("fileAllowFiles"),
                        PathFormat = Config.GetString("filePathFormat"),
                        SizeLimit = Config.GetInt("fileMaxSize"),
                        UploadFieldName = Config.GetString("fileFieldName")
                    }, _uploadFolder);
                    break;

                case "listimage":
                    action = new ListFileManager(context, Config.GetString("imageManagerListPath"), Config.GetStringList("imageManagerAllowFiles"), _uploadFolder);
                    break;

                case "listfile":
                    action = new ListFileManager(context, Config.GetString("fileManagerListPath"), Config.GetStringList("fileManagerAllowFiles"), _uploadFolder);
                    break;

                case "catchimage":
                    action = new CrawlerHandler(context, _uploadFolder);
                    break;

                default:
                    action = new NotSupportedHandler(context);
                    break;
            }
            action.Process();
            return Content("");
        }
    }
}
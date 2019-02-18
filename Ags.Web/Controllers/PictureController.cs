using System;
using System.Linq;
using Ags.Data.Common;
using Ags.Data.Core;
using Ags.Services.Media;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Controllers
{
    public partial class PictureController : BasePublicController
    {
        #region Fields

        private readonly IDownloadService _downloadService;
        private readonly IAgsFileProvider _fileProvider;
        private readonly IPictureService _pictureService;

        #endregion

        #region Ctor

        public PictureController(IDownloadService downloadService,
            IAgsFileProvider fileProvider,
            IPictureService pictureService)
        {
            this._downloadService = downloadService;
            this._fileProvider = fileProvider;
            this._pictureService = pictureService;
        }

        #endregion

        #region Methods

        [HttpPost]
        //do not validate request token (XSRF)


       public virtual IActionResult AsyncUpload()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.UploadPictures))
            //    return Json(new { success = false, error = "You do not have required permissions" }, "text/plain");


            IFormFile httpPostedFile = Request.Form.Files.FirstOrDefault();
                if (httpPostedFile == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "No file uploaded",
                        downloadGuid = Guid.Empty
                    });
                }

            byte[] fileBinary = _downloadService.GetDownloadBits(httpPostedFile);

                const string qqFileNameParameter = "qqfilename";
            string fileName = httpPostedFile.FileName;
                if (string.IsNullOrEmpty(fileName) && Request.Form.ContainsKey(qqFileNameParameter))
                    fileName = Request.Form[qqFileNameParameter].ToString();
                //remove path (passed in IE)
                fileName = _fileProvider.GetFileName(fileName);

            string contentType = httpPostedFile.ContentType;

            string fileExtension = _fileProvider.GetFileExtension(fileName);
                if (!string.IsNullOrEmpty(fileExtension))
                    fileExtension = fileExtension.ToLowerInvariant();

                //contentType is not always available
                //that's why we manually update it here
                //http://www.sfsu.edu/training/mimetype.htm
                if (string.IsNullOrEmpty(contentType))
                {
                    switch (fileExtension)
                    {
                        case ".bmp":
                            contentType = MimeTypes.ImageBmp;
                            break;
                        case ".gif":
                            contentType = MimeTypes.ImageGif;
                            break;
                        case ".jpeg":
                        case ".jpg":
                        case ".jpe":
                        case ".jfif":
                        case ".pjpeg":
                        case ".pjp":
                            contentType = MimeTypes.ImageJpeg;
                            break;
                        case ".png":
                            contentType = MimeTypes.ImagePng;
                            break;
                        case ".tiff":
                        case ".tif":
                            contentType = MimeTypes.ImageTiff;
                            break;
                        default:
                            break;
                    }
                }

            var picture = _pictureService.InsertPicture(fileBinary, contentType, null);

                //when returning JSON the mime-type must be set to text/plain
                //otherwise some browsers will pop-up a "Save As" dialog.
                return Json(new { success = true, pictureId = picture.Id, imageUrl = _pictureService.GetPictureUrl(picture.Id, 100) });


        }

        #endregion
    }
}
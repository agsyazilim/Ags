using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Ags.Data.Core;
using Ags.Data.Core.Pages;
using Ags.Data.Core.Repository;
using Ags.Data.Domain;
using Ags.Data.Domain.Media;
using Ags.Data.Domain.News;
using Ags.Services.Configuration;
using Ags.Services.Events;
using Ags.Services.Seo;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace Ags.Services.Media
{
    /// <summary>
    /// Picture service
    /// </summary>
    public class PictureService : IPictureService
    {
        #region Fields

        private readonly IDbContext _dbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IAgsFileProvider _fileProvider;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<PictureBinary> _pictureBinaryRepository;
        private readonly ISettingService _settingService;
        private readonly IRepository<PhotoGalleryMapping> _photoGalleryRepository;
        private readonly IRepository<SliderPictureMapping> _sliderPictuRepository;
        private readonly IRepository<NewsPictureMapping> _newsPictRepository;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWebHelper _webHelper;
        private readonly MediaSettings _mediaSettings;

        #endregion

        #region Ctor

        public PictureService(
            IDbContext dbContext,
            IAgsFileProvider fileProvider,
            IRepository<Picture> pictureRepository,
            IRepository<PictureBinary> pictureBinaryRepository,
            ISettingService settingService,
            IUrlRecordService urlRecordService,
            IWebHelper webHelper, IEventPublisher eventPublisher, MediaSettings mediaSettings, IRepository<NewsPictureMapping> newsPictRepository, IRepository<PhotoGalleryMapping> photoGalleryRepository, IRepository<SliderPictureMapping> sliderPictuRepository)
        {
            _dbContext = dbContext;
            this._fileProvider = fileProvider;
            this._pictureRepository = pictureRepository;
            this._pictureBinaryRepository = pictureBinaryRepository;
            this._settingService = settingService;
            this._urlRecordService = urlRecordService;
            this._webHelper = webHelper;
            this._eventPublisher = eventPublisher;
            this._mediaSettings = mediaSettings;
            this._newsPictRepository = newsPictRepository;
            this._photoGalleryRepository = photoGalleryRepository;
            this._sliderPictuRepository = sliderPictuRepository;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Calculates picture dimensions whilst maintaining aspect
        /// </summary>
        /// <param name="originalSize">The original picture size</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="resizeType">Resize type</param>
        /// <param name="ensureSizePositive">A value indicating whether we should ensure that size values are positive</param>
        /// <returns></returns>
        protected virtual Size CalculateDimensions(Size originalSize, int targetSize,
            ResizeType resizeType = ResizeType.LongestSide, bool ensureSizePositive = true)
        {
            float width, height;

            switch (resizeType)
            {
                case ResizeType.LongestSide:
                    if (originalSize.Height > originalSize.Width)
                    {
                        // portrait
                        width = originalSize.Width * (targetSize / (float)originalSize.Height);
                        height = targetSize;
                    }
                    else
                    {
                        // landscape or square
                        width = targetSize;
                        height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    }

                    break;
                case ResizeType.Width:
                    width = targetSize;
                    height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    break;
                case ResizeType.Height:
                    width = originalSize.Width * (targetSize / (float)originalSize.Height);
                    height = targetSize;
                    break;
                default:
                    throw new Exception("Not supported ResizeType");
            }

            if (!ensureSizePositive)
            {
                return new Size((int)Math.Round(width), (int)Math.Round(height));
            }

            if (width < 1)
            {
                width = 1;
            }

            if (height < 1)
            {
                height = 1;
            }

            //we invoke Math.Round to ensure that no white background is rendered - https://www.nopcommerce.com/boards/t/40616/image-resizing-bug.aspx
            return new Size((int)Math.Round(width), (int)Math.Round(height));
        }

        /// <summary>
        /// Returns the file extension from mime type.
        /// </summary>
        /// <param name="mimeType">Mime type</param>
        /// <returns>File extension</returns>
        protected virtual string GetFileExtensionFromMimeType(string mimeType)
        {
            if (mimeType == null)
            {
                return null;
            }

            //TODO use FileExtensionContentTypeProvider to get file extension

            string[] parts = mimeType.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }

            return lastPart;
        }

        /// <summary>
        /// Loads a picture from file
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="mimeType">MIME type</param>
        /// <returns>Picture binary</returns>
        protected virtual byte[] LoadPictureFromFile(int pictureId, string mimeType)
        {
            string lastPart = GetFileExtensionFromMimeType(mimeType);
            string fileName = $"{pictureId:0000000}_0.{lastPart}";
            string filePath = GetPictureLocalPath(fileName);

            return _fileProvider.ReadAllBytes(filePath);
        }

        /// <summary>
        /// Save picture on file system
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="pictureBinary">Picture binary</param>
        /// <param name="mimeType">MIME type</param>
        protected virtual void SavePictureInFile(int pictureId, byte[] pictureBinary, string mimeType)
        {
            string lastPart = GetFileExtensionFromMimeType(mimeType);
            string fileName = $"{pictureId:0000000}_0.{lastPart}";
            _fileProvider.WriteAllBytes(GetPictureLocalPath(fileName), pictureBinary);
        }

        /// <summary>
        /// Delete a picture on file system
        /// </summary>
        /// <param name="picture">Picture</param>
        protected virtual void DeletePictureOnFileSystem(Picture picture)
        {
            if (picture == null)
            {
                throw new ArgumentNullException(nameof(picture));
            }

            string lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            string fileName = $"{picture.Id:0000000}_0.{lastPart}";
            string filePath = GetPictureLocalPath(fileName);
            _fileProvider.DeleteFile(filePath);
        }

        /// <summary>
        /// Delete picture thumbs
        /// </summary>
        /// <param name="picture">Picture</param>
        protected virtual void DeletePictureThumbs(Picture picture)
        {
            string filter = $"{picture.Id:0000000}*.*";
            string[] currentFiles = _fileProvider.GetFiles(_fileProvider.GetAbsolutePath(AgsMediaDefaults.ImageThumbsPath), filter, false);
            foreach (string currentFileName in currentFiles)
            {
                string thumbFilePath = GetThumbLocalPath(currentFileName);
                _fileProvider.DeleteFile(thumbFilePath);
            }
        }

        /// <summary>
        /// Get picture (thumb) local path
        /// </summary>
        /// <param name="thumbFileName">Filename</param>
        /// <returns>Local picture thumb path</returns>
        protected virtual string GetThumbLocalPath(string thumbFileName)
        {
            string thumbsDirectoryPath = _fileProvider.GetAbsolutePath(AgsMediaDefaults.ImageThumbsPath);

            if (_mediaSettings.MultipleThumbDirectories)
            {
                //get the first two letters of the file name
                string fileNameWithoutExtension = _fileProvider.GetFileNameWithoutExtension(thumbFileName);
                if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > AgsMediaDefaults.MultipleThumbDirectoriesLength)
                {
                    string subDirectoryName = fileNameWithoutExtension.Substring(0, AgsMediaDefaults.MultipleThumbDirectoriesLength);
                    thumbsDirectoryPath = _fileProvider.GetAbsolutePath(AgsMediaDefaults.ImageThumbsPath, subDirectoryName);
                    _fileProvider.CreateDirectory(thumbsDirectoryPath);
                }
            }

            string thumbFilePath = _fileProvider.Combine(thumbsDirectoryPath, thumbFileName);
            return thumbFilePath;
        }

        /// <summary>
        /// Get picture (thumb) URL
        /// </summary>
        /// <param name="thumbFileName">Filename</param>
        /// <param name="storeLocation">Store location URL; null to use determine the current store location automatically</param>
        /// <returns>Local picture thumb path</returns>
        protected virtual string GetThumbUrl(string thumbFileName, string storeLocation = null)
        {
            storeLocation = !string.IsNullOrEmpty(storeLocation)
                                    ? storeLocation
                                    : _webHelper.GetStoreLocation();
            string url = storeLocation + "images/thumbs/";

            if (_mediaSettings.MultipleThumbDirectories)
            {
                //get the first two letters of the file name
                string fileNameWithoutExtension = _fileProvider.GetFileNameWithoutExtension(thumbFileName);
                if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > AgsMediaDefaults.MultipleThumbDirectoriesLength)
                {
                    string subDirectoryName = fileNameWithoutExtension.Substring(0, AgsMediaDefaults.MultipleThumbDirectoriesLength);
                    url = url + subDirectoryName + "/";
                }
            }

            url = url + thumbFileName;
            return url;
        }

        /// <summary>
        /// Get picture local path. Used when images stored on file system (not in the database)
        /// </summary>
        /// <param name="fileName">Filename</param>
        /// <returns>Local picture path</returns>
        protected virtual string GetPictureLocalPath(string fileName)
        {
            return _fileProvider.GetAbsolutePath("images", fileName);
        }

        /// <summary>
        /// Gets the loaded picture binary depending on picture storage settings
        /// </summary>
        /// <param name="picture">Picture</param>
        /// <param name="fromDb">Load from database; otherwise, from file system</param>
        /// <returns>Picture binary</returns>
        protected virtual byte[] LoadPictureBinary(Picture picture, bool fromDb)
        {
            if (picture == null)
            {
                throw new ArgumentNullException(nameof(picture));
            }

            byte[] result = fromDb ? picture.PictureBinary?.BinaryData ?? new byte[0]
                : LoadPictureFromFile(picture.Id, picture.MimeType);

            return result;
        }

        /// <summary>
        /// Get a value indicating whether some file (thumb) already exists
        /// </summary>
        /// <param name="thumbFilePath">Thumb file path</param>
        /// <param name="thumbFileName">Thumb file name</param>
        /// <returns>Result</returns>
        protected virtual bool GeneratedThumbExists(string thumbFilePath, string thumbFileName)
        {
            return _fileProvider.FileExists(thumbFilePath);
        }

        /// <summary>
        /// Save a value indicating whether some file (thumb) already exists
        /// </summary>
        /// <param name="thumbFilePath">Thumb file path</param>
        /// <param name="thumbFileName">Thumb file name</param>
        /// <param name="mimeType">MIME type</param>
        /// <param name="binary">Picture binary</param>
        protected virtual void SaveThumb(string thumbFilePath, string thumbFileName, string mimeType, byte[] binary)
        {
            //ensure \thumb directory exists
            string thumbsDirectoryPath = _fileProvider.GetAbsolutePath(AgsMediaDefaults.ImageThumbsPath);
            _fileProvider.CreateDirectory(thumbsDirectoryPath);

            //save
            _fileProvider.WriteAllBytes(thumbFilePath, binary);
        }

        /// <summary>
        /// Encode the image into a byte array in accordance with the specified image format
        /// </summary>
        /// <typeparam name="T">Pixel data type</typeparam>
        /// <param name="image">Image data</param>
        /// <param name="imageFormat">Image format</param>
        /// <param name="quality">Quality index that will be used to encode the image</param>
        /// <returns>Image binary data</returns>
        protected virtual byte[] EncodeImage<T>(Image<T> image, IImageFormat imageFormat, int? quality = null) where T : struct, IPixel<T>
        {
            using (MemoryStream stream = new MemoryStream())
            {
                IImageEncoder imageEncoder = SixLabors.ImageSharp.Configuration.Default.ImageFormatsManager.FindEncoder(imageFormat);
                switch (imageEncoder)
                {
                    case JpegEncoder jpegEncoder:
                        jpegEncoder.Quality = quality ?? _mediaSettings.DefaultImageQuality;
                        jpegEncoder.Encode(image, stream);
                        break;

                    case PngEncoder pngEncoder:
                        pngEncoder.ColorType = PngColorType.Rgb;
                        pngEncoder.Encode(image, stream);
                        break;

                    case BmpEncoder bmpEncoder:
                        bmpEncoder.BitsPerPixel = BmpBitsPerPixel.Pixel32;
                        bmpEncoder.Encode(image, stream);
                        break;

                    case GifEncoder gifEncoder:
                        gifEncoder.Encode(image, stream);
                        break;

                    default:
                        imageEncoder.Encode(image, stream);
                        break;
                }

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Updates the picture binary data
        /// </summary>
        /// <param name="picture">The picture object</param>
        /// <param name="binaryData">The picture binary data</param>
        /// <returns>Picture binary</returns>
        protected virtual PictureBinary UpdatePictureBinary(Picture picture, byte[] binaryData)
        {
            if (picture == null)
            {
                throw new ArgumentNullException(nameof(picture));
            }

            PictureBinary pictureBinary = picture.PictureBinary;

            bool isNew = pictureBinary == null;

            if (isNew)
            {
                pictureBinary = new PictureBinary
                {
                    PictureId = picture.Id
                };
            }

            pictureBinary.BinaryData = binaryData;

            if (isNew)
            {
                _pictureBinaryRepository.Insert(pictureBinary);
            }
            else
            {
                _pictureBinaryRepository.Update(pictureBinary);
            }

            return pictureBinary;
        }

        #endregion

        #region Getting picture local path/URL methods

        /// <summary>
        /// Gets the loaded picture binary depending on picture storage settings
        /// </summary>
        /// <param name="picture">Picture</param>
        /// <returns>Picture binary</returns>
        public virtual byte[] LoadPictureBinary(Picture picture)
        {
            return LoadPictureBinary(picture, StoreInDb);
        }

        /// <summary>
        /// Get picture SEO friendly name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Result</returns>
        public virtual string GetPictureSeName(string name)
        {
            return _urlRecordService.GetSeName(name, true, false);
        }

        /// <summary>
        /// Gets the default picture URL
        /// </summary>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="defaultPictureType">Default picture type</param>
        /// <param name="storeLocation">Store location URL; null to use determine the current store location automatically</param>
        /// <returns>Picture URL</returns>
        public virtual string GetDefaultPictureUrl(int targetSize = 0,
            PictureType defaultPictureType = PictureType.Entity,
            string storeLocation = null)
        {
            string defaultImageFileName;
            switch (defaultPictureType)
            {
                case PictureType.Avatar:
                    defaultImageFileName = _settingService.GetSettingByKey("Media.Customer.DefaultAvatarImageName", AgsMediaDefaults.DefaultAvatarFileName);
                    break;
                case PictureType.Entity:
                default:
                    defaultImageFileName = _settingService.GetSettingByKey("Media.DefaultImageName", AgsMediaDefaults.DefaultImageFileName);
                    break;
            }

            string filePath = GetPictureLocalPath(defaultImageFileName);
            if (!_fileProvider.FileExists(filePath))
            {
                return string.Empty;
            }

            if (targetSize == 0)
            {
                string url = (!string.IsNullOrEmpty(storeLocation)
                                 ? storeLocation
                                 : _webHelper.GetStoreLocation())
                                 + "images/" + defaultImageFileName;
                return url;
            }
            else
            {
                string fileExtension = _fileProvider.GetFileExtension(filePath);
                string thumbFileName = $"{_fileProvider.GetFileNameWithoutExtension(filePath)}_{targetSize}{fileExtension}";
                string thumbFilePath = GetThumbLocalPath(thumbFileName);
                if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                {
                    using (Image<Rgba32> image = Image.Load(filePath, out IImageFormat imageFormat))
                    {
                        image.Mutate(imageProcess => imageProcess.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            Size = CalculateDimensions(image.Size(), targetSize)
                        }));
                        byte[] pictureBinary = EncodeImage(image, imageFormat);
                        SaveThumb(thumbFilePath, thumbFileName, imageFormat.DefaultMimeType, pictureBinary);
                    }
                }

                string url = GetThumbUrl(thumbFileName, storeLocation);
                return url;
            }
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <param name="storeLocation">Store location URL; null to use determine the current store location automatically</param>
        /// <param name="defaultPictureType">Default picture type</param>
        /// <param name="resizeType"></param>
        /// <returns>Picture URL</returns>
        public virtual string GetPictureUrl(int pictureId,
            int targetSize = 0,
            bool showDefaultPicture = true,
            string storeLocation = null,
            PictureType defaultPictureType = PictureType.Entity)
        {
            Picture picture = GetPictureById(pictureId);
            return GetPictureUrl(picture, targetSize, showDefaultPicture, storeLocation, defaultPictureType);
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <param name="storeLocation">Store location URL; null to use determine the current store location automatically</param>
        /// <param name="defaultPictureType">Default picture type</param>
       /// <returns>Picture URL</returns>
        public virtual string GetPictureUrl(Picture picture,
            int targetSize = 0,
            bool showDefaultPicture = true,
            string storeLocation = null,
            PictureType defaultPictureType = PictureType.Entity)
        {
            string url = string.Empty;
            byte[] pictureBinary = null;
            if (picture != null)
            {
                pictureBinary = LoadPictureBinary(picture);
            }

            if (picture == null || pictureBinary == null || pictureBinary.Length == 0)
            {
                if (showDefaultPicture)
                {
                    url = GetDefaultPictureUrl(targetSize, defaultPictureType, storeLocation);
                }

                return url;
            }

            if (picture.IsNew)
            {
                DeletePictureThumbs(picture);

                //we do not validate picture binary here to ensure that no exception ("Parameter is not valid") will be thrown
                picture = UpdatePicture(picture.Id,
                    pictureBinary,
                    picture.MimeType,
                    picture.SeoFilename,
                    picture.AltAttribute,
                    picture.TitleAttribute,
                    false,
                    false);
            }

            string seoFileName = picture.SeoFilename; // = GetPictureSeName(picture.SeoFilename); //just for sure

            string lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            string thumbFileName;
            if (targetSize == 0)
            {
                thumbFileName = !string.IsNullOrEmpty(seoFileName)
                    ? $"{picture.Id:0000000}_{seoFileName}.{lastPart}"
                    : $"{picture.Id:0000000}.{lastPart}";
            }
            else
            {
                thumbFileName = !string.IsNullOrEmpty(seoFileName)
                    ? $"{picture.Id:0000000}_{seoFileName}_{targetSize}.{lastPart}"
                    : $"{picture.Id:0000000}_{targetSize}.{lastPart}";
            }

            string thumbFilePath = GetThumbLocalPath(thumbFileName);

            //the named mutex helps to avoid creating the same files in different threads,
            //and does not decrease performance significantly, because the code is blocked only for the specific file.
            using (Mutex mutex = new Mutex(false, thumbFileName))
            {
                if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                {
                    mutex.WaitOne();

                    //check, if the file was created, while we were waiting for the release of the mutex.
                    if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                    {
                        byte[] pictureBinaryResized;
                        if (targetSize != 0)
                        {
                            //resizing required
                            using (Image<Rgba32> image = Image.Load(pictureBinary, out IImageFormat imageFormat))
                            {
                                image.Mutate(imageProcess => imageProcess.Resize(new ResizeOptions
                                {
                                    Mode = ResizeMode.Max,
                                    Size = CalculateDimensions(image.Size(), targetSize)

                                }));

                                pictureBinaryResized = EncodeImage(image, imageFormat);
                            }
                        }
                        else
                        {
                            //create a copy of pictureBinary
                            pictureBinaryResized = pictureBinary.ToArray();
                        }

                        SaveThumb(thumbFilePath, thumbFileName, picture.MimeType, pictureBinaryResized);
                    }

                    mutex.ReleaseMutex();
                }
            }

            url = GetThumbUrl(thumbFileName, storeLocation);
            return url;
        }

        /// <summary>
        /// Get a picture local path
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <returns></returns>
        public virtual string GetThumbLocalPath(Picture picture, int targetSize = 0, bool showDefaultPicture = true)
        {
            string url = GetPictureUrl(picture, targetSize, showDefaultPicture);
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            return GetThumbLocalPath(_fileProvider.GetFileName(url));
        }

        #endregion

        #region CRUD methods

        /// <summary>
        /// Gets a picture
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <returns>Picture</returns>
        public virtual Picture GetPictureById(int pictureId)
        {
            if (pictureId == 0)
            {
                return null;
            }

            return _pictureRepository.GetById(pictureId);
        }

        /// <summary>
        /// Deletes a picture
        /// </summary>
        /// <param name="picture">Picture</param>
        public virtual void DeletePicture(Picture picture)
        {
            if (picture == null)
            {
                throw new ArgumentNullException(nameof(picture));
            }

            //delete thumbs
            DeletePictureThumbs(picture);

            //delete from file system
            if (!StoreInDb)
            {
                DeletePictureOnFileSystem(picture);
            }

            //delete from database
            _pictureRepository.Delete(picture);

            //event notification
            _eventPublisher.EntityDeleted(picture);
        }

        /// <summary>
        /// Gets a collection of pictures
        /// </summary>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Items on each page</param>
        /// <returns>Paged list of pictures</returns>
        public virtual IPagedList<Picture> GetPictures(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            IOrderedQueryable<Picture> query = from p in _pictureRepository.Table
                        orderby p.Id descending
                        select p;
            PagedList<Picture> pics = new PagedList<Picture>(query, pageIndex, pageSize);
            return pics;
        }
        /// <summary>
        /// GetPictureByNewsItemId
        /// </summary>
        /// <param name="newsItemId"></param>
        /// <param name="recordsToReturn"></param>
        /// <returns></returns>
        public IList<Picture> GetPictureByNewsItemId(int newsItemId, int recordsToReturn = 0)
        {
            if (newsItemId == 0)
                return new List<Picture>();

            var query = from p in _pictureRepository.Table
                join pp in _newsPictRepository.Table on p.Id equals pp.PictureId
                orderby pp.DisplayOrder, pp.Id
                where pp.NewsId == newsItemId
                select p;

            if (recordsToReturn > 0)
                query = query.Take(recordsToReturn);

            var pics = query.ToList();
            return pics;
        }
        /// <summary>
        /// GetPictureBySliderId
        /// </summary>
        /// <param name="sliderId"></param>
        /// <param name="recordsToReturn"></param>
        /// <returns></returns>
        public IList<Picture> GetPictureBySliderId(int sliderId, int recordsToReturn = 0)
        {
            if (sliderId == 0)
                return new List<Picture>();

            var query = from p in _pictureRepository.Table
                join pp in _sliderPictuRepository.Table on p.Id equals pp.PictureId
                orderby pp.DisplayOrder, pp.Id
                where pp.SliderId == sliderId
                select p;

            if (recordsToReturn > 0)
                query = query.Take(recordsToReturn);

            var pics = query.ToList();
            return pics;
        }
        /// <summary>
        /// GetPictureByGalleryId
        /// </summary>
        /// <param name="galleryId"></param>
        /// <param name="recordsToReturn"></param>
        /// <returns></returns>
        public IList<Picture> GetPictureByGalleryId(int galleryId, int recordsToReturn = 0)
        {
            if (galleryId == 0)
                return new List<Picture>();

            var query = from p in _pictureRepository.Table
                join pp in _photoGalleryRepository.Table on p.Id equals pp.PictureId
                orderby pp.DisplayOrder, pp.Id
                where pp.GalleryId == galleryId
                select p;

            if (recordsToReturn > 0)
                query = query.Take(recordsToReturn);

            var pics = query.ToList();
            return pics;
        }


        /// <summary>
        /// Inserts a picture
        /// </summary>
        /// <param name="pictureBinary">The picture binary</param>
        /// <param name="mimeType">The picture MIME type</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <param name="altAttribute">"alt" attribute for "img" HTML element</param>
        /// <param name="titleAttribute">"title" attribute for "img" HTML element</param>
        /// <param name="isNew">A value indicating whether the picture is new</param>
        /// <param name="validateBinary">A value indicating whether to validated provided picture binary</param>
        /// <returns>Picture</returns>
        public virtual Picture InsertPicture(byte[] pictureBinary, string mimeType, string seoFilename,
            string altAttribute = null, string titleAttribute = null,
            bool isNew = true, bool validateBinary = true)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            if (validateBinary)
            {
                pictureBinary = ValidatePicture(pictureBinary, mimeType);
            }

            Picture picture = new Picture
            {
                MimeType = mimeType,
                SeoFilename = seoFilename,
                AltAttribute = altAttribute,
                TitleAttribute = titleAttribute,
                IsNew = isNew
            };
            _pictureRepository.Insert(picture);
            UpdatePictureBinary(picture, StoreInDb ? pictureBinary : new byte[0]);

            if (!StoreInDb)
            {
                SavePictureInFile(picture.Id, pictureBinary, mimeType);
            }

            //event notification
            _eventPublisher.EntityInserted(picture);

            return picture;
        }

        /// <summary>
        /// Updates the picture
        /// </summary>
        /// <param name="pictureId">The picture identifier</param>
        /// <param name="pictureBinary">The picture binary</param>
        /// <param name="mimeType">The picture MIME type</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <param name="altAttribute">"alt" attribute for "img" HTML element</param>
        /// <param name="titleAttribute">"title" attribute for "img" HTML element</param>
        /// <param name="isNew">A value indicating whether the picture is new</param>
        /// <param name="validateBinary">A value indicating whether to validated provided picture binary</param>
        /// <returns>Picture</returns>
        public virtual Picture UpdatePicture(int pictureId, byte[] pictureBinary, string mimeType,
            string seoFilename, string altAttribute = null, string titleAttribute = null,
            bool isNew = true, bool validateBinary = true)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            if (validateBinary)
            {
                pictureBinary = ValidatePicture(pictureBinary, mimeType);
            }

            Picture picture = GetPictureById(pictureId);
            if (picture == null)
            {
                return null;
            }

            //delete old thumbs if a picture has been changed
            if (seoFilename != picture.SeoFilename)
            {
                DeletePictureThumbs(picture);
            }

            picture.MimeType = mimeType;
            picture.SeoFilename = seoFilename;
            picture.AltAttribute = altAttribute;
            picture.TitleAttribute = titleAttribute;
            picture.IsNew = isNew;

            _pictureRepository.Update(picture);
            UpdatePictureBinary(picture, StoreInDb ? pictureBinary : new byte[0]);

            if (!StoreInDb)
            {
                SavePictureInFile(picture.Id, pictureBinary, mimeType);
            }

            //event notification
            _eventPublisher.EntityUpdated(picture);

            return picture;
        }

        /// <summary>
        /// Updates a SEO filename of a picture
        /// </summary>
        /// <param name="pictureId">The picture identifier</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <returns>Picture</returns>
        public virtual Picture SetSeoFilename(int pictureId, string seoFilename)
        {
            Picture picture = GetPictureById(pictureId);
            if (picture == null)
            {
                throw new ArgumentException("No picture found with the specified id");
            }

            //update if it has been changed
            if (seoFilename != picture.SeoFilename)
            {
                //update picture
                picture = UpdatePicture(picture.Id,
                    LoadPictureBinary(picture),
                    picture.MimeType,
                    seoFilename,
                    picture.AltAttribute,
                    picture.TitleAttribute,
                    true,
                    false);
            }

            return picture;
        }

        /// <summary>
        /// Validates input picture dimensions
        /// </summary>
        /// <param name="pictureBinary">Picture binary</param>
        /// <param name="mimeType">MIME type</param>
        /// <returns>Picture binary or throws an exception</returns>
        public virtual byte[] ValidatePicture(byte[] pictureBinary, string mimeType)
        {
            using (Image<Rgba32> image = Image.Load(pictureBinary, out IImageFormat imageFormat))
            {
                //resize the image in accordance with the maximum size
                if (Math.Max(image.Height, image.Width) > _mediaSettings.MaximumImageSize)
                {
                    image.Mutate(imageProcess => imageProcess.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(_mediaSettings.MaximumImageSize)
                    }));
                }

                return EncodeImage(image, imageFormat);
            }
        }

        /// <summary>
        /// Get pictures hashes
        /// </summary>
        /// <param name="picturesIds">Pictures Ids</param>
        /// <returns></returns>
        public IDictionary<int, string> GetPicturesHash(int[] picturesIds)
        {
            int supportedLengthOfBinaryHash = 8000;
            if (supportedLengthOfBinaryHash == 0 || !picturesIds.Any())
            {
                return new Dictionary<int, string>();
            }

            const string strCommand = "SELECT [PictureId], HASHBYTES('sha1', substring([BinaryData], 0, {0})) as [Hash] FROM [PictureBinary] where [PictureId] in ({1})";
            return _dbContext
                .QueryFromSql<PictureHashItem>(string.Format(strCommand, supportedLengthOfBinaryHash, picturesIds.Select(p => p.ToString()).Aggregate((all, current) => all + ", " + current))).Distinct()
                .ToDictionary(p => p.PictureId, p => BitConverter.ToString(p.Hash).Replace("-", string.Empty));
        }
        public virtual string GetCategoriThumbUrl(string thumbFileName, string storeLocation = null)
        {
            storeLocation = !string.IsNullOrEmpty(storeLocation)
                                    ? storeLocation
                                    : _webHelper.GetStoreLocation();
            string url = storeLocation + "images/uploded/category/";

            if (_mediaSettings.MultipleThumbDirectories)
            {
                //get the first two letters of the file name
                string fileNameWithoutExtension = _fileProvider.GetFileNameWithoutExtension(thumbFileName);
                if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > AgsMediaDefaults.MultipleThumbDirectoriesLength)
                {
                    string subDirectoryName = fileNameWithoutExtension;
                    url = url + subDirectoryName;
                }
            }


            return url;
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the images should be stored in data base.
        /// </summary>
        public virtual bool StoreInDb
        {
            get => _settingService.GetSettingByKey("Media.Images.StoreInDB", true);
            set
            {
                //check whether it's a new value
                if (StoreInDb == value)
                {
                    return;
                }

                //save the new setting value
                _settingService.SetSetting("Media.Images.StoreInDB", value);

                int pageIndex = 0;
                const int pageSize = 400;
                try
                {
                    while (true)
                    {
                        IPagedList<Picture> pictures = GetPictures(pageIndex, pageSize);
                        pageIndex++;

                        //all pictures converted?
                        if (!pictures.Any())
                        {
                            break;
                        }

                        foreach (Picture picture in pictures)
                        {
                            byte[] pictureBinary = LoadPictureBinary(picture, !value);

                            //we used the code below before. but it's too slow
                            //let's do it manually (uncommented code) - copy some logic from "UpdatePicture" method
                            /*just update a picture (all required logic is in "UpdatePicture" method)
                            we do not validate picture binary here to ensure that no exception ("Parameter is not valid") will be thrown when "moving" pictures
                            UpdatePicture(picture.Id,
                                          pictureBinary,
                                          picture.MimeType,
                                          picture.SeoFilename,
                                          true,
                                          false);*/
                            if (value)
                            {
                                //delete from file system. now it's in the database
                                DeletePictureOnFileSystem(picture);
                            }
                            else
                            {
                                //now on file system
                                SavePictureInFile(picture.Id, pictureBinary, picture.MimeType);
                            }
                            //update appropriate properties
                            UpdatePictureBinary(picture, value ? pictureBinary : new byte[0]);
                            picture.IsNew = true;
                            //raise event?
                            //_eventPublisher.EntityUpdated(picture);
                        }

                        //save all at once
                        _pictureRepository.Update(pictures);
                        //detach them in order to release memory
                        foreach (Picture picture in pictures)
                        {
                            _dbContext.Detach(picture);
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        #endregion


    }
}
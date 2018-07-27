using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Auth.Infrastructure.Utility
{
    public enum PositionOptions
    {
        LeftAndTop,
        LeftAndCenter,
        LeftAndBottom,
        CenterAndTop,
        Center,
        CenterAndBottom,
        RightAndTop,
        RightAndCenter,
        RightAndBottom
    }

    public sealed class ImageHelper
        : IDisposable
    {
        private readonly Image _originalImage;
        private readonly ImageFormat _originalImageFromat;
        private readonly ImageCodecInfo _jpegICinfo;
        private readonly EncoderParameters _ep;


        #region 构造函数

        public ImageHelper(Stream originalStream, ImageFormat format = null)
        {
            if (originalStream == null)
                throw new ArgumentNullException(nameof(originalStream));

            _originalImage = Image.FromStream(originalStream, true);
            _originalImageFromat = format ?? _originalImage.RawFormat;
        }

        public ImageHelper(Image originalImage, ImageFormat format = null, ImageCodecInfo jpegICinfo = null, EncoderParameters ep = null)
        {
            if (originalImage == null)
                throw new ArgumentNullException(nameof(originalImage));

            _originalImage = originalImage;
            _originalImageFromat = format ?? _originalImage.RawFormat;
            _jpegICinfo = jpegICinfo;
            _ep = ep;
        }

        #endregion 构造函数

        #region 公开方法成员

        /// <summary>
        /// 添加文本水印
        /// <remarks>该方法会直接将水印直接追加在原始图片流中</remarks>
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="fontFamily">文本字体</param>
        /// <param name="fontSize">文本大小，单位像素</param>
        /// <param name="color">文本颜色，如果要设置文本透明，请设置Color中ARGB的A(0~255)</param>
        /// <param name="position">文本位置</param>
        public void AddWaterByText(string text, string fontFamily, float fontSize, Color color, PositionOptions position)
        {
            if (String.IsNullOrWhiteSpace(text)) return;

            using (var graphics = Graphics.FromImage(_originalImage))
            {
                var font = new Font(fontFamily, fontSize, GraphicsUnit.Pixel);
                var brush = new SolidBrush(color);

                //计算文本的x轴和y轴坐标
                float x, y;
                var waterSize = graphics.MeasureString(text, font);
                CalculateWaterPosition(position, waterSize.Width, waterSize.Height, out x, out y);

                graphics.DrawString(text, font, brush, x, y);
            }
        }

        /// <summary>
        /// 添加图片水印
        /// <remarks>注意：水印图片流waterImage没有进行释放，请自行释放</remarks>
        /// </summary>
        /// <param name="waterImage">水印图片</param>
        /// <param name="opacity">透明度，在 0.0f~1.0f 之间</param>
        /// <param name="position">水印位置</param>
        public void AddWaterByImage(Image waterImage, float opacity, PositionOptions position)
        {
            if (waterImage == null) return;
            if (_originalImage.Width < waterImage.Width || _originalImage.Height < waterImage.Height) return;

            var graphics = Graphics.FromImage(_originalImage);
            var colorMap = new[] { new ColorMap{
                OldColor = Color.FromArgb(255, 0, 255, 0),
                NewColor = Color.FromArgb(0, 0, 0, 0)
            }};
            float[][] colorMatrixElements = {
                new [] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                new [] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                new [] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                new [] {0.0f,  0.0f,  0.0f,  opacity, 0.0f},
                new [] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
            };
            var colorMatrix = new ColorMatrix(colorMatrixElements);

            float x, y;
            CalculateWaterPosition(position, waterImage.Width, waterImage.Height, out x, out y);

            var imageAttr = new ImageAttributes();
            imageAttr.SetRemapTable(colorMap, ColorAdjustType.Bitmap);
            imageAttr.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(waterImage, new Rectangle((int)x, (int)y, waterImage.Width, waterImage.Height), 0, 0, waterImage.Width, waterImage.Height, GraphicsUnit.Pixel, imageAttr);
        }

        /// <summary>
        /// 生成一张缩略图
        /// </summary>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        public ImageHelper CreateThumbnail(double maxWidth, double maxHeight)
        {
            if (_originalImage.Width <= maxWidth && _originalImage.Height <= maxHeight)
                return new ImageHelper(_originalImage);

            double thumbnailWidth;
            double thumbnailHeight;

            CalculateThumbnailSize(maxWidth, maxHeight, out thumbnailWidth, out thumbnailHeight);

            Image thumbnailImage = new Bitmap((int)thumbnailWidth, (int)thumbnailHeight);
            Graphics graphics = Graphics.FromImage(thumbnailImage);

            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.Clear(Color.White);

            graphics.DrawImage(_originalImage, new Rectangle(0, 0, thumbnailImage.Width, thumbnailImage.Height), new Rectangle(0, 0, _originalImage.Width, _originalImage.Height), GraphicsUnit.Pixel);
            return new ImageHelper(thumbnailImage, _originalImageFromat);
        }

        /// <summary>
        /// 生成一张 压缩质量 缩略图 
        /// </summary> 
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        /// <param name="flag">1-100</param> 
        /// <returns></returns>
        public ImageHelper GetPicThumbnail(double maxWidth, double maxHeight, int flag)
        {
            //小于0  不做质量 压缩
            if (Convert.ToInt32(maxWidth) == int.MaxValue || Convert.ToInt32(maxHeight) == int.MaxValue)
            {
                return CreateThumbnail(maxWidth, maxHeight);
            }

            double thumbnailWidth;
            double thumbnailHeight;

            CalculateThumbnailSize(maxWidth, maxHeight, out thumbnailWidth, out thumbnailHeight);

            //Image iSource =  Image.FromFile(sFile);
            Image iSource = _originalImage;
            Image newImg;
            ImageFormat tFormat = iSource.RawFormat;

            //以下代码为保存图片时，设置压缩质量  
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100  
            EncoderParameter eParam = new EncoderParameter(Encoder.Quality, qy);
            ep.Param[0] = eParam;

            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICinfo = null;

            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICinfo = arrayICI[x];
                    break;
                }
            }
            newImg = iSource.GetThumbnailImage((int)maxWidth, (int)maxHeight, null, IntPtr.Zero);
            return new ImageHelper(newImg, tFormat, jpegICinfo, ep);
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        public void Save(Stream stream)
        {
            if (_jpegICinfo != null)
            {
                _originalImage.Save(stream, _jpegICinfo, _ep);
            }
            else
            {
                _originalImage.Save(stream, _originalImageFromat);
            }

            stream.Seek(0, SeekOrigin.Begin);
        }


        #endregion 公开方法成员

        #region 私有方法成员

        /// <summary>
        /// 计算并输出图片缩略后的大小
        /// </summary>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        /// <param name="thumbnailWidth">输出计算后的宽度</param>
        /// <param name="thumbnailHeight">输出计算后的高度</param>
        private void CalculateThumbnailSize(double maxWidth, double maxHeight, out double thumbnailWidth, out double thumbnailHeight)
        {
            if (_originalImage.Width >= _originalImage.Height)
            {
                if (_originalImage.Width > maxWidth)
                {
                    thumbnailWidth = maxWidth;
                    thumbnailHeight = _originalImage.Height * (maxWidth / _originalImage.Width);
                    return;
                }
            }
            else
            {
                if (_originalImage.Height > maxHeight)
                {
                    thumbnailWidth = _originalImage.Width * (maxHeight / _originalImage.Height);
                    thumbnailHeight = maxHeight;
                    return;
                }
            }

            thumbnailWidth = _originalImage.Width;
            thumbnailHeight = _originalImage.Height;
        }

        /// <summary>
        /// 计算并输出水印图片位置坐标(x轴,y轴)
        /// </summary>
        /// <param name="position">水印图片要放置的位置</param>
        /// <param name="waterWidth">水印图片的宽度</param>
        /// <param name="waterHeight">水印图片的高度</param>
        /// <param name="x">输出计算后的x轴</param>
        /// <param name="y">输出计算后的y轴</param>
        private void CalculateWaterPosition(PositionOptions position, double waterWidth, double waterHeight, out float x, out float y)
        {
            switch (position)
            {
                case PositionOptions.LeftAndTop:
                default:
                    x = 0f;
                    y = 0f;
                    break;

                case PositionOptions.LeftAndCenter:
                    x = 0f;
                    y = (float)((_originalImage.Height - waterHeight) / 2);
                    break;

                case PositionOptions.LeftAndBottom:
                    x = 0f;
                    y = (float)(_originalImage.Height - waterHeight);
                    break;

                case PositionOptions.CenterAndTop:
                    x = (float)((_originalImage.Width - waterWidth) / 2);
                    y = 0f;
                    break;

                case PositionOptions.Center:
                    x = (float)((_originalImage.Width - waterWidth) / 2);
                    y = (float)((_originalImage.Height - waterHeight) / 2);
                    break;

                case PositionOptions.CenterAndBottom:
                    x = (float)((_originalImage.Width - waterWidth) / 2);
                    y = (float)(_originalImage.Height - waterHeight);
                    break;

                case PositionOptions.RightAndTop:
                    x = (float)(_originalImage.Width - waterWidth);
                    y = 0f;
                    break;

                case PositionOptions.RightAndCenter:
                    x = (float)(_originalImage.Width - waterWidth);
                    y = (float)((_originalImage.Height - waterHeight) / 2);
                    break;

                case PositionOptions.RightAndBottom:
                    x = (float)(_originalImage.Width - waterWidth);
                    y = (float)(_originalImage.Height - waterHeight);
                    break;
            }
        }

        #endregion 私有方法成员

        #region IDisposable 接口实现

        public void Dispose()
        {
            _originalImage?.Dispose();
        }

        #endregion IDisposable 接口实现
    }
}

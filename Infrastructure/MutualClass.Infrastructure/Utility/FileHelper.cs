using Auth.Infrastructure.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Utility
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public static class FileHelper
    {
        private static readonly string[] ImageFileExtensions = { ".gif", ".jpg", ".jpeg", ".png", ".bmp" };

        private static readonly IDictionary<string, string> FileFormats = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {".gif","7173"},
            {".jpg","255216"},
            {".jpeg","255216"},
            {".png","13780"},
            {".bmp","6677"},
            {".zip","8075"}
        };

        public static bool CheckFileFormatIsTrue(string fileName, Stream stream)
        {
            if (String.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException("fileName");

            if (!Path.HasExtension(fileName))
                throw new ArgumentException("必须携带文件的拓展名，如demo.jpg", "fileName");

            var extension = Path.GetExtension(fileName).ToLower();

            //这个对象没释放是因为关联到外部的stream，如果在这里进行释放那么外部的stream将无法操作，这并不是BUG，请不要修改。
            var reader = new BinaryReader(stream);
            try
            {
                if (!FileFormats.ContainsKey(extension))
                    return false;
                var formatCode = reader.ReadByte().ToString(CultureInfo.InvariantCulture) + reader.ReadByte();

                return formatCode == FileFormats[extension];
            }
            catch
            {
                return false;
            }
            finally
            {
                stream.Seek(0, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// 根据文件名称来验证是否是一个图片类型的文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <returns>如果是一个图片类型的文件则返回true，否则返回false。</returns>
        public static bool IsImage(string fileName)
        {
            //文件名称为空。
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            //文件扩展名为空。
            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(extension))
                return false;

            //文件扩展名是否在系统定义的文件扩展名之内。
            return ImageFileExtensions.Contains(extension.ToLower());
        }

        /// <summary>
        /// 根据文件名称的扩展名获取一个随机的文件名称。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <param name="extension">文件扩展名（需要加.）。</param>
        /// <returns>随机的文件名称。</returns>
        public static string GetRandomFileNameByFileName(string fileName, string extension = null)
        {
            if (string.IsNullOrWhiteSpace(extension))
                extension = Path.GetExtension(fileName);
            else if (!extension.StartsWith("."))
                extension = extension.Insert(0, ".");

            var name = DateTime.Now.ToString("yyyyMMddHHmmss_") + new Random().Next(1000, 9999);
            return extension == null ? name : name + extension;
        }

        /// <summary>
        /// 根据文件信息来验证是否是一个图片类型的文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <param name="stream">流。</param>
        /// <returns>如果是一个图片类型的文件则返回true，否则返回false。</returns>
        public static bool IsImage(string fileName, Stream stream)
        {
            if (stream == null || stream.Length < 2)
                return false;
            var bytes = new[] { (byte)stream.ReadByte(), (byte)stream.ReadByte() };
            stream.Seek(0, SeekOrigin.Begin);
            return IsImage(fileName, bytes);
        }

        /// <summary>
        /// 根据文件信息来验证是否是一个图片类型的文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <param name="fileBytes">文件字节组。</param>
        /// <returns>如果是一个图片类型的文件则返回true，否则返回false。</returns>
        public static bool IsImage(string fileName, byte[] fileBytes)
        {
            //文件字节组是否符合规范。
            if (fileBytes == null || fileBytes.Length < 2 || !IsImage(fileName))
                return false;
            var extension = Path.GetExtension(fileName);
            var fileCode = extension == null ? null : FileFormats[extension.ToLower()];

            //文件Code是否符合对应的文件类型。
            return fileCode != null && fileCode.Equals(fileBytes[0].ToString(CultureInfo.InvariantCulture) + fileBytes[1]);
        }

        /// <summary>
        /// 文件是否是一个Png格式的图片。
        /// </summary>
        /// <param name="fileBytes">文件字节组。</param>
        /// <returns>如果是一张Png则返回true，否则返回false。</returns>
        public static bool IsPng(byte[] fileBytes)
        {
            return IsFormat(".png", fileBytes);
        }

        /// <summary>
        /// 文件是否是一个Zip格式的文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <returns>如果是Zip文件则返回true，否则返回false。</returns>
        public static bool IsZip(string fileName)
        {
            fileName.NotEmptyOrWhiteSpace("fileName");

            return string.Equals(".zip", Path.GetExtension(fileName), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 文件是否是一个Zip格式的文件。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <param name="stream">文件流。</param>
        /// <returns>如果是Zip文件则返回true，否则返回false。</returns>
        public static bool IsZip(string fileName, Stream stream)
        {
            stream.NotNull("stream");
            if (!IsZip(fileName) || stream.Length < 2)
                return false;

            var bytes = new[] { (byte)stream.ReadByte(), (byte)stream.ReadByte() };

            stream.Seek(0, SeekOrigin.Begin);

            return IsFormat(".zip", bytes);
        }

        /// <summary>
        /// 文件是否是一个Zip格式的文件。
        /// </summary>
        /// <param name="fileBytes">文件字节组。</param>
        /// <returns>如果是Zip文件则返回true，否则返回false。</returns>
        public static bool IsZip(byte[] fileBytes)
        {
            return IsFormat(".zip", fileBytes);
        }

        private static bool IsFormat(string extension, byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length < 2 || !FileFormats.ContainsKey(extension))
                return false;

            var fileCode = FileFormats[extension];
            return fileCode.Equals(fileBytes[0].ToString(CultureInfo.InvariantCulture) + fileBytes[1]);
        }
    }
}

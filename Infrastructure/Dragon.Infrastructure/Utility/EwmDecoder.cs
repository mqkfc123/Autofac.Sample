using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace Dragon.Infrastructure.Utility
{
    public class EwmDecoder
    {
        /// <summary>
        /// 二维码解码
        /// </summary>
        /// <param name="filePath">图片路径</param>
        /// <returns></returns>
        public static string CodeDecoder(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                return "";
            Bitmap myBitmap = new Bitmap(System.Drawing.Image.FromFile(filePath));
            QRCodeDecoder decoder = new QRCodeDecoder();
            return decoder.decode(new QRCodeBitmapImage(myBitmap));
        }
    }
}

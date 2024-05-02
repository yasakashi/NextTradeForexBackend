using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRCoder;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;


namespace Base.Common.Generator
{
    public class QrCodeGenerator
    {
        //public Byte[] GeneratorQrCode(string strQrCode)
        //{
        //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //    QRCodeData _qrCodeData = qrGenerator.CreateQrCode(strQrCode, QRCodeGenerator.ECCLevel.Q);
        //    QRCode qrCode = new QRCode(_qrCodeData);
        //    Bitmap qrCodeImage = qrCode.GetGraphic(20);
        //    return BitmapToBytesCode(qrCodeImage);
        //}

        //private static Byte[] BitmapToBytesCode(Bitmap image)
        //{
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        image.Save(stream, ImageFormat.Png);
        //        return stream.ToArray();
        //    }
        //}
    }
}

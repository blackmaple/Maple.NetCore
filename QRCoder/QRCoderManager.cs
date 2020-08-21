using QRCoder;
using System.Drawing;

namespace Maple.NetCore
{
    [CDISingleton(typeof(IQRCoderManager))]
    public class CQRCoderManager : IQRCoderManager
    {
        public Bitmap BuildQRCodeBitmap(string content, int pixel = 20)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.M, true);
                using (var qrCode = new QRCode(qrCodeData))
                {
                    var qrCodeImage = qrCode.GetGraphic(pixel, Color.Black, Color.White, true);
                    return qrCodeImage;
                }
            }
        }

        public string BuildQRCodeBase64(string content, int pixel = 20)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.M, true);
                using (var qrCode = new PngByteQRCode(qrCodeData))
                {
                    var qrCodeImage = qrCode.GetGraphic(pixel);
                    var base64 = qrCodeImage.ToBase64();
                    return base64;
                }
            }
        }


    }
}

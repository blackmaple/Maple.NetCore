using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore
{
    public interface IQRCoderManager
    {
        System.DrawingCore.Bitmap BuildQRCodeBitmap(string content, int pixel = 20);
        string BuildQRCodeBase64(string content, int pixel = 20);
    }
}

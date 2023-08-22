
// Type: CashSwiftUtil.Imaging.ImageManipuation
// Assembly: CashSwiftUtil, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 885F1C6C-21D2-4135-B89E-154B0975A233
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwiftUtil.dll

using CashSwift.Library.Standard.Utilities;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace CashSwiftUtil.Imaging
{
    public class ImageManipuation
    {
        public static BitmapImage GetBitmapFromFile(string path)
        {
            BitmapImage bitmapFromFile = new BitmapImage(path.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory + "\\Resources").Replace("{ResourceDir}", "pack://application:,,,").ToURI());
            bitmapFromFile.Freeze();
            return bitmapFromFile;
        }

        public static BitmapImage GetBitmapFromBytes(byte[] blob)
        {
            if (blob == null)
                return null;
            MemoryStream memoryStream1 = new MemoryStream();
            memoryStream1.Write(blob, 0, blob.Length);
            memoryStream1.Position = 0L;
            Image image = Image.FromStream(memoryStream1);
            BitmapImage bitmapFromBytes = new BitmapImage();
            bitmapFromBytes.BeginInit();
            MemoryStream memoryStream2 = new MemoryStream();
            MemoryStream memoryStream3 = memoryStream2;
            ImageFormat png = ImageFormat.Png;
            image.Save(memoryStream3, png);
            memoryStream2.Seek(0L, SeekOrigin.Begin);
            bitmapFromBytes.StreamSource = memoryStream2;
            bitmapFromBytes.EndInit();
            bitmapFromBytes.Freeze();
            return bitmapFromBytes;
        }
    }
}

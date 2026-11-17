using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CRUD_App {
    internal static class Utility {
        public static BitmapImage ByteArrayToBitmapImage(byte[] byteArray) {
            MemoryStream memory = new MemoryStream(byteArray);
            BitmapImage imgSource = new BitmapImage();
            imgSource.BeginInit();
            imgSource.StreamSource = memory;
            imgSource.EndInit();
            return imgSource;
        }
    }
}

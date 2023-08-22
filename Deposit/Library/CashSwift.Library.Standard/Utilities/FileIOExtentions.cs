// Utilities.FileIOExtentions


using System.IO;

namespace CashSwift.Library.Standard.Utilities
{
    public static class FileIOExtentions
    {
        public static void AppendAllBytes(string path, byte[] bytes)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Append))
                fileStream.Write(bytes, 0, bytes.Length);
        }
    }
}

using System.IO;

namespace LibDescent.Tests
{
    static class TestUtils
    {
        public static byte[] GetArrayFromResourceStream(string resourceName)
        {
            var resourceStream = typeof(TestUtils).Assembly.GetManifestResourceStream(typeof(TestUtils), resourceName);
            var memoryStream = new MemoryStream();
            resourceStream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}

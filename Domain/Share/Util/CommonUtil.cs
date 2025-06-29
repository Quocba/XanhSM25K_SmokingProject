using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Domain.Share.Util
{
    public static class CommonUtil
    {
        public static string HTMLLoading(string fileName)
        {
            var htmlAssembly = typeof(HTMLEmail.HTMLEmail.EmailTemplateMarker).Assembly;

            var resourceName = htmlAssembly.GetManifestResourceNames()
                .FirstOrDefault(name => name.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));

            if (resourceName == null)
                throw new FileNotFoundException($"Không tìm thấy resource: {fileName}");

            using var stream = htmlAssembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream ?? throw new InvalidOperationException("Stream is null"));
            return reader.ReadToEnd();
        }

        public static long GenerateOrderCode()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}

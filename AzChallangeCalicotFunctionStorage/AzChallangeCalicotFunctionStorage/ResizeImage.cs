using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AzChallangeCalicotFunctionStorage
{
    public static class ResizeImage
    {
        [FunctionName("ResizeImage")]
        public static void Run([BlobTrigger("photos/{name}",
            Connection = "BlobStorageConnectionString")]Stream image,
             [Blob("thumb/{name}", FileAccess.Write)] Stream imageSmall,
            string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {image.Length} Bytes");

            IImageFormat format;

            using (Image<Rgba32> input = Image.Load<Rgba32>(image, out format))
            {
                ResizeImages(input, imageSmall, ImageSize.ExtraSmall, format);
            }
        }

        public static void ResizeImages(Image<Rgba32> input, Stream output, ImageSize size, IImageFormat format)
        {
            // var dimensions = imageDimensionsTable[size];
            var dimensions = CalculateDimensionsThumb(input);

            input.Mutate(x => x.Resize(dimensions.Item1, dimensions.Item2));
            input.Save(output, format);
        }

        public enum ImageSize { ExtraSmall, Small, Medium }

        private static Dictionary<ImageSize, (int, int)> imageDimensionsTable = new Dictionary<ImageSize, (int, int)>() {
        { ImageSize.ExtraSmall, (320, 200) },
        { ImageSize.Small,      (640, 400) },
        { ImageSize.Medium,     (800, 600) }
    };

        private static Tuple<int, int> CalculateDimensionsThumb(Image<Rgba32> image)
        {
            // calculate a 250px thumbnail
            int width;
            int height;
            if (image.Width > image.Height)
            {
                width = 250;
                height = 250 * image.Height / image.Width;
            }
            else
            {
                height = 250;
                width = 250 * image.Width / image.Height;
            }

            return new Tuple<int, int>(width, height);
        }
    }

}

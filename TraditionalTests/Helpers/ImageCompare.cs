using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Hackathon.TraditionalTests.Helpers
{
    internal class ImageCompare
    {
        // Adapted from here: https://stackoverflow.com/questions/35151067/algorithm-to-compare-two-images-in-c-sharp

        public static string ScreenshotsPath = AppDomain.CurrentDomain.BaseDirectory + "TraditionalTests\\Screenshots" + Path.DirectorySeparatorChar;

        public static List<bool> GetHash(Bitmap bmpSource)
        {
            List<bool> lResult = new List<bool>();
            //create new image with 16x16 pixel
            Bitmap bmpMin = new Bitmap(bmpSource, new Size(16, 16));
            for (int j = 0; j < bmpMin.Height; j++)
            {
                for (int i = 0; i < bmpMin.Width; i++)
                {
                    //reduce colors to true / false
                    lResult.Add(bmpMin.GetPixel(i, j).GetBrightness() < 0.5f);
                }
            }
            bmpMin.Dispose();
            return lResult;
        }

        public static bool CompareImages(string baselinePath, string screenshotPath)
        {
            List<bool> iHash1 = GetHash(new Bitmap(baselinePath));
            List<bool> iHash2 = GetHash(new Bitmap(screenshotPath));

            //determine the number of equal pixel (x of 256)
            int equalElements = iHash1.Zip(iHash2, (i, j) => i == j).Count(eq => eq);

            return equalElements == 256;
        }
    }
}
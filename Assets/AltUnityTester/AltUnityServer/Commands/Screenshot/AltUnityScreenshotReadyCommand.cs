using Altom.Server.Logging;
using Newtonsoft.Json;
using NLog;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityScreenshotReadyCommand : AltUnityCommand
    {
        private static readonly Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();
        UnityEngine.Vector2 size;
        int quality;

        public AltUnityScreenshotReadyCommand(params string[] parameters) : base(parameters, 4)
        {
            this.size = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[2]);
            this.quality = JsonConvert.DeserializeObject<int>(parameters[3]);
        }

        public override string Execute()
        {
            var screenshot = UnityEngine.ScreenCapture.CaptureScreenshotAsTexture();
            int width = (int)size.x;
            int height = (int)size.y;

            quality = UnityEngine.Mathf.Clamp(quality, 1, 100);
            if (width == 0 && height == 0)
            {
                width = screenshot.width;
                height = screenshot.height;
            }
            else
            {
                var heightDifference = screenshot.height - height;
                var widthDifference = screenshot.width - width;
                if (heightDifference >= 0 || widthDifference >= 0)
                {
                    if (heightDifference > widthDifference)
                    {
                        width = height * screenshot.width / screenshot.height;
                    }
                    else
                    {
                        height = width * screenshot.height / screenshot.width;
                    }
                }

            }
            string[] fullResponse = new string[5];

            fullResponse[0] = JsonConvert.SerializeObject(new UnityEngine.Vector2(screenshot.width, screenshot.height), new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            width = width * quality / 100;
            height = height * quality / 100;
            AltUnityTextureScale.Bilinear(screenshot, width, height);
            screenshot.Compress(false);
            screenshot.Apply(false);


            var screenshotSerialized = screenshot.GetRawTextureData();

            logger.Trace("Start Compression");
            var screenshotCompressed = AltUnityRunner.CompressScreenshot(screenshotSerialized);
            logger.Trace("Finished Compression");

            var length = screenshotCompressed.LongLength;
            fullResponse[1] = length.ToString();

            var format = screenshot.format;
            fullResponse[2] = format.ToString();

            var newSize = new UnityEngine.Vector3(screenshot.width, screenshot.height);
            fullResponse[3] = JsonConvert.SerializeObject(newSize, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            logger.Trace("Start serialize screenshot");
            fullResponse[4] = JsonConvert.SerializeObject(screenshotCompressed, new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            });
            logger.Trace("Finished Serialize Screenshot");

            screenshot.Apply(false, true);
            UnityEngine.Object.DestroyImmediate(screenshot);
            AltUnityRunner._altUnityRunner.destroyHightlight = true;
            return JsonConvert.SerializeObject(fullResponse);
        }
    }
}

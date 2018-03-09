using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CSHttpClientSample
{
    static class Program
    {
        // **********************************************
        // *** Update or verify the following values. ***
        // **********************************************

        // Replace the subscriptionKey string value with your valid subscription key.
        const string subscriptionKey = "8f966ebea21f4261ae60d8c57382f157";

       
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0";
        private static string result = "";

        static void Main()
        {
            Console.WriteLine("Detect faces:");
            Console.Write("Enter the path to an image with faces that you wish to analyze: ");
            string imageFilePath = "C:\\Photo\\3.jpg";

            MakeAnalysisRequest(imageFilePath);

            Console.WriteLine("\nPlease wait a moment for the results to appear. Then, press Enter to exit...\n");
            Console.ReadLine();
        }


        /// <summary>
        /// Gets the analysis of the specified image file by using the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file.</param>
        static async void MakeAnalysisRequest(string imageFilePath)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                result = await response.Content.ReadAsStringAsync();
                //string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                Console.WriteLine("\nResponse:\n");
            }
                List<FaceObject> lst = new List<FaceObject>();
                try
                {
                    lst = JsonConvert.DeserializeObject<List<FaceObject>>(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                foreach (var item in lst)
                {
                    Console.WriteLine($"Face:                      \t{lst.IndexOf(item) + 1}");
                    Console.WriteLine($"Anger:                     \t{item.scores.anger}");
                    Console.WriteLine($"Contempt:                  \t{item.scores.contempt}");
                    Console.WriteLine($"Disgust:                   \t{item.scores.disgust}");
                    Console.WriteLine($"Fear:                      \t{item.scores.fear}");
                    Console.WriteLine($"Happiness:                 \t{item.scores.happiness}");
                    Console.WriteLine($"Neutral:                   \t{item.scores.neutral}");
                    Console.WriteLine($"Sadness:                   \t{item.scores.sadness}");
                    Console.WriteLine($"Surprise:                  \t{item.scores.surprise}");
                    Console.WriteLine($"The most bright emotion is:\t{MaxEmotion(item.scores)}");
                    Console.WriteLine("_____________________________________________________");
                }
           }

            // Console.WriteLine(JsonPrettyPrint(result));
        }
    }


        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }



static string JsonPrettyPrint(string json)
{
    if (string.IsNullOrEmpty(json))
        return string.Empty;

    json = json.Replace(Environment.NewLine, "").Replace("\t", "");

    StringBuilder sb = new StringBuilder();
    bool quote = false;
    bool ignore = false;
    int offset = 0;
    int indentLength = 3;

    //    foreach (char ch in json)
    //    {
    //        switch (ch)
    //        {
    //            case '"':
    //                if (!ignore) quote = !quote;
    //                break;
    //            case '\'':
    //                if (quote) ignore = !ignore;
    //                break;
    //        }

    //        if (quote)
    //            sb.Append(ch);
    //        else
    //        {
    //            switch (ch)
    //            {
    //                case '{':
    //                case '[':
    //                    sb.Append(ch);
    //                    sb.Append(Environment.NewLine);
    //                    sb.Append(new string(' ', ++offset * indentLength));
    //                    break;
    //                case '}':
    //                case ']':
    //                    sb.Append(Environment.NewLine);
    //                    sb.Append(new string(' ', --offset * indentLength));
    //                    sb.Append(ch);
    //                    break;
    //                case ',':
    //                    sb.Append(ch);
    //                    sb.Append(Environment.NewLine);
    //                    sb.Append(new string(' ', offset * indentLength));
    //                    break;
    //                case ':':
    //                    sb.Append(ch);
    //                    sb.Append(' ');
    //                    break;
    //                default:
    //                    if (ch != ' ') sb.Append(ch);
    //                    break;
    //            }
    //        }
    //    }

    return sb.ToString().Trim();
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FaceTry2
{
    static class Program
    {

        const string subscriptionKey = "8f966ebea21f4261ae60d8c57382f157";
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0";

        private static List<string> imagePaths = new List<string>
        {
            @"C:\Photo\1.jpg",
            @"C:\Photo\2.jpg",
            @"C:\Photo\3.jpg",
            @"C:\Photo\4.jpg",
            @"C:\Photo\5.jpg",
            @"C:\Photo\6.jpg"
        };

        private static string result = "";
        private static int photo = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Detect faces:");
            Console.WriteLine($"There are {imagePaths.Count} photo's links in list. Please, choose one: ");
            photo = Int32.Parse(Console.ReadLine()) - 1;
            // Console.Write("Enter the path to an image with faces that you wish to analyze: ");
            //    string imageFilePath = "C:\\Photo\\3.jpg";

            MakeAnalysisRequest();
            Console.ReadKey();
        }

        static byte[] GetImage(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader bin = new BinaryReader(file);
            return bin.ReadBytes((int)file.Length);
        }

        static async void MakeAnalysisRequest()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;
            byte[] byteData = GetImage(imagePaths[photo]);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                result = await response.Content.ReadAsStringAsync();
                Console.WriteLine("\nResponse:\n");
                //   Console.WriteLine(JsonPrettyPrint(result));
            }
            List<FaceObject> json = new List<FaceObject>();
            json = JsonConvert.DeserializeObject<List<FaceObject>>(result);
            foreach (var item in json)
            {
                Console.WriteLine($"Face:                      \t{json.IndexOf(item) + 1}");
                Console.WriteLine($"Anger:                     \t{item.scores.anger}");
                Console.WriteLine($"Contempt:                  \t{item.scores.contempt}");
                Console.WriteLine($"Disgust:                   \t{item.scores.disgust}");
                Console.WriteLine($"Fear:                      \t{item.scores.fear}");
                Console.WriteLine($"Happiness:                 \t{item.scores.happiness}");
                Console.WriteLine($"Neutral:                   \t{item.scores.neutral}");
                Console.WriteLine($"Sadness:                   \t{item.scores.sadness}");
                Console.WriteLine($"Surprise:                  \t{item.scores.surprise}");
                //  Console.WriteLine($"The most bright emotion is:\t{MaxEmotion(item.scores)}");
                Console.WriteLine("_____________________________________________________");
            }
        }
        static string MaxEmotion(Scores score)
        {
            string result = "";
            double max = 0;
            if (score.anger > max)
            {
                result = "anger";
                max = score.anger;
            }
            if (score.contempt > max)
            {
                result = "contempt";
                max = score.contempt;
            }
            if (score.disgust > max)
            {
                result = "disgust";
                max = score.disgust;
            }
            if (score.fear > max)
            {
                result = "fear";
                max = score.fear;
            }
            if (score.happiness > max)
            {
                result = "happines";
                max = score.happiness;
            }
            if (score.neutral > max)
            {
                result = "neutral";
                max = score.neutral;
            }
            if (score.sadness > max)
            {
                result = "sadness";
                max = score.sadness;
            }
            if (score.surprise > max)
            {
                result = "surprise";
                max = score.surprise;
            }
            return result;
        }
    }
}


        //foreach (char ch in json)
        //{

        //switch (ch)
        //{
        //    case '"':
        //        if (!ignore) quote = !quote;
        //        break;
        //    case '\'':
        //        if (quote) ignore = !ignore;
        //        break;
        //}

        //if (quote)
        //    sb.Append(ch);
        //else
        //{
        //    switch (ch)
        //    {
        //        case '{':
        //        case '[':
        //            sb.Append(ch);
        //            sb.Append(Environment.NewLine);
        //            sb.Append(new string(' ', ++offset * indentLength));
        //            break;
        //        case '}':
        //        case ']':
        //            sb.Append(Environment.NewLine);
        //            sb.Append(new string(' ', --offset * indentLength));
        //            sb.Append(ch);
        //            break;
        //        case ',':
        //            sb.Append(ch);
        //            sb.Append(Environment.NewLine);
        //            sb.Append(new string(' ', offset * indentLength));
        //            break;
        //        case ':':
        //            sb.Append(ch);
        //            sb.Append(' ');
        //            break;
        //        default:
        //            if (ch != ' ') sb.Append(ch);
        //            break;
        //    }
        //}
        // }

        // return sb.ToString().Trim();
   //  }
//}
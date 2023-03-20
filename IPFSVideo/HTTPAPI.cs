using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.DataFormats;
using MyToolkit;

namespace IPFSVideo
{
    public class HttpClientAPI
    {
        public readonly HttpClient HttpApi = new();

        public HttpClientAPI()
        {
            //HttpRequest.BaseAddress = new Uri("http://localhost:5001/api/v0/");
        }

        public string GetMessage()
        {
            var respones = HttpApi.GetAsync("http://baidu.com");
            var stream = respones.Result.Content.ReadAsStringAsync();
            return stream.Result;
            //HttpContent postContent = new MultipartFormDataContent();
            //postContent.Add(new StreamContent());
            //var respones2 = HttpRequest.PostAsync("http://baidu.com", postContent);
        }
        /// <summary>
        /// 建立命令Uri
        /// </summary>
        /// <param name="command">IPFS HTTP API 指令</param>
        /// <param name="arg">关键参数</param>
        /// <param name="options">其余参数</param>
        /// <returns></returns>
        public static Uri BuildCommand(string command, string? arg = null, params string[] options)
        {
            var url = "/api/v0/" + command;

            var q = new StringBuilder();
            if (arg != null)
            {
                q.Append("&arg=");
                q.Append(WebUtility.UrlEncode(arg));
            }

            foreach (var option in options)
            {
                q.Append('&');
                var i = option.IndexOf('=');
                if (i < 0)
                {
                    q.Append(option);
                }
                else
                {
                    q.Append(option.Substring(0, i));
                    q.Append('=');
                    q.Append(WebUtility.UrlEncode(option.Substring(i + 1)));
                }
            }

            if (q.Length > 0)
            {
                q[0] = '?';
                q.Insert(0, url);
                url = q.ToString();
            }

            return new Uri(new Uri(Environment.GetEnvironmentVariable("IpfsHttpApi")
                    ?? "http://localhost:5001"), url);
        }

        public async Task<string> DoCommandAsync(Uri command)
        {
            return await HttpApi.PostAsync(command, null).Result.Content.ReadAsStringAsync();
        }

        public async Task<Stream> DownloadAsync(Uri command)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, command);
            var response = await HttpApi.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<string> UploadAsync(Uri command, StreamContent streamContent, string? name = null)
        {
            var content = new MultipartFormDataContent();
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            if (string.IsNullOrEmpty(name))
                content.Add(streamContent, "file", "unknown");
            else
                content.Add(streamContent, "file", name);

            var response = await HttpApi.PostAsync(command, content);
            //await ThrowOnErrorAsync(response);
            var json = await response.Content.ReadAsStringAsync();
            return json;
        }

        public string Add(string path)
        {
            string url = $"http://localhost:5001/api/v0/add{path}?chunker=size-262144&recursive=false";

            HttpContent content = new StreamContent(FileManager.GetFileStream("test"));
            var respones = HttpApi.PostAsync(url, content);
            return respones.Result.Content.ReadAsStringAsync().Result;
        }
    }
}

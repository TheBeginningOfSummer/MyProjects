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
using System.Net.Http;

namespace IPFSVideo
{
    public class HTTPAPI
    {
        //Mutipart上传代码如下
        public static string HttpMultiPartPost(string url, int timeOut, string path, bool isDir)
        {
            string responseContent;
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            // 边界符  
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            // 边界符  
            var beginBoundary = Encoding.ASCII.GetBytes("\r\n" + "--" + boundary + "\r\n");
            // 最后的结束符  
            var endBoundary = Encoding.ASCII.GetBytes("\r\n" + "--" + boundary + "--\r\n");
            // 设置属性  
            webRequest.Method = "POST";
            webRequest.Timeout = timeOut;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            var requestStream = webRequest.GetRequestStream();
            if (isDir)
            {
                //发送分割符
                requestStream.Write(beginBoundary, 0, beginBoundary.Length);
                //发送文件夹头
                const string folderHeaderFormat =
                "Content-Disposition: file; filename=\"{0}\"\r\n" +
                "Content-Type: application/x-directory\r\n\r\n";
                var folderHeader = string.Format(folderHeaderFormat, GetDirectoryName(path));
                var headerbytes = Encoding.UTF8.GetBytes(folderHeader);
                requestStream.Write(headerbytes, 0, headerbytes.Length);
                DirectoryInfo directory = new DirectoryInfo(path);
                FileInfo[] fileInfo = directory.GetFiles();
                foreach (FileInfo item in fileInfo)
                {
                    PostFileItem(requestStream, beginBoundary, item, GetDirectoryName(path));
                }
            }
            else
            {
                PostFileItem(requestStream, beginBoundary, new FileInfo(path), null);
            }
            // 写入最后的结束边界符  
            requestStream.Write(endBoundary, 0, endBoundary.Length);
            requestStream.Close();
            var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
            using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8")))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }
            httpWebResponse.Close();
            webRequest.Abort();
            return responseContent;
        }

        private static string GetDirectoryName(string path)
        {
            throw new NotImplementedException();
        }

        private static void PostFileItem(Stream stream, byte[] boundary, FileInfo fileInfo, string dirName)
        {
            string name = fileInfo.Name;
            if (!string.IsNullOrEmpty(dirName))
            {
                name = dirName + "/" + name;
            }
            //发送分割符
            stream.Write(boundary, 0, boundary.Length);
            //发送文件头
            const string headerFormat =
                            "Abspath: {0}\r\n" +
                            "Content-Disposition: file; filename=\"{1}\"\r\n" +
                            "Content-Type: application/octet-stream\r\n\r\n";
            var header = string.Format(headerFormat, fileInfo.FullName, name);
            var headerbytes = Encoding.UTF8.GetBytes(header);
            stream.Write(headerbytes, 0, headerbytes.Length);
            //发送文件流
            var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
            var buffer = new byte[1024];
            int bytesRead; // =0  
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                stream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();
        }

        //add Api调用代码如下：
        public static string Add(string path, bool isDir)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            string format = "";
            if (isDir)
            {
                format = "{0}?chunker=size-262144&recursive=true";
            }
            else
            {
                //如果需要保存名字，需要wrap一个dir
                //format = "{0}?chunker=size-262144&recursive=false&wrap-with-directory=true";
                format = "{0}?chunker=size-262144&recursive=false";
            }
            string url = string.Format(format, "http://localhost:5001/api/v0/add");
            string output = HttpMultiPartPost(url, 500000, path, isDir);
            return output;
        }

        //大部分的api都是以get的方式请求，相比add简单许多，这里提供一个C#的httpget方法供参考.
        public static string HttpGet(string url, int timeOut)
        {
            string responseContent;
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            webRequest.Timeout = timeOut;
            var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
            using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8")))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }
            httpWebResponse.Close();
            webRequest.Abort();
            return responseContent;
        }

        //如name publish这样的操作就可以这样调用
        public static string NamePublish(string hash, int hours)
        {
            //lifetime失效期
            string format = "{0}?arg={1}&lifetime={2}h";
            string url = string.Format(format, @"http://localhost:5001/api/v0/name/publish", hash, hours);
            return HttpGet(url, 100000);
        }

    }

    public class HttpClientAPI
    {
        public readonly HttpClient HttpApi = new();

        public HttpClientAPI()
        {
            
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
        /// <summary>
        /// 发送指令
        /// </summary>
        /// <param name="command">指令</param>
        /// <returns>json格式返回值</returns>
        public async Task<string> DoCommandAsync(Uri command)
        {
            return await HttpApi.PostAsync(command, null).Result.Content.ReadAsStringAsync();
        }
        /// <summary>
        /// 下载流信息
        /// </summary>
        /// <param name="command">下载指令</param>
        /// <returns>信息流</returns>
        public async Task<Stream> DownloadAsync(Uri command)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, command);
            var response = await HttpApi.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStreamAsync();
        }
        /// <summary>
        /// 下载流信息
        /// </summary>
        /// <param name="command">下载指令</param>
        /// <returns>信息流</returns>
        public async Task<string> DownloadStringAsync(Uri command)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, command);
            var response = await HttpApi.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStringAsync();
        }
        /// <summary>
        /// 上传数据
        /// </summary>
        /// <param name="command">上传指令</param>
        /// <param name="streamContent">数据</param>
        /// <param name="name">名称</param>
        /// <param name="options">条件</param>
        /// <returns>上传结果，json格式</returns>
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
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> AddAsync(Stream stream, string name = "", AddFileOptions? options = null)
        {
            options ??= new AddFileOptions();
            var opts = new List<string>();
            if (!options.Pin) opts.Add("pin=false");
            if (options.Wrap) opts.Add("wrap-with-directory=true");
            if (options.RawLeaves) opts.Add("raw-leaves=true");
            if (options.OnlyHash) opts.Add("only-hash=true");
            if (options.Trickle) opts.Add("trickle=true");
            //if (options.Progress != null)opts.Add("progress=true");
            if (options.Hash != "sha2-256") opts.Add($"hash=${options.Hash}");
            if (options.Encoding != "base58btc") opts.Add($"cid-base=${options.Encoding}");
            if (!string.IsNullOrWhiteSpace(options.ProtectionKey)) opts.Add($"protect={options.ProtectionKey}");
            opts.Add($"chunker=size-{options.ChunkSize}");
            opts.Add($"recursive=false");
            //opts.Add("progress=true");
            //opts.Add($"cid-version=1");

            StreamContent content = new(stream);
            return await UploadAsync(BuildCommand("add", null, opts.ToArray()), content, name);
        }

    }

    public class AddFileOptions
    {
        /// <summary>
        /// Determines if the data is pinned to local storage.
        /// If true the data is pinned to local storage and will not be garbage collected.
        ///  The default is true.
        /// </summary>
        public bool Pin { get; set; } = true;

        /// <summary>
        /// The maximum number of data bytes in a block.
        /// The default is 256 * 1024 (‭262,144) bytes.‬
        /// </summary>
        public int ChunkSize { get; set; } = 262144;

        /// <summary>
        /// Determines if the trickle-dag format is used for dag generation.
        /// The default is false.
        /// </summary>
        public bool Trickle { get; set; }

        /// <summary>
        /// Determines if added file(s) are wrapped in a directory object.
        /// The default is false.
        /// </summary>
        public bool Wrap { get; set; }

        /// <summary>
        /// Determines if raw blocks are used for leaf data blocks.
        /// The default is false.
        /// RawLeaves and Ipfs.CoreApi.AddFileOptions.ProtectionKey are mutually exclusive.
        /// </summary>
        public bool RawLeaves { get; set; }
 
        /// <summary>
        /// The hashing algorithm name to use.
        /// The Ipfs.MultiHash algorithm name used to produce the Ipfs.Cid. Defaults to Ipfs.MultiHash.DefaultAlgorithmName.
        /// </summary>
        public string Hash { get; set; } = "sha2-256";

        /// <summary>
        /// The encoding algorithm name to use.
        /// The Ipfs.MultiBase algorithm name used to produce the Ipfs.Cid. Defaults to Ipfs.MultiBase.DefaultAlgorithmName.
        /// </summary>
        public string Encoding { get; set; } = "base58btc";

        /// <summary>
        /// Determines if only file information is produced.
        /// If true no data is added to IPFS. The default is false.
        /// </summary>
        public bool OnlyHash { get; set; }

        /// <summary>
        /// The key name used to protect (encrypt) the file contents.
        /// The name of an existing key.
        /// ProtectionKey and Ipfs.CoreApi.AddFileOptions.RawLeaves are mutually exclusive.
        /// </summary>
        public string? ProtectionKey { get; set; }

        //
        // 摘要:
        //     Used to report the progress of a file transfer.
        //public IProgress<TransferProgress> Progress { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using IPFS.Models;
using System.Net.Http;

namespace IPFSVideo
{
    public class HttpClientAPI
    {
        public readonly HttpClient HttpApi = new();

        public HttpClientAPI()
        {
            HttpApi.Timeout = TimeSpan.FromSeconds(300);
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

        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="command">指令</param>
        /// <returns>结果（json）</returns>
        public async Task<string> DoCommandAsync(Uri command)
        {
            return await HttpApi.PostAsync(command, null).Result.Content.ReadAsStringAsync();
        }
        /// <summary>
        /// 下载，返回流信息
        /// </summary>
        /// <param name="command">下载指令</param>
        /// <returns>下载的流</returns>
        public async Task<Stream> DownloadAsync(Uri command)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, command);
            var response = await HttpApi.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStreamAsync();
        }
        /// <summary>
        /// 上传，返回字符串
        /// </summary>
        /// <param name="command">上传指令</param>
        /// <param name="streamContent">上传的流</param>
        /// <param name="name">文件名称</param>
        /// <returns>上传结果</returns>
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
        /// <summary>
        /// 上传，返回流信息
        /// </summary>
        /// <param name="command">上传指令</param>
        /// <param name="streamContent">上传的流</param>
        /// <param name="name">文件名称</param>
        /// <returns>返回的结果流</returns>
        public async Task<Stream> UploadGetStreamAsync(Uri command, HttpContent streamContent, string? name = null)
        {
            var content = new MultipartFormDataContent();
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            if (string.IsNullOrEmpty(name))
                content.Add(streamContent, "file", "unknown");
            else
                content.Add(streamContent, "file", name);

            var response = await HttpApi.PostAsync(command, content);
            //await ThrowOnErrorAsync(response);
            
            return await response.Content.ReadAsStreamAsync();
        }
        /// <summary>
        /// 处理上传结果流
        /// </summary>
        /// <param name="response">上传结果</param>
        /// <param name="options">设置</param>
        /// <param name="name">文件名</param>
        /// <param name="fileLength">文件长度</param>
        /// <returns></returns>
        public static async Task<FileData?> ReadStreamAsync(Stream response, AddFileOptions options, string name = "", long fileLength = 0)
        {
            FileData? fileData = null;
            if (response != null)
            {
                using var sr = new StreamReader(response);
                using var jr = new JsonTextReader(sr) { SupportMultipleContent = true };
                while (jr.Read())
                {
                    JObject r = await JObject.LoadAsync(jr);
                    // If a progress report.
                    if (r.ContainsKey("Bytes"))
                    {
                        options.Progress?.Report($"Name:{r["Name"]} Bytes:{r["Bytes"]} Length:{fileLength}");
                        //options.Progress?.Report(new TransferProgress
                        //{
                        //    Name = (string)r["Name"]!,
                        //    Bytes = (ulong)r["Bytes"]!,
                        //    AllLength = fileLength
                        //});
                    }
                    // Else must be an added file.
                    else
                    {
                        fileData = new FileData(name, (string)r["Hash"]!, long.Parse((string)r["Size"]!));
                    }
                }
            }
            return fileData;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="name">文件名</param>
        /// <param name="options">参数设置</param>
        /// <returns>结果</returns>
        public async Task<FileData?> AddAsync(Stream stream, string name = "", AddFileOptions? options = null, long fileLength = 0)
        {
            #region 上传参数
            options ??= new AddFileOptions();
            var opts = new List<string>();
            if (!options.Pin)
                opts.Add("pin=false");
            if (options.Wrap)
                opts.Add("wrap-with-directory=true");
            if (options.RawLeaves)
                opts.Add("raw-leaves=true");
            if (options.OnlyHash)
                opts.Add("only-hash=true");
            if (options.Trickle)
                opts.Add("trickle=true");
            if (options.Progress != null)
                opts.Add("progress=true");
            //if (options.Hash != MultiHash.DefaultAlgorithmName)
            //    opts.Add($"hash=${options.Hash}");
            //if (options.Encoding != MultiBase.DefaultAlgorithmName)
            //    opts.Add($"cid-base=${options.Encoding}");
            if (!string.IsNullOrWhiteSpace(options.ProtectionKey))
                opts.Add($"protect={options.ProtectionKey}");
            opts.Add($"chunker=size-{options.ChunkSize}");
            opts.Add($"recursive=false");
            #endregion

            StreamContent content = new(stream);
            var response = await UploadGetStreamAsync(BuildCommand("add", null, opts.ToArray()), content, name);
            return await ReadStreamAsync(response, options, name, fileLength);
        }

        public async Task<Dictionary<string, string>> GetIPNSAsync()
        {
            Uri command = BuildCommand("key/list");
            string resultString = await DoCommandAsync(command);
            JObject resultObject = JObject.Parse(resultString);
            Dictionary<string, string> ipnsDic = new();
            if (resultObject.ContainsKey("Keys"))
            {
                foreach (var ipns in resultObject["Keys"]!)
                    ipnsDic.Add(ipns["Name"]!.ToString(), ipns["Id"]!.ToString());
            }
            return ipnsDic;
        }
    }

    public class AddFileOptions
    {
        /// <summary>
        /// Determines if the data is pinned to local storage.
        /// </summary>
        /// <returns>
        /// If true the data is pinned to local storage and will not be garbage collected.The default is true.
        /// </returns>
        public bool Pin { get; set; } = true;

        /// <summary>
        /// The maximum number of data bytes in a block.
        /// </summary>
        /// <returns>
        /// The default is 256 * 1024 (‭262,144) bytes.‬
        /// </returns>
        public int ChunkSize { get; set; } = 262144;

        /// <summary>
        /// Determines if the trickle-dag format is used for dag generation.
        /// </summary>
        /// <returns>
        /// The default is false.
        /// </returns>
        public bool Trickle { get; set; }

        /// <summary>
        /// Determines if added file(s) are wrapped in a directory object.
        /// </summary>
        /// <returns>
        /// The default is false.
        /// </returns>
        public bool Wrap { get; set; }

        /// <summary>
        /// Determines if raw blocks are used for leaf data blocks.
        /// RawLeaves and Ipfs.CoreApi.AddFileOptions.ProtectionKey are mutually exclusive.
        /// </summary>
        /// <returns>
        /// The default is false.
        /// </returns>
        public bool RawLeaves { get; set; }

        /// <summary>
        /// The hashing algorithm name to use.
        /// </summary>
        /// <returns>
        /// The Ipfs.MultiHash algorithm name used to produce the Ipfs.Cid. Defaults to Ipfs.MultiHash.DefaultAlgorithmName.
        /// </returns>
        public string Hash { get; set; } = "sha2-256";

        /// <summary>
        /// The encoding algorithm name to use.
        /// </summary>
        /// <returns>
        /// The Ipfs.MultiBase algorithm name used to produce the Ipfs.Cid. Defaults to Ipfs.MultiBase.DefaultAlgorithmName.
        /// </returns>
        public string Encoding { get; set; } = "base58btc";

        /// <summary>
        /// Determines if only file information is produced.
        /// </summary>
        /// <returns>
        /// If true no data is added to IPFS. The default is false.
        /// </returns>
        public bool OnlyHash { get; set; }

        /// <summary>
        /// The key name used to protect (encrypt) the file contents.
        /// ProtectionKey and Ipfs.CoreApi.AddFileOptions.RawLeaves are mutually exclusive.
        /// </summary>
        /// <returns>
        /// The name of an existing key.
        /// </returns>
        public string? ProtectionKey { get; set; }

        /// <summary>
        /// Used to report the progress of a file transfer.
        /// </summary>
        public IProgress<string>? Progress { get; set; }

        //public AddFileOptions(IProgress<TransferProgress> progress)
        //{
        //    Progress = progress;
        //}

        public AddFileOptions()
        {

        }
    }
}

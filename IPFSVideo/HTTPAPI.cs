using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
        static readonly HttpClient httpClient = new HttpClient();

        public void GetMessage()
        {
            var respones = httpClient.GetAsync("http://baidu.com");
            var stream = respones.Result.Content.ReadAsStreamAsync();
            var postContent = new MultipartFormDataContent();
            //postContent.Add(new StreamContent());
            var respones2 = httpClient.PostAsync("http://baidu.com", postContent);
        }
    }
}

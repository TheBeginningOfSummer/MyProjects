using IPFSVideo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPFSVideo.IPFS
{
    public class ProgressableHttpContent : HttpContent
    {
        private const int DefaultBufferSize = 5 * 4096;//20KB
        private HttpContent content;
        private int bufferSize;
        private IProgress<TransferProgress> progress;
        private TransferProgress progressData;

        //public ProgressableHttpContent(HttpContent content, IProgress<TransferProgress> progress) : this(content, DefaultBufferSize, progress)
        //{

        //}

        public ProgressableHttpContent(HttpContent content, int bufferSize, IProgress<TransferProgress> progress, TransferProgress transferProgress)
        {
            if (bufferSize <= 0) throw new ArgumentOutOfRangeException(nameof(bufferSize));
            this.content = content ?? throw new ArgumentNullException(nameof(content));
            this.bufferSize = bufferSize;
            this.progress = progress ?? throw new ArgumentNullException(nameof(progress));
            progressData = transferProgress;
            foreach (var header in content.Headers)
            {
                Headers.Add(header.Key, header.Value);
            }
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            long length = 0;
            using (var contentStream = await content.ReadAsStreamAsync())
            {
                await contentStream.CopyToAsync(stream);
                length += contentStream.Length;
                progressData.Bytes = (ulong)length;
                progress.Report(progressData);
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            length = content.Headers.ContentLength.GetValueOrDefault();
            return content.Headers.ContentLength.HasValue;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) content.Dispose();
            base.Dispose(disposing);
        }
    }
}

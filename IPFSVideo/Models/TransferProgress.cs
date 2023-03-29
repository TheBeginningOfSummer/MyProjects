using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPFSVideo.Models
{
    public class TransferProgress
    {
        public string? Name;
        //The cumulative number of bytes transfered for the TransferProgress.Name.
        public ulong Bytes;

        public long AllLength;

    }
}

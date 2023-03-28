using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPFSVideo.IPFS
{
    public interface IProgress<in T>
    {
        void Report(T value);
    }
}

using IPFS.Models;
using MyToolkit;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IPFS.Services;

public class CommonServiceLoader
{
    //单例模式
    private static CommonServiceLoader? _instance;
    private static readonly object _instanceLock = new();
    public static CommonServiceLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                    _instance ??= new CommonServiceLoader();
            }
            return _instance;
        }
    }

    public readonly SQLiteService SQLite;
    public readonly HttpClientAPI IPFSApi;

    private CommonServiceLoader()
    {
        SQLite = new();
        IPFSApi = new();
    }

    public async Task<FileData?> LoadAndUploadFileAsync(string path)
    {
        //文件名
        string fileName = path.Split('\\').LastOrDefault("nofile");
        //文件长度
        long fileLength = new FileInfo(path).Length;
        //上传文件
        return await IPFSApi.AddAsync
        (FileManager.GetFileStream(path), fileName, null, fileLength);
    }

    public async Task<string> PublishDatabaseAsync(Animation animation, bool changeOrDelete = true)
    {
        if (changeOrDelete)
            //数据插入或更新（先读取判断此条数据是否存在）
            await animation.DatabaseUpdateAsync(SQLite);
        else
            await SQLite.SQLConnection.DeleteAsync(animation);
        //上传数据库到IPFS
        var databaseInfo = await IPFSApi.AddAsync
        (FileManager.GetFileStream(SQLite.DatabasePath), SQLite.DatabaseName, null, new FileInfo(SQLite.DatabasePath).Length);
        //发布到IPNS
        if (databaseInfo != null)
            return await IPFSApi.DoCommandAsync
                    (HttpClientAPI.BuildCommand("name/publish", databaseInfo.Cid, "key=self"));
        return "Failed";
    }
}

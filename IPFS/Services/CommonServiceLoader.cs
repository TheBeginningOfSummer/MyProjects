using IPFS.Models;
using MyToolkit;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IPFS.Services;

public class CommonServiceLoader
{
    #region 单例模式
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
    #endregion

    public readonly SQLiteService SQLite;
    public readonly HttpClientAPI IPFSApi;

    private CommonServiceLoader()
    {
        SQLite = new();
        IPFSApi = new();
    }
    /// <summary>
    /// 加载上传文件到IPFS
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns>结果</returns>
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
    /// <summary>
    /// 更新animation数据库并上传至IPFS
    /// </summary>
    /// <param name="animation">数据（带数据库）</param>
    /// <param name="changeOrDelete">更改还是删除数据true为更改</param>
    /// <returns>发布结果</returns>
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

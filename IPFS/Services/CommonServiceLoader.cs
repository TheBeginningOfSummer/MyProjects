using IPFS.Models;
using MyToolkit.FileManagement;
using System.Collections.Generic;
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

    public readonly HttpClientAPI IPFSApi = new();
    public readonly Dictionary<string, SQLiteService> Databases = new();
    public readonly Dictionary<string, KeyValueManager> Configs = new();

    private CommonServiceLoader()
    {
        Databases.Add("Local", new SQLiteService());//本地数据
        //Databases.Add("Remote", new SQLiteService("self", "IPNSData"));//
        Configs.Add("Config", new("Configuration.json", "Config"));
        Configs.Add("RemoteIPNS", new("IPNSList.json", "Config"));
    }

    /// <summary>
    /// 加载然后上传文件到IPFS
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns>结果</returns>
    public async Task<FileData?> LoadThenUploadFileAsync(string path)
    {
        //文件名
        string fileName = path.Split('\\').LastOrDefault("nofile");
        //文件长度
        long fileLength = new FileInfo(path).Length;
        //上传文件
        return await HttpClientAPI.AddAsync
        (FileManager.GetFileStream(path), fileName, null, fileLength);
    }
    /// <summary>
    /// 更新数据库上传至IPFS，并发布至IPNS
    /// </summary>
    /// <param name="animation">数据（带数据库）</param>
    /// <param name="changeOrDelete">更改还是删除数据true为更改</param>
    /// <returns>发布结果</returns>
    public async Task<string> PublishIPNSDatabaseAsync(Album animation, string ipnsName = "self", bool changeOrDelete = true)
    {
        if (changeOrDelete)
            //数据插入或更新（先读取判断此条数据是否存在，不删除原有文件列表）
            await animation.DataUpdateAsync(Databases["Local"], 1);
        else
            await Databases["Local"].SQLConnection.DeleteAsync(animation);
        //上传数据库到IPFS
        var databaseInfo = await HttpClientAPI.AddAsync
        (FileManager.GetFileStream(Databases["Local"].DatabasePath), Databases["Local"].DatabaseName, null, new FileInfo(Databases["Local"].DatabasePath).Length);
        //发布到IPNS
        if (databaseInfo != null)
            return await HttpClientAPI.DoCommandAsync
                    (HttpClientAPI.BuildCommand("name/publish", databaseInfo.Cid, $"key={ipnsName}"));
        return "Failed";
    }
    /// <summary>
    /// 从ipns下载数据文件
    /// </summary>
    /// <param name="ipnsText">ipns地址</param>
    /// <returns></returns>
    public async Task<string> DownloadIPNSDatabaseAsync(string? ipnsText, string storagePath = "IPNSData")
    {
        if (string.IsNullOrEmpty(ipnsText)) return "";
        if (ipnsText.Contains(':'))
        {
            string name = ipnsText.Split(':')[0];
            string value = ipnsText.Split(":")[1];
            string cid = await HttpClientAPI.ResolveIPNSAsync(value);
            await HttpClientAPI.DownloadFileAsync(cid, name, storagePath);
            return name;
        }
        return "";
    }
}

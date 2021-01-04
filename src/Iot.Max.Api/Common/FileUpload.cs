using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Iot.Max.Api.Common
{
    public class FileUpload
    {
        private IWebHostEnvironment _host;
        public FileUpload(IWebHostEnvironment host) 
        {
            _host = host;
        }
        public async Task<List<string>> UploadAsync(IFormFileCollection files,string strDirName) 
        {
            List<string> path = new List<string>();
            foreach (var item in files)
            {
                if(item.Length>0)
                {
                    var www = _host.WebRootPath;
                    var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(item.FileName);
                    var dirPath = Path.Combine(www, strDirName);
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    var filePath = Path.Combine(dirPath, fileName); //文件的保存路径

                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        await item.CopyToAsync(stream);//保存文件
                        stream.Dispose();

                        path.Add($"/{strDirName}/{fileName}");
                    }
                }
            }

            return path;
        }
    }
}

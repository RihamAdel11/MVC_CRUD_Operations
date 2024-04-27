﻿using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PLL.Helpers
{
    public static class DocumentSetting
    {
        public static string UploadFile(IFormFile file,string FolderName)
        {
            string FolderPath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\Files",FolderName);
            string FileName=$"{Guid.NewGuid()}{file.FileName}";
            string FilePath=Path.Combine(FolderPath,FileName);
            var Fs=new FileStream (FilePath,FileMode.Create);
            file.CopyTo(Fs);
            return FileName;
        }
        public static void DeleteFile(string FileName, string FolderName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName,FileName);
            if(File.Exists (FilePath))
            {
                File.Delete (FilePath);
            }
           
          
        }
    }
}

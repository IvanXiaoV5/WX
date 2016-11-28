using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksum;

namespace IvanXiao
{
    public class ZipHelper
    {
        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="dirPath">压缩文件夹的路径</param>
        /// <param name="fileName">生成的zip文件路径</param>
        /// <param name="level">压缩级别 0 - 9 0是存储级别 9是最大压缩</param>
        /// <param name="bufferSize">读取文件的缓冲区大小</param>
        public void CompressDirectory(string dirPath, string fileName, int level, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            using (ZipOutputStream s = new ZipOutputStream(File.Create(fileName)))
            {
                s.SetLevel(level);
                CompressDirectory(dirPath, dirPath, s, buffer);
                s.Finish();
                s.Close();
            }
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="root">压缩文件夹路径</param>
        /// <param name="path">压缩文件夹内当前要压缩的文件夹路径</param>
        /// <param name="s"></param>
        /// <param name="buffer">读取文件的缓冲区大小</param>
        private void CompressDirectory(string root, string path, ZipOutputStream s, byte[] buffer)
        {
            //root = root.TrimEnd('/');// + "\\";
            string[] fileNames = Directory.GetFiles(path);
            string[] dirNames = Directory.GetDirectories(path);
            string relativePath = path.Replace(root, "");
            if (relativePath != "")
            {
                relativePath = relativePath.Replace("//", "/") + "/";
            }
            int sourceBytes;
            foreach (string file in fileNames)
            {

                ZipEntry entry = new ZipEntry(relativePath + Path.GetFileName(file));
                entry.DateTime = DateTime.Now;
                s.PutNextEntry(entry);
                using (FileStream fs = File.OpenRead(file))
                {
                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        s.Write(buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);
                }
            }

            foreach (string dirName in dirNames)
            {
                string relativeDirPath = dirName.Replace(root, "");
                ZipEntry entry = new ZipEntry(relativeDirPath.Replace("//", "/") + "/");
                s.PutNextEntry(entry);
                CompressDirectory(root, dirName, s, buffer);
            }
        }

        /// <summary>
        /// 解压缩zip文件
        /// </summary>
        /// <param name="zipFilePath">解压的zip文件路径</param>
        /// <param name="extractPath">解压到的文件夹路径</param>
        /// <param name="bufferSize">读取文件的缓冲区大小</param>
        public void Extract(string zipFilePath, string extractPath, int bufferSize)
        {
            extractPath = extractPath.TrimEnd('/') + "//";
            byte[] data = new byte[bufferSize];
            int size;
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                ZipEntry entry;
                while ((entry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(entry.Name);
                    string fileName = Path.GetFileName(entry.Name);

                    //先创建目录
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(extractPath + directoryName);
                    }

                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(extractPath + entry.Name.Replace("/", "//")))
                        {
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

    }


}


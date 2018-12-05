//-----------------------------------------------------------------------
// <filename>CustomDownloadHandlerFile</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #自定义下载文件，解决默认在下载失败时自动创建文件# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/5 星期三 12:13:49# </time>
//-----------------------------------------------------------------------

using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace GameFramework.Taurus
{
    public class CustomDownloadHandlerFile:DownloadHandlerScript
    {
        // Standard scripted download handler - will allocate memory on each ReceiveData callback
        public CustomDownloadHandlerFile()
            : base()
        {
        }

        // Pre-allocated scripted download handler
        // Will reuse the supplied byte array to deliver data.
        // Eliminates memory allocation.
        public CustomDownloadHandlerFile(string localpath,byte[] buffer)
            : base(buffer)
        {
            Init(localpath);
        }

        // Required by DownloadHandler base class. Called when you address the 'bytes' property.
        protected override byte[] GetData() { return null; }

        // Called once per frame when data has been received from the network.
        protected override bool ReceiveData(byte[] byteFromServer, int dataLength)
        {
            if (byteFromServer == null || byteFromServer.Length < 1)
            {
                Debug.Log("CustomWebRequest :: ReceiveData - received a null/empty buffer");
                return false;
            }

            //Write the current data chunk to file
            AppendFile(byteFromServer, dataLength);

            return true;
        }

        //Where to save the video file
        private string _localpath;
        //The FileStream to save the file
        /// <summary>
        /// 临时文件流
        /// </summary>
        private FileStream fileStream = null;
        //Used to determine if there was an error while opening or saving the file
        private bool success = true;

        void Init(string localpath)
        {
            _localpath = localpath;
            if (File.Exists(_localpath + ".temp"))
                File.Delete(_localpath + ".temp");
            fileStream = new FileStream(_localpath + ".temp", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        void AppendFile(byte[] buffer, int length)
        {
            if (success)
            {
                try
                {
                    //Write the current data to the file
                    fileStream.Write(buffer, 0, length);
                    Debug.Log("Written data chunk to: " + _localpath.Replace("/", "\\"));
                }
                catch (Exception e)
                {
                    success = false;
                }
            }
        }

        // Called when all data has been received from the server and delivered via ReceiveData
        protected override void CompleteContent()
        {
            //Create Directory if it does not exist
            if (!Directory.Exists(Path.GetDirectoryName(_localpath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_localpath));
            }

            try
            {
                fileStream.Close();
                if (File.Exists(_localpath))
                    File.Delete(_localpath);
                File.Move(_localpath + ".temp", _localpath);

                //Open the current file to write to
                //FileStream saveStream = new FileStream(_localpath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                //Debug.Log("File Successfully opened at" + _localpath.Replace("/", "\\"));
                success = true;
            }
            catch (Exception e)
            {
                success = false;
                Debug.LogError("Failed to Open File at Dir: " + _localpath.Replace("/", "\\") + "\r\n" + e.Message);
            }

            //Close filestream
            //fileStream.Close();

            if (success)
            {
#if UNITY_EDITOR
                Debug.Log("Done! Saved File to: " + _localpath.Replace("/", "\\"));
#endif
            } 
            else
            {
#if UNITY_EDITOR
                Debug.LogError("Failed to Save File to: " + _localpath.Replace("/", "\\"));
#endif
            }
                
        }

        // Called when a Content-Length header is received from the server.
        protected override void ReceiveContentLength(int contentLength)
        {
            //Debug.Log(string.Format("CustomWebRequest :: ReceiveContentLength - length {0}", contentLength));
        }
    }
}

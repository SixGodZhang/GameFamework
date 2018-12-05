//-----------------------------------------------------------------------
// <filename>BPDownloadHandler</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #支持断点续传类# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/4 星期二 21:22:36# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

namespace GameFramework.Taurus
{
    /// <summary>
    /// 自定义下载类
    /// </summary>
    public class BPDownloadHandler: DownloadHandlerScript
    {
        #region 字段&属性
        /// <summary>
        /// 文件流:写入文件
        /// </summary>
        private FileStream fs = null;

        /// <summary>
        /// 下载文件总长度
        /// </summary>
        private int _contentLength = 0;
        public int ContentLength
        {
            get { return _contentLength; }
        }

        /// <summary>
        /// 已经下载的文件长度
        /// </summary>
        private int _downedLength = 0;
        public int DownedLength
        {
            get { return _downedLength; }
        }

        /// <summary>
        /// 需要保存的文件名
        /// </summary>
        private string _savefilename;
        public string SaveFileName
        {
            get { return _savefilename; }
        }

        /// <summary>
        /// 临时文件名
        /// </summary>
        public string TempFileName
        {
            get { return _savefilename + ".temp"; }
        }

        /// <summary>
        /// 保存的文件路径
        /// </summary>
        private string _savePath = null;
        public string DirectoryPath
        {
            get { return _savePath.Substring(0, _savePath.LastIndexOf('/')); }
        }

        /// <summary>
        /// 远程下载地址
        /// </summary>
        private string _remoteurl = null;
        #endregion

        #region 事件
        public Action<int> GetTotalContentLengthEvent;
        /// <summary>
        /// 下载进度remoteurl localpath 下载大小 下载进度 已花费时间
        /// </summary>
        public Action<string, string, ulong, float, float> GetDownloadProgressEvent;
        /// <summary>
        /// 下载完成事件 remoteurl localpath result additive
        /// </summary>
        public Action<string, string, bool, string> CompleteDownloadEvent;
        #endregion

        #region 构造函数
        public BPDownloadHandler(string remoteurl, string localpath):base(new byte[1024*200])
        {
            _remoteurl = remoteurl;

            _savePath = localpath.Replace('\\', '/');
            _savefilename = _savePath.Substring(_savePath.LastIndexOf('/') + 1);

            //文件流操作的是临时文件，结尾添加.temp扩展名
            this.fs = new FileStream(_savePath + ".temp", FileMode.Append, FileAccess.Write);
            //设置已经下载的数据长度
            _downedLength = (int)fs.Length;
            //设置下次下载的起始点
            fs.Position = _downedLength;
        }
        #endregion

        /// <summary>
        /// 回调函数,接收数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        protected override bool ReceiveData(byte[] data, int dataLength)
        {
            if (data == null || dataLength == 0)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Log("未接收到数据...");
#endif
                return false;
            }

            fs.Write(data, 0, dataLength);//????
            _downedLength += dataLength;

            if (GetDownloadProgressEvent != null)
                GetDownloadProgressEvent.Invoke(_remoteurl,_savePath, (ulong)(data.Length/(1024.0f*1024.0f)),(float)_downedLength / _contentLength,0);

            return true;
        }

        /// <summary>
        /// 请求下载时会先接收到数据的长度
        /// </summary>
        /// <param name="contentLength"></param>
        protected override void ReceiveContentLength(int contentLength)
        {
            _contentLength = contentLength + _downedLength;
            if (GetTotalContentLengthEvent != null)
                GetTotalContentLengthEvent.Invoke(_contentLength);
        }

        /// <summary>
        /// 下载完成
        /// </summary>
        protected override void CompleteContent()
        {
            string CompleteFilePath = DirectoryPath + "/" + _savefilename;   //完整路径
            string TempFilePath = fs.Name;   //临时文件路径
            OnDispose();

            if (File.Exists(TempFilePath))
            {
                if (File.Exists(CompleteFilePath))
                {
                    File.Delete(CompleteFilePath);
                }
                File.Move(TempFilePath, CompleteFilePath);
                //Debug.Log("重命名文件！");

                if (CompleteDownloadEvent != null)
                    CompleteDownloadEvent.Invoke(_remoteurl, _savePath, true,"下载资源成功!");
            }
            else
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Log("生成文件失败=>下载的文件不存在！");
                if (CompleteDownloadEvent != null)
                    CompleteDownloadEvent.Invoke(_remoteurl, _savePath, false, "生成文件失败!");
#endif
            }

        }

        public void OnDispose()
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log("下载总量=>" + _downedLength);
#endif
            fs.Close();
            fs.Dispose();
        }
    }
}

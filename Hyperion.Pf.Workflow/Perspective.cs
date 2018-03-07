using System;
using System.Collections.Generic;
using System.Linq;

namespace Hyperion.Pf.Workflow
{
    /// <summary>
    /// パースペクティブ
    /// </summary>
    /// <remarks>
    /// 1つのフレームに1つのコンテントを設定できます。
    /// </remarks>
    public class Perspective
    {
        private readonly WorkflowManager _Context;

        /// <summary>
        /// キー=フレーム名
        /// 値=コンテント名
        /// </summary>
        /// <returns></returns>
        private readonly Dictionary<string, string> _ContentDict = new Dictionary<string, string>();

        /// <summary>
        /// パースペクティブの状態を取得する
        /// </summary>
        /// <returns></returns>
        public PerspectiveStatus Status { get; internal set; }

        /// <summary>
        /// 調停モードを取得します
        /// </summary>
        /// <returns></returns>
        public ArbitrationMode Mode { get; private set; }

        /// <summary>
        /// パースペクティブ名を取得します
        /// </summary>
        /// <returns></returns>
        public string Name { get; private set; }

        /// <summary>
        /// パースペクティブに関連付けされたコンテントのフレーム一覧を取得します
        /// </summary>
        /// <returns></returns>
        public string[] FrameList
        {
            get
            {
                return _ContentDict.Keys.ToArray();
            }
        }

        /// <summary>
        /// インスタンス化されたコンテント一覧を取得します
        /// </summary>
        /// <returns></returns>
        public Content[] Contents
        {
            get
            {
                var contentList = new List<Content>();

                foreach(var contentName in _ContentDict.Values){
                    contentList.Add(_Context.GetContent(contentName));
                }

                return contentList.ToArray();
            }
        }

        /// <summary>
        /// パースペクティブにコンテントを登録する
        /// </summary>
        /// <param name="frameName">フレーム名</param>
        /// <param name="startContent">コンテント</param>
        // internal void AddContent(string frameName, Content startContent)
        // {
        //     if (!_ContentDict.ContainsKey(frameName))
        //         _ContentDict.Add(frameName, startContent);
        //     else
        //         throw new ApplicationException("指定のフレームにコンテントが割当済みです");
        // }

        // /// <summary>
        // /// パースペクティブからコンテントを除去します
        // /// </summary>
        // /// <param name="frameName"></param>
        // internal void RemoveContent(string frameName)
        // {
        //     _ContentDict.Remove(frameName);
        // }

        /// <summary>
        /// パースペクティブからコンテントを除去します
        /// </summary>
        // /// <param name="disposedContent">除去するコンテント</param>
        // internal void RemoveContent(Content disposedContent)
        // {
        //     var myKey = _ContentDict.FirstOrDefault(x => x.Value == disposedContent).Key;
        //     if (myKey != null)
        //     {
        //         _ContentDict.Remove(myKey);
        //     }
        // }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="PerspectiveName">パースペクティブ名</param>
        /// <param name="Mode">調停モード</param>
        /// <param name="ContentDict">フレーム別コンテントビルダー対応辞書</param>
        public Perspective(string PerspectiveName, ArbitrationMode Mode, Dictionary<string, string> ContentDict, WorkflowManager context)
        {
            this._Context = context;
            this.Name = PerspectiveName;
            this.Mode = Mode;

            foreach (var key in ContentDict.Keys)
            {
                _ContentDict.Add(key, ContentDict[key]);
            }
        }

        internal Content GetFrameContent(string frameName)
        {
            // 異常系設計
            var contentName = _ContentDict[frameName];
            return _Context.GetContent(contentName);
        }
    }

    /// <summary>
    /// パースペクティブ状態区分
    /// </summary>
    public enum PerspectiveStatus
    {
        Deactive,
        Active,
    }

    /// <summary>
    /// パースペクティブの調停モード区分
    /// </summary>
    public enum ArbitrationMode
    {

        AWAB,
        BWAB,
        OVERRIDE,
        INTRCOMP,
        INTRCOOR
    }
}
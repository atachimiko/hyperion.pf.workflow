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
        /// <summary>
        /// フレーム名とコンテントとの対応辞書
        /// </summary>
        /// <returns></returns>
        private readonly Dictionary<string, IContentBuilder> _ContentBuilderDict = new Dictionary<string, IContentBuilder>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private readonly Dictionary<string, Content> _ContentDict = new Dictionary<string, Content>();

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
                return _ContentBuilderDict.Keys.ToArray();
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
                return _ContentDict.Values.ToArray();
            }
        }

        /// <summary>
        /// コンテントビルダーを取得します
        /// </summary>
        /// <param name="FrameName">フレーム名</param>
        /// <returns></returns>
        public IContentBuilder GetContentBuilder(string FrameName)
        {
            return _ContentBuilderDict[FrameName];
        }

        /// <summary>
        /// パースペクティブにコンテントを登録する
        /// </summary>
        /// <param name="frameName">フレーム名</param>
        /// <param name="startContent">コンテント</param>
        internal void AddContent(string frameName, Content startContent)
        {
            if (!_ContentDict.ContainsKey(frameName))
                _ContentDict.Add(frameName, startContent);
            else
                throw new ApplicationException("指定のフレームにコンテントが割当済みです");
        }

        /// <summary>
        /// パースペクティブからコンテントを除去します
        /// </summary>
        /// <param name="frameName"></param>
        internal void RemoveContent(string frameName)
        {
            _ContentDict.Remove(frameName);
        }

        /// <summary>
        /// パースペクティブからコンテントを除去します
        /// </summary>
        /// <param name="disposedContent">除去するコンテント</param>
        internal void RemoveContent(Content disposedContent)
        {
            var myKey = _ContentDict.FirstOrDefault(x => x.Value == disposedContent).Key;
            if (myKey != null)
            {
                _ContentDict.Remove(myKey);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="PerspectiveName">パースペクティブ名</param>
        /// <param name="Mode">調停モード</param>
        /// <param name="ContentDict">フレーム別コンテントビルダー対応辞書</param>
        public Perspective(string PerspectiveName, ArbitrationMode Mode, Dictionary<string, IContentBuilder> ContentDict)
        {
            this.Name = PerspectiveName;
            this.Mode = Mode;

            foreach (var key in ContentDict.Keys)
            {
                _ContentBuilderDict.Add(key, ContentDict[key]);
            }
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
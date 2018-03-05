namespace Hyperion.Pf.Workflow
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContentBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attach">作成したコンテントが所属するパースペクティブ</param>
        /// <returns></returns>
        Content Build(Perspective attach);
    }
}

using System.Threading.Tasks;
using NLog;

namespace Pixstock.Applus.Foundations.Preview.Transitions
{
    /// <summary>
    /// ジェネレータツールで生成したワークフローモデルクラス（Content）のPartial化したクラス
    /// </summary>
    public partial class ImagrePreviewTransitionWorkflow
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        // Partialして呼び出しているため、実態をここで定義する
        async Task OnACT_SHOW_NAVINEXT_PREVIEW(object param)
        {
            _logger.Debug("IN");
            await Task.Delay(1);
            _logger.Debug("OUT");
        }

        async Task OnACT_SHOW_NAVIPREV_PREVIEW(object param)
        {
            _logger.Debug("IN");
            await Task.Delay(1);
            _logger.Debug("OUT");
        }
    }
}
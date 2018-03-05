using System;
using System.Collections.Generic;
using Moq;
using NLog;
using Xunit;

namespace Hyperion.Pf.Workflow.Tests
{
    public class WorkflowManagerTest
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// StartPerspectiveメソッドの試験
        /// </summary>
        [Fact]
        public void StartPerspective_Test1()
        {
            _logger.Info("{}の実行を開始します",nameof(StartPerspective_Test1));
            string PERSPECTIVE_NAME = "TestPerspective";

            // テスト対象の生成と設定
            var manager = new WorkflowManager();
            var dict = new Dictionary<string, IContentBuilder>();
            dict.Add("MyFrameA", BuildContentBuilder());
            var perspective = new Perspective(PERSPECTIVE_NAME, ArbitrationMode.AWAB, dict);
            manager.RegisterPerspective(perspective);

            // テスト対象のロジック呼び出し
            manager.StartPerspective(PERSPECTIVE_NAME);

            // TODO: ここに、テストコードを実装する
        }

        IContentBuilder BuildContentBuilder()
        {
            var mock = new Mock<IContentBuilder>();
            mock.Setup(x => x.Build(It.IsAny<Perspective>()))
                .Returns<Perspective>(s =>
                {
                    _logger.Trace("Contentオブジェクトの作成");
                    var cmock = new Mock<Content>(s);
                    cmock.Setup(cx => cx.OnStart()).Returns(true);
                    return cmock.Object;
                });
            return mock.Object;
        }
    }
}

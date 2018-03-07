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
        /// <remarks>
        /// 何もPerspectiveを実行していない状態で、新しいPerspectiveを開始する
        /// </remarks>
        [Fact]
        public void StartPerspective_AWAB_Test1()
        {
            _logger.Info("{}の実行を開始します", nameof(StartPerspective_AWAB_Test1));
            string PERSPECTIVE_NAME = "TestPerspective";

            List<IContentBuilder> contentBuilders = new List<IContentBuilder>();
            contentBuilders.Add(BuildContentBuilder("MyContent1"));

            // テスト対象の生成と設定
            var manager = new WorkflowManager();
            manager.Verify(contentBuilders);

            var dict = new Dictionary<string, string>();
            dict.Add("MyFrameA", "MyContent1");
            var perspective = new Perspective(PERSPECTIVE_NAME, ArbitrationMode.AWAB, dict, manager);
            manager.RegisterPerspective(perspective);

            // テスト対象のロジック呼び出し
            manager.StartPerspective(PERSPECTIVE_NAME);

            // TODO: ここに、テストコードを実装する
            var stack = manager._FrameList.GetContentStack("MyFrameA");
            Assert.Single(stack);
        }

        /// <summary>
        /// StartPerspectiveメソッドの試験
        /// </summary>
        /// <remarks>
        /// 1つのPerspectiveを実行してる状態で、重複しないフレームにコンテントを持つ新しいPerspectiveを開始する
        /// </remarks>
        [Fact]
        public void StartPerspective_AWAB_Test2()
        {
            _logger.Info("{}の実行を開始します", nameof(StartPerspective_AWAB_Test2));
            string PERSPECTIVE_NAME1 = "TestPerspective1";
            string PERSPECTIVE_NAME2 = "TestPerspective2";

            List<IContentBuilder> contentBuilders = new List<IContentBuilder>();
            contentBuilders.Add(BuildContentBuilder("MyContent1"));
            contentBuilders.Add(BuildContentBuilder("MyContent2"));

            // テスト対象の生成と設定
            var manager = new WorkflowManager();
            manager.Verify(contentBuilders);

            var dict = new Dictionary<string, string>();
            dict.Add("MyFrameA", "MyContent1");
            var perspective = new Perspective(PERSPECTIVE_NAME1, ArbitrationMode.AWAB, dict, manager);
            manager.RegisterPerspective(perspective);

            var dict2 = new Dictionary<string, string>();
            dict2.Add("MyFrameB", "MyContent2");
            var perspective2 = new Perspective(PERSPECTIVE_NAME2, ArbitrationMode.AWAB, dict2, manager);
            manager.RegisterPerspective(perspective2);

            manager.StartPerspective(PERSPECTIVE_NAME1);

            // テスト対象のロジック呼び出し
            manager.StartPerspective(PERSPECTIVE_NAME2);

            // TODO: ここに、テストコードを実装する
            var stack_MyFrameA = manager._FrameList.GetContentStack("MyFrameA");
            Assert.Single(stack_MyFrameA);
            var stack_MyFrameB = manager._FrameList.GetContentStack("MyFrameB");
            Assert.Single(stack_MyFrameB);
        }

        /// <summary>
        /// StartPerspectiveメソッドの試験
        /// </summary>
        /// <remarks>
        /// 1つのPerspectiveを実行してる状態で、重複するフレームにコンテントを持つ新しいPerspectiveを開始する
        /// </remarks>
        [Fact]
        public void StartPerspective_AWAB_Test3()
        {
            _logger.Info("{}の実行を開始します", nameof(StartPerspective_AWAB_Test3));
            string PERSPECTIVE_NAME1 = "TestPerspective1";
            string PERSPECTIVE_NAME2 = "TestPerspective2";

            List<IContentBuilder> contentBuilders = new List<IContentBuilder>();
            contentBuilders.Add(BuildContentBuilder("MyContent1"));
            contentBuilders.Add(BuildContentBuilder("MyContent2"));

            // テスト対象の生成と設定
            var manager = new WorkflowManager();
            manager.Verify(contentBuilders);

            var dict = new Dictionary<string, string>();
            dict.Add("MyFrameA", "MyContent1");
            var perspective = new Perspective(PERSPECTIVE_NAME1, ArbitrationMode.AWAB, dict, manager);
            manager.RegisterPerspective(perspective);

            var dict2 = new Dictionary<string, string>();
            dict2.Add("MyFrameA", "MyContent2"); // 重複するフレームにコンテントを配置
            var perspective2 = new Perspective(PERSPECTIVE_NAME2, ArbitrationMode.AWAB, dict2, manager);
            manager.RegisterPerspective(perspective2);

            manager.StartPerspective(PERSPECTIVE_NAME1);

            // テスト対象のロジック呼び出し
            manager.StartPerspective(PERSPECTIVE_NAME2);

            // TODO: ここに、テストコードを実装する
            var stack_MyFrameA = manager._FrameList.GetContentStack("MyFrameA");
            Assert.Single(stack_MyFrameA);
        }

        /// <summary>
        /// StartPerspectiveメソッドの試験
        /// </summary>
        /// <remarks>
        /// 1つのPerspectiveを実行してる状態で、重複しないフレームにコンテントを持つ新しいPerspectiveを開始する
        /// </remarks>
        [Fact]
        public void StartPerspective_BWAB_Test1()
        {
            _logger.Info("{}の実行を開始します", nameof(StartPerspective_BWAB_Test1));
            string PERSPECTIVE_NAME1 = "TestPerspective1";
            string PERSPECTIVE_NAME2 = "TestPerspective2";

            List<IContentBuilder> contentBuilders = new List<IContentBuilder>();
            contentBuilders.Add(BuildContentBuilder("MyContent1"));
            contentBuilders.Add(BuildContentBuilder("MyContent2"));

            // テスト対象の生成と設定
            var manager = new WorkflowManager();
            manager.Verify(contentBuilders);

            var dict = new Dictionary<string, string>();
            dict.Add("MyFrameA", "MyContent1");
            var perspective = new Perspective(PERSPECTIVE_NAME1, ArbitrationMode.AWAB, dict, manager);
            manager.RegisterPerspective(perspective);

            var dict2 = new Dictionary<string, string>();
            dict2.Add("MyFrameB", "MyContent2");
            var perspective2 = new Perspective(PERSPECTIVE_NAME2, ArbitrationMode.BWAB, dict2, manager);
            manager.RegisterPerspective(perspective2);

            manager.StartPerspective(PERSPECTIVE_NAME1);

            // テスト対象のロジック呼び出し
            manager.StartPerspective(PERSPECTIVE_NAME2);

            // TODO: ここに、テストコードを実装する
            var stack_MyFrameA = manager._FrameList.GetContentStack("MyFrameA");
            Assert.Single(stack_MyFrameA);
            var stack_MyFrameB = manager._FrameList.GetContentStack("MyFrameB");
            Assert.Single(stack_MyFrameB);
        }

        /// <summary>
        /// StartPerspectiveメソッドの試験
        /// </summary>
        /// <remarks>
        /// 1つのPerspectiveを実行してる状態で、重複するフレームにコンテントを持つ新しいPerspectiveを開始する
        /// </remarks>
        [Fact]
        public void StartPerspective_BWAB_Test2()
        {
            _logger.Info("{}の実行を開始します", nameof(StartPerspective_BWAB_Test2));
            string PERSPECTIVE_NAME1 = "TestPerspective1";
            string PERSPECTIVE_NAME2 = "TestPerspective2";

            List<IContentBuilder> contentBuilders = new List<IContentBuilder>();
            contentBuilders.Add(BuildContentBuilder("MyContent1"));
            contentBuilders.Add(BuildContentBuilder("MyContent2"));

            // テスト対象の生成と設定
            var manager = new WorkflowManager();
            manager.Verify(contentBuilders);

            var dict = new Dictionary<string, string>();
            dict.Add("MyFrameA", "MyContent1");
            var perspective = new Perspective(PERSPECTIVE_NAME1, ArbitrationMode.AWAB, dict, manager);
            manager.RegisterPerspective(perspective);

            var dict2 = new Dictionary<string, string>();
            dict2.Add("MyFrameA", "MyContent2"); // 重複するフレームにコンテントを配置
            var perspective2 = new Perspective(PERSPECTIVE_NAME2, ArbitrationMode.BWAB, dict2, manager);
            manager.RegisterPerspective(perspective2);

            manager.StartPerspective(PERSPECTIVE_NAME1);

            // テスト対象のロジック呼び出し
            manager.StartPerspective(PERSPECTIVE_NAME2);

            // TODO: ここに、テストコードを実装する
            var stack_MyFrameA = manager._FrameList.GetContentStack("MyFrameA");
            Assert.Single(stack_MyFrameA);
        }

        IContentBuilder BuildContentBuilder(string contentName)
        {
            var mock = new Mock<IContentBuilder>();
            mock.Setup(x => x.Build())
                .Returns(() =>
                {
                    _logger.Trace("Contentオブジェクトの作成");
                    var cmock = new Mock<Content>(contentName);
                    cmock.Setup(cx => cx.OnStart()).Returns(true);
                    cmock.Setup(cx => cx.OnPreDestroy()).Returns(true);
                    cmock.Setup(cx => cx.OnPreResume()).Returns(true);
                    return cmock.Object;
                });
            return mock.Object;
        }
    }
}

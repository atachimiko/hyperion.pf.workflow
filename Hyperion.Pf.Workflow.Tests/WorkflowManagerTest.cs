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
            var manager = new HarmonicManager();
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
            Assert.Equal(stack.ToArray()[0].Name, "MyContent1");
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
            var manager = new HarmonicManager();
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
            Assert.Equal(stack_MyFrameA.ToArray()[0].Name, "MyContent1");
            Assert.Equal(stack_MyFrameB.ToArray()[0].Name, "MyContent2");
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
            var manager = new HarmonicManager();
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
            Assert.Equal(stack_MyFrameA.ToArray()[0].Name, "MyContent2");
            Assert.Equal(stack_MyFrameA.ToArray()[0].Status, ContentStatus.Run);
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
            var manager = new HarmonicManager();
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
        /// 【試験概要】
        /// 1つのPerspectiveを実行してる状態で、重複するフレームにコンテントを持つ新しいPerspectiveを開始する
        /// 【合格基準】
        /// ・新しいPerspectiveが起動しないこと
        /// ・既存のPerspectiveのコンテントがRun状態であること
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
            var manager = new HarmonicManager();
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
            Assert.Equal(stack_MyFrameA.ToArray()[0].Name, "MyContent1");
            Assert.Equal(stack_MyFrameA.ToArray()[0].Status, ContentStatus.Run);
        }

        /// <summary>
        /// StartPerspectiveメソッドの試験
        /// </summary>
        /// <remarks>
        /// 【試験概要】
        /// 1つのPerspectiveを実行してる状態で、重複するフレームにコンテントを持つ新しいPerspectiveを開始する
        /// 【合格基準】
        /// ・新しいPerspectiveが起動すること
        /// </remarks>
        [Fact]
        public void StartPerspective_OVERRIDE_Test1()
        {
            _logger.Info("{}の実行を開始します", nameof(StartPerspective_BWAB_Test2));
            string PERSPECTIVE_NAME1 = "TestPerspective1";
            string PERSPECTIVE_NAME2 = "TestPerspective2";

            List<IContentBuilder> contentBuilders = new List<IContentBuilder>();
            contentBuilders.Add(BuildContentBuilder("MyContent1a"));
            contentBuilders.Add(BuildContentBuilder("MyContent1b"));
            contentBuilders.Add(BuildContentBuilder("MyContent2"));

            // テスト対象の生成と設定
            var manager = new HarmonicManager();
            manager.Verify(contentBuilders);

            var dict = new Dictionary<string, string>();
            dict.Add("MyFrameA", "MyContent1a"); // 重複するフレームにコンテントを配置
            dict.Add("MyFrameB", "MyContent1b"); // 重複しないフレームにコンテントを配置
            var perspective = new Perspective(PERSPECTIVE_NAME1, ArbitrationMode.AWAB, dict, manager);
            manager.RegisterPerspective(perspective);

            var dict2 = new Dictionary<string, string>();
            dict2.Add("MyFrameA", "MyContent2"); // 重複するフレームにコンテントを配置
            var perspective2 = new Perspective(PERSPECTIVE_NAME2, ArbitrationMode.OVERRIDE, dict2, manager);
            manager.RegisterPerspective(perspective2);

            manager.StartPerspective(PERSPECTIVE_NAME1);

            // テスト対象のロジック呼び出し
            manager.StartPerspective(PERSPECTIVE_NAME2);

            // TODO: ここに、テストコードを実装する
            var stack_MyFrameA = manager._FrameList.GetContentStack("MyFrameA");
            Assert.Single(stack_MyFrameA);
            Assert.Equal(stack_MyFrameA.ToArray()[0].Name, "MyContent2");
            Assert.Equal(stack_MyFrameA.ToArray()[0].Status, ContentStatus.Run);

            var stack_MyFrameB = manager._FrameList.GetContentStack("MyFrameB");
            Assert.Equal(0, stack_MyFrameB.Count);
        }

        /// <summary>
        /// StartPerspectiveメソッドの試験
        /// </summary>
        /// <remarks>
        /// 【試験概要】
        /// 1つのPerspectiveを実行してる状態で、重複するフレームにコンテントを持つ新しいPerspectiveを開始する
        /// 【合格基準】
        /// ・新しいPerspectiveが起動すること
        /// ・既存のPerspectiveのコンテント状態がSuspendであること
        /// </remarks>
        [Fact]
        public void StartPerspective_INTRCOMP_Test1()
        {
            _logger.Info("{}の実行を開始します", nameof(StartPerspective_INTRCOMP_Test1));
            string PERSPECTIVE_NAME1 = "TestPerspective1";
            string PERSPECTIVE_NAME2 = "TestPerspective2";

            List<IContentBuilder> contentBuilders = new List<IContentBuilder>();
            contentBuilders.Add(BuildContentBuilder("MyContent1a"));
            contentBuilders.Add(BuildContentBuilder("MyContent1b"));
            contentBuilders.Add(BuildContentBuilder("MyContent2"));

            // テスト対象の生成と設定
            var manager = new HarmonicManager();
            manager.Verify(contentBuilders);

            var dict = new Dictionary<string, string>();
            dict.Add("MyFrameA", "MyContent1a"); // 重複するフレームにコンテントを配置
            dict.Add("MyFrameB", "MyContent1b"); // 重複しないフレームにコンテントを配置(→Suspend状態になること)
            var perspective = new Perspective(PERSPECTIVE_NAME1, ArbitrationMode.AWAB, dict, manager);
            manager.RegisterPerspective(perspective);

            var dict2 = new Dictionary<string, string>();
            dict2.Add("MyFrameA", "MyContent2"); // 重複するフレームにコンテントを配置
            var perspective2 = new Perspective(PERSPECTIVE_NAME2, ArbitrationMode.INTRCOMP, dict2, manager);
            manager.RegisterPerspective(perspective2);

            manager.StartPerspective(PERSPECTIVE_NAME1);

            // テスト対象のロジック呼び出し
            manager.StartPerspective(PERSPECTIVE_NAME2);

            // TODO: ここに、テストコードを実装する
            var stack_MyFrameA = manager._FrameList.GetContentStack("MyFrameA");
            Assert.Equal(2, stack_MyFrameA.Count);
            Assert.Equal(stack_MyFrameA.ToArray()[0].Name, "MyContent2");
            Assert.Equal(stack_MyFrameA.ToArray()[0].Status, ContentStatus.Run);
            Assert.Equal(stack_MyFrameA.ToArray()[1].Name, "MyContent1a");
            Assert.Equal(stack_MyFrameA.ToArray()[1].Status, ContentStatus.Suspend);

            var stack_MyFrameB = manager._FrameList.GetContentStack("MyFrameB");
            Assert.Single(stack_MyFrameB);
            Assert.Equal(stack_MyFrameB.ToArray()[0].Name, "MyContent1b");
            Assert.Equal(stack_MyFrameB.ToArray()[0].Status, ContentStatus.Suspend);
        }

        /// <summary>
        /// StartPerspectiveメソッドの試験
        /// </summary>
        /// <remarks>
        /// 【試験概要】
        /// 1つのPerspectiveを実行してる状態で、重複するフレームにコンテントを持つ新しいPerspectiveを開始する
        /// 【合格基準】
        /// ・新しいPerspectiveが起動すること
        /// ・既存のPerspectiveのコンテント状態がRunであること
        /// </remarks>
        [Fact]
        public void StartPerspective_INTRCOOR_Test1()
        {
            _logger.Info("{}の実行を開始します", nameof(StartPerspective_INTRCOOR_Test1));
            string PERSPECTIVE_NAME1 = "TestPerspective1";
            string PERSPECTIVE_NAME2 = "TestPerspective2";

            List<IContentBuilder> contentBuilders = new List<IContentBuilder>();
            contentBuilders.Add(BuildContentBuilder("MyContent1a"));
            contentBuilders.Add(BuildContentBuilder("MyContent1b"));
            contentBuilders.Add(BuildContentBuilder("MyContent2"));

            // テスト対象の生成と設定
            var manager = new HarmonicManager();
            manager.Verify(contentBuilders);

            var dict = new Dictionary<string, string>();
            dict.Add("MyFrameA", "MyContent1a"); // 重複するフレームにコンテントを配置
            dict.Add("MyFrameB", "MyContent1b"); // 重複しないフレームにコンテントを配置(→Run状態のまま)
            var perspective = new Perspective(PERSPECTIVE_NAME1, ArbitrationMode.AWAB, dict, manager);
            manager.RegisterPerspective(perspective);

            var dict2 = new Dictionary<string, string>();
            dict2.Add("MyFrameA", "MyContent2"); // 重複するフレームにコンテントを配置
            var perspective2 = new Perspective(PERSPECTIVE_NAME2, ArbitrationMode.INTRCOOR, dict2, manager);
            manager.RegisterPerspective(perspective2);

            manager.StartPerspective(PERSPECTIVE_NAME1);

            // テスト対象のロジック呼び出し
            manager.StartPerspective(PERSPECTIVE_NAME2);

            // TODO: ここに、テストコードを実装する
            var stack_MyFrameA = manager._FrameList.GetContentStack("MyFrameA");
            Assert.Equal(2, stack_MyFrameA.Count);
            Assert.Equal(stack_MyFrameA.ToArray()[0].Name, "MyContent2");
            Assert.Equal(stack_MyFrameA.ToArray()[0].Status, ContentStatus.Run);
            Assert.Equal(stack_MyFrameA.ToArray()[1].Name, "MyContent1a");
            Assert.Equal(stack_MyFrameA.ToArray()[1].Status, ContentStatus.Suspend);

            var stack_MyFrameB = manager._FrameList.GetContentStack("MyFrameB");
            Assert.Single(stack_MyFrameB);
            Assert.Equal(stack_MyFrameB.ToArray()[0].Name, "MyContent1b");
            Assert.Equal(stack_MyFrameB.ToArray()[0].Status, ContentStatus.Run);
        }

        IContentBuilder BuildContentBuilder(string contentName)
        {
            var mock = new Mock<IContentBuilder>();
            mock.Setup(x => x.Build())
                .Returns(() =>
                {
                    _logger.Trace("Contentオブジェクトの作成");
                    var cmock = new Mock<TestContent>(contentName) { CallBase = true };
                    cmock.Setup(cx => cx.OnPreResume()).Returns(true);
                    cmock.Setup(cx => cx.OnPreStop()).Returns(true);
                    return cmock.Object;
                });
            return mock.Object;
        }
    }

    public abstract class TestContent : Content
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public TestContent(string contentName) : base(contentName)
        {
        }

        public override void OnDestroy()
        {
            _logger.Debug("Execute");
        }

        public override void OnDiscontinue()
        {
            _logger.Debug("Execute");
        }

        public override void OnEnd()
        {
            _logger.Debug("Execute");
        }

        public override void OnIdle()
        {
            _logger.Debug("Execute");
        }

        public override void OnInitialize()
        {
            _logger.Debug("Execute");
        }

        public override void OnRestart()
        {
            _logger.Debug("Execute");
        }

        public override void OnResume()
        {
            _logger.Debug("IN");
            CompleteStart();
            _logger.Debug("OUT");
        }

        public override void OnRun()
        {
            _logger.Debug("Execute");
        }

        public override void OnStop()
        {
            _logger.Debug("IN");
            CompleteStop();
            _logger.Debug("OUT");
        }

        public override void OnSuspend()
        {
            _logger.Debug("Execute");
        }
    }
}
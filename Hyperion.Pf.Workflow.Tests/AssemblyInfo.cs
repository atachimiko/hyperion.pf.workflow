using Xunit;

// 複数テストのパラレル実行は実施しない
[assembly: CollectionBehavior(MaxParallelThreads = 1)]
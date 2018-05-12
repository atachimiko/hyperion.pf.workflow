namespace Hyperion.Pf.Workflow
{
    public enum ContentStatus
    {
        Create,

        Initialize,
        Idle,
        Run,
        Restart,
        Stop,
        Suspend,
        Resume,
        PreStop,
        Discontinue,
        PreResume,
        Destroy,
        End
    }
}
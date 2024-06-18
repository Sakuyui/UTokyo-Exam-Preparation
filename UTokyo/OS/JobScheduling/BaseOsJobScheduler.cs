namespace UTokyo.OS.JobScheduling
{
    public abstract class BaseOsJobScheduler
    {
        public delegate object JobSchedulerCallBack(params object[] objects);
        public abstract void Refresh();
        public abstract void JoinJob(OsJob osJob, int joinTime, JobSchedulerCallBack jobJoinCallback = null);
        public abstract void RemoveJob(OsJob osJob);
        public abstract void BeginSimulate(int endTime, 
          JobSchedulerCallBack jobExecuteCallBack = null, JobSchedulerCallBack jobFinishCallBack = null
          , JobSchedulerCallBack jobSwitchCallBack = null);
        public abstract object ClockCallBack(int curClock, params object[] objects);
    }
}
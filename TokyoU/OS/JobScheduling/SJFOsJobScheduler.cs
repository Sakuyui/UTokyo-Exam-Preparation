using System.Linq;
using TokyoU.Structure;

namespace TokyoU.OS.JobScheduling
{
    public class SjfOsJobScheduler : BaseOsJobScheduler
    {
        public PriorityQueue<(int eTime, int restTime), OsJob> PriorityQueue = new PriorityQueue<(int eTime, int restTime), OsJob>(
            (e1, e2) => 
                e1.restTime == e2.restTime ? e1.eTime.CompareTo(e2.eTime) : e1.restTime.CompareTo(e2.restTime));

        private int _curClock = 0;
        public override void Refresh()
        {
            PriorityQueue.Clear();
            _curClock = 0;
        }

        public override void JoinJob(OsJob osJob, int joinTime, JobSchedulerCallBack jobJoinCallback = null)
        {
            PriorityQueue.EnQueue((eTime: osJob.RTime, restTime: joinTime), osJob);
            jobJoinCallback?.Invoke(joinTime, osJob);
        }

        public override void RemoveJob(OsJob osJob)
        {
            PriorityQueue.RemoveByItem(osJob);
        }

        public override void BeginSimulate(int endTime, JobSchedulerCallBack jobExecuteCallBack = null,
            JobSchedulerCallBack jobFinishCallBack = null, JobSchedulerCallBack jobSwitchCallBack = null)
        {
            for (var i = 0; i <= endTime && PriorityQueue.Any(); i ++)
            {
                var j = PriorityQueue.DeQueue();
                jobExecuteCallBack?.Invoke(j.item, _curClock);
                _curClock++;
                var newRestTime = j.priority.restTime - 1;
                if (newRestTime > 0)
                {
                    PriorityQueue.EnQueue((j.priority.eTime, newRestTime), j.item);
                }
                else
                {
                    jobFinishCallBack?.Invoke(j.item, _curClock);
                }
            }
        }

        public override object ClockCallBack(int curClock, params object[] objects)
        {
            throw new System.NotImplementedException();
        }
    }
}
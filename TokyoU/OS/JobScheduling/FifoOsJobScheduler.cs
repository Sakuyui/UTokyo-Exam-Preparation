using System;
using System.Collections.Generic;
using System.Linq;
using C5;
using TokyoU.Structure;

namespace TokyoU.OS.JobScheduling
{
    public class FifoOsJobScheduler : BaseOsJobScheduler
    {
        PriorityQueue<int, OsJob> _priorityQueue = new PriorityQueue<int, OsJob>();
        private int _curClock = 0;
        public override void Refresh()
        {
            _priorityQueue.Clear();
            _curClock = 0;
        }

        public override void JoinJob(OsJob osJob, int joinTime, JobSchedulerCallBack jobJoinCallback = null)
        {
            if(_priorityQueue.ContainsKey(joinTime))
                throw new ArithmeticException();
            _priorityQueue.EnQueue(joinTime, osJob);
        }

        public override void RemoveJob(OsJob osJob)
        {
            _priorityQueue.RemoveByItem(osJob);
        }

        public override void BeginSimulate(int endTime
             , JobSchedulerCallBack jobExecuteCallBack = null,
             JobSchedulerCallBack jobFinishCallBack = null
             , JobSchedulerCallBack jobSwitchCallBack = null)
        {
            while (_priorityQueue.Any())
            {

                var j = _priorityQueue.DeQueue();
                jobExecuteCallBack?.Invoke(j,_curClock);
                ClockCallBack(_curClock, j);
                jobFinishCallBack?.Invoke(j, _curClock);
            }
        }

        public override object ClockCallBack(int curClock, params object[] objects)
        {
            var e = (OsJob) objects[0];
            _curClock += e.RTime;
            return e.DoWork();
        }
    }
}
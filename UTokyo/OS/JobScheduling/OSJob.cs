using System;

namespace UTokyo.OS.JobScheduling
{
    public class OsJob
    {
        public delegate object OsJobDetails(params object[] objs);

        public OsJobDetails _osJobDetails;
        public int RTime;
        public OsJob(int requireTime, OsJobDetails osJobDetails = null)
        {
            RTime = requireTime;
            _osJobDetails = osJobDetails;
        }

       

        public object DoWork(params object[] objects)
        {
            return _osJobDetails(objects);
        }
    }
}
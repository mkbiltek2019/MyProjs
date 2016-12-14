﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adidas.clb.job.UpdateTriggering.Utility
{
    interface IAppInsight
    {
        void TrackEvent(string message);
        void TrackMetric(string message, long duration);

        void Exception(string message, Exception exception, string EventID);
        void TrackStartEvent(string methodname);
        void TrackEndEvent(string methodname);
    }

}
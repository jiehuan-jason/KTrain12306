using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTrain12306
{
    class StationQueryInfo
    {
        public StationInfo station_info;
        public enum query_label
        {
            From,
            To
        }
        public query_label status;
        public StationQueryInfo(StationInfo info,query_label status)
        {
            this.status = status;
            station_info = info;
        }
    }
}

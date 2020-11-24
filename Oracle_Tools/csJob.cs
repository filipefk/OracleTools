using System;
using System.Collections.Generic;
using System.Text;

namespace Oracle_Tools
{
    class csJob
    {
        public string OWNER = "";
        public string JOB_NAME = "";
        public string JOB_ACTION = "";
        public string ENABLED = "";
        public string JOB_TYPE = "";
        public string PROGRAM_OWNER = "";
        public string PROGRAM_NAME = "";
        public string SCHEDULE_OWNER = "";
        public string SCHEDULE_NAME = "";
        public string SCHEDULE_TYPE = "";
        public DateTime START_DATE = DateTime.MinValue;
        public string REPEAT_INTERVAL = "";
        public DateTime LAST_START_DATE = DateTime.MinValue;
        public DateTime LAST_RUN_DURATION = DateTime.MinValue;
        public DateTime NEXT_RUN_DATE = DateTime.MinValue;
        public decimal RUN_COUNT = 0;
        public decimal FAILURE_COUNT = 0;
        public string COMMENTS = "";
    }
}

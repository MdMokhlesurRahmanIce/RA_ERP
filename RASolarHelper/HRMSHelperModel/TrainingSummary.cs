using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.HRMSHelperModel
{
    public class TrainingSummary
    {
        public string TrainingTitle { get; set; }
        public string TopicsCovered { get; set; }
        public string Institute { get; set; }
        public string Country { get; set; }
        public string Location { get; set; }
        public string TrainingYear { get; set; }
        public string StartsOn { get; set; }
        public string EndsOn { get; set; }
        public string Duration { get; set; }
        public string Remarks { get; set; }
    }
}

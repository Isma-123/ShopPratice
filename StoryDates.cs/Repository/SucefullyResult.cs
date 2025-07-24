

namespace StoryDates.cs.Repository
{
    public  class SucefullyResult
    {


        public string message { get; set; } = null!;
        public dynamic result { get; set; } = null!;
        public bool status { get; set; }
        public SucefullyResult() => this.status = true;
    }
}

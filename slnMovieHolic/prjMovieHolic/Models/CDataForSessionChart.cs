namespace prjMovieHolic.Models
{
    public class CDataForSessionChart
    {
        public CDataForSessionChart()
        {
            name = "";
            data = new List<SessionTheaterAndTimeData>();
        }
        public string name { get; set; }
        //電影名稱
        public List<SessionTheaterAndTimeData> data { get; set; }
    }

    public class SessionTheaterAndTimeData
    {
        public string? x { get; set; }
        //影廳名稱
        public long[]? y { get; set; }
        //Session開始時間及結束時間(從1970年1月1日開始的毫秒數)
    }
}

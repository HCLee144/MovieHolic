namespace prjMovieHolic.Models
{
    public class CDataForMovieIncome
    {
        //電影收入stack長條圖


        public string? name { get; set; }
        //series:電影名稱

        public List<IncomeDataPerDay>? data { get; set; }
    }

    public class IncomeDataPerDay
    {
        public string x { get; set; }
        //電影收入stack長條圖，X為日期
        public int y { get; set; }
        //電影收入stack長條圖，Y為收入
    }


    public class CDataForMemberAge
    {
        public string? name { get; set; }
        //series:不同年份分別

        public List<MemberAgeData>? data { get; set; }
    }

    public class MemberAgeData
    {
        public string x { get; set; }
        //年齡分布
        public int y { get; set; }
        //人數
    }

    public class PieData
    {
        public List<int> series { get; set; }
        public List<string> labels { get; set; }
    }

    public class SimpleBarData
    {
        public List<Data> data { get; set; }
    }

    public class Data
    {
        //長條圖的data
        public string x { get;set; }
        public int y { get; set; }
    }
}

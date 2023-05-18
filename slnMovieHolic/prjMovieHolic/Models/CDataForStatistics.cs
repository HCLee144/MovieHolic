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

    public class SimpleData
    {
        public List<int> data { get; set; }
    }
}

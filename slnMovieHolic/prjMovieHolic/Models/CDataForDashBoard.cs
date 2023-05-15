namespace prjMovieHolic.Models
{
    public class CDataForMovieIncome
    {
        //電影收入stack長條圖


        public string? name { get; set; }
        //series:電影名稱

        public List<Data>? data { get; set; }
    }

    public class Data
    {
        public string x { get; set; }
        //電影收入stack長條圖，X為日期
        public int y { get; set; }
        //電影收入stack長條圖，Y為收入
    }
}

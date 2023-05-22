namespace prjMovieHolic.Models
{


    public class PieData
    {
        public List<int> series { get; set; } = new List<int>();
        public List<string> labels { get; set; } = new List<string>();
    }

    
    public class BarSimpleDatas
    {
        public BarSimpleDatas()
        {
            data = new List<int>();
        }
        public List<int> data { get; set; }
    }

    public class BarSimpleLabels
    {
        public BarSimpleLabels()
        {
            categories = new List<string>();
        }
       public List<string> categories { get; set; }
    }


    public class BarSeries
    {
        public string name { get; set; } = "";
        public List<BarSeriesData> data { get; set; } = new List<BarSeriesData>();
    }

    public class BarSeriesData
    {
        public string x { get; set; } = "";
        public int y { get; set; } = 0;
    }
}

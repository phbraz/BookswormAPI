namespace BookswormAPI.DTOS;

public class BooksRootResponseDto
{
    public string status { get; set; }
    public string copyright { get; set; }
    public int num_results { get; set; }
    public string last_modified { get; set; }
    public NewYorkTimesResult results { get; set; }
}


public class NewYorkTimesResult
{
    public string list_name { get; set; }
    public string bestsellers_date { get; set; }
    public string published_date { get; set; }
    public string display_name { get; set; }
    public int normal_list_ends_at { get; set; }
    public string updated { get; set; }
    public List<BooksDto> books { get; set; }
    public List<object> corrections { get; set; }
}
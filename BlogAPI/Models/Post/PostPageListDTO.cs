namespace BlogAPI.Models.Post;

public class PostPageListDTO
{
    public List<PostDto> Posts { get; set; }

    public PageInfoModel Pagination { get; set; }
}
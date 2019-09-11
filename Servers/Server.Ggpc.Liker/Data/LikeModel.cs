
namespace Server.Ggpc.Liker.Data
{
    public class LikeModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public bool IsLiked { get; set; }
    }
}

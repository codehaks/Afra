using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Ggpc.Liker.Data;

namespace Server.Ggpc.Liker
{
    public class LikerService : Liker.LikerBase
    {
        private readonly ILogger<LikerService> _logger;
        private readonly LikeDbContext _likeDbContext;
        public LikerService(ILogger<LikerService> logger, LikeDbContext likeDbContext)
        {
            _logger = logger;
            _likeDbContext = likeDbContext;
        }
        public override async Task<TotalLikesReply> GetImageLikes(PostIdRequest request, ServerCallContext context)
        {
            var cancellationToken = context.CancellationToken;

            (bool isExted, LikeModel post) = await ExistedPost(request, cancellationToken);
            if (isExted)
            {
                var likesOfPost = await SumAllLikes(post.Id, cancellationToken);
                WritingInformationLog($"Total likes is: {likesOfPost} ");
                return new TotalLikesReply()
                {
                    TotalCount = likesOfPost
                };
            }
            else
            {
                WritingInformationLog($"Total likes is: 0 ");
                return new TotalLikesReply()
                {
                    TotalCount = 0
                };
            }


        }


        public override async Task<ImagesAndLikesReply> GetAllImagesAndLikes(Ids request, ServerCallContext context)
        {
            var cancellationToken = context.CancellationToken;

            var posts= new List<LikeModel>();
            var postlikes = new List<int>();
            foreach (var postId in request.ArrayPostid)
            {
                var findPost = await FindPostById(postId, cancellationToken);
                var likes = await SumAllLikes(postId, cancellationToken);

                
                posts.Add(findPost);
                postlikes.Add(likes);
            }
            var imagesAndLikesList = new List<ImagesAndLikesReply.Types.ImagesAndLikes>();
            for (int i = 0; i < posts.Count; i++)
            {

                var imagesAndLikes = new ImagesAndLikesReply.Types.ImagesAndLikes()
                {
                    PostId = posts[i].PostId,
                    TotalCount = postlikes[i]
                };
                imagesAndLikesList.Add(imagesAndLikes);
            }


            var reply = new ImagesAndLikesReply();
            reply.AllImagesLikes.Add(imagesAndLikesList);
            return reply;

        }


        public override async Task<LikeStatus> AddImageLike(PostIdRequest request, ServerCallContext context)
        {
            var cancellationToken = context.CancellationToken;
            var (isExted, post) = await ExistedPost(request, cancellationToken);
            if (isExted)
            {
                await UnLike(request, cancellationToken);
                return new LikeStatus()
                {
                    Status = LikeStatus.Types.Status.UnLike
                };
            }
            else
            {
                var like = new LikeModel()
                {
                    PostId = request.PostId,
                    UserId = request.UserId,
                    IsLiked = true
                };
                await _likeDbContext.Likes.AddAsync(like, cancellationToken);
                WritingInformationLog($"added like :like id = {like.Id} by: user id {like.UserId}");
                return new LikeStatus()
                {
                    Status = LikeStatus.Types.Status.Like
                };
            }


        }

        private async Task UnLike(PostIdRequest request, CancellationToken cancellationToken)
        {
            var (isExted, post) = await ExistedPost(request, cancellationToken);
            if (isExted)
            {
                if (post.IsLiked)
                {
                    post.IsLiked = false;
                    _likeDbContext.Likes.Update(post);
                    await _likeDbContext.SaveChangesAsync(cancellationToken);
                    WritingInformationLog($"Unlike like id: {post.Id} By user id {post.UserId}");
                }
            }

        }

        private async Task<(bool isExted, LikeModel post)> ExistedPost(PostIdRequest post, CancellationToken cancellationToken)
        {
            var findPost = await FindPostById(post.PostId, cancellationToken);
            return (findPost != null, findPost);
        }
        private void WritingInformationLog(string logString)
        {
            _logger.LogInformation(logString);

        }

        private ValueTask<LikeModel> FindPostById(int postId, CancellationToken cancellationToken)
        {
            WritingInformationLog($"Searching in db for find post with {postId} id");
            return _likeDbContext.Likes.FindAsync(postId, cancellationToken);
        }

        private Task<int> SumAllLikes(int postId, CancellationToken cancellationToken)
        {
            WritingInformationLog($"Take Task of total count of likes from db");
            return _likeDbContext.Likes.CountAsync(l => l.PostId == postId && l.IsLiked, cancellationToken);
        }



    }
}

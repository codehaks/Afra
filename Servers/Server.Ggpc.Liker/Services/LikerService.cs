using System;
using System.Collections.Generic;
using System.Linq;
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

            (bool isExted, LikeModel post) = await ExistedPostByIdAsync(request, cancellationToken);
            if (isExted)
            {

                var likesOfPost = await SumAllLikesByPostId(post.PostId, cancellationToken);
                WritingInformationLog($"Total likes is: {likesOfPost} ");

                var selfUserLike = await LikeIdentity(post.PostId, post.UserId, cancellationToken);
                if (selfUserLike)
                {
                    return new TotalLikesReply()
                    {
                        TotalCount = likesOfPost,
                        UserId = request.UserId,
                        Status = TotalLikesReply.Types.Status.Like
                    };
                }
                else
                {
                    return new TotalLikesReply()
                    {
                        TotalCount = likesOfPost,
                        UserId = request.UserId,
                        Status = TotalLikesReply.Types.Status.UnLike
                    };
                }

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
        private void WritingInformationLog(string logString)
        {
            _logger.LogWarning(logString);

        }

        public override async Task<ImagesAndLikesReply> GetAllImagesAndLikes(Ids request, ServerCallContext context)
        {
            try
            {
                var cancellationToken = context.CancellationToken;

                var reply = new ImagesAndLikesReply();

                foreach (var postId in request.ArrayPostid)
                {
                    var findLike = await FindPostByIdAsync(postId, cancellationToken);
                    if (findLike == null) continue;
                    var likes = await SumAllLikesByPostId(findLike.PostId, cancellationToken);
                    reply.ImageLikeList.Add(new ImagesAndLikesReply.Types.ImageAndLike() { PostId = findLike.PostId, TotalCount = likes });
                }

                return reply;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }


        public override async Task<Empty> AddImageLike(PostIdRequest request, ServerCallContext context)
        {
            var cancellationToken = context.CancellationToken;
            var (isExted, post) = await ExistedPostByIdAsync(request, cancellationToken);
            if (isExted)
            {
                if (post.IsLiked)
                {
                    await UnLike(request, cancellationToken);

                }
                else
                {
                    await Like(request, cancellationToken);

                }


            }
            else
            {
                await AddImageLike(request, cancellationToken);

            }

            return new Empty();
        }



        #region Clean Code

        private async Task AddImageLike(PostIdRequest request, CancellationToken cancellationToken)
        {
            var like = new LikeModel()
            {
                PostId = request.PostId,
                UserId = request.UserId,
                IsLiked = true
            };
            await _likeDbContext.Likes.AddAsync(like, cancellationToken);
            await _likeDbContext.SaveChangesAsync(cancellationToken);
            WritingInformationLog($"added like :like id = {like.Id} by: user id {like.UserId}");

        }
        private async Task UnLike(PostIdRequest request, CancellationToken cancellationToken)
        {
            var (isExted, post) = await ExistedPostByIdAsync(request, cancellationToken);
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
        private async Task Like(PostIdRequest request, CancellationToken cancellationToken)
        {
            var (isExted, post) = await ExistedPostByIdAsync(request, cancellationToken);
            if (isExted)
            {
                if (!post.IsLiked)
                {
                    post.IsLiked = true;
                    _likeDbContext.Likes.Update(post);
                    await _likeDbContext.SaveChangesAsync(cancellationToken);
                    WritingInformationLog($"like like id: {post.Id} By user id {post.UserId}");
                }
            }

        }

        private async Task<(bool isExted, LikeModel post)> ExistedPostByIdAsync(PostIdRequest post, CancellationToken cancellationToken)
        {
            var findPost = await FindPostByIdAsync(post.PostId, cancellationToken);
            return (findPost != null, findPost);
        }


        private async ValueTask<LikeModel> FindPostByIdAsync(int postId, CancellationToken cancellationToken)
        {
            try
            {
                WritingInformationLog($"Searching in db for find post with {postId} id");
                return await _likeDbContext.Likes.FirstOrDefaultAsync(x => x.PostId == postId, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private Task<int> SumAllLikesByPostId(int postId, CancellationToken cancellationToken)
        {
            WritingInformationLog($"Take Task of total count of likes from db");
            return _likeDbContext.Likes.CountAsync(l => l.PostId == postId && l.IsLiked == true, cancellationToken);
        }

        private async Task<bool> LikeIdentity(int postId, string userId, CancellationToken cancellationToken)
        {
            var findPost = await FindPostByIdAsync(postId, cancellationToken);
            if (findPost.UserId == userId && findPost.IsLiked==true)
                return true;
            else
                return false;
        }
        #endregion

    }
}

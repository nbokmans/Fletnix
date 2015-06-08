using System.Linq;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace Fletnix.Models.Youtube
{
    public class Youtube
    {
        private const string ApiKey = "AIzaSyBk6qBBbpOOy6YVv-K8188YH9ZIArGPCEA";
        private readonly YouTubeService _service;

        public Youtube()
        {
            _service = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = ApiKey,
                ApplicationName = GetType().ToString()
            });
        }
        
        /// <summary>
        /// Uses the Youtube API to search for a video where the title contains queryString. 
        /// 
        /// </summary>
        /// <param name="queryString">The title to search for.</param>
        /// <returns>Returns the videoID of the first result, or NULL if no video is found.</returns>
        public string GetVideoIdOfFirstResult(string queryString)
        {
            var searchListRequest = _service.Search.List("snippet");
            searchListRequest.Q = queryString;
            searchListRequest.MaxResults = 1;

            var searchListResponse = searchListRequest.ExecuteAsync().Result;

            var result = searchListResponse.Items.FirstOrDefault(res => res.Id.Kind == "youtube#video");

            if (result == null) 
                return null;

            return result.Id.VideoId;
        }
    }
}
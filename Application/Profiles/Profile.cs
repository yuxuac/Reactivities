using System.Collections.Generic;
using System.Text.Json.Serialization;
using Domain;

namespace Application.Profiles
{
    public class Profile
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }

        /// <summary>
        /// 是否被当前登录用户关注
        /// </summary>
        /// <value></value>
        [JsonPropertyName("following")]
        public bool IsFollowed { get; set; }

        /// <summary>
        /// 此用户被关注人数
        /// </summary>
        /// <value></value>
        public int FollowersCount { get; set; }

        /// <summary>
        /// 此用户关注人数
        /// </summary>
        /// <value></value>
        public int FollowingCount { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}